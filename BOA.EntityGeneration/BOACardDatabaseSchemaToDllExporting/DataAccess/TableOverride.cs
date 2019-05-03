using System;
using BOA.EntityGeneration.DbModel;

namespace BOA.EntityGeneration.BOACardDatabaseSchemaToDllExporting.DataAccess
{
    public class TableOverride
    {
        public void Override(DbModel.TableInfo data)
        {
            foreach (var item in data.Columns)
            {
                Reprocess(item);
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


        public string GetColumnDotnetType(string dbColumnName, string dotnetType, bool isNullable)
        {
            if (dbColumnName.EndsWith("_FLAG",StringComparison.OrdinalIgnoreCase))
            {
                if (isNullable)
                {
                    return DotNetTypeName.GetDotNetNullableType(DotNetTypeName.DotNetBool);
                }

                return  DotNetTypeName.DotNetBool;
                
            }

            return dotnetType;
        }



    }
}