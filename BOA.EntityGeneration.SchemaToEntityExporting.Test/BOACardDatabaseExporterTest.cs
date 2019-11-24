using BOA.EntityGeneration.SchemaToEntityExporting;
using BOA.EntityGeneration.SchemaToEntityExporting.Exporters;
using BOA.EntityGeneration.SchemaToEntityExporting.FileExporters;
using BOA.EntityGeneration.SchemaToEntityExporting.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BOA.EntityGeneration.SchemaToEntityExporting
{
    [TestClass]
    public class BOACardDatabaseExporterTest
    {
        #region Public Methods
        ////[TestMethod]
        //public void Export()
        //{
        //    using (var kernel = new Kernel())
        //    {
        //        BOACardDatabaseExporter.Export(kernel);
        //    }
        //}

        [TestMethod]
        public void ExportPRM()
        {
            //var context = EntityGenerationDataContextCreator.Create();

            //context.Add(FileSystem.IntegrateWithTFSAndCheckInAutomatically, false);
            // context.AttachEvent(DataEvent.AfterFetchedAllTableNamesInSchema, SelectFirstTenTable);

            var schemaExporter = new SchemaExporter();
            schemaExporter.InitializeContext();
            schemaExporter.Export("CRD");
        }
        #endregion

        #region Methods
        static void SelectFirstTenTable(Context context)
        {
            //var       list  = context.Get(Data.TableNamesInSchema);
            //const int count = 5;
            //list.RemoveRange(count, list.Count - count);
        }
        #endregion
    }
}