using System.IO;
using BOA.Common.Helpers;
using BOA.EntityGeneration.BOACardDatabaseSchemaToDllExporting.Exporters;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BOA.EntityGeneration.DbModel.SqlServerDataAccess
{
    [TestClass]
    public class SampleDatabaseTest
    {
        #region Public Methods
        [TestMethod]
        public void AllInOne()
        {
            using (var kernel = new Kernel())
            {
                using (var database = kernel.CreateConnection())
                {
                    database.CommandText = @"
IF NOT EXISTS 
(
    SELECT schema_name
      FROM information_schema.schemata
     WHERE schema_name = 'ERP' ) 

BEGIN
EXEC sp_executesql N'CREATE SCHEMA ERP'
END
";

                    database.ExecuteNonQuery();

                    database.CommandText = @"

IF EXISTS (SELECT TOP 1 1 FROM sys.objects WHERE object_id = OBJECT_ID(N'[ERP].[Person]') AND type = 'U')
    DROP TABLE ERP.Person
";
                    database.ExecuteNonQuery();

                    database.CommandText = @"

CREATE TABLE ERP.Person 
(
    PersonId INT not null  identity(1,1)  primary key,
    Name varchar(64)
)
";
                    database.ExecuteNonQuery();
                }

                BOACardDatabaseExporter.Export(kernel);

                ShouldBeSame(@"D:\temp\ERP\BOA.Types.Kernel.Card.ERP\All.cs", 
                             @"D:\github\WhiteStone\BOA.EntityGeneration.Test\ERP\BOA.Types.Kernel.Card.ERP\All.cs.txt");

                ShouldBeSame(@"D:\temp\ERP\BOA.Business.Kernel.Card.ERP\All.cs", 
                             @"D:\github\WhiteStone\BOA.EntityGeneration.Test\ERP\BOA.Business.Kernel.Card.ERP\All.cs.txt");
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