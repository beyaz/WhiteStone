using BOA.EntityGeneration.CustomSQLExporting.Exporters;
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
            var context = new CustomSqlDataContextCreator().Create();
            CustomSqlExporter.Export(context, "CC_OPERATIONS");
            context.CloseBracket();
            context.IsEmpty.Should().BeTrue();
        }
        #endregion
    }
}