using System;
using System.Globalization;

namespace WhiteStone.Helpers
{
    /// <summary>
    ///     Helper methods for numeric operations.
    /// </summary>
    public static class NumberUtility
    {
        /// <summary>
        ///     Returns integer part of decimal value
        ///     <para></para>
        ///     <example>Example</example>
        ///     <para></para>
        ///     GetIntegerPart(12.999) returns 12
        ///     <para></para>
        ///     GetIntegerPart(-12.999) returns -12
        ///     <para></para>
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static decimal GetIntegerPart(this decimal value)
        {
            return decimal.Truncate(value);
        }

        /// <summary>
        ///     Gets fractional part as string
        ///     <para></para>
        ///     <example>Example</example>
        ///     <para></para>
        ///     GetFractionalPart(12.998000) returns "998"
        ///     <para></para>
        ///     GetFractionalPart(-12.998) returns "998"
        ///     <para></para>
        ///     GetFractionalPart(12.00998) returns "00998"
        ///     <para></para>
        ///     GetFractionalPart(12) returns null
        ///     <para></para>
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string GetFractionalPart(this decimal value)
        {
            value -= decimal.Truncate(value);
            value = Math.Abs(value);
            var text = value.ToString(new CultureInfo("en-US"));
            string result;
            if (text.Contains("."))
            {
                var length = text.Length;
                while (length-- > 0)
                {
                    if (text[length] != '0')
                    {
                        break;
                    }
                }
                if (length == 1)
                {
                    result = null;
                }
                else
                {
                    result = text.Substring(2, length - 1);
                }
            }
            else
            {
                result = null;
            }
            return result;
        }
    }
}