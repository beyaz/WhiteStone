using System.Collections.Generic;
using BOA.EntityGeneration.BOACardCustomSqlIntoProjectInjection.AllInOne;
using BOA.EntityGeneration.BOACardCustomSqlIntoProjectInjection.DataAccess;
using BOA.EntityGeneration.BOACardCustomSqlIntoProjectInjection.Models;
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
                code.Should().Contain("public string PartChar { get; set; }");
                code.Should().Contain("public bool? PartCharWithFlag { get; set; }");
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
                code.Should().Contain("DBLayer.AddInParameter(command, \"@PART_CHAR\", SqlDbType.Char, request.PartChar);");
                code.Should().Contain("DBLayer.AddInParameter(command, \"@PART_CHAR_WITH_FLAG\", SqlDbType.Char, request.PartCharWithFlag ? \"1\" : \"0\");");


                code.Should().Contain("contract.RecordId = SQLDBHelper.GetInt64NullableValue(reader[\"RECORD_ID\"]);");
                code.Should().Contain("contract.RecordIdNotNull = SQLDBHelper.GetInt64Value(reader[\"RECORD_ID_NOT_NULL\"]);");
                code.Should().Contain("contract.CustomerId = SQLDBHelper.GetInt32NullableValue(reader[\"Customer_ID\"]);");
                code.Should().Contain("contract.SupplementaryCardFlag = SQLDBHelper.GetBooleanNullableValue2(reader[\"SUPPLEMENTARY_CARD_FLAG\"]);");
                code.Should().Contain("contract.CardRefNumber = SQLDBHelper.GetStringValue(reader[\"CARD_REF_NUMBER\"]);");
                code.Should().Contain("contract.MainCardRefNumber = SQLDBHelper.GetStringValue(reader[\"MAIN_CARD_REF_NUMBER\"]);");
                code.Should().Contain("contract.PartDate = SQLDBHelper.GetDateTimeNullableValue(reader[\"PART_DATE\"]);");
                code.Should().Contain("contract.PartDateTime = SQLDBHelper.GetDateTimeNullableValue(reader[\"PART_DATE_TIME\"]);");
                code.Should().Contain("contract.PartLong = SQLDBHelper.GetInt64NullableValue(reader[\"PART_LONG\"]);");
                code.Should().Contain("contract.PartBit = SQLDBHelper.GetBooleanNullableValue(reader[\"PART_BIT\"]);");
                code.Should().Contain("contract.PartDecimal = SQLDBHelper.GetDecimalNullableValue(reader[\"PART_DECIMAL\"]);");
                code.Should().Contain("contract.PartSmallInt = SQLDBHelper.GetInt16NullableValue(reader[\"PART_SMALL_INT\"]);");
                code.Should().Contain("contract.PartChar = SQLDBHelper.GetStringValue(reader[\"PART_CHAR\"]);");
                code.Should().Contain("contract.PartCharWithFlag = SQLDBHelper.GetBooleanNullableValue2(reader[\"PART_CHAR_WITH_FLAG\"]);");
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
                                },
                                new CustomSqlInfoParameter
                                {
                                    Name       = "PART_CHAR",
                                    DataType   = "char",
                                    IsNullable = true
                                },
                                new CustomSqlInfoParameter
                                {
                                    Name       = "PART_CHAR_WITH_FLAG",
                                    DataType   = "char",
                                    IsNullable = true
                                }
                            },
                            ResultColumns = new List<CustomSqlInfoResult>
                            {
                                new CustomSqlInfoResult
                                {
                                    Name       = "RECORD_ID",
                                    DataType   = "bigInt",
                                    IsNullable = true
                                },
                                new CustomSqlInfoResult
                                {
                                    Name       = "RECORD_ID_NOT_NULL",
                                    DataType   = "bigInt",
                                    IsNullable = false
                                },

                                new CustomSqlInfoResult
                                {
                                    Name       = "Customer_ID",
                                    DataType   = "Int",
                                    IsNullable = true
                                },
                                new CustomSqlInfoResult
                                {
                                    Name       = "SUPPLEMENTARY_CARD_FLAG",
                                    DataType   = "Varchar",
                                    IsNullable = true
                                },

                                new CustomSqlInfoResult
                                {
                                    Name       = "CARD_REF_NUMBER",
                                    DataType   = "Varchar",
                                    IsNullable = true
                                },

                                new CustomSqlInfoResult
                                {
                                    Name       = "MAIN_CARD_REF_NUMBER",
                                    DataType   = "char",
                                    IsNullable = true
                                },

                                new CustomSqlInfoResult
                                {
                                    Name       = "PART_DATE",
                                    DataType   = "Date",
                                    IsNullable = true
                                },
                                new CustomSqlInfoResult
                                {
                                    Name       = "PART_DATE_TIME",
                                    DataType   = "daTeTime",
                                    IsNullable = true
                                },

                                new CustomSqlInfoResult
                                {
                                    Name       = "PART_LONG",
                                    DataType   = "long",
                                    IsNullable = true
                                },
                                
                                new CustomSqlInfoResult
                                {
                                    Name       = "PART_BIT",
                                    DataType   = "bit",
                                    IsNullable = true
                                },

                                new CustomSqlInfoResult
                                {
                                    Name       = "PART_DECIMAL",
                                    DataType   = "decimal",
                                    IsNullable = true
                                },

                                new CustomSqlInfoResult
                                {
                                    Name       = "PART_SMALL_INT",
                                    DataType   = "smallint",
                                    IsNullable = true
                                },
                                new CustomSqlInfoResult
                                {
                                    Name       = "PART_CHAR",
                                    DataType   = "char",
                                    IsNullable = true
                                },
                                new CustomSqlInfoResult
                                {
                                    Name       = "PART_CHAR_WITH_FLAG",
                                    DataType   = "char",
                                    IsNullable = true
                                }
                            }
                        }
                    }
                };
            }
            #endregion
        }
    }
}