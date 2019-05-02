using System.IO;
using BOA.EntityGeneration.BOACardDatabaseSchemaToDllExporting;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BOA.EntityGeneration.SchemaToDllExporting
{
    [TestClass]
    public class BOACardDatabaseExporterTest
    {
        #region Public Methods
        [TestMethod]
        public void Export()
        {
            using (var kernel = new TestKernel())
            {
                BOACardDatabaseExporter.Export(kernel);
            }
        }

        [TestMethod]
        public void ExportPRM()
        {
            using (var kernel = new TestKernel())
            {
                BOACardDatabaseExporter.Export(kernel, "DLV");
            }
        }
        #endregion
    }

    class BatExporter
    {
        public void Export(string schemaName)
        {
            const string dir = @"\\srvktfs\KTBirimlerArasi\BT-Uygulama Gelistirme 3\Abdullah_Beyaztas\BOACardEntityGeneration\";

            var content = $@"
cd\
cd windows
cd system32

robocopy ""{dir}Generator"" ""d:\boa\BOACard.EntityGeneration"" /E

start D:\boa\BOACard.EntityGeneration\BOACardEntityGenerationWrapper.exe %~n0

exit

";
            File.WriteAllText(dir+schemaName+".bat", content);
        }
    }

    class TestKernel : Kernel
    {
    }
}