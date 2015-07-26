using System;
using System.IO;
using System.Threading.Tasks;
using IpodCopyFix.Common.AsyncSynchronization;
using IpodCopyFix.Common.Util;
using File = TagLib.File;

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
        }

        /// <see cref="IIpodFix.StartAsync"/>
        public async Task StartAsync(string[] directories, string destinationPath)
        {
            if (_running)
            {
                throw new InvalidOperationException(@"already running");
            }
            _destinationPath = destinationPath;

            _running = true;
            await Task.Run(() => Parallel.ForEach(directories, FixDirectoryAsync));
            _running = false;
        }

        private async void FixDirectoryAsync(string path)
        {
            var files = Directory.GetFiles(path);
            foreach (var file in files)
            {
                var tag = File.Create(file);
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
            }
        }

        #region Fields

        private readonly AsyncLock _lock;
        private bool _running;
        private string _destinationPath;

        #endregion
    }
}
