﻿using BOA.EntityGeneration.CustomSQLExporting.Wrapper;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BOA.EntityGeneration.CustomSQLExporting
{
    [TestClass]
    public class CustomSqlExporterDebug
    {
        #region Public Methods
        [TestMethod]
        public void CustomSqlInjection()
        {
            var customSqlExporter = new CustomSqlExporter();
            customSqlExporter.InitializeContext();
            customSqlExporter.Export("CC_OPERATIONS");
        }
        #endregion
    }
}