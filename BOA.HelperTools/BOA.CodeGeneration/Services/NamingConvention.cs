using System.Linq;
using BOA.CodeGeneration.Model;
using BOA.Common.Helpers;
using BOA.EntityGeneration.DbModel;
using BOA.EntityGeneration.DbModel.SqlServerDataAccess;

namespace BOA.CodeGeneration.Services
{
    public class NamingConvention
    {
        #region Public Properties
        public WriterContext Context { private get; set; }
        #endregion

        #region Properties
        string TableNameInDatabase
        {
            get
            {
                if (Context.Config.HasOriginalTable)
                {
                    return Context.Config.TableName + "_ORJ";
                }

                return Context.Config.TableName;
            }
        }
        #endregion

        #region Public Methods
        public void InitializeNames()
        {
            var tableConfig = Context.Config;
            var tableName   = tableConfig.TableName;

            var naming = new NamingModel
            {
                DatabaseTableFullPath                       = tableConfig.DatabaseTableFullPath,
                NamespaceName                               = tableConfig.NamespaceName,
                DatabaseEnumName                            = tableConfig.DatabaseEnumName,
                NameOfSqlProceduresWillBeRunCatalogName     = tableConfig.DatabaseEnumName.ToUpperEN(),
                SchemaName                                  = tableConfig.SchemaName,
                ContractName                                = tableConfig.ContractName ?? tableName + "Contract",
                OutputFileNameContract                      = tableConfig.ContractName ?? tableName + "Contract",
                ClassNameOfBusiness                         = tableConfig.ClassNameOfBusiness ?? tableName,
                OutputFileNameBusiness                      = tableConfig.ClassNameOfBusiness ?? tableName,
                NameOfSqlProcedureInsert                    = "ins_" + tableName,
                NameOfSqlProcedureUpdate                    = "upd_" + tableName,
                NameOfSqlProcedureDelete                    = "del_" + tableName,
                NameOfSqlProcedureSelectByKey               = "sel_" + tableName + "ByKey",
                NameOfSqlProcedureSelectByKeyList           = "sel_" + tableName + "ByKeyList",
                NameOfSqlProcedureSelectByValueListFormat   = "sel_" + tableName + "By{0}List",
                NameOfSqlProcedureInsertWithSchema          = tableConfig.SchemaName + "." + "ins_" + tableName,
                NameOfSqlProcedureUpdateWithSchema          = tableConfig.SchemaName + "." + "upd_" + tableName,
                NameOfSqlProcedureDeleteWithSchema          = tableConfig.SchemaName + "." + "del_" + tableName,
                NameOfSqlProcedureSelectByKeyWithSchema     = tableConfig.SchemaName + "." + "sel_" + tableName + "ByKey",
                NameOfSqlProcedureSelectByKeyListWithSchema = tableConfig.SchemaName + "." + "sel_" + tableName + "ByKeyList",
                NameOfDotNetMethodSelectByKey               = "SelectByKey",
                NameOfDotNetMethodSelectByKeyList           = "SelectByKeyList",
                NameOfDotNetMethodSelectByValueListFormat   = "SelectBy{0}List",
                NameOfDotNetMethodInsert                    = "Insert",
                NameOfDotNetMethodUpdate                    = "Update",
                NameOfDotNetMethodDelete                    = "Delete",
                NameOfDotNetMethodSelectByColumns           = "SelectByColumns",
                OutputFileNameOrchestration                 = tableName,
                ClassNameOfOrchestration                    = tableName,
                NamespaceNameOfBusinessClass                = "BOA.Business." + tableConfig.NamespaceName
            };

            tableConfig.AfterNamingModelCreated?.Invoke(naming);

            if (tableName != null)
            {
                Context.Table = InitializeTableInformation();
            }
            else
            {
                Context.Table = new TableInfo();
            }

            Context.Naming = naming;
        }
        #endregion

        #region Methods
        TableInfo InitializeTableInformation()
        {
            var tableConfig = Context.Config;

            using (var database = tableConfig.GetDatabase())
            {
                var dao = new TableInfoDao
                {
                    Database = database,
                    IndexInfoAccess = new IndexInfoAccess
                    {
                        Database = database
                    }
                };

                var tableInfo = dao.GetInfo(tableConfig.DatabaseName, tableConfig.SchemaName, TableNameInDatabase);

                if (tableConfig.ExcludeColumns.HasValue())
                {
                    var columnNames = tableConfig.ExcludeColumns.Split(',');

                    tableInfo.Columns = tableInfo.Columns.Where(x => !columnNames.Contains(x.ColumnName)).ToList();
                }

                return tableInfo;
            }
        }
        #endregion
    }
}