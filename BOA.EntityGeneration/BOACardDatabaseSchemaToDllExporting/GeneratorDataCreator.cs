using System;
using System.Linq;
using BOA.Common.Helpers;
using BOA.DatabaseAccess;
using BOA.EntityGeneration.DbModel;
using BOA.EntityGeneration.DbModel.SqlServerDataAccess;
using Ninject;

namespace BOA.EntityGeneration.BOACardDatabaseSchemaToDllExporting
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


        public string GetColumnDotnetType(string dbColumnName,string dotnetType,bool isNullable)
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
    public class GeneratorDataCreator
    {
        #region Public Properties
        [Inject]
        public IDatabase Database { get; set; }

        [Inject]
        public TableOverride TableOverride { get; set; }
        #endregion

        #region Public Methods
        public TableInfo Create(DbModel.TableInfo tableInfo)
        {
            var uniqueIndexIdentifiers = tableInfo.IndexInfoList.Where(x => !x.IsPrimaryKey && x.IsUnique).ToList();

            var nonUniqueIndexIdentifiers = tableInfo.IndexInfoList.Where(x => !x.IsPrimaryKey && !x.IsUnique).ToList();

            var isSupportGetAll = tableInfo.SchemaName == "PRM";

            var isSupportSelectByKey         = tableInfo.PrimaryKeyColumns.Any();
            var isSupportSelectByUniqueIndex = uniqueIndexIdentifiers.Any();
            var isSupportSelectByIndex       = nonUniqueIndexIdentifiers.Any();

            var sequenceName = "SEQ_" + tableInfo.TableName;

            var hasSequenceInDatabase = Database.HasSequenceLike(tableInfo.SchemaName, sequenceName);

            var hasSequence = false;
            if (hasSequenceInDatabase && tableInfo.PrimaryKeyColumns.Count == 1)
            {
                if (tableInfo.PrimaryKeyColumns[0].ColumnName == Names2.RECORD_ID)
                {
                    hasSequence = true;
                }
            }

            var data = JsonHelper.Deserialize<TableInfo>(JsonHelper.Serialize(tableInfo));

            data.UniqueIndexInfoList          = uniqueIndexIdentifiers;
            data.NonUniqueIndexInfoList       = nonUniqueIndexIdentifiers;
            data.IsSupportGetAll              = isSupportGetAll;
            data.IsSupportSelectByKey         = isSupportSelectByKey;
            data.IsSupportSelectByIndex       = isSupportSelectByIndex;
            data.IsSupportSelectByUniqueIndex = isSupportSelectByUniqueIndex;
            data.DatabaseEnumName             = tableInfo.CatalogName;
            data.SequenceName                 = hasSequence ? tableInfo.SchemaName + "." + sequenceName : null;

            TableOverride.Override(data);

            return data;
        }
        #endregion
    }
}