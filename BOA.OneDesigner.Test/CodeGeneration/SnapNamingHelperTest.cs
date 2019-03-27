using BOA.OneDesigner.CodeGenerationModel;
using BOA.OneDesigner.JsxElementModel;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BOA.OneDesigner.CodeGeneration
{
    [TestClass]
    public class SnapNamingHelperTest
    {
        #region Public Methods
        [TestMethod]
        public void Combo_snap_name_test()
        {
            var data = new BComboBox
            {
                SelectedValueBindingPath = "A.Bbb.UserId"
            };

            var writerContext = new WriterContext();

            SnapNamingHelper.InitSnapName(writerContext,data);

            data.SnapName.Should().Be("userIdComboBox");

            data.TypeScriptMethodNameOfGetGridColumns.Should().Be("getUserIdComboBoxColumns");


            SnapNamingHelper.InitSnapName(writerContext,data);

            data.SnapName.Should().Be("userIdComboBox1");

            data.TypeScriptMethodNameOfGetGridColumns.Should().Be("getUserIdComboBox1Columns");
        }
        #endregion
    }
}