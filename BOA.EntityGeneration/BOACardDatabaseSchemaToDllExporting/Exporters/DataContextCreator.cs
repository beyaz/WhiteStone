using System.IO;
using ___Company___.DataFlow;
using ___Company___.EntityGeneration.DataFlow;
using BOA.Common.Helpers;
using BOA.DatabaseAccess;
using BOA.EntityGeneration.BOACardDatabaseSchemaToDllExporting.AllInOne;
using BOA.EntityGeneration.BOACardDatabaseSchemaToDllExporting.ClassWriters;
using BOA.EntityGeneration.BOACardDatabaseSchemaToDllExporting.DataAccess;
using BOA.EntityGeneration.BOACardDatabaseSchemaToDllExporting.ProjectExporters;
using BOA.EntityGeneration.BOACardDatabaseSchemaToDllExporting.Util;
using BOA.TfsAccess;
using FileAccess = BOA.TfsAccess.FileAccess;

namespace BOA.EntityGeneration.BOACardDatabaseSchemaToDllExporting.Exporters
{
    public class DataContextCreator
    {
        #region Public Properties
        public string CheckinComment      { get; set; }
        public string ConfigFilePath      { get; set; }
        public bool   IsFileAccessWithTfs { get; set; }
        #endregion

        #region Public Methods
        public IDataContext Create()
        {
            var context = new DataContext();

            if (ConfigFilePath == null)
            {
                ConfigFilePath = Path.GetDirectoryName(typeof(DataContextCreator).Assembly.Location) + Path.DirectorySeparatorChar + "BOA.EntityGeneration.json";
            }

            context.Add(Data.Config, JsonHelper.Deserialize<Config>(File.ReadAllText(ConfigFilePath)));
            context.Add(Data.Database, new SqlDatabase(context.Get(Data.Config).ConnectionString) {CommandTimeout = 1000 * 60 * 60});
            context.Add(Data.MsBuildQueue, new MsBuildQueue());

            if (IsFileAccessWithTfs)
            {
                context.Add(Data.FileAccess, new FileSystem
                {
                    Config     = context.Get(Data.Config),
                    FileAccess = new FileAccessWithAutoCheckIn {CheckInComment = CheckinComment}
                });
            }
            else
            {
                context.Add(Data.FileAccess, new FileSystem
                {
                    Config     = context.Get(Data.Config),
                    FileAccess = new FileAccess()
                });
            }

            context.Add(Data.AllSchemaGenerationProcess, new ProcessInfo());
            context.Add(Data.SchemaGenerationProcess, new ProcessInfo());
            context.Add(Data.CustomSqlGenerationOfProfileIdProcess, new ProcessInfo());

            AttachEvents(context);

            return context;
        }
        #endregion

        #region Methods
        protected virtual void AttachEvents(IDataContext context)
        {
            context.AttachEvent(DataEvent.StartToExportTable, GeneratorOfTypeClass.WriteClass);
            context.AttachEvent(DataEvent.StartToExportTable, GeneratorOfBusinessClass.CreateBusinessClassWriterContext);
            context.AttachEvent(DataEvent.StartToExportTable, GeneratorOfBusinessClass.WriteClass);
            context.AttachEvent(DataEvent.StartToExportTable, SharedDalClassWriter.Write);
            context.AttachEvent(DataEvent.StartToExportTable, GeneratorOfBusinessClass.RemoveBusinessClassWriterContext);

            context.AttachEvent(DataEvent.StartToExportSchema, SharedDalClassWriter.WriteUsingList);
            context.AttachEvent(DataEvent.StartToExportSchema, GeneratorOfTypeClass.WriteUsingList);
            context.AttachEvent(DataEvent.StartToExportSchema, GeneratorOfBusinessClass.WriteUsingList);
            context.AttachEvent(DataEvent.StartToExportSchema, GeneratorOfTypeClass.BeginNamespace);
            context.AttachEvent(DataEvent.StartToExportSchema, AllBusinessClassesInOne.BeginNamespace);
            context.AttachEvent(DataEvent.StartToExportSchema, Events.OnSchemaStartedToExport);
            context.AttachEvent(DataEvent.StartToExportSchema, SharedDalClassWriter.EndNamespace);
            context.AttachEvent(DataEvent.StartToExportSchema, GeneratorOfTypeClass.EndNamespace);
            context.AttachEvent(DataEvent.StartToExportSchema, GeneratorOfBusinessClass.EndNamespace);

            context.AttachEvent(DataEvent.StartToExportSchema, TypesProjectExporter.ExportTypeDll);
            context.AttachEvent(DataEvent.StartToExportSchema, BusinessProjectExporter.Export);
            context.AttachEvent(DataEvent.StartToExportSchema, SharedDalClassWriter.ExportFile);

            context.AttachEvent(DataEvent.StartToExportSchema, MsBuildQueue.Build);
        }
        #endregion
    }
}