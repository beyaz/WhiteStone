﻿using BOA.DataFlow;
using BOA.EntityGeneration.BOACardDatabaseSchemaToDllExporting.ProjectExporters;
using BOA.EntityGeneration.DataFlow;
using BOA.EntityGeneration.Exporters;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BOA.EntityGeneration.BOACardDatabaseSchemaToDllExporting.Exporters
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
            var context = EntityGenerationDataContextCreator.Create();

            context.Add(FileSystem.IntegrateWithTFSAndCheckInAutomatically, false);
            // context.AttachEvent(DataEvent.AfterFetchedAllTableNamesInSchema, SelectFirstTenTable);

            SchemaExporter.Export(context, "CRD");
        }
        #endregion

        #region Methods
        static void SelectFirstTenTable(IContext context)
        {
            var       list  = context.Get(Data.TableNamesInSchema);
            const int count = 5;
            list.RemoveRange(count, list.Count - count);
        }
        #endregion
    }
}