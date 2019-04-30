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
        #region Public Methods
        [TestMethod]
        public void BigIntInputsShouldBePropertyTypeAsLong()
        {
            using (var kernel = new Kernel())
            {
                kernel.Bind<DataAccess>().To<DataAccess2>();

                var projectCustomSqlInfo = kernel.Get<DataAccess>().GetByProfileId(string.Empty);

                var code = kernel.Get<AllInOneForTypeDll>().GetCode(projectCustomSqlInfo);

                code.Should().Contain("public long? RecordId {get; set; }");
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