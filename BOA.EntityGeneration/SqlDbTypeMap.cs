using System;
using System.Collections.Generic;
using System.Data;
using BOA.Common.Helpers;
using  static BOA.EntityGeneration.SqlDataType;

namespace BOA.EntityGeneration
{
    public class SqlDbTypeMap
    {
        #region Static Fields
        static readonly Dictionary<string, SqlDbType> DataTypeToSqlDbType = new Dictionary<string, SqlDbType>
        {
            {Int, SqlDbType.Int},
            {BigInt, SqlDbType.BigInt},
            {TinyInt, SqlDbType.TinyInt},
            {SmallInt, SqlDbType.SmallInt},
            {Float, SqlDbType.Float},
            {Real, SqlDbType.Real},
            {SmallMoney, SqlDbType.SmallMoney},
            {SqlDataType.DateTime, SqlDbType.DateTime},
            {Date, SqlDbType.Date},
            {SmallDateTime, SqlDbType.SmallDateTime},
            {NVarChar, SqlDbType.NVarChar},
            {UniqueIdentifier, SqlDbType.UniqueIdentifier},
            {VarBinary, SqlDbType.VarBinary},
            {Timestamp, SqlDbType.Timestamp},
            {Binary, SqlDbType.Binary},
            {Image, SqlDbType.Image},
            {NText, SqlDbType.NText},
            {Text, SqlDbType.Text},
            {Time, SqlDbType.Time},
            {Xml, SqlDbType.Xml}
        };

        static readonly Dictionary<string, string> DatabaseTypesToDotNetTypes = new Dictionary<string, string>
        {
            {TinyInt, DotNetTypeName.DotNetByte},
            {SmallInt, DotNetTypeName.DotNetInt16},
            {Int, DotNetTypeName.DotNetInt32},
            {BigInt, DotNetTypeName.DotNetInt64},
            {SqlDataType.DateTime, DotNetTypeName.DotNetDateTime},
            {Date, DotNetTypeName.DotNetDateTime},
            {SmallDateTime, DotNetTypeName.DotNetDateTime},
            {Bit, DotNetTypeName.DotNetBool},
            {UniqueIdentifier, DotNetTypeName.DotNetGuid},
            {Float, DotNetTypeName.DotNetSingle},
            {SmallMoney, DotNetTypeName.DotNetDecimal},
            {Real, DotNetTypeName.DotNetDouble},
            {NText, DotNetTypeName.DotNetStringName},
            {VarBinary, DotNetTypeName.DotNetByteArray},
            {Timestamp, DotNetTypeName.DotNetByteArray},
            {Binary, DotNetTypeName.DotNetByteArray},
            {Image, DotNetTypeName.DotNetObject},
            {Text, DotNetTypeName.DotNetStringName},
            {Time, DotNetTypeName.DotNetTimeSpan},
            {Xml, DotNetTypeName.DotNetStringName},
            {DECIMAL, DotNetTypeName.DotNetDecimal}
        };

        static readonly Dictionary<Type, SqlReaderMethods> SqlReaderMethodCache = new Dictionary<Type, SqlReaderMethods>
        {
            {typeof(string), SqlReaderMethods.GetStringValue},

            {typeof(decimal), SqlReaderMethods.GetDecimalValue},
            {typeof(decimal?), SqlReaderMethods.GetDecimalNullableValue},

            {typeof(bool), SqlReaderMethods.GetBooleanValue},
            {typeof(bool?), SqlReaderMethods.GetBooleanNullableValue},

            {typeof(DateTime), SqlReaderMethods.GetDateTimeValue},
            {typeof(DateTime?), SqlReaderMethods.GetDateTimeNullableValue},

            {typeof(long), SqlReaderMethods.GetInt64Value},
            {typeof(long?), SqlReaderMethods.GetInt64NullableValue},

            {typeof(Guid), SqlReaderMethods.GetGUIDValue},

            {typeof(int), SqlReaderMethods.GetInt32Value},
            {typeof(int?), SqlReaderMethods.GetInt32NullableValue},

            {typeof(short), SqlReaderMethods.GetInt16Value},
            {typeof(short?), SqlReaderMethods.GetInt16NullableValue},

            {typeof(byte), SqlReaderMethods.GetByteValue},
            {typeof(byte?), SqlReaderMethods.GetByteNullableValue},

            {typeof(double), SqlReaderMethods.GetDoubleValue},
            {typeof(double?), SqlReaderMethods.GetDoubleNullableValue},

            {typeof(float), SqlReaderMethods.GetSingleValue},
            {typeof(float?), SqlReaderMethods.GetSingleNullableValue},

            {typeof(byte[]), SqlReaderMethods.GetBinaryValue}
        };
        #endregion

        
        public virtual SqlDbType GetSqlDbType(string dataType)
        {
            
            if (DataTypeToSqlDbType.ContainsKey(dataType))
            {
                return DataTypeToSqlDbType[dataType];
            }

            if (dataType.StartsWith("VARCHAR",StringComparison.OrdinalIgnoreCase))
            {
                return SqlDbType.VarChar;
            }

            if (dataType.StartsWith("CHAR(",StringComparison.OrdinalIgnoreCase))
            {
                return SqlDbType.Char;
            }

            if (dataType.StartsWith("NVARCHAR",StringComparison.OrdinalIgnoreCase))
            {
                return SqlDbType.NVarChar;
            }

            if (dataType.StartsWith("NCHAR",StringComparison.OrdinalIgnoreCase))
            {
                return SqlDbType.NChar;
            }

            if (dataType.StartsWith("BIT",StringComparison.OrdinalIgnoreCase))
            {
                return SqlDbType.Bit;
            }

            if (dataType.StartsWith("NUMERIC",StringComparison.OrdinalIgnoreCase))
            {
                return SqlDbType.Decimal;
            }

            if (dataType.StartsWith("MONEY",StringComparison.OrdinalIgnoreCase))
            {
                return SqlDbType.Money;
            }

            throw new ArgumentException(dataType);
        }



        
        public virtual SqlReaderMethods GetSqlReaderMethodEnum(string dataType, bool isNullable)
        {
            if (dataType == Timestamp)
            {
                return SqlReaderMethods.GetTimeStampValue;
            }

            if (dataType == VarBinary ||
                dataType == Binary ||
                dataType == Image)
            {
                return SqlReaderMethods.GetBinaryValue;
            }

            if (dataType == Float)
            {
                return isNullable ? SqlReaderMethods.GetSingleNullableValue : SqlReaderMethods.GetSingleValue;
            }

            if (dataType == Real)
            {
                return isNullable ? SqlReaderMethods.GetDoubleNullableValue : SqlReaderMethods.GetDoubleValue;
            }

            if (dataType == TinyInt)
            {
                return isNullable ? SqlReaderMethods.GetByteNullableValue : SqlReaderMethods.GetByteValue;
            }

            if (dataType == SmallInt)
            {
                return isNullable ? SqlReaderMethods.GetInt16NullableValue : SqlReaderMethods.GetInt16Value;
            }

            if (dataType == Int)
            {
                return isNullable ? SqlReaderMethods.GetInt32NullableValue : SqlReaderMethods.GetInt32Value;
            }

            if (dataType == Time)
            {
                return SqlReaderMethods.GetTimeSpanValue;
            }

            if (dataType == UniqueIdentifier)
            {
                return SqlReaderMethods.GetGUIDValue;
            }

            if (dataType == BigInt)
            {
                return isNullable ? SqlReaderMethods.GetInt64NullableValue : SqlReaderMethods.GetInt64Value;
            }

            if (dataType == SqlDataType.DateTime ||
                dataType == SmallDateTime ||
                dataType == Date)
            {
                return isNullable ? SqlReaderMethods.GetDateTimeNullableValue : SqlReaderMethods.GetDateTimeValue;
            }

            if (dataType.StartsWith("VARCHAR",StringComparison.OrdinalIgnoreCase) ||
                dataType.StartsWith("CHAR(",StringComparison.OrdinalIgnoreCase) ||
                dataType.StartsWith("NCHAR",StringComparison.OrdinalIgnoreCase) ||
                dataType.StartsWith("NVARCHAR",StringComparison.OrdinalIgnoreCase) ||
                dataType == Text ||
                dataType == NText ||
                dataType == Xml)
            {
                return SqlReaderMethods.GetStringValue;
            }

            if (dataType.StartsWith(Bit,StringComparison.OrdinalIgnoreCase))
            {
                return isNullable ? SqlReaderMethods.GetBooleanNullableValue : SqlReaderMethods.GetBooleanValue;
            }

            if (dataType.StartsWith("NUMERIC",StringComparison.OrdinalIgnoreCase) || dataType.StartsWith("MONEY",StringComparison.OrdinalIgnoreCase) || dataType == SmallMoney)
            {
                return isNullable ? SqlReaderMethods.GetDecimalNullableValue : SqlReaderMethods.GetDecimalValue;
            }

            throw new ArgumentException(dataType);
        }



        public  string GetDotNetType(string dataType, bool isNullable)
        {
            dataType = dataType.ToUpperEN();

            if (DatabaseTypesToDotNetTypes.ContainsKey(dataType))
            {
                var dotNetType = DatabaseTypesToDotNetTypes[dataType];

                if (isNullable)
                {
                    return DotNetTypeName.GetDotNetNullableType(dotNetType);
                }

                return dotNetType;
            }

            if (dataType.StartsWith("VARCHAR", StringComparison.Ordinal) ||
                dataType.StartsWith("CHAR(", StringComparison.Ordinal) ||
                dataType.StartsWith("CHAR", StringComparison.Ordinal) ||
                dataType.StartsWith("NCHAR(", StringComparison.Ordinal) ||
                dataType.StartsWith("NVARCHAR(", StringComparison.Ordinal) |
                dataType.StartsWith("NVARCHAR", StringComparison.Ordinal))
            {
                return DotNetTypeName.DotNetStringName;
            }

            if (dataType.StartsWith("NUMERIC", StringComparison.Ordinal) || dataType.StartsWith("MONEY", StringComparison.Ordinal))
            {
                return isNullable ? DotNetTypeName.DotNetNullableDecimal : DotNetTypeName.DotNetDecimal;
            }

            throw new ArgumentException(dataType);
        }

    }
}