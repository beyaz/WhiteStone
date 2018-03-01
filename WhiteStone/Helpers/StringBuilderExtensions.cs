using System.Text;

namespace WhiteStone.Helpers
{
    /// <summary>
    ///     Helper methods for StringBuilder class.
    /// </summary>
    public static class StringBuilderExtensions
    {
        /// <summary>
        ///     Returns true if stringBuilder endsWith given parameter <paramref name="text" />
        /// </summary>
        /// <param name="sb"></param>
        /// <param name="text"></param>
        /// <returns></returns>
        public static bool EndsWith(this StringBuilder sb, string text)
        {
            if (sb.Length < text.Length)
            {
                return false;
            }

            var sbLength = sb.Length;
            var textLength = text.Length;
            for (var i = 1; i <= textLength; i++)
            {
                if (text[textLength - i] != sb[sbLength - i])
                {
                    return false;
                }
            }
            return true;
        }
    }
}