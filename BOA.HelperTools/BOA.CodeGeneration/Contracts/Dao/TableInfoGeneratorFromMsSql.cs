using System;
using System.Collections.Generic;
using System.Data;
using BOA.CodeGeneration.Common;
using BOA.Common.Helpers;
using ColumnInfo = BOA.CodeGeneration.Contracts.ColumnInfo;

namespace BOA.CodeGeneration.Services
{
    public static class TableInfoGeneratorFromMsSql
    {
        #region Public Methods
        
        public static IReadOnlyList<Contracts.ColumnInfo> CreateColumns(DataTable table)
        {
            var items = new List<Contracts.ColumnInfo>();

            for (var i = 0; i < table.Rows.Count; i++)
            {
                var row = table.Rows[i];

                items.Add(CreateColumn(row));
            }

            return items;
        }
        #endregion

        #region Methods
        static Contracts.ColumnInfo CreateColumn(DataRow row)
        {
            var columnName = row["COLUMN_NAME"].ToString();

            #region  var dataType = row["DATA_TYPE"
            var dataType = row["DATA_TYPE"].ToString().ToUpperEN();
            if (dataType == "VARCHAR" || dataType == "NVARCHAR" || dataType == "CHAR" || dataType == "NCHAR")
            {
                var characterLength = row["CHARACTER_MAXIMUM_LENGTH"].ToString();
                if (characterLength == "-1")
                {
                    characterLength = "MAX";
                }

                dataType = dataType + "(" + characterLength + ")";
            }
            else if (dataType == SqlDataType.SmallDateTime ||
                     dataType == SqlDataType.TinyInt ||
                     dataType == SqlDataType.DateTime ||
                     dataType == SqlDataType.Date ||
                     dataType == SqlDataType.SmallInt ||
                     dataType == SqlDataType.Int ||
                     dataType == SqlDataType.Bit ||
                     dataType == SqlDataType.BigInt ||
                     dataType == SqlDataType.Money ||
                     dataType == SqlDataType.UniqueIdentifier ||
                     dataType == SqlDataType.Float ||
                     dataType == SqlDataType.SmallMoney ||
                     dataType == SqlDataType.Image ||
                     dataType == SqlDataType.VarBinary ||
                     dataType == SqlDataType.Binary ||
                     dataType == SqlDataType.Real ||
                     dataType == SqlDataType.NText ||
                     dataType == SqlDataType.Text ||
                     dataType == SqlDataType.Time ||
                     dataType == SqlDataType.Xml ||
                     dataType == SqlDataType.Timestamp)
            {
            }
            else if (dataType == "NUMERIC")
            {
                var numericPrecision = row["NUMERIC_PRECISION"] + "";
                var numericScale     = row["NUMERIC_SCALE"] + "";

                if (numericPrecision.IsNullOrWhiteSpace())
                {
                    throw new InvalidOperationException(nameof(numericPrecision))
                        .AddData(nameof(columnName), columnName);
                }

                if (numericScale.IsNullOrWhiteSpace())
                {
                    throw new InvalidOperationException(nameof(numericScale))
                        .AddData(nameof(columnName), columnName);
                }

                dataType = dataType + "(" + numericPrecision + "," + numericScale + ")";
            }
            else if (dataType == "DECIMAL")
            {
                var numericPrecision = row["NUMERIC_PRECISION"] + "";
                var numericScale     = row["NUMERIC_SCALE"] + "";

                if (numericPrecision.IsNullOrWhiteSpace())
                {
                    throw new InvalidOperationException(nameof(numericPrecision))
                        .AddData(nameof(columnName), columnName);
                }

                if (numericScale.IsNullOrWhiteSpace())
                {
                    throw new InvalidOperationException(nameof(numericScale))
                        .AddData(nameof(columnName), columnName);
                }

                dataType = "NUMERIC" + "(" + numericPrecision + "," + numericScale + ")";
            }
            else
            {
                throw new ArgumentException(dataType);
            }
            #endregion

            var isIdentity = row["IsIdentity"] + "" == "True";

            var isNullable = row["IS_NULLABLE"].ToString() == "YES";

            return new ColumnInfo
            {
                ColumnName = columnName,
                DataType = dataType,
                IsIdentity = isIdentity,
                IsNullable = isNullable,
                SqlDatabaseTypeName = SqlDataType.GetSqlDbType(dataType),
                DotNetType          = SqlDataType.GetDotNetType(dataType, isNullable),
                SqlReaderMethod     = SqlDataType.GetSqlReaderMethod(dataType, isNullable)
            };
        }
        #endregion
    }
}