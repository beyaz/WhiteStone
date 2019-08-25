using BOA.EntityGeneration.BOACardDatabaseSchemaToDllExporting.Exporters;
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
    PersonId INT identity not null,
    Name varchar(64)
)
";
                    database.ExecuteNonQuery();
                }

                BOACardDatabaseExporter.Export(kernel);
            }
        }
        #endregion
    }
}