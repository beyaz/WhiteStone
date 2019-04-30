using System.Collections.Generic;
using BOA.EntityGeneration.BOACardCustomSqlIntoProjectInjection;
using BOA.EntityGeneration.BOACardDatabaseSchemaToDllExporting;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ninject;

namespace BOA.EntityGeneration.SchemaToDllExporting
{
    [TestClass]
    public class ProjectInjectorTest
    {
        #region Enums
        enum Rule
        {
            when_input_data_type_is_BigInt_then_request_property_type_should_be_long,
            when_input_data_type_is_Int_then_request_property_type_should_be_int,
            when_input_data_type_is_VarChar_and_Name_contains_FLAG_suffix_then_request_property_type_should_be_boolean,
            when_input_data_type_is_VarChar_then_request_property_type_should_be_string,
            when_input_data_type_is_Char_then_request_property_type_should_be_string
        }
        #endregion


        [TestMethod]
        public void SqlParametersShouldMatchRequestProperties()
        {
            using (var kernel = new Kernel())
            {
                kernel.Bind<DataAccess>().To<DataAccess2>();

                var projectCustomSqlInfo = kernel.Get<DataAccess>().GetByProfileId(string.Empty);

                var code = kernel.Get<AllInOneForBusinessDll>().GetCode(projectCustomSqlInfo);

                code.Should().Contain("DBLayer.AddInParameter(command, \"@RECORD_ID\", SqlDbType.BigInt, request.RecordId);");
                code.Should().Contain("DBLayer.AddInParameter(command, \"@RECORD_ID_NOT_NULL\", SqlDbType.BigInt, request.RecordIdNotNull);");
                code.Should().Contain("DBLayer.AddInParameter(command, \"@Customer_ID\", SqlDbType.Int, request.CustomerId);");
                code.Should().Contain("DBLayer.AddInParameter(command, \"@SUPPLEMENTARY_CARD_FLAG\", SqlDbType.VarChar, request.SupplementaryCardFlag);");
                code.Should().Contain("DBLayer.AddInParameter(command, \"@CARD_REF_NUMBER\", SqlDbType.VarChar, request.CardRefNumber);");
                code.Should().Contain("DBLayer.AddInParameter(command, \"@MAIN_CARD_REF_NUMBER\", SqlDbType.Char, request.MainCardRefNumber);");
                


            }
        }
        #region Public Methods
        [TestMethod]
        public void RequestPropertiesShouldMatchSqlInputs()
        {
            using (var kernel = new Kernel())
            {
                kernel.Bind<DataAccess>().To<DataAccess2>();

                var projectCustomSqlInfo = kernel.Get<DataAccess>().GetByProfileId(string.Empty);

                var code = kernel.Get<AllInOneForTypeDll>().GetCode(projectCustomSqlInfo);

                code.Should().Contain("public long? RecordId { get; set; }",
                                      Rule.when_input_data_type_is_BigInt_then_request_property_type_should_be_long.ToString());

                code.Should().Contain("public long RecordIdNotNull { get; set; }",
                                      Rule.when_input_data_type_is_BigInt_then_request_property_type_should_be_long.ToString());

                code.Should().Contain("public int? CustomerId { get; set; }",
                                      Rule.when_input_data_type_is_Int_then_request_property_type_should_be_int.ToString());

                code.Should().Contain("public bool? SupplementaryCardFlag { get; set; }",
                                      Rule.when_input_data_type_is_VarChar_and_Name_contains_FLAG_suffix_then_request_property_type_should_be_boolean.ToString());

                code.Should().Contain("public string CardRefNumber { get; set; }",
                                      Rule.when_input_data_type_is_VarChar_then_request_property_type_should_be_string.ToString());


                code.Should().Contain("public string MainCardRefNumber { get; set; }",
                                      Rule.when_input_data_type_is_Char_then_request_property_type_should_be_string.ToString());


            }
        }
        #endregion

        class DataAccess2 : DataAccess
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