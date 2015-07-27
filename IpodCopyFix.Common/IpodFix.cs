using System;
using System.IO;
using System.Threading.Tasks;
using IpodCopyFix.Common.AsyncSynchronization;
using IpodCopyFix.Common.Util;
using log4net;

namespace IpodCopyFix.Common
{
    /// <summary>
    /// Concrete implementation of <see cref="IIpodFix"/>.
    /// </summary>
    public class IpodFix :IIpodFix
    {
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
            int skipped = 0;
            foreach (var file in files)
            {
                var result = await FixFileAsync(file);
                if (result)
                {
                    skipped++;
                    var name = Path.GetFileName(file);
                    Logger.DebugFormat("Skipping file {0}.", name);
                }
            }
            Logger.DebugFormat("Finished fixing directory {0}.  Skipped {1} of {2} files.", path,
                skipped, files.Length);
        }

        private async Task<bool> FixFileAsync(string path)
        {
            var tag = TagLib.File.Create(path);
            var artist = tag.Tag.FirstAlbumArtist;
            if (!string.IsNullOrEmpty(artist))
            {
                artist = artist.RemoveIllegalFilenameChars();
                using (await _lock.LockAsync())
                {
                    // create directory if it doesn't already exist
                    var dir = Path.Combine(_destinationPath, artist);
                    if (!Directory.Exists(dir))
                    {
                        Directory.CreateDirectory(dir);
                    }
                }
            }
            else
            {
                return false;
            }
            return true;
        }

        #region Fields

        private static readonly ILog Logger = LogManager.GetLogger(typeof (IpodFix));

        private readonly AsyncLock _lock;
        private bool _running;
        private string _destinationPath;

        #endregion
    }
}
