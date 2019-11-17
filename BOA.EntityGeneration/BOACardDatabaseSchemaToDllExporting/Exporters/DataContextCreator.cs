using System.IO;
using BOA.Common.Helpers;
using BOA.DatabaseAccess;
using BOA.DataFlow;
using BOA.EntityGeneration.BOACardDatabaseSchemaToDllExporting.AllInOne;
using BOA.EntityGeneration.BOACardDatabaseSchemaToDllExporting.ClassWriters;
using BOA.EntityGeneration.BOACardDatabaseSchemaToDllExporting.DataAccess;
using BOA.EntityGeneration.BOACardDatabaseSchemaToDllExporting.ProjectExporters;
using BOA.EntityGeneration.BOACardDatabaseSchemaToDllExporting.Util;
using BOA.EntityGeneration.DataFlow;
using BOA.EntityGeneration.SharedRepositoryFileExporting;
using static BOA.EntityGeneration.DataFlow.DataEvent;

namespace BOA.EntityGeneration.BOACardDatabaseSchemaToDllExporting.Exporters
{
    public class DataContextCreatorBase
    {
        #region Public Properties
        public string ConfigFilePath { get; set; }
        #endregion

        #region Public Methods
        public void InitializeServices(IDataContext context)
        {
            if (ConfigFilePath == null)
            {
                ConfigFilePath = Path.GetDirectoryName(typeof(DataContextCreator).Assembly.Location) + Path.DirectorySeparatorChar + "BOA.EntityGeneration.json";
            }

            context.Add(Data.Config, JsonHelper.Deserialize<ConfigContract>(File.ReadAllText(ConfigFilePath)));
            context.Add(Data.Database, new SqlDatabase(context.Get(Data.Config).ConnectionString) {CommandTimeout = 1000 * 60 * 60});
            context.Add(MsBuildQueue.MsBuildQueueId, new MsBuildQueue());

            context.Add(Data.AllSchemaGenerationProcess, new ProcessContract());
            context.Add(Data.SchemaGenerationProcess, new ProcessContract());
            
        }
        #endregion
    }

    public class DataContextCreator : DataContextCreatorBase
    {
        #region Public Methods
        public IDataContext Create()
        {
            var context = new DataContext();

            InitializeServices(context);

            AttachEvents(context);

            return context;
        }
        #endregion

        #region Methods
        protected virtual void AttachEvents(IDataContext context)
        {
            context.AttachEvent(TableExportingEvent.TableExportStarted, TableNamingPatternInitializer.Initialize);
            EntityFileExporter.AttachEvents(context);
            SharedFileExporter.AttachEvents(context);

            

            
            context.AttachEvent(TableExportingEvent.TableExportStarted, GeneratorOfBusinessClass.WriteClass);

            context.AttachEvent(TableExportingEvent.TableExportFinished, TableNamingPatternInitializer.Remove);

            
            context.AttachEvent(SchemaExportingEvent.SchemaExportStarted, GeneratorOfBusinessClass.WriteUsingList);
            
            context.AttachEvent(SchemaExportingEvent.SchemaExportStarted, AllBusinessClassesInOne.BeginNamespace);
            context.AttachEvent(SchemaExportingEvent.SchemaExportStarted, Events.OnSchemaStartedToExport);
            
            context.AttachEvent(SchemaExportingEvent.SchemaExportStarted, GeneratorOfBusinessClass.EndNamespace);

            context.AttachEvent(SchemaExportingEvent.SchemaExportStarted, TypesProjectExporter.ExportTypeDll);
            context.AttachEvent(SchemaExportingEvent.SchemaExportStarted, BusinessProjectExporter.Export);


            context.AttachEvent(SchemaExportingEvent.SchemaExportStarted, MsBuildQueue.Build);
        }
        #endregion
    }
}