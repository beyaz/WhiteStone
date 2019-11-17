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
            var namingPattern = context.Get(NamingPattern.Id);

            var typeContractName = $"{tableInfo.TableName.ToContractName()}Contract";
            if (typeContractName == "TransactionLogContract" ||
                typeContractName == "BoaUserContract") // resolve conflig
            {
                typeContractName = $"{namingPattern.EntityNamespace}.{typeContractName}";
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

       
    }
}