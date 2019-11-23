using BOA.EntityGeneration.BOACardDatabaseSchemaToDllExporting.Util;
using BOA.EntityGeneration.DataFlow;
using BOA.EntityGeneration.Models.Interfaces;
using BOA.EntityGeneration.Naming;

namespace BOA.EntityGeneration
{
    class ContextContainer : BOA.DataFlow.ContextContainer
    {
        #region Properties
        protected ConfigContract             config                                                   => Data.Config[Context];
        protected NamingPatternContract      namingPattern                                            => NamingPatternContract.NamingPattern[Context];
        protected ProcessContract            processInfo                                              => Data.ProcessInfo[Context];
        protected ITableInfo                 tableInfo                                                => Data.TableInfo[Context];
        protected TableNamingPatternContract tableNamingPattern                                       => TableNamingPatternContract.TableNamingPattern[Context];
        protected string                     tableEntityClassNameForMethodParametersInRepositoryFiles => Data.TableEntityClassNameForMethodParametersInRepositoryFiles[Context];

        protected string schemaName
        {
            get=> Data.SchemaName[Context];
            set=> Data.SchemaName[Context]=value;
        }
        
        #endregion
    }
}