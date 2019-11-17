using System.Collections.Generic;
using BOA.Common.Helpers;
using BOA.DatabaseAccess;
using BOA.DataFlow;
using BOA.EntityGeneration.BOACardDatabaseSchemaToDllExporting;
using BOA.EntityGeneration.BOACardDatabaseSchemaToDllExporting.Models.Interfaces;
using BOA.EntityGeneration.BOACardDatabaseSchemaToDllExporting.Util;

namespace BOA.EntityGeneration.DataFlow
{


    static class TableExportingEvent
    {
        public static readonly IEvent TableExportStarted = new Event {Name = nameof(TableExportStarted)};
        public static readonly IEvent TableExportFinished = new Event {Name = nameof(TableExportFinished)};
    }

    static class SchemaExportingEvent
    {
        public static readonly IEvent SchemaExportStarted = new Event {Name = nameof(SchemaExportStarted)};
        public static readonly IEvent SchemaExportFinished = new Event {Name = nameof(SchemaExportFinished)};
    }
     static class DataEvent
    {
        #region Static Fields
        public static readonly IEvent AfterFetchedAllTableNamesInSchema = new Event {Name = nameof(AfterFetchedAllTableNamesInSchema)};
        
        
        #endregion
    }

    /// <summary>
    ///     The data
    /// </summary>
     static class Data
    {
        internal static readonly IDataConstant<NamingPattern> NamingPattern = DataConstant.Create<NamingPattern>(nameof(BOACardDatabaseSchemaToDllExporting.NamingPattern));
        internal static readonly IDataConstant<TableNamingPattern> TableNamingPattern = DataConstant.Create<TableNamingPattern>(nameof(BOACardDatabaseSchemaToDllExporting.TableNamingPattern));

        #region Static Fields
        internal static readonly IDataConstant<ConfigContract> Config               = DataConstant.Create<ConfigContract>();
        
        internal static readonly IDataConstant<IDatabase> Database             = DataConstant.Create<IDatabase>();
        
        internal static readonly IDataConstant<ITableInfo> TableInfo            = DataConstant.Create<ITableInfo>();
        internal static readonly IDataConstant<List<string>> TableNamesInSchema   = DataConstant.Create<List<string>>();
        #endregion

        #region Naming
        internal static readonly IDataConstant<string> SchemaName                                               = DataConstant.Create<string>(nameof(SchemaName));
        internal static readonly IDataConstant<string> TableEntityClassNameForMethodParametersInRepositoryFiles = DataConstant.Create<string>(nameof(TableEntityClassNameForMethodParametersInRepositoryFiles));
        
        #endregion

        #region Process Indicators
        internal static readonly IDataConstant<ProcessContract> AllSchemaGenerationProcess            = DataConstant.Create<ProcessContract>(nameof(AllSchemaGenerationProcess));
        internal static readonly IDataConstant<ProcessContract> SchemaGenerationProcess               = DataConstant.Create<ProcessContract>(nameof(SchemaGenerationProcess));
        #endregion

        #region Files
        
        
        
        #endregion
    }
}