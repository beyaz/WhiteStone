using System;
using BOA.Business.Kernel.Card.ERP.Repository;
using BOA.Common.Helpers;
using BOA.DatabaseAccess;
using BOA.EntityGeneration.SchemaToEntityExporting.Exporters;
using BOA.EntityGeneration.SchemaToEntityExporting.FileExporters.AllSchemaInOneClassRepositoryFile;
using BOA.EntityGeneration.SchemaToEntityExporting.FileExporters.EntityFileExporting;
using BOA.Types.Kernel.Card.ERP;
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

            TestDmlOperations(schemaExporter.Database);
        }
        #endregion

        #region Methods
        static void TestDmlOperations(IDatabase database)
        {
            var repository = new ERPRepository(database);

            var contract = new SampleTableContract
            {
                FieldVarbinary                = new byte[] {8},
                FieldChar                     = "y",
                FieldNchar                    = "t      ",
                FieldVarchar50                = "y",
                FieldNvarchar                 = "5",
                FieldVarchar50Nullable        = "y",
                RowGuid                       = "y",
                FieldSmalldatetime            = DateTime.Today,
                FieldDatetime                 = DateTime.Today,
                FieldDatetimeNullable         = DateTime.Today,
                FieldNumeric270Nullable       = 4,
                FieldIntNullable              = 5,
                FieldMoneyNullable            = 57.7M,
                FieldNvarcharNullable         = "012",
                FieldNcharNullable            = "4      ",
                FieldSmalldatetimeNullable    = DateTime.Today,
                FieldBigintNullable           = 6,
                FieldBitNullable              = true,
                FieldCharNullable             = "1",
                FieldDecimalNullable          = 3,
                FieldSmallintNullable         = 3,
                FieldTinyintNullable          = 5,
                FieldUniqueidentifierNullable = new Guid(),
                FieldVarbinaryNullable        = new byte[] {8},
                FieldIndex33                  = 4,
                UpdateDate                    = DateTime.Today,
                UpdateUserId                  = "5",
                UpdateTokenId                 = "5"
            };

            repository.Insert(contract);

            contract.SampleTableId.Should().Be(1);

            var dbRecord = repository.GetSampleTableBySampleTableId(contract.SampleTableId);

            dbRecord.InsertDate = contract.InsertDate;
            dbRecord.Should().BeEquivalentTo(contract);
            

            contract.FieldNvarcharNullable = "012345";

            repository.ModifySampleTable(contract);

            dbRecord = repository.GetSampleTableBySampleTableId(contract.SampleTableId);

            dbRecord.FieldNvarcharNullable.Should().Be("012345");

            dbRecord.UpdateDate = contract.UpdateDate;
            dbRecord.InsertDate = contract.InsertDate;
            dbRecord.Should().BeEquivalentTo(contract);

            repository.DeleteSampleTableContract(dbRecord.SampleTableId).Should().Be(1);


        }
        #endregion
    }
}