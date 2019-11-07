using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using BOA.DatabaseAccess;
using BOA.EntityGeneration.BOACardDatabaseSchemaToDllExporting.Models.Impl;
using BOA.EntityGeneration.DbModel;
using BOA.EntityGeneration.DbModel.Interfaces;
using BOA.EntityGeneration.DbModel.Types;
using Ninject;
using WhiteStone.Helpers;
using ITableInfo = BOA.EntityGeneration.BOACardDatabaseSchemaToDllExporting.Models.Interfaces.ITableInfo;
using TableInfo = BOA.EntityGeneration.BOACardDatabaseSchemaToDllExporting.Models.Impl.TableInfo;

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

     
        #endregion

        static IColumnInfo ReEvaluate(IColumnInfo columnInfo)
        {
            var item = ColumnInfo.CreateFrom(columnInfo);

            if (item.ColumnName.EndsWith("_FLAG", StringComparison.OrdinalIgnoreCase))
            {
                if (item.IsNullable)
                {
                    item.DotNetType      = DotNetTypeName.GetDotNetNullableType(DotNetTypeName.DotNetBool);
                    item.SqlReaderMethod = SqlReaderMethods.GetBooleanNullableValue;
                }
                else
                {
                    item.DotNetType      = DotNetTypeName.DotNetBool;
                    item.SqlReaderMethod = SqlReaderMethods.GetBooleanValue;
                }
            }

            return item;
        }
        #region Public Methods
        /// <summary>
        ///     Creates the specified table information.
        /// </summary>
        public ITableInfo Create(DbModel.Interfaces.ITableInfo tableInfo)
        {
            var uniqueIndexIdentifiers = tableInfo.IndexInfoList.Where(x => !x.IsPrimaryKey && x.IsUnique).ToList();

            var nonUniqueIndexIdentifiers = tableInfo.IndexInfoList.Where(x => !x.IsPrimaryKey && !x.IsUnique).ToList();

            var isSupportSelectByKey         = tableInfo.PrimaryKeyColumns.Any();
            var isSupportSelectByUniqueIndex = uniqueIndexIdentifiers.Any();
            var isSupportSelectByIndex       = nonUniqueIndexIdentifiers.Any();



            var SequenceList = new List<SequenceInfo>();
            if (Config.SqlSequenceInformationOfTable == null)
            {
                SequenceList = new List<SequenceInfo>();
            }
            else
            {
                Database.CommandText  = Config.SqlSequenceInformationOfTable;
                Database["schema"]    = tableInfo.SchemaName;
                Database["tableName"] = tableInfo.TableName;

                SequenceList = Database.ExecuteReader().ToList<SequenceInfo>().Where(x => tableInfo.Columns.Any(c => c.ColumnName == x.TargetColumnName)).ToList();
            }

            var data = new TableInfo
            {
                CatalogName                  = tableInfo.CatalogName,
                Columns                      = tableInfo.Columns.ToList().ConvertAll(ReEvaluate),
                HasIdentityColumn            = tableInfo.HasIdentityColumn,
                IdentityColumn               = tableInfo.IdentityColumn,
                IndexInfoList                = tableInfo.IndexInfoList,
                PrimaryKeyColumns            = tableInfo.PrimaryKeyColumns,
                SchemaName                   = tableInfo.SchemaName,
                TableName                    = tableInfo.TableName,
                UniqueIndexInfoList          = uniqueIndexIdentifiers,
                NonUniqueIndexInfoList       = nonUniqueIndexIdentifiers,
                IsSupportSelectByKey         = isSupportSelectByKey,
                IsSupportSelectByIndex       = isSupportSelectByIndex,
                IsSupportSelectByUniqueIndex = isSupportSelectByUniqueIndex,
                DatabaseEnumName             = Config.DatabaseEnumName,
                SequenceList                 = SequenceList,
            };


           

            if (tableInfo.Columns.Any(x => x.ColumnName.Equals("VALID_FLAG", StringComparison.OrdinalIgnoreCase) && x.SqlDbType == SqlDbType.Char))
            {
                data.ShouldGenerateSelectAllByValidFlagMethodInBusinessClass = true;
            }

            return data;
        }
        #endregion
    }
}