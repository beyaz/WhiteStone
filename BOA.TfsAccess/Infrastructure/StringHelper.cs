using System.Globalization;
using BOA.Common.Helpers;

namespace BOA.TfsAccess
{
    static class StringHelper
    {
        /// <summary>
        ///     Determines whether [is equal as data] [the specified left].
        /// </summary>
        public static bool IsEqualAsData(string left, string right)
        {
            return IsEqualAsData(left, right, CultureInfo.CurrentCulture);
        }

        /// <summary>
        ///     Determines whether [is equal as data] [the specified left].
        /// </summary>
        public static bool IsEqualAsData(string left, string right, CultureInfo cultureInfo)
        {
            return new SpaceCaseInsensitiveComparator(cultureInfo).Compare(left, right);
        }
    }
}