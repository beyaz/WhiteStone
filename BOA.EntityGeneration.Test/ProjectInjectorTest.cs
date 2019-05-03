using System.Collections.Generic;
using BOA.EntityGeneration.BOACardCustomSqlIntoProjectInjection;
using BOA.EntityGeneration.BOACardCustomSqlIntoProjectInjection.AllInOne;
using BOA.EntityGeneration.BOACardCustomSqlIntoProjectInjection.DataAccess;
using BOA.EntityGeneration.BOACardCustomSqlIntoProjectInjection.Model;
using BOA.EntityGeneration.BOACardDatabaseSchemaToDllExporting;
using BOA.EntityGeneration.BOACardDatabaseSchemaToDllExporting.Exporters;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ninject;

namespace BOA.EntityGeneration.SchemaToDllExporting
{
    [TestClass]
    public class ProjectInjectorTest
    {
        #region Public Methods
        [TestMethod]
        public void RequestPropertiesShouldMatchSqlInputs()
        {
            using (var kernel = new Kernel())
            {
                kernel.Bind<ProjectCustomSqlInfoDataAccess>().To<ProjectCustomSqlInfoDataAccess2>();

                var projectCustomSqlInfo = kernel.Get<ProjectCustomSqlInfoDataAccess>().GetByProfileId(string.Empty);

                var code = kernel.Get<AllInOneForTypeDll>().GetCode(projectCustomSqlInfo);

                code.Should().Contain("public long? RecordId { get; set; }");
                code.Should().Contain("public long RecordIdNotNull { get; set; }");
                code.Should().Contain("public int? CustomerId { get; set; }");
                code.Should().Contain("public bool? SupplementaryCardFlag { get; set; }");
                code.Should().Contain("public string CardRefNumber { get; set; }");
                code.Should().Contain("public string MainCardRefNumber { get; set; }");
                code.Should().Contain("public DateTime? PartDate { get; set; }");
                code.Should().Contain("public DateTime? PartDateTime { get; set; }");
                code.Should().Contain("public long? PartLong { get; set; }");
                code.Should().Contain("public string PartString { get; set; }");
                code.Should().Contain("public bool? PartBit { get; set; }");
                code.Should().Contain("public decimal? PartDecimal { get; set; }");
                code.Should().Contain("public short? PartShort { get; set; }");
                code.Should().Contain("public short? PartSmallInt { get; set; }");
            }
        }

        [TestMethod]
        public void SqlParametersShouldMatchRequestProperties()
        {
            using (var kernel = new Kernel())
            {
                kernel.Bind<ProjectCustomSqlInfoDataAccess>().To<ProjectCustomSqlInfoDataAccess2>();

                var projectCustomSqlInfo = kernel.Get<ProjectCustomSqlInfoDataAccess>().GetByProfileId(string.Empty);

                var code = kernel.Get<AllInOneForBusinessDll>().GetCode(projectCustomSqlInfo);

                code.Should().Contain("DBLayer.AddInParameter(command, \"@RECORD_ID\", SqlDbType.BigInt, request.RecordId);");
                code.Should().Contain("DBLayer.AddInParameter(command, \"@RECORD_ID_NOT_NULL\", SqlDbType.BigInt, request.RecordIdNotNull);");
                code.Should().Contain("DBLayer.AddInParameter(command, \"@Customer_ID\", SqlDbType.Int, request.CustomerId);");
                code.Should().Contain("DBLayer.AddInParameter(command, \"@SUPPLEMENTARY_CARD_FLAG\", SqlDbType.VarChar, request.SupplementaryCardFlag);");
                code.Should().Contain("DBLayer.AddInParameter(command, \"@CARD_REF_NUMBER\", SqlDbType.VarChar, request.CardRefNumber);");
                code.Should().Contain("DBLayer.AddInParameter(command, \"@MAIN_CARD_REF_NUMBER\", SqlDbType.Char, request.MainCardRefNumber);");

                code.Should().Contain("DBLayer.AddInParameter(command, \"@PART_DATE\", SqlDbType.DateTime, request.PartDate);");
                code.Should().Contain("DBLayer.AddInParameter(command, \"@PART_DATE_TIME\", SqlDbType.DateTime, request.PartDateTime);");
                code.Should().Contain("DBLayer.AddInParameter(command, \"@PART_LONG\", SqlDbType.BigInt, request.PartLong);");
                code.Should().Contain("DBLayer.AddInParameter(command, \"@PART_STRING\", SqlDbType.VarChar, request.PartString);");
                code.Should().Contain("DBLayer.AddInParameter(command, \"@PART_BIT\", SqlDbType.Bit, request.PartBit);");
                code.Should().Contain("DBLayer.AddInParameter(command, \"@PART_DECIMAL\", SqlDbType.Decimal, request.PartDecimal);");
                code.Should().Contain("DBLayer.AddInParameter(command, \"@PART_SHORT\", SqlDbType.SmallInt, request.PartShort);");
                code.Should().Contain("DBLayer.AddInParameter(command, \"@PART_SMALL_INT\", SqlDbType.SmallInt, request.PartSmallInt);");
            }
        }
        #endregion

        class ProjectCustomSqlInfoDataAccess2 : ProjectCustomSqlInfoDataAccess
        {
            #region Methods
            protected override ProjectCustomSqlInfo GetByProfileIdFromDatabase(string profileId)
            {
                return new ProjectCustomSqlInfo
                {
                    CustomSqlInfoList = new[]
                    {
                        new CustomSqlInfo
                        {
                            Name = "AA",
                            Parameters = new[]
                            {
                                new CustomSqlInfoParameter
                                {
                                    Name       = "RECORD_ID",
                                    DataType   = "bigInt",
                                    IsNullable = true
                                },
                                new CustomSqlInfoParameter
                                {
                                    Name       = "RECORD_ID_NOT_NULL",
                                    DataType   = "bigInt",
                                    IsNullable = false
                                },

                                new CustomSqlInfoParameter
                                {
                                    Name       = "Customer_ID",
                                    DataType   = "Int",
                                    IsNullable = true
                                },
                                new CustomSqlInfoParameter
                                {
                                    Name       = "SUPPLEMENTARY_CARD_FLAG",
                                    DataType   = "Varchar",
                                    IsNullable = true
                                },

                                new CustomSqlInfoParameter
                                {
                                    Name       = "CARD_REF_NUMBER",
                                    DataType   = "Varchar",
                                    IsNullable = true
                                },

                                new CustomSqlInfoParameter
                                {
                                    Name       = "MAIN_CARD_REF_NUMBER",
                                    DataType   = "char",
                                    IsNullable = true
                                },

                                new CustomSqlInfoParameter
                                {
                                    Name       = "PART_DATE",
                                    DataType   = "daTe",
                                    IsNullable = true
                                },
                                new CustomSqlInfoParameter
                                {
                                    Name       = "PART_DATE_TIME",
                                    DataType   = "daTeTime",
                                    IsNullable = true
                                },

                                new CustomSqlInfoParameter
                                {
                                    Name       = "PART_LONG",
                                    DataType   = "long",
                                    IsNullable = true
                                },
                                new CustomSqlInfoParameter
                                {
                                    Name       = "PART_STRING",
                                    DataType   = "string",
                                    IsNullable = true
                                },

                                new CustomSqlInfoParameter
                                {
                                    Name       = "PART_BIT",
                                    DataType   = "bit",
                                    IsNullable = true
                                },

                                new CustomSqlInfoParameter
                                {
                                    Name       = "PART_DECIMAL",
                                    DataType   = "decimal",
                                    IsNullable = true
                                },

                                new CustomSqlInfoParameter
                                {
                                    Name       = "PART_SHORT",
                                    DataType   = "Int16",
                                    IsNullable = true
                                },

                                new CustomSqlInfoParameter
                                {
                                    Name       = "PART_SMALL_INT",
                                    DataType   = "smallint",
                                    IsNullable = true
                                }
                            },
                            ResultColumns = new List<CustomSqlInfoResult>()
                        }
                    }
                };
            }
            #endregion
        }
    }
}