using System;
using System.Collections.Generic;
using BOA.Common.Helpers;

namespace BOA.EntityGeneration
{
    public static class SqlDataType
    {
        #region Static Fields
        static readonly Dictionary<string, string> DataTypeToSqlDbType = new Dictionary<string, string>
        {
            {Int, "Int"},
            {BigInt, "BigInt"},
            {TinyInt, "TinyInt"},
            {SmallInt, "SmallInt"},
            {Float, "Float"},
            {Real, "Real"},
            {SmallMoney, "SmallMoney"},
            {DateTime, "DateTime"},
            {Date, "Date"},
            {SmallDateTime, "SmallDateTime"},
            {NVarChar, "NVarChar"},
            {UniqueIdentifier, "UniqueIdentifier"},
            {VarBinary, "VarBinary"},
            {Timestamp, "Timestamp"},
            {Binary, "Binary"},
            {Image, "Image"},
            {NText, "NText"},
            {Text, "Text"},
            {Time, "Time"},
            {Xml, "Xml"}
        };

        static readonly Dictionary<string, string> DatabaseTypesToDotNetTypes = new Dictionary<string, string>
        {
            {TinyInt, DotNetTypeName.DotNetByte},
            {SmallInt, DotNetTypeName.DotNetInt16},
            {Int, DotNetTypeName.DotNetInt32},
            {BigInt, DotNetTypeName.DotNetInt64},
            {DateTime, DotNetTypeName.DotNetDateTime},
            {Date, DotNetTypeName.DotNetDateTime},
            {SmallDateTime, DotNetTypeName.DotNetDateTime},
            {Bit, DotNetTypeName.DotNetBool},
            {UniqueIdentifier, DotNetTypeName.DotNetGuid},
            {Float, "float"},
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

        #region Public Properties
        public static string BigInt => "BIGINT";

        public static string Binary => "BINARY";

        public static string Bit => "BIT";

        public static string Date => "DATE";

        public static string DateTime => "DATETIME";

        public static string DECIMAL => "DECIMAL";

        public static string Float => "FLOAT";

        public static string Image => "IMAGE";

        public static string Int => "INT";

        public static string Money => "MONEY";

        public static string NText => "NTEXT";

        public static string NVarChar => "NVARCHAR";

        public static string Real => "REAL";

        public static string SmallDateTime => "SMALLDATETIME";

        public static string SmallInt => "SMALLINT";

        public static string SmallMoney => "SMALLMONEY";

        public static string Text => "TEXT";

        public static string Time => "TIME";

        public static string Timestamp => "TIMESTAMP";

        public static string TinyInt => "TINYINT";

        public static string UniqueIdentifier => "UNIQUEIDENTIFIER";

        public static string VarBinary => "VARBINARY";

        public static string VARCHAR => "VARCHAR";

        public static string Xml => "XML";
        #endregion

        #region Public Methods
        public static string GetDotNetNullableType(string dotNetType)
        {
            if (dotNetType == null)
            {
                throw new ArgumentNullException(nameof(dotNetType));
            }

            if (dotNetType == DotNetTypeName.DotNetByteArray ||
                dotNetType == DotNetTypeName.DotNetStringName ||
                dotNetType == DotNetTypeName.DotNetObject ||
                dotNetType.EndsWith("?"))
            {
                return dotNetType;
            }

            return dotNetType += "?";
        }

        public static string GetDotNetType(string dataType, bool isNullable)
        {
            dataType = dataType.ToUpperEN();

            if (DatabaseTypesToDotNetTypes.ContainsKey(dataType))
            {
                var dotNetType = DatabaseTypesToDotNetTypes[dataType];

                if (isNullable)
                {
                    return GetDotNetNullableType(dotNetType);
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

            if (dataType.IsStartsWith("NUMERIC") || dataType.IsStartsWith("MONEY"))
            {
                return isNullable ? DotNetTypeName.DotNetNullableDecimal : DotNetTypeName.DotNetDecimal;
            }

            throw new ArgumentException(dataType);
        }

        public static string GetSqlDbType(string dataType)
        {
            if (DataTypeToSqlDbType.ContainsKey(dataType))
            {
                return DataTypeToSqlDbType[dataType];
            }

            if (dataType.IsStartsWith("VARCHAR"))
            {
                return "VarChar";
            }

            if (dataType.IsStartsWith("CHAR("))
            {
                return "Char";
            }

            if (dataType.IsStartsWith("NVARCHAR"))
            {
                return "NVarChar";
            }

            if (dataType.IsStartsWith("NCHAR"))
            {
                return "NChar";
            }

            if (dataType.IsStartsWith("BIT"))
            {
                return "Bit";
            }

            if (dataType.IsStartsWith("NUMERIC"))
            {
                return "Decimal";
            }

            if (dataType.IsStartsWith("MONEY"))
            {
                return "Money";
            }

            throw new ArgumentException(dataType);
        }

        public static string GetSqlReaderMethod(string dataType, bool isNullable)
        {
            return GetSqlReaderMethodEnum(dataType, isNullable).ToString();
        }
        #endregion

        #region Methods
        internal static SqlReaderMethods GetSqlReaderMethod(Type t)
        {
            return SqlReaderMethodCache[t];
        }

        static SqlReaderMethods GetSqlReaderMethodEnum(string dataType, bool isNullable)
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

            if (dataType == DateTime ||
                dataType == SmallDateTime ||
                dataType == Date)
            {
                return isNullable ? SqlReaderMethods.GetDateTimeNullableValue : SqlReaderMethods.GetDateTimeValue;
            }

            if (dataType.IsStartsWith("VARCHAR") ||
                dataType.IsStartsWith("CHAR(") ||
                dataType.IsStartsWith("NCHAR") ||
                dataType.IsStartsWith("NVARCHAR") ||
                dataType == Text ||
                dataType == NText ||
                dataType == Xml)
            {
                return SqlReaderMethods.GetStringValue;
            }

            if (dataType.IsStartsWith(Bit))
            {
                return isNullable ? SqlReaderMethods.GetBooleanNullableValue : SqlReaderMethods.GetBooleanValue;
            }

            if (dataType.IsStartsWith("NUMERIC") || dataType.IsStartsWith("MONEY") || dataType == SmallMoney)
            {
                return isNullable ? SqlReaderMethods.GetDecimalNullableValue : SqlReaderMethods.GetDecimalValue;
            }

            throw new ArgumentException(dataType);
        }
        #endregion
    }
}