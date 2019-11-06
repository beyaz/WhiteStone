using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using BOA.Common.Helpers;
using BOA.DatabaseAccess;
using BOA.EntityGeneration.BOACardDatabaseSchemaToDllExporting.Models;
using Ninject;
using WhiteStone.Helpers;

namespace BOA.EntityGeneration.BOACardDatabaseSchemaToDllExporting.DataAccess
{
    /// <summary>
    ///     The generator data creator
    /// </summary>
    public class GeneratorDataCreator
    {
        #region Public Properties
        /// <summary>
        ///     Gets or sets the configuration.
        /// </summary>
        [Inject]
        public Config Config { get; set; }

        /// <summary>
        ///     Gets or sets the database.
        /// </summary>
        [Inject]
        public IDatabase Database { get; set; }

        /// <summary>
        ///     Gets or sets the table override.
        /// </summary>
        [Inject]
        public TableOverride TableOverride { get; set; }
        #endregion

        #region Public Methods
        /// <summary>
        ///     Creates the specified table information.
        /// </summary>
        public ITableInfo Create(DbModel.ITableInfo tableInfo)
        {
            var uniqueIndexIdentifiers = tableInfo.IndexInfoList.Where(x => !x.IsPrimaryKey && x.IsUnique).ToList();

            var nonUniqueIndexIdentifiers = tableInfo.IndexInfoList.Where(x => !x.IsPrimaryKey && !x.IsUnique).ToList();

            var isSupportSelectByKey         = tableInfo.PrimaryKeyColumns.Any();
            var isSupportSelectByUniqueIndex = uniqueIndexIdentifiers.Any();
            var isSupportSelectByIndex       = nonUniqueIndexIdentifiers.Any();

            var data = JsonHelper.Deserialize<TableInfo>(JsonHelper.Serialize(tableInfo));

            data.UniqueIndexInfoList          = uniqueIndexIdentifiers;
            data.NonUniqueIndexInfoList       = nonUniqueIndexIdentifiers;
            data.IsSupportSelectByKey         = isSupportSelectByKey;
            data.IsSupportSelectByIndex       = isSupportSelectByIndex;
            data.IsSupportSelectByUniqueIndex = isSupportSelectByUniqueIndex;
            data.DatabaseEnumName             = Config.DatabaseEnumName;

            if (Config.SqlSequenceInformationOfTable == null)
            {
                data.SequenceList = new List<SequenceInfo>();
            }
            else
            {
                Database.CommandText  = Config.SqlSequenceInformationOfTable;
                Database["schema"]    = tableInfo.SchemaName;
                Database["tableName"] = tableInfo.TableName;

                data.SequenceList = Database.ExecuteReader().ToList<SequenceInfo>().Where(x => tableInfo.Columns.Any(c => c.ColumnName == x.TargetColumnName)).ToList();
            }

            foreach (var item in data.Columns)
            {
                if (item.ColumnName.EndsWith("_FLAG", StringComparison.OrdinalIgnoreCase))
                {
                    if (item.IsNullable)
                    {
                        item.DotNetType = DotNetTypeName.GetDotNetNullableType(DotNetTypeName.DotNetBool);
                        item.SqlReaderMethod = SqlReaderMethods.GetBooleanNullableValue;
                    }
                    else
                    {
                        item.DotNetType = DotNetTypeName.DotNetBool;
                        item.SqlReaderMethod = SqlReaderMethods.GetBooleanValue;
                    }
                }
               
            }

            if (data.Columns.Any(x => x.ColumnName.Equals("VALID_FLAG", StringComparison.OrdinalIgnoreCase) && x.SqlDbType == SqlDbType.Char))
            {
                data.ShouldGenerateSelectAllByValidFlagMethodInBusinessClass = true;
            }


            return data;
        }
        #endregion
    }
}