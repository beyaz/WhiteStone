using System.IO;
using BOA.Common.Helpers;
using BOA.EntityGeneration.Exporters;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BOA.EntityGeneration.DbModel.SqlServerDataAccess
{
    [TestClass]
    public class SampleDatabaseTest
    {
        #region Public Methods
        [TestInitialize]
        public void Initialize()
        {
            var schemaExporter = new SchemaExporter
            {
                ConfigFilePath = @"D:\github\WhiteStone\BOA.EntityGeneration.Test\BOA.EntityGeneration.json"
            };
            schemaExporter.InitializeContext();
            AttachTests(schemaExporter);

            schemaExporter.database.CreateTables();
            schemaExporter.Export("ERP");
        }
        #endregion

        #region Methods
        static void AttachTests(SchemaExporter schemaExporter)
        {
            const string ExpectedResultsDirectory = @"D:\github\WhiteStone\BOA.EntityGeneration.Test\SampleDatabaseTest.ExpectedResults\";
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