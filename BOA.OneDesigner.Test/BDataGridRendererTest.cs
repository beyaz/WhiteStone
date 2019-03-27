using BOA.OneDesigner.AppModel;
using BOA.OneDesigner.CodeGenerationModel;
using BOA.OneDesigner.JsxElementModel;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BOA.OneDesigner.CodeGeneration
{
    [TestClass]
    public class BDataGridRendererTest
    {
        [TestMethod]
        public void InitMethodNameOfGridColumns_should_evaluate_unique_name()
        {
            var data = new BDataGrid{DataSourceBindingPath = "Data.UserList"};

            var writerContext = new WriterContext();

            var memberName= BDataGridRenderer.EvaluateMethodNameOfGridColumns(writerContext, data);

            memberName.Should().BeEquivalentTo("getDataGridColumnsOfUserList");
        }
    }
}