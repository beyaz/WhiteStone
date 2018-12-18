using System;
using System.Globalization;

namespace BOA.Jaml
{
        static class Extensions
        {
            public static Type GetType(this ITypeFinder finder, string fullTypeName)
            {
                var type = finder.Find(fullTypeName);
                if (type == null)
                {
                    throw Errors.TypeNotFound(fullTypeName);
                }
                return type;
            }

            /// <summary>
            ///     Removes value from start of str
            /// </summary>
            public static string RemoveFromStart(this string data, string value)
            {
                if (data == null)
                {
                    return null;
                }

                if (data.StartsWith(value, StringComparison.CurrentCulture))
                {
                    return data.Substring(value.Length, data.Length - value.Length);
                }

                return data;
            }

            /// <summary>
            ///     Removes value from end of str
            /// </summary>
            public static string RemoveFromEnd(this string data, string value)
            {
                if (data.EndsWith(value, StringComparison.CurrentCulture))
                {
                    return data.Substring(0, data.Length - value.Length);
                }

                return data;
            }

            /// <summary>
            ///     Returns a copy of this string converted to uppercase, using the casing rules of 'English' culture
            /// </summary>
            /// <param name="value"></param>
            /// <returns></returns>
            public static string ToUpperEN(this string value)
            {
                return value.ToUpper(new CultureInfo("en-US"));
            }

            internal static double ToDouble(this object value)
            {
                if (value is double)
                {
                    return (double) value;
                }

                return double.Parse(value.ToString(), CultureInfo.InvariantCulture);
            }



        }
    }
