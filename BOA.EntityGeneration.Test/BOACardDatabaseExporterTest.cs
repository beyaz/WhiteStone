using BOA.Common.Helpers;
using BOA.DatabaseAccess;
using BOA.EntityGeneration.BOACardDatabaseSchemaToDllExporting.ClassWriters;
using BOA.EntityGeneration.BOACardDatabaseSchemaToDllExporting.DataAccess;
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

                database.CommandText = "CREATE SCHEMA CRD";
                database.ExecuteNonQuery();

                database.CommandText =
                    @"

CREATE TABLE CRD.SAMPLE_TABLE
(
    SAMPLE_TABLE_ID INT PRIMARY KEY IDENTITY (1, 1),

    FIELD_VARCHAR_50            VARCHAR (50) NULL,
    FIELD_VARCHAR_50_NULLABLE   VARCHAR (50) NULL,

    FIELD_DATETIME              DATETIME,
    FIELD_DATETIME_NULLABLE     DATETIME NULL,

    FIELD_NUMERIC_27_0              NUMERIC(27,0),
    FIELD_NUMERIC_27_0_NULLABLE     NUMERIC(27,0) NULL,

    FIELD_INT                   INT,
    FIELD_INT_NULLABLE          INT NULL
)

";

                database.ExecuteNonQuery();

                var sb = new PaddedStringBuilder();

                var generatorOfBusinessClass = kernel.Get<GeneratorOfBusinessClass>();
                var tableInfo                = kernel.Get<SchemaExporterDataPreparer>().GetTableInfo("CRD", "SAMPLE_TABLE");

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
            using (var kernel = new Kernel())
            {
                BOACardDatabaseExporter.Export(kernel, "CRD");
            }
        }
        #endregion

        
    }

    class TestKernel : Kernel
    {
        #region Public Methods
        public override string GetConfigFilePath()
        {
            return @"D:\github\WhiteStone\BOA.EntityGeneration.Test\BOA.EntityGeneration.json";
        }
        #endregion
    }
}