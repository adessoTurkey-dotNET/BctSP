using System.Text.RegularExpressions;

namespace BctSP.Extensions
{
    /// <summary>
    /// string extensions.
    /// </summary>
    public static class StringExtensions
    {
        /// <summary>
        /// check the text is connection string or not.
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static bool CheckIfConnectionString(this string text)
        {
            var regex = new Regex("(?<Key>[^=;]+)=(?<Val>[^;]+)");
            return regex.IsMatch(text);
        }
    }
}