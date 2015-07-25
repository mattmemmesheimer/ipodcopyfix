using System;
using System.IO;
using System.Threading.Tasks;
using IpodCopyFix.Common.AsyncSynchronization;

namespace IpodCopyFix.Common
{
    /// <summary>
    /// Concrete implementation of <see cref="IIPodFix"/>.
    /// </summary>
    public class IpodFix :IIPodFix
    {
        /// <summary>
        /// Constructs a new <see cref="IpodFix"/>.
        /// </summary>
        public IpodFix()
        {
            _lock = new AsyncLock();
        }

        public async Task StartAsync(string[] directories)
        {
            if (_running)
            {
                throw new InvalidOperationException(@"already running");
            }
            _running = true;
            await Task.Factory.StartNew(() =>
            { Parallel.ForEach(directories, path => FixDirectoryAsync(path)); });
            _running = false;
        }

        public async Task FixDirectoryAsync(string path)
        {
            var files = Directory.GetFiles(path);
            foreach (var file in files)
            {
                var tag = TagLib.File.Create(file);
                if (tag.Tag.FirstAlbumArtist != string.Empty)
                {
                    using (await _lock.LockAsync())
                    {
                        // create directory if it doesn't already exist
                    }
                }
            }
        }

        #region Fields

        private readonly AsyncLock _lock;
        private bool _running;

        #endregion
    }
}
