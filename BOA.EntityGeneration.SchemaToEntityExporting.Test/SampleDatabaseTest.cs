using BOA.Common.Helpers;
using BOA.EntityGeneration.SchemaToEntityExporting.Exporters;
using BOA.EntityGeneration.SchemaToEntityExporting.FileExporters.AllSchemaInOneClassRepositoryFile;
using BOA.EntityGeneration.SchemaToEntityExporting.FileExporters.EntityFileExporting;
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
            ReflectionHelper.CopyProperties(SchemaExporterConfig.CreateFromFile("SampleDatabaseTest.SchemaExporterConfig.yaml"), SchemaExporter.Config);
            ReflectionHelper.CopyProperties(EntityFileExporterConfig.LoadFromFile("SampleDatabaseTest.EntityFileExporterConfig.yaml"), EntityFileExporter.Config);
            ReflectionHelper.CopyProperties(AllSchemaInOneClassRepositoryFileExporterConfig.CreateFromFile("SampleDatabaseTest.AllSchemaInOneClassRepositoryFileExporterConfig.yaml"), AllSchemaInOneClassRepositoryFileExporter.Config);

            var schemaExporter = new SchemaExporter();

            schemaExporter.InitializeContext();

            schemaExporter.Database.CreateTables();

            schemaExporter.Export("ERP");

            schemaExporter.Context.ErrorList.Should().BeEmpty();
        }
        #endregion
    }
}