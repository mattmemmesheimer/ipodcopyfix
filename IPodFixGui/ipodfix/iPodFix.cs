using System;
using System.Linq;
using System.Text;
using System.IO;
using System.ComponentModel;
using System.Collections;
using System.Collections.Generic;

namespace IPodFixGui.ipodfix
{
    class iPodFix
    {
        // constants
        public const string MUSIC_DIR = "Music";
        public const int MAX_NUM_THREADS = 25;
        // members
        public string SourceDir
        { 
            get { return m_SourceDir; } 
            set 
            {
                m_SourceDir = value;
                m_MusicDir = Path.Combine(m_SourceDir, MUSIC_DIR);
            } 
        }
        public string DestinationDir { get; set; }
        private string m_SourceDir;
        private string m_MusicDir;
        private BackgroundWorker m_MainThread;
        private List<BackgroundWorker> m_Threads;
        private int m_NumDirs = 0;
        private int m_NumDirsFixed = 0;
        private object m_Lock;
        // events
        public delegate void StatusUpdateHandler(object sender, ProgressEventArgs e);
        public event StatusUpdateHandler OnUpdateStatus;

        /// <summary>
        /// Class containing info on the result of the fix operation.
        /// </summary>
        public class iPodFixResult
        {
            public bool Cancelled { get; set; }
            public bool Success { get; set; }

            public iPodFixResult()
            {
                this.Cancelled = false;
                this.Success = false;
            }
        }

        /// <summary>
        /// Public constructor.
        /// </summary>
        /// <param name="SourceDir">source directory</param>
        /// <param name="DestinationDir">destination directory</param>
        public iPodFix(string SourceDir, string DestinationDir)
        {
            this.SourceDir = SourceDir;
            m_MusicDir = Path.Combine(SourceDir, MUSIC_DIR);
            this.DestinationDir = DestinationDir;
        }

        public iPodFix() : this("", "") { }

        /// <summary>
        /// Determines if the source directory is valid by checking for the presence of
        /// the "Music" directory.
        /// </summary>
        /// <returns>true if the source directory is valid, false otherwise</returns>
        public bool ValidSourceDir()
        {
            return Directory.Exists(m_MusicDir);
        }

        /// <summary>
        /// Starts the fix asynchronously.
        /// </summary>
        public void StartFixAsync()
        {
            m_MainThread = new BackgroundWorker();
            m_MainThread.DoWork += new DoWorkEventHandler(DoFixInBackground);
            m_MainThread.RunWorkerCompleted += new RunWorkerCompletedEventHandler(OnFixCompleted);
            m_MainThread.RunWorkerAsync();
        }

        /// <summary>
        /// Starts the iPod fix (synchronously).
        /// </summary>
        /// <returns></returns>
        public iPodFixResult StartFix()
        {
            iPodFixResult res = new iPodFixResult();
            if (!ValidSourceDir())
            {
                res.Success = false;
                return res;
            }
            foreach (string MusicDir in Directory.GetDirectories(m_MusicDir))
            {
                FixDirectory(MusicDir);
            }
            res.Success = true;
            return res;
        }

        /// <summary>
        /// "Main" thread that will manage worker threads to fix each directory.
        /// </summary>
        /// <param name="sender">object that raised the event</param>
        /// <param name="e">event arguments</param>
        private void DoFixInBackground(object sender, DoWorkEventArgs e)
        {
            string[] Dirs = Directory.GetDirectories(m_MusicDir);
            m_Threads = new List<BackgroundWorker>(MAX_NUM_THREADS);
            m_NumDirs = Dirs.Length;
            m_NumDirsFixed = 0;
            m_Lock = new object();
            // init each background worker
            for (int i = 0; i < MAX_NUM_THREADS; i++)
            {
                BackgroundWorker w = new BackgroundWorker();
                w.DoWork += new DoWorkEventHandler(FixDirectoryInBackground);
                w.RunWorkerCompleted += new RunWorkerCompletedEventHandler(OnFixDirectoryCompleted);
                m_Threads.Add(w);
            }

            foreach (string Dir in Dirs)
            {
                bool DirectoryThreadLaunched = false;
                // wait for a thread to be available
                while (!DirectoryThreadLaunched)
                {
                    foreach (BackgroundWorker w in m_Threads)
                    {
                        if (w.IsBusy) { continue; }
                        w.RunWorkerAsync(Dir);
                        DirectoryThreadLaunched = true;
                        break;
                    }
                }
            }
        }

        /// <summary>
        /// Callback for when "main" thread is completed.
        /// </summary>
        /// <param name="sender">object that raised the event</param>
        /// <param name="e">event arguments</param>
        private void OnFixCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
        }

        /// <summary>
        /// Worker thread that fixes the specified directory.
        /// </summary>
        /// <param name="sender">object that raised the event</param>
        /// <param name="e">event arguments</param>
        private void FixDirectoryInBackground(object sender, DoWorkEventArgs e)
        {
            string DirectoryToFix = (string)e.Argument;
            FixDirectory(DirectoryToFix);
        }

        /// <summary>
        /// Callback for when a worker thread completes.
        /// </summary>
        /// <param name="sender">object that raised the event</param>
        /// <param name="e">event arguments</param>
        private void OnFixDirectoryCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            lock (m_Lock)
            {
                m_NumDirsFixed++;
                double percent = (((double)((double)m_NumDirsFixed / (double)m_NumDirs))) * 100.00;
                ProgressEventArgs args = new ProgressEventArgs(false, percent);
                if (OnUpdateStatus != null)
                {
                    if (m_NumDirsFixed == m_NumDirs)
                    {
                        args.Completed = true;
                        args.PercentCompleted = 100.00;
                    }
                    OnUpdateStatus(this, args);
                }
            }
        }

        /// <summary>
        /// Creates the album artist directory if it does not exist.
        /// </summary>
        /// <param name="artist">album artist name</param>
        /// <returns>path of the album artist directory</returns>
        private string CreateAlbumArtistDirectory(string artist)
        {
            string AlbumArtistPath = Path.Combine(this.DestinationDir, artist);
            if (!Directory.Exists(AlbumArtistPath))
            {
                Directory.CreateDirectory(AlbumArtistPath);
            }
            return AlbumArtistPath;
        }

        /// <summary>
        /// Creates the album directory if it does not exist.
        /// </summary>
        /// <param name="DestDir">directory to create the album directory in</param>
        /// <param name="album">album name</param>
        /// <returns>path of the album directory</returns>
        private string CreateAlbumDirectory(string DestDir, string album)
        {
            string AlbumPath = Path.Combine(DestDir, album);
            if (!Directory.Exists(AlbumPath))
            {
                Directory.CreateDirectory(AlbumPath);
            }
            return AlbumPath;
        }

        /// <summary>
        /// Copies the source audio file to the destination directory using the TagLib.File
        /// to create the file name.
        /// </summary>
        /// <param name="SourceFile">source audio file</param>
        /// <param name="DestDir">destination directory</param>
        /// <param name="TagFile">tag of the source file</param>
        private void CreateAudioFile(string SourceFile, string DestDir, TagLib.File TagFile)
        {
            string Ext = Path.GetExtension(SourceFile);
            string Filename = Pad(TagFile.Tag.Track) + " - " + TagFile.Tag.Title + Ext;
            string Dest = Path.Combine(DestDir, Filename);
            if (!File.Exists(Dest))
            {
                File.Copy(SourceFile, Dest);
            }
        }

        /// <summary>
        /// Fixes the files in the specified directory.
        /// </summary>
        /// <param name="DirectoryToFix">directory to fix</param>
        private void FixDirectory(string DirectoryToFix)
        {
            foreach (string AudioFile in Directory.GetFiles(DirectoryToFix))
            {
                try
                {
                    TagLib.File TagFile = TagLib.File.Create(AudioFile);
                    // ensure an album artist exists
                    if (String.IsNullOrEmpty(TagFile.Tag.FirstAlbumArtist) &&
                        String.IsNullOrEmpty(TagFile.Tag.FirstPerformer)
                        ) { Console.WriteLine("skipping" + AudioFile); continue; }
                    // create directories if necessary
                    string artist = String.IsNullOrEmpty(TagFile.Tag.FirstPerformer) ? TagFile.Tag.FirstAlbumArtist : TagFile.Tag.FirstPerformer;
                    string AlbumArtistDir = CreateAlbumArtistDirectory(artist);
                    string AlbumDir = CreateAlbumDirectory(AlbumArtistDir, TagFile.Tag.Album);
                    // create the audio file
                    CreateAudioFile(AudioFile, AlbumDir, TagFile);
                }
                catch (Exception ex) { continue; }
            }
        }

        /// <summary>
        /// Pads a number with a 0 if it is less than 10.
        /// </summary>
        /// <param name="n">number to pad</param>
        /// <returns>number as a string</returns>
        private string Pad(uint n) { return n < 10 ? "0" + n : "" + n; }
    }
}
