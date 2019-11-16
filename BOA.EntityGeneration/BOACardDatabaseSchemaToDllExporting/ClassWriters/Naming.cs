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
            var config    = context.Get(Config);


            var typeContractName = $"{tableInfo.TableName.ToContractName()}Contract";
            if (typeContractName == "TransactionLogContract" ||
                typeContractName == "BoaUserContract") // resolve conflig
            {
                typeContractName = $"{context.GetEntityNamespace()}.{typeContractName}";
            }

            context.Add(TableEntityClassNameForMethodParametersInRepositoryFiles,typeContractName);
            context.Add(RepositoryClassName, tableInfo.TableName.ToContractName());
            context.Add(SharedRepositoryClassName,config.SharedRepositoryClassNameFormat.Replace("{TableName}", tableInfo.TableName.ToContractName()));


        }

        public static void RemoveNamesRelatedWithTable(IDataContext context)
        {
            context.Remove(TableEntityClassNameForMethodParametersInRepositoryFiles);
            context.Remove(RepositoryClassName);
            context.Remove(SharedRepositoryClassName);
        }
        #endregion

        #region Table
        public static void PushNamesRelatedWithSchema(IDataContext context)
        {
            var config    = context.Get(Data.Config);

            context.Add(BusinessClassNamespace,context.GetRepositoryNamespace());
        }

        public static void RemoveNamesRelatedWithSchema(IDataContext context)
        {
            context.Remove(BusinessClassNamespace);
        }
        #endregion
    }
}