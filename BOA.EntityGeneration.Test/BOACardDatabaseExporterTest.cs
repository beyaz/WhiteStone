using System.Collections.Generic;
using System.Data;
using System.IO;
using BOA.Common.Helpers;
using BOA.DatabaseAccess;
using BOA.EntityGeneration.BOACardCustomSqlIntoProjectInjection;
using BOA.EntityGeneration.BOACardDatabaseSchemaToDllExporting;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ninject;

namespace BOA.EntityGeneration.SchemaToDllExporting
{

  
    [TestClass]
    public class BOACardDatabaseExporterTest
    {


        [TestMethod]
        public void CustomSqlInjection()
        {
            using (var kernel = new TestKernel())
            {
                kernel.Get<ProjectInjector>().Inject("CC_OPERATIONS");
            }
        }

        #region Public Methods
        [TestMethod]
        public void Export()
        {
            using (var kernel = new TestKernel())
            {
                BOACardDatabaseExporter.Export(kernel);
            }
        }
        #endregion

        

        class TestKernel : Kernel
        {
           
        }
    }
}