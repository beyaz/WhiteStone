using System.Collections.Generic;
using BOA.Common.Helpers;
using BOA.DataFlow;
using BOA.EntityGeneration.DataFlow;

namespace BOA.EntityGeneration.BOACardDatabaseSchemaToDllExporting
{
    static class TableNamingPatternInitializer
    {
        #region Public Methods
        public static void Initialize(IDataContext context)
        {
            var config    = context.Get(Data.Config);
            var tableInfo = context.Get(Data.TableInfo);

            var initialValues = new Dictionary<string, string>
            {
                {nameof(Data.SchemaName), context.Get(Data.SchemaName)},
                {"CamelCasedTableName", tableInfo.TableName.ToContractName()}
            };

            var dictionary = ConfigurationDictionaryCompiler.Compile(config.TableNamingPattern, initialValues);

            var tableNamingPattern = new TableNamingPattern
            {
                EntityClassName                              = dictionary[nameof(TableNamingPattern.EntityClassName)],
                SharedRepositoryClassName                    = dictionary[nameof(TableNamingPattern.SharedRepositoryClassName)],
                BoaRepositoryClassName                       = dictionary[nameof(TableNamingPattern.BoaRepositoryClassName)],
                SharedRepositoryClassNameInBoaRepositoryFile = dictionary[nameof(TableNamingPattern.SharedRepositoryClassNameInBoaRepositoryFile)]
                
            };
            context.Add(Data.TableNamingPattern, tableNamingPattern);


            // TODO: move to usings
            var typeContractName = tableNamingPattern.EntityClassName;
            if (typeContractName == "TransactionLogContract" ||
                typeContractName == "BoaUserContract") // resolve conflig
            {
                typeContractName = $"{context.Get(Data.NamingPattern).EntityNamespace}.{typeContractName}";
            }

            context.Add(Data.TableEntityClassNameForMethodParametersInRepositoryFiles,typeContractName);



        }

        public static void Remove(IDataContext context)
        {
            context.Remove(Data.TableNamingPattern);
            context.Remove(Data.TableEntityClassNameForMethodParametersInRepositoryFiles);
        }
        #endregion
    }
}