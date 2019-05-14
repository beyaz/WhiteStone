using System;
using System.Data;
using System.Linq;
using BOA.EntityGeneration.DbModel;
using TableInfo = BOA.EntityGeneration.BOACardDatabaseSchemaToDllExporting.Models.TableInfo;

namespace BOA.EntityGeneration.BOACardDatabaseSchemaToDllExporting.DataAccess
{
    public class TableOverride
    {
        #region Public Methods
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