using System;
using System.Data;
using System.Linq;
using BOA.EntityGeneration.DbModel;
using TableInfo = BOA.EntityGeneration.BOACardDatabaseSchemaToDllExporting.Models.TableInfo;

namespace BOA.EntityGeneration.BOACardDatabaseSchemaToDllExporting.DataAccess
{
    /// <summary>
    ///     The table override
    /// </summary>
    public class TableOverride
    {
        #region Public Methods
        /// <summary>
        ///     Gets the type of the column dotnet.
        /// </summary>
        public string GetColumnDotnetType(string dbColumnName, string dotnetType, bool isNullable)
        {
            if (dbColumnName.EndsWith("_FLAG", StringComparison.OrdinalIgnoreCase))
            {
                if (isNullable)
                {
                    return DotNetTypeName.GetDotNetNullableType(DotNetTypeName.DotNetBool);
                }

                return DotNetTypeName.DotNetBool;
            }

            return dotnetType;
        }

        /// <summary>
        ///     Overrides the specified table information.
        /// </summary>
        public void Override(TableInfo tableInfo)
        {
            foreach (var item in tableInfo.Columns)
            {
                Reprocess(item);
            }

            if (tableInfo.Columns.Any(x => x.ColumnName.Equals("VALID_FLAG", StringComparison.OrdinalIgnoreCase) && x.SqlDbType == SqlDbType.Char))
            {
                tableInfo.ShouldGenerateSelectAllByValidFlagMethodInBusinessClass = true;
            }
        }

        /// <summary>
        ///     Reprocesses the specified item.
        /// </summary>
        public void Reprocess(ColumnInfo item)
        {
            item.DotNetType = GetColumnDotnetType(item.ColumnName, item.DotNetType, item.IsNullable);

            if (item.ColumnName.EndsWith("_FLAG", StringComparison.OrdinalIgnoreCase))
            {
                item.SqlReaderMethod = item.IsNullable ? SqlReaderMethods.GetBooleanNullableValue : SqlReaderMethods.GetBooleanValue;
            }
        }
        #endregion
    }
}