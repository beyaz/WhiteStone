using BOA.DataFlow;
using BOA.EntityGeneration.BOACardDatabaseSchemaToDllExporting.Util;
using BOA.EntityGeneration.DataFlow;
using static BOA.EntityGeneration.DataFlow.Data;

namespace BOA.EntityGeneration.BOACardDatabaseSchemaToDllExporting.ClassWriters
{
    public static class Naming
    {
        #region Table
        public static void PushNamesRelatedWithTable(IDataContext context)
        {
            var tableInfo = context.Get(TableInfo);
            var config    = context.Get(Data.Config);


            var typeContractName = $"{tableInfo.TableName.ToContractName()}Contract";
            if (typeContractName == "TransactionLogContract" ||
                typeContractName == "BoaUserContract") // resolve conflig
            {
                typeContractName = $"{NamingHelper.GetTypeClassNamespace(tableInfo.SchemaName,config)}.{typeContractName}";
            }

            context.Add(TableEntityClassNameForMethodParametersInRepositoryFiles,typeContractName);
            context.Add(RepositoryClassName, tableInfo.TableName.ToContractName());
        }

        public static void RemoveNamesRelatedWithTable(IDataContext context)
        {
            context.Remove(TableEntityClassNameForMethodParametersInRepositoryFiles);
            context.Remove(RepositoryClassName);
        }
        #endregion

        #region Table
        public static void PushNamesRelatedWithSchema(IDataContext context)
        {
            var config    = context.Get(Data.Config);

            context.Add(BusinessClassNamespace,NamingHelper.GetBusinessClassNamespace(context.Get(SchemaName),config));
        }

        public static void RemoveNamesRelatedWithSchema(IDataContext context)
        {
            context.Remove(BusinessClassNamespace);
        }
        #endregion
    }
}