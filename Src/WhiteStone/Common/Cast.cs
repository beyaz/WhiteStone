using System;
using System.Globalization;
using System.Threading;
using WhiteStone.Helpers;

namespace WhiteStone.Common
{
    /// <summary>
    ///     Utility methods for casting operations
    /// </summary>
    public static class Cast
    {
        /// <summary>
        ///     Casts value to 'TTargetType'
        /// </summary>
        /// <typeparam name="TTargetType">Target type</typeparam>
        public static TTargetType To<TTargetType>(object value, IFormatProvider provider)
        {
            if (value is TTargetType)
            {
                return (TTargetType) value;
            }

            var convertible = value as IConvertible;
            if (convertible != null)
            {
                return convertible.To<TTargetType>(provider);
            }

            if (value == null)
            {
                return default(TTargetType);
            }
            return DoCasting<TTargetType>(value);
        }

        /// <summary>
        ///     Casts value to 'TTargetType'
        /// </summary>
        /// <typeparam name="TTargetType">Target type</typeparam>
        public static TTargetType To<TTargetType>(object value)
        {
            return To<TTargetType>(value, CultureInfo.InvariantCulture);
        }


        static TargetType DoCasting<TargetType>(object value)
        {
            try
            {
                return (TargetType) value;
            }
            catch (Exception ex)
            {
                var message = string.Format(Thread.CurrentThread.CurrentCulture, "'{0}' not casted to '{1}' .Exception:'{2}'", value, typeof (TargetType), ex.Message);
                throw new InvalidCastException(message);
            }
        }
    }

    //public class DateParser
    //{
    //    public static string ToString(DateTime? date, string format, IFormatProvider formatProvider)
    //    {
    //        if (date.HasValue)
    //        {
    //            return date.Value.ToString(format, formatProvider);
    //        }
    //        return null;
    //    }

    //    public static DateTime ParseDate(string value)
    //    {
    //        if (value == null)
    //        {
    //            throw new ArgumentNullException("value");
    //        }

    //        var arr = value.Split(' ');

    //        if (arr.Length == 2)
    //        {
    //            var day = ParseDayPart(arr[0]);

    //            var array = arr[1].Split(':');

    //            // hh:mm:ss
    //            if (array.Length == 3)
    //            {
    //                // for supporting some sql date string values
    //                if (array[2].Contains("."))
    //                {
    //                    var arr2 = array[2].Split('.');

    //                    return new DateTime(day.Year, day.Month, day.Day, To<int>(array[0]), To<int>(array[1]), To<int>(arr2[0]), To<int>(arr2[1]));
    //                }
    //                return new DateTime(day.Year, day.Month, day.Day, To<int>(array[0]), To<int>(array[1]), To<int>(array[2]));
    //            }

    //            if (array.Length == 2)
    //            {
    //                return new DateTime(day.Year, day.Month, day.Day, To<int>(array[0]), To<int>(array[1]), 0);
    //            }

    //            throw CreateInvalidCastExceptionForDate(value);
    //        }

    //        return ParseDayPart(value);
    //    }

    //    static DateTime ParseDayPart(string value)
    //    {
    //        string[] arr;
    //        if (value.Contains("/"))
    //        {
    //            arr = value.Split('/');
    //        }
    //        else if (value.Contains("."))
    //        {
    //            arr = value.Split('.');
    //        }
    //        else
    //        {
    //            throw CreateInvalidCastExceptionForDate(value);
    //        }

    //        if (arr.Length != 3)
    //        {
    //            throw CreateInvalidCastExceptionForDate(value);
    //        }

    //        // 2015
    //        if (arr[2].Length == 4)
    //        {
    //            return new DateTime(To<int>(arr[2]), To<int>(arr[1]), To<int>(arr[0]));
    //        }

    //        // 2015
    //        if (arr[0].Length == 4)
    //        {
    //            return new DateTime(To<int>(arr[0]), To<int>(arr[1]), To<int>(arr[2]));
    //        }

    //        throw CreateInvalidCastExceptionForDate(value);
    //    }

    //    static InvalidCastException CreateInvalidCastExceptionForDate(string value)
    //    {
    //        return new InvalidCastException(value + " is not parsed to date.");
    //    }
    //}
}