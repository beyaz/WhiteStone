using System.Collections.Generic;
using BOA.Common.Helpers;
using BOA.DatabaseAccess;
using BOA.DataFlow;
using BOA.EntityGeneration.BOACardDatabaseSchemaToDllExporting.Models.Interfaces;
using BOA.EntityGeneration.BOACardDatabaseSchemaToDllExporting.Util;

namespace BOA.EntityGeneration.DataFlow
{

  

   

     static class DataEvent
    {
        #region Static Fields
        public static readonly IEvent AfterFetchedAllTableNamesInSchema = new Event {Name = nameof(AfterFetchedAllTableNamesInSchema)};
        public static readonly IEvent StartToExportSchema               = new Event {Name = nameof(StartToExportSchema)};
        public static readonly IEvent StartToExportTable                = new Event {Name = nameof(StartToExportTable)};
        #endregion
    }

    /// <summary>
    ///     The data
    /// </summary>
     static class Data
    {
        #region Static Fields
        internal static readonly IDataConstant<ConfigContract>        Config               = DataConstant.Create<ConfigContract>();
        
        internal static readonly IDataConstant<IDatabase>             Database             = DataConstant.Create<IDatabase>();
        
        internal static readonly IDataConstant<ITableInfo>            TableInfo            = DataConstant.Create<ITableInfo>();
        internal static readonly IDataConstant<List<string>>          TableNamesInSchema   = DataConstant.Create<List<string>>();
        #endregion

        #region Naming
        internal static readonly IDataConstant<string> SchemaName                                               = DataConstant.Create<string>(nameof(SchemaName));
        internal static readonly IDataConstant<string> TableEntityClassNameForMethodParametersInRepositoryFiles = DataConstant.Create<string>(nameof(TableEntityClassNameForMethodParametersInRepositoryFiles));
        internal static readonly IDataConstant<string> RepositoryClassName                                      = DataConstant.Create<string>(nameof(RepositoryClassName));
        internal static readonly IDataConstant<string> SharedRepositoryClassName = DataConstant.Create<string>(nameof(SharedRepositoryClassName));
        #endregion

        #region Process Indicators
        internal static readonly IDataConstant<ProcessContract> AllSchemaGenerationProcess            = DataConstant.Create<ProcessContract>(nameof(AllSchemaGenerationProcess));
        internal static readonly IDataConstant<ProcessContract> SchemaGenerationProcess               = DataConstant.Create<ProcessContract>(nameof(SchemaGenerationProcess));
        #endregion

        #region Files
        internal static readonly IDataConstant<PaddedStringBuilder> EntityFile = DataConstant.Create<PaddedStringBuilder>(nameof(EntityFile));
        internal static readonly IDataConstant<PaddedStringBuilder> SharedRepositoryFile = DataConstant.Create<PaddedStringBuilder>(nameof(SharedRepositoryFile));
        internal static readonly IDataConstant<PaddedStringBuilder> BoaRepositoryFile    = DataConstant.Create<PaddedStringBuilder>(nameof(BoaRepositoryFile));
        #endregion
    }
}