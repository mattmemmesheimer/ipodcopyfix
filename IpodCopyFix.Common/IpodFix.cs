using System;
using System.IO;
using System.Threading.Tasks;
using IpodCopyFix.Common.AsyncSynchronization;
using IpodCopyFix.Common.Util;
using log4net;
using TagLib;

namespace IpodCopyFix.Common
{
    /// <summary>
    /// Concrete implementation of <see cref="IIpodFix"/>.
    /// </summary>
    public class IpodFix :IIpodFix
    {
        #region Constants

        private const int MaxFilePath = 260;

        #endregion

        /// <summary>
        /// Constructs a new <see cref="IpodFix"/>.
        /// </summary>
        public IpodFix()
        {
            _lock = new AsyncLock();

            Logger.DebugFormat("Creating iPodFix instance.");
        }

        /// <see cref="IIpodFix.StartAsync"/>
        public async Task StartAsync(string[] directories, string destinationPath)
        {
            if (_running)
            {
                throw new InvalidOperationException(@"already running");
            }
            _destinationPath = destinationPath;

            Logger.DebugFormat("Starting iPod fix on {0} directories to destination {1}.",
                directories.Length, destinationPath);

            _running = true;
            await Task.Run(() => Parallel.ForEach(directories, FixDirectoryAsync));
            _running = false;
        }

        private async void FixDirectoryAsync(string path)
        {
            var files = Directory.GetFiles(path);
            Logger.DebugFormat("Starting fix on directory {0} ({1} file(s)).", path, files.Length);
            foreach (var file in files)
            {
                await FixFileAsync(file);
            }
            Logger.DebugFormat("Finished fixing directory {0}", path);
        }

        private async Task FixFileAsync(string path)
        {
            var file = TagLib.File.Create(path);
            var artistPath = await CreateArtistDirectory(file.Tag);
            if (string.IsNullOrEmpty(artistPath))
            {
                // TODO: Handle the case where we are unable to create the artist directory.
                // Perhaps allow the user to select an "Unknown Artists" directory to serve
                // as a home to these misfit files.
                Logger.WarnFormat("Unable to create artist directory for file {0}.", path);
                return;
            }
            var albumPath = await CreateAlbumDirectory(file.Tag, artistPath);
            if (string.IsNullOrEmpty(albumPath))
            {
                // TODO: Handle the case where we are unable to create the album directory.
                // At this point, we know the artist directory was created, but the album
                // directory was not.  Perhaps we place the misfit file in a 
                // "Unknown Albums" directory.
                Logger.WarnFormat("Unable to create album directory for file {0}.", path);
                return;
            }
            await CreateTitleFile(file.Tag, path, albumPath);
        }

        private async Task<string> CreateArtistDirectory(Tag tag)
        {
            var artist = GetArtistName(tag);
            if (string.IsNullOrEmpty(artist))
            {
                return null;
            }
            var dir = Path.Combine(_destinationPath, artist);
            if (dir.Length >= MaxFilePath)
            {
                Logger.ErrorFormat("Max file path length exceeded: {0}", dir);
                return null;
            }
            using (await _lock.LockAsync())
            {
                Directory.CreateDirectory(dir);
            }
            return dir;
        }

        private async Task<string> CreateAlbumDirectory(Tag tag, string path)
        {
            var album = GetAlbumName(tag);
            if (string.IsNullOrEmpty(album))
            {
                return null;
            }
            var dir = Path.Combine(path, album);
            if (dir.Length >= MaxFilePath)
            {
                Logger.ErrorFormat("Max file path length exceeded: {0}", dir);
                return null;
            }
            using (await _lock.LockAsync())
            {
                Directory.CreateDirectory(dir);
            }
            return dir;
        }

        private async Task CreateTitleFile(Tag tag, string sourceFile, string destinationPath)
        {
            var title = GetTitle(tag);
            if (string.IsNullOrEmpty(title))
            {
                return;
            }
            title += Path.GetExtension(sourceFile);
            var destinationFile = Path.Combine(destinationPath, title);
            if (destinationFile.Length >= MaxFilePath)
            {
                Logger.ErrorFormat("Max file path length exceeded: {0}", destinationFile);
                return;
            }
            using (await _lock.LockAsync())
            {
                if (System.IO.File.Exists(destinationFile))
                {
                    return;
                }
                using (var source = System.IO.File.Open(sourceFile, FileMode.Open))
                {
                    using (var destination = System.IO.File.Create(destinationFile))
                    {
                        await source.CopyToAsync(destination);
                    }
                }
            }
        }

        private static string GetArtistName(Tag tag)
        {
            string artist = null;
            if (!string.IsNullOrEmpty(tag.FirstAlbumArtist))
            {
                artist = tag.FirstAlbumArtist;
            }
            else if (!string.IsNullOrEmpty(tag.FirstPerformer))
            {
                artist = tag.FirstPerformer;
            }
            if (artist != null)
            {
                artist = artist.RemoveIllegalFilenameChars();
            }
            return artist;
        }

        private static string GetAlbumName(Tag tag)
        {
            string album = tag.Album;
            if (album != null)
            {
                album = album.RemoveIllegalFilenameChars();
            }
            return album;
        }

        private static string GetTitle(Tag tag)
        {
            string title = tag.Title;
            if (title != null)
            {
                title = title.RemoveIllegalFilenameChars();
            }
            return title;
        }

        #region Fields

        private static readonly ILog Logger = LogManager.GetLogger(typeof (IpodFix));

        private readonly AsyncLock _lock;
        private bool _running;
        private string _destinationPath;

        #endregion
    }
}
