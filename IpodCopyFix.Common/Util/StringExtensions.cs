using System.IO;
using System.Linq;

namespace IpodCopyFix.Common.Util
{
    /// <summary>
    /// String extension methods.
    /// </summary>
    public static class StringExtensions
    {
        /// <summary>
        /// Removes characters that are illegal in a file name.
        /// </summary>
        /// <param name="input">Input string.</param>
        /// <returns>String with illegal characters removed.</returns>
        public static string RemoveIllegalFilenameChars(this string input)
        {
            var invalidChars = Path.GetInvalidFileNameChars();
            return new string(input.Where(x => !invalidChars.Contains(x)).ToArray());
        }
    }
}
