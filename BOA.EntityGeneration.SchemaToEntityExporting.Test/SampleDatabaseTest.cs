using System.IO;
using BOA.Common.Helpers;
using BOA.EntityGeneration.SchemaToEntityExporting.Exporters;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BOA.EntityGeneration.SchemaToEntityExporting
{
    [TestClass]
    public class SampleDatabaseTest
    {
        #region Public Methods
        [TestMethod]
        public void AllScenario()
        {
            
            SchemaExporter.Config.ConnectionString = @"Data Source = (LocalDB)\MSSQLLocalDB; AttachDbFilename = D:\github\WhiteStone\BOA.EntityGeneration.SchemaToEntityExporting.Test\SampleDatabase.mdf; Integrated Security = True";
            SchemaExporter.Config.TableCatalog = @"D:\GITHUB\WHITESTONE\BOA.ENTITYGENERATION.SCHEMATOENTITYEXPORTING.TEST\SAMPLEDATABASE.MDF";
            SchemaExporter.Config.DatabaseEnumName = "SampleDatabase";
            SchemaExporter.Config.NamingPattern["SlnDirectoryPath"] = @"d:\temp\";
            SchemaExporter.Config.SqlSequenceInformationOfTable = null;

            var schemaExporter = new SchemaExporter();
            schemaExporter.InitializeContext();
            
            // AttachTests(schemaExporter);

            schemaExporter.Database.CreateTables();
            schemaExporter.Export("ERP");

            schemaExporter.Context.ErrorList.Should().BeEmpty();


        }
        #endregion

        #region Methods
        static void AttachTests(SchemaExporter schemaExporter)
        {
            const string ExpectedResultsDirectory = @"D:\github\WhiteStone\BOA.EntityGeneration.SchemaToEntityExporting.Test\SampleDatabaseTest.ExpectedResults\";
            schemaExporter.Context.EntityFileContentCompleted += content =>
            {
                var expected = File.ReadAllText(ExpectedResultsDirectory + @"ERP\BOA.Types.Kernel.Card.ERP\All.cs.txt");

                StringHelper.IsEqualAsData(content, expected).Should().BeTrue();
            };
            schemaExporter.Context.SharedRepositoryFileContentCompleted += content =>
            {
                var expected = File.ReadAllText(ExpectedResultsDirectory + @"ERP\BOA.Business.Kernel.Card.ERP\Shared.cs.txt");

                StringHelper.IsEqualAsData(content, expected).Should().BeTrue();
            };
        }
        #endregion
    }
}