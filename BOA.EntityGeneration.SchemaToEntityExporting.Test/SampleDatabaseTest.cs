using System.IO;
using BOA.Common.Helpers;
using BOA.EntityGeneration.SchemaToEntityExporting.Exporters;
using BOA.EntityGeneration.SchemaToEntityExporting.FileExporters.AllSchemaInOneClassRepositoryFile;
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
            //SchemaExporter.Config.CanExportBoaRepository = false;
            //AllSchemaInOneClassRepositoryFileExporter.Config.NamingPattern["UsingLines"] = AllSchemaInOneClassRepositoryFileExporter.Config.NamingPattern["UsingLines"].Replace("using IUnitOfWork = BOA.Card.Core.UOW.IUnitOfWork;", "using IUnitOfWork = BOA.DatabaseAccess.IDatabase;");
            //AllSchemaInOneClassRepositoryFileExporter.Config.NamingPattern["ExtraAssemblyReferences"] = "<Reference Include=\"WhiteStone\"><HintPath>D:\\github\\WhiteStone\\WhiteStone\\bin\\Debug\\WhiteStone.dll</HintPath></Reference>";
            //SchemaExporter.Config.CanExportAllSchemaInOneClassRepository = false;
            

            var schemaExporter = new SchemaExporter();
            schemaExporter.InitializeContext();

            schemaExporter.Database.CreateTables();
            schemaExporter.Export("ERP");

            schemaExporter.Context.ErrorList.Should().BeEmpty();


        }
        #endregion

      
    }
}