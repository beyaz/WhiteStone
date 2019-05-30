using BOA.Common.Helpers;
using BOA.DatabaseAccess;
using BOA.EntityGeneration.BOACardDatabaseSchemaToDllExporting.ClassWriters;
using BOA.EntityGeneration.BOACardDatabaseSchemaToDllExporting.DataAccess;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ninject;

namespace BOA.EntityGeneration.BOACardDatabaseSchemaToDllExporting.Exporters
{
    [TestClass]
    public class BOACardDatabaseExporterTest
    {
        #region Public Methods
        [TestMethod]
        public void All_db_types_should_be_handled()
        {
            using (var kernel = new TestKernel())
            {
                var database = kernel.Get<IDatabase>();

                database.BeginTransaction();

                database.CommandText =
                    @"

IF EXISTS (SELECT OBJECT_ID('DLV.SAMPLE_TABLE') )
BEGIN
  DROP TABLE DLV.SAMPLE_TABLE
END

CREATE TABLE DLV.SAMPLE_TABLE
(
    ID INT PRIMARY KEY IDENTITY (1, 1),
    FIELD_VARCHAR_50 VARCHAR (50) NULL,    
    FIELD_DATETIME DATETIME,
    FIELD_INT INT
)

";


                database.ExecuteNonQuery();


                var sb = new PaddedStringBuilder();

                var generatorOfBusinessClass = kernel.Get<GeneratorOfBusinessClass>();
                var tableInfo = kernel.Get<SchemaExporterDataPreparer>().GetTableInfo("DLV","SAMPLE_TABLE");

                generatorOfBusinessClass.WriteClass(sb, tableInfo);

//                sb.ToString().Should().Be(@"

//".Trim());

                database.Rollback();

            }
        }

        //[TestMethod]
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
                BOACardDatabaseExporter.Export(kernel, "CRD");
            }
        }
        #endregion

        class TestKernel : Kernel
        {
        }
    }
}