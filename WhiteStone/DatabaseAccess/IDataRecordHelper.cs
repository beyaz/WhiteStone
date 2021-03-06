﻿using System;
using System.Data;

namespace BOA.DatabaseAccess
{
    /// <summary>
    ///     The data reader helper
    /// </summary>
    public static class IDataRecordHelper
    {
        #region Public Methods
        /// <summary>
        ///     Gets the binary value.
        /// </summary>
        public static byte[] GetBinaryValue(this IDataRecord reader, string columnName)
        {
            var value = reader[columnName];
            if (value == DBNull.Value)
            {
                return null;
            }

            return (byte[]) value;
        }

        /// <summary>
        ///     Gets the boolean nullable value.
        /// </summary>
        public static bool? GetBooleanNullableValue(this IDataRecord reader, string columnName)
        {
            var value = reader[columnName];
            if (value == DBNull.Value)
            {
                return null;
            }

            return Convert.ToBoolean(value);
        }

        /// <summary>
        ///     Gets the boolean nullable value from character.
        /// </summary>
        public static bool? GetBooleanNullableValueFromChar(this IDataRecord reader, string columnName)
        {
            return GetInt32Value(reader, columnName) == 1;
        }

        /// <summary>
        ///     Gets the boolean value.
        /// </summary>
        public static bool GetBooleanValue(this IDataRecord reader, string columnName)
        {
            return reader.GetBooleanNullableValue(columnName).GetValueOrDefault();
        }

        /// <summary>
        ///     Gets the boolean value from character.
        /// </summary>
        public static bool GetBooleanValueFromChar(this IDataRecord reader, string columnName)
        {
            return GetInt32Value(reader, columnName) == 1;
        }

        /// <summary>
        ///     Gets the byte nullable value.
        /// </summary>
        public static byte? GetByteNullableValue(this IDataRecord reader, string columnName)
        {
            var value = reader[columnName];
            if (value == DBNull.Value)
            {
                return null;
            }

            return Convert.ToByte(value);
        }

        /// <summary>
        ///     Gets the byte value.
        /// </summary>
        public static byte GetByteValue(this IDataRecord reader, string columnName)
        {
            return reader.GetByteNullableValue(columnName).GetValueOrDefault();
        }

        /// <summary>
        ///     Gets the date time nullable value.
        /// </summary>
        public static DateTime? GetDateTimeNullableValue(this IDataRecord reader, string columnName)
        {
            var value = reader[columnName];
            if (value == DBNull.Value)
            {
                return null;
            }

            return Convert.ToDateTime(value);
        }

        /// <summary>
        ///     Gets the date time value.
        /// </summary>
        public static DateTime GetDateTimeValue(this IDataRecord reader, string columnName)
        {
            return reader.GetDateTimeNullableValue(columnName).GetValueOrDefault();
        }

        /// <summary>
        ///     Gets the decimal nullable value.
        /// </summary>
        public static decimal? GetDecimalNullableValue(this IDataRecord reader, string columnName)
        {
            var value = reader[columnName];
            if (value == DBNull.Value)
            {
                return null;
            }

            return Convert.ToDecimal(value);
        }

        /// <summary>
        ///     Gets the decimal value.
        /// </summary>
        public static decimal GetDecimalValue(this IDataRecord reader, string columnName)
        {
            return reader.GetDecimalNullableValue(columnName).GetValueOrDefault();
        }

        /// <summary>
        ///     Gets the double nullable value.
        /// </summary>
        public static double? GetDoubleNullableValue(this IDataRecord reader, string columnName)
        {
            var value = reader[columnName];
            if (value == DBNull.Value)
            {
                return null;
            }

            return Convert.ToDouble(value);
        }

        /// <summary>
        ///     Gets the double value.
        /// </summary>
        public static double GetDoubleValue(this IDataRecord reader, string columnName)
        {
            return reader.GetDoubleNullableValue(columnName).GetValueOrDefault();
        }

        /// <summary>
        ///     Gets the GUID value.
        /// </summary>
        public static Guid GetGuidValue(this IDataRecord reader, string columnName)
        {
            var value = reader[columnName];
            if (value == DBNull.Value)
            {
                return Guid.Empty;
            }

            return (Guid) value;
        }

        /// <summary>
        ///     Gets the int16 nullable value.
        /// </summary>
        public static short? GetInt16NullableValue(this IDataRecord reader, string columnName)
        {
            var value = reader[columnName];
            if (value == DBNull.Value)
            {
                return null;
            }

            return Convert.ToInt16(value);
        }

        /// <summary>
        ///     Gets the int16 value.
        /// </summary>
        public static short GetInt16Value(this IDataRecord reader, string columnName)
        {
            return reader.GetInt16NullableValue(columnName).GetValueOrDefault();
        }

        /// <summary>
        ///     Gets the int32 nullable value.
        /// </summary>
        public static int? GetInt32NullableValue(this IDataRecord reader, string columnName)
        {
            var value = reader[columnName];
            if (value == DBNull.Value)
            {
                return null;
            }

            return Convert.ToInt32(value);
        }

        /// <summary>
        ///     Gets the int32 value.
        /// </summary>
        public static int GetInt32Value(this IDataRecord reader, string columnName)
        {
            return reader.GetInt32NullableValue(columnName).GetValueOrDefault();
        }

        /// <summary>
        ///     Gets the int64 nullable value.
        /// </summary>
        public static long? GetInt64NullableValue(this IDataRecord reader, string columnName)
        {
            var value = reader[columnName];
            if (value == DBNull.Value)
            {
                return null;
            }

            return Convert.ToInt64(value);
        }

        /// <summary>
        ///     Gets the int64 value.
        /// </summary>
        public static long GetInt64Value(this IDataRecord reader, string columnName)
        {
            return reader.GetInt64NullableValue(columnName).GetValueOrDefault();
        }

        /// <summary>
        ///     Gets the s byte nullable value.
        /// </summary>
        public static sbyte? GetSByteNullableValue(this IDataRecord reader, string columnName)
        {
            var value = reader[columnName];
            if (value == DBNull.Value)
            {
                return null;
            }

            return Convert.ToSByte(value);
        }

        /// <summary>
        ///     Gets the s byte value.
        /// </summary>
        public static sbyte GetSByteValue(this IDataRecord reader, string columnName)
        {
            return reader.GetSByteNullableValue(columnName).GetValueOrDefault();
        }

        /// <summary>
        ///     Gets the single nullable value.
        /// </summary>
        public static float? GetSingleNullableValue(this IDataRecord reader, string columnName)
        {
            var value = reader[columnName];
            if (value == DBNull.Value)
            {
                return null;
            }

            return Convert.ToSingle(value);
        }

        /// <summary>
        ///     Gets the single value.
        /// </summary>
        public static float GetSingleValue(this IDataRecord reader, string columnName)
        {
            return reader.GetSingleNullableValue(columnName).GetValueOrDefault();
        }

        /// <summary>
        ///     Gets the string value.
        /// </summary>
        public static string GetStringValue(this IDataRecord reader, string columnName)
        {
            return reader.GetStringValue(columnName, false);
        }

        /// <summary>
        ///     Gets the string value.
        /// </summary>
        public static string GetStringValue(this IDataRecord reader, string columnName, bool trim)
        {
            var value = reader[columnName];
            if (value == DBNull.Value)
            {
                return null;
            }

            if (trim)
            {
                return Convert.ToString(value).Trim();
            }

            return Convert.ToString(value);
        }

        /// <summary>
        ///     Gets the time span value.
        /// </summary>
        public static TimeSpan GetTimeSpanValue(this IDataRecord reader, string columnName)
        {
            var value = reader[columnName];
            if (value == DBNull.Value)
            {
                return default(TimeSpan);
            }

            return (TimeSpan) value;
        }
        #endregion
    }
}