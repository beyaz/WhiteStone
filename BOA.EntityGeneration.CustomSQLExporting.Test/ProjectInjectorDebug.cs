using BOA.EntityGeneration.CustomSQLExporting.Wrapper;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BOA.EntityGeneration.BOACardCustomSqlIntoProjectInjection.Injectors
{
    [TestClass]
    public class CustomSqlExporterDebug
    {
        #region Public Methods
        [TestMethod]
        public void CustomSqlInjection()
        {
            new CustomSqlExporter().Export( "CC_OPERATIONS");
        }
        #endregion
    }
}