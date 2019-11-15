﻿using BOA.DataFlow;
using BOA.EntityGeneration.DataFlow;
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
            var context = new DataContextCreator().Create();

            context.Get(Data.Config).BuildAfterCodeGenerationIsCompleted = false;
            context.Get(Data.Config).IntegrateWithBOATfs = false;
            context.AttachEvent(DataEvent.AfterFetchedAllTableNamesInSchema, SelectFirstTenTable);

            BOACardDatabaseExporter.Export(context, "CRD");
        }
        #endregion

        #region Methods
        static void SelectFirstTenTable(IDataContext context)
        {
            var       list  = context.Get(Data.TableNamesInSchema);
            const int count = 30;
            list.RemoveRange(count, list.Count - count);
        }
        #endregion
    }
}