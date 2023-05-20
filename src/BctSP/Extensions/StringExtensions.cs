using System.Text.RegularExpressions;

namespace BctSP.Extensions
{
    public static class StringExtensions
    {
        public static bool CheckIfConnectionString(this string text)
        {
            var regex = new Regex("(?<Key>[^=;]+)=(?<Val>[^;]+)");
            return regex.IsMatch(text);
        }
    }
}