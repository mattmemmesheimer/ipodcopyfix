using System;
using System.Linq;
using System.Text;
using System.IO;
using System.ComponentModel;
using System.Collections;
using System.Collections.Generic;

namespace IPodFixGui.ipodfix
{
    class PodFix
    {
        // constants
        public const string MusicDir = "Music";
        public const int MaxNumThreads = 25;
        private static readonly log4net.ILog Log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        // members
        public string SourceDir
        { 
            get { return _sourceDir; } 
            set 
            {
                _sourceDir = value;
                _musicDir = Path.Combine(_sourceDir, MusicDir);
            } 
        }
        public string DestinationDir { get; set; }
        private string _sourceDir;
        private string _musicDir;
        private BackgroundWorker _mainThread;
        private List<BackgroundWorker> _threads;
        private int _numDirs = 0;
        private int _numDirsFixed = 0;
        private object _lock;
        // events
        public delegate void StatusUpdateHandler(object sender, ProgressEventArgs e);
        public event StatusUpdateHandler OnUpdateStatus;

        /// <summary>
        /// Class containing info on the result of the fix operation.
        /// </summary>
        public class PodFixResult
        {
            public bool Cancelled { get; set; }
            public bool Success { get; set; }

            public PodFixResult()
            {
                this.Cancelled = false;
                this.Success = false;
            }
        }

        /// <summary>
        /// Public constructor.
        /// </summary>
        /// <param name="sourceDir">source directory</param>
        /// <param name="destinationDir">destination directory</param>
        public PodFix(string sourceDir, string destinationDir)
        {
            this.SourceDir = sourceDir;
            _musicDir = Path.Combine(sourceDir, MusicDir);
            this.DestinationDir = destinationDir;
            Log.Info("Source: " + sourceDir);
            Log.Info("Destination: " + destinationDir);
        }

        public PodFix() : this("", "") { }

        /// <summary>
        /// Determines if the source directory is valid by checking for the presence of
        /// the "Music" directory.
        /// </summary>
        /// <returns>true if the source directory is valid, false otherwise</returns>
        public bool ValidSourceDir()
        {
            return Directory.Exists(_musicDir);
        }

        /// <summary>
        /// Starts the fix asynchronously.
        /// </summary>
        public void StartFixAsync()
        {
            Log.Info("Starting async fix");
            _mainThread = new BackgroundWorker();
            _mainThread.DoWork += new DoWorkEventHandler(DoFixInBackground);
            _mainThread.RunWorkerCompleted += new RunWorkerCompletedEventHandler(OnFixCompleted);
            _mainThread.RunWorkerAsync();
        }

        /// <summary>
        /// Starts the iPod fix (synchronously).
        /// </summary>
        /// <returns></returns>
        public PodFixResult StartFix()
        {
            var res = new PodFixResult();
            if (!ValidSourceDir())
            {
                res.Success = false;
                return res;
            }
            foreach (string musicDir in Directory.GetDirectories(_musicDir))
            {
                FixDirectory(musicDir);
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
            string[] dirs = Directory.GetDirectories(_musicDir);
            _threads = new List<BackgroundWorker>(MaxNumThreads);
            _numDirs = dirs.Length;
            _numDirsFixed = 0;
            _lock = new object();
            // init each background worker
            for (int i = 0; i < MaxNumThreads; i++)
            {
                var w = new BackgroundWorker();
                w.DoWork += new DoWorkEventHandler(FixDirectoryInBackground);
                w.RunWorkerCompleted += new RunWorkerCompletedEventHandler(OnFixDirectoryCompleted);
                _threads.Add(w);
            }

            foreach (string dir in dirs)
            {
                var directoryThreadLaunched = false;
                // wait for a thread to be available
                while (!directoryThreadLaunched)
                {
                    foreach (var thread in _threads.Where(w => !w.IsBusy))
                    {
                        thread.RunWorkerAsync(dir);
                        directoryThreadLaunched = true;
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
            var directoryToFix = (string)e.Argument;
            FixDirectory(directoryToFix);
        }

        /// <summary>
        /// Callback for when a worker thread completes.
        /// </summary>
        /// <param name="sender">object that raised the event</param>
        /// <param name="e">event arguments</param>
        private void OnFixDirectoryCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            lock (_lock)
            {
                _numDirsFixed++;
                double percent = (((double)((double)_numDirsFixed / (double)_numDirs))) * 100.00;
                var args = new ProgressEventArgs(false, percent);
                if (OnUpdateStatus != null)
                {
                    if (_numDirsFixed == _numDirs)
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
            string albumArtistPath = Path.Combine(this.DestinationDir, artist);
            if (!Directory.Exists(albumArtistPath))
            {
                Directory.CreateDirectory(albumArtistPath);
            }
            return albumArtistPath;
        }

        /// <summary>
        /// Creates the album directory if it does not exist.
        /// </summary>
        /// <param name="destDir">directory to create the album directory in</param>
        /// <param name="album">album name</param>
        /// <returns>path of the album directory</returns>
        private string CreateAlbumDirectory(string destDir, string album)
        {
            string albumPath = Path.Combine(destDir, album);
            if (!Directory.Exists(albumPath))
            {
                Directory.CreateDirectory(albumPath);
            }
            return albumPath;
        }

        /// <summary>
        /// Copies the source audio file to the destination directory using the TagLib.File
        /// to create the file name.
        /// </summary>
        /// <param name="sourceFile">source audio file</param>
        /// <param name="destDir">destination directory</param>
        /// <param name="tagFile">tag of the source file</param>
        private void CreateAudioFile(string sourceFile, string destDir, TagLib.File tagFile)
        {
            string ext = Path.GetExtension(sourceFile);
            string filename = Pad(tagFile.Tag.Track) + " - " + tagFile.Tag.Title + ext;
            string dest = Path.Combine(destDir, filename);
            if (!File.Exists(dest))
            {
                File.Copy(sourceFile, dest);
            }
        }

        /// <summary>
        /// Fixes the files in the specified directory.
        /// </summary>
        /// <param name="directoryToFix">directory to fix</param>
        private void FixDirectory(string directoryToFix)
        {
            foreach (string audioFile in Directory.GetFiles(directoryToFix))
            {
                try
                {
                    var tagFile = TagLib.File.Create(audioFile);
                    // ensure an album artist exists
                    if (String.IsNullOrEmpty(tagFile.Tag.FirstAlbumArtist) &&
                        String.IsNullOrEmpty(tagFile.Tag.FirstPerformer)
                        ) {
                            Log.Info("skipping" + audioFile);
                            Console.WriteLine("skipping" + audioFile); 
                            continue; 
                    }
                    // create directories if necessary
                    string artist = String.IsNullOrEmpty(tagFile.Tag.FirstPerformer) ? tagFile.Tag.FirstAlbumArtist : tagFile.Tag.FirstPerformer;
                    string albumArtistDir = CreateAlbumArtistDirectory(artist);
                    string albumDir = CreateAlbumDirectory(albumArtistDir, tagFile.Tag.Album);
                    // create the audio file
                    //log.Info("Creating " + AudioFile + " in " + AlbumDir);
                    CreateAudioFile(audioFile, albumDir, tagFile);
                }
                catch (Exception ex) {
                    Log.Error("Error processing " + audioFile, ex);
                    continue; 
                }
            }
        }

        /// <summary>
        /// Pads a number with a 0 if it is less than 10.
        /// </summary>
        /// <param name="n">number to pad</param>
        /// <returns>number as a string</returns>
        private static string Pad(uint n) { return n < 10 ? "0" + n : "" + n; }
    }
}
