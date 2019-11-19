using System.Collections.Generic;
using BOA.Common.Helpers;
using BOA.DataFlow;
using BOA.EntityGeneration.DataFlow;

namespace BOA.EntityGeneration.Naming
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

            var tableNamingPattern = new TableNamingPatternContract
            {
                EntityClassName                              = dictionary[nameof(TableNamingPatternContract.EntityClassName)],
                SharedRepositoryClassName                    = dictionary[nameof(TableNamingPatternContract.SharedRepositoryClassName)],
                BoaRepositoryClassName                       = dictionary[nameof(TableNamingPatternContract.BoaRepositoryClassName)],
                SharedRepositoryClassNameInBoaRepositoryFile = dictionary[nameof(TableNamingPatternContract.SharedRepositoryClassNameInBoaRepositoryFile)]
                
            };
            context.Add(TableNamingPatternContract.TableNamingPattern, tableNamingPattern);


            // TODO: move to usings
            var typeContractName = tableNamingPattern.EntityClassName;
            if (typeContractName == "TransactionLogContract" ||
                typeContractName == "BoaUserContract") // resolve conflig
            {
                typeContractName = $"{context.Get(NamingPatternContract.NamingPattern).EntityNamespace}.{typeContractName}";
            }

            context.Add(Data.TableEntityClassNameForMethodParametersInRepositoryFiles,typeContractName);



        }

        
        #endregion
    }
}