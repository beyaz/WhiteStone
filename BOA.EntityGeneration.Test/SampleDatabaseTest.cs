using System.IO;
using BOA.Common.Helpers;
using BOA.DatabaseAccess;
using BOA.EntityGeneration.BOACardDatabaseSchemaToDllExporting.Exporters;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ninject;

namespace BOA.EntityGeneration.DbModel.SqlServerDataAccess
{
    [TestClass]
    public class SampleDatabaseTest
    {
        #region Public Methods
        [TestMethod]
        public void All_db_types_should_be_handled()
        {
            using (var kernel = new TestKernel())
            {
                var database = kernel.Get<IDatabase>();

                database.BeginTransaction();

                database.CommandText = "CREATE SCHEMA ERP";
                database.ExecuteNonQuery();

                database.CommandText =
                    @"

CREATE TABLE ERP.SAMPLE_TABLE
(
    SAMPLE_TABLE_ID INT PRIMARY KEY IDENTITY (1, 1),

    FIELD_VARCHAR_50            VARCHAR (50) NULL,
    FIELD_VARCHAR_50_NULLABLE   VARCHAR (50) NULL,

    FIELD_DATETIME              DATETIME,
    FIELD_DATETIME_NULLABLE     DATETIME NULL,

    FIELD_NUMERIC_27_0              NUMERIC(27,0),
    FIELD_NUMERIC_27_0_NULLABLE     NUMERIC(27,0) NULL,

    FIELD_INT                   INT,
    FIELD_INT_NULLABLE          INT NULL,

    FIELD_MONEY                   MONEY,
    FIELD_MONEY_NULLABLE          MONEY NULL,

    FIELD_NVARCHAR                   NVARCHAR(7),
    FIELD_NVARCHAR_NULLABLE          NVARCHAR(7) NULL,

    FIELD_NCHAR                   NCHAR(7),
    FIELD_NCHAR_NULLABLE          NCHAR(7) NULL,

    FIELD_SMALLDATETIME                   SMALLDATETIME,
    FIELD_SMALLDATETIME_NULLABLE          SMALLDATETIME NULL,

    FIELD_SMALLINT                   SMALLINT,
    FIELD_SMALLINT_NULLABLE          SMALLINT NULL,

    FIELD_TINYINT                   TINYINT,
    FIELD_TINYINT_NULLABLE          TINYINT NULL,

    FIELD_CHAR                   CHAR,
    FIELD_CHAR_NULLABLE          CHAR NULL,

    FIELD_BIGINT                   BIGINT,
    FIELD_BIGINT_NULLABLE          BIGINT NULL,

    FIELD_BIT                   BIT,
    FIELD_BIT_NULLABLE          BIT NULL,

    FIELD_DECIMAL                   DECIMAL,
    FIELD_DECIMAL_NULLABLE          DECIMAL NULL,

    FIELD_UNIQUEIDENTIFIER                   UNIQUEIDENTIFIER,
    FIELD_UNIQUEIDENTIFIER_NULLABLE          UNIQUEIDENTIFIER NULL,

    FIELD_VARBINARY                  VARBINARY,
    FIELD_VARBINARY_NULLABLE          VARBINARY NULL
)

";

                database.ExecuteNonQuery();

                BOACardDatabaseExporter.Export(kernel);

                ShouldBeSame(@"D:\temp\ERP\BOA.Types.Kernel.Card.ERP\All.cs",
                             @"D:\github\WhiteStone\BOA.EntityGeneration.Test\ERP\BOA.Types.Kernel.Card.ERP\All.cs.txt");

                ShouldBeSame(@"D:\temp\ERP\BOA.Business.Kernel.Card.ERP\All.cs",
                             @"D:\github\WhiteStone\BOA.EntityGeneration.Test\ERP\BOA.Business.Kernel.Card.ERP\All.cs.txt");

                database.Rollback();
            }
        }
        #endregion

        #region Methods
        static void ShouldBeSame(string currentFilePath, string expectedFilePath)
        {
            var current  = File.ReadAllText(currentFilePath);
            var expected = File.ReadAllText(expectedFilePath);

            StringHelper.IsEqualAsData(current, expected).Should().BeTrue();
        }
        #endregion

        
    }
}