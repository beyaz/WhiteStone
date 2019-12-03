using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

namespace BOA.Common.Helpers
{
    /// <summary>
    ///     Utility methods for string class.
    /// </summary>
    public static class StringHelper
    {
        #region Public Methods
        /// <summary>
        ///     Determines whether this instance has value.
        /// </summary>
        public static bool HasValue(this string value)
        {
            return !string.IsNullOrWhiteSpace(value);
        }

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

        /// <summary>
        ///     Indicates whether the specified string is null or an System.String.Empty
        /// </summary>
        public static bool IsNullOrEmpty(this string value)
        {
            return string.IsNullOrEmpty(value);
        }

        /// <summary>
        ///     Indicates whether a specified string is null, empty, or consists only of white-space characters.
        /// </summary>
        public static bool IsNullOrWhiteSpace(this string value)
        {
            return string.IsNullOrWhiteSpace(value);
        }

        /// <summary>
        ///     Compare strings using ordinal sort rules.
        /// </summary>
        public static bool IsStartsWith(this string data, string value)
        {
            if (data == null)
            {
                return false;
            }

            return data.StartsWith(value, StringComparison.Ordinal);
        }

        /// <summary>
        ///     Removes the specified value.
        /// </summary>
        public static string Remove(this string data, string value)
        {
            if (data == null)
            {
                return null;
            }

            return data.Replace(value, "");
        }

        /// <summary>
        ///     Removes value from end of str
        /// </summary>
        public static string RemoveFromEnd(this string data, string value)
        {
            return RemoveFromEnd(data, value, StringComparison.CurrentCulture);
        }

        /// <summary>
        ///     Removes from end.
        /// </summary>
        public static string RemoveFromEnd(this string data, string value, StringComparison comparison)
        {
            if (data.EndsWith(value, comparison))
            {
                return data.Substring(0, data.Length - value.Length);
            }

            return data;
        }

        /// <summary>
        ///     Removes value from start of str
        /// </summary>
        public static string RemoveFromStart(this string data, string value)
        {
            return RemoveFromStart(data, value, StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>
        ///     Removes value from start of str
        /// </summary>
        public static string RemoveFromStart(this string data, string value, StringComparison comparison)
        {
            if (data == null)
            {
                return null;
            }

            if (data.StartsWith(value, comparison))
            {
                return data.Substring(value.Length, data.Length - value.Length);
            }

            return data;
        }

        /// <summary>
        ///     Splits the and clear.
        /// </summary>
        public static IReadOnlyList<string> SplitAndClear(this string input, string splitter)
        {
            if (input == null)
            {
                return new string[0];
            }

            return input.Split(splitter.ToCharArray()).Where(x => !string.IsNullOrWhiteSpace(x)).Select(x => x.Trim()).ToList();
        }

        /// <summary>
        ///     Splits to lines.
        /// </summary>
        public static IEnumerable<string> SplitToLines(this string input)
        {
            if (input == null)
            {
                yield break;
            }

            using (var reader = new StringReader(input))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    yield return line;
                }
            }
        }

        /// <summary>
        ///     To the lower and clear spaces.
        /// </summary>
        public static string ToLowerAndClearSpaces(this string value)
        {
            return ToLowerAndClearSpaces(value, CultureInfo.CurrentCulture);
        }

        /// <summary>
        ///     To the lower and clear spaces.
        /// </summary>
        public static string ToLowerAndClearSpaces(this string value, CultureInfo cultureInfo)
        {
            if (value == null)
            {
                return null;
            }

            return SpaceCaseInsensitiveComparator.ExceptChars(value.ToLower(cultureInfo), SpaceCaseInsensitiveComparator.ExceptCharacters);
        }

        /// <summary>
        ///     Returns a copy of this string converted to lowercase, using the casing rules of 'English' culture
        /// </summary>
        public static string ToLowerEN(this string value)
        {
            return value.ToLower(new CultureInfo("en-US"));
        }

        /// <summary>
        ///     Returns a copy of this string converted to lowercase, using the casing rules of 'Turkish' culture
        /// </summary>
        public static string ToLowerTR(this string value)
        {
            return value.ToLower(new CultureInfo("tr-TR"));
        }

        /// <summary>
        ///     Returns a copy of this string converted to uppercase, using the casing rules of 'English' culture
        /// </summary>
        public static string ToUpperEN(this string value)
        {
            return value.ToUpper(new CultureInfo("en-US"));
        }

        /// <summary>
        ///     Returns a copy of this string converted to uppercase, using the casing rules of Turkish culture
        /// </summary>
        public static string ToUpperTR(this string value)
        {
            return value.ToUpper(new CultureInfo("tr-TR"));
        }
        #endregion
    }
}