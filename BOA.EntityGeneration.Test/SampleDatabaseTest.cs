using System.IO;
using BOA.DataFlow;
using BOA.Common.Helpers;
using BOA.EntityGeneration.BOACardDatabaseSchemaToDllExporting.ClassWriters;
using BOA.EntityGeneration.BOACardDatabaseSchemaToDllExporting.Exporters;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using static BOA.EntityGeneration.DataFlow.Data;

namespace BOA.EntityGeneration.DbModel.SqlServerDataAccess
{
    [TestClass]
    public partial class SampleDatabaseTest
    {
        #region Constants
        const string ExpectedResultsDirectory = @"D:\github\WhiteStone\BOA.EntityGeneration.Test\SampleDatabaseTest.ExpectedResults\";
        #endregion

        #region Static Fields
        static readonly IDataConstant<PaddedStringBuilder> EntityFileTemp           = DataConstant.Create<PaddedStringBuilder>();
        static readonly IDataConstant<PaddedStringBuilder> SharedRepositoryFileTemp = DataConstant.Create<PaddedStringBuilder>();
        #endregion

        #region Fields
        IDataContext context;
        #endregion 

        #region Public Methods
        [TestMethod]
        public void CheckEntityFiles()
        {
            var expected = File.ReadAllText(ExpectedResultsDirectory + @"ERP\BOA.Types.Kernel.Card.ERP\All.cs.txt");
            var value    = context.Get(EntityFileTemp).ToString();

            StringHelper.IsEqualAsData(value, expected).Should().BeTrue();
        }

        [TestMethod]
        public void CheckSharedRepository()
        {
            var expected = File.ReadAllText(ExpectedResultsDirectory + @"ERP\BOA.Business.Kernel.Card.ERP\Shared.cs.txt");
            var value    = context.Get(SharedRepositoryFileTemp).ToString();


            StringHelper.IsEqualAsData(value, expected).Should().BeTrue();
        }

        [TestCleanup]
        public void Clean()
        {
            context.Get(Database).Rollback();
        }

        [TestInitialize]
        public void Initialize()
        {
            context = new TestDataContextCreator().Create();

            context.CreateTables();

            BOACardDatabaseExporter.Export(context, "ERP");
        }
        #endregion

        #region Methods
        static void HoldSomeDataForCheckingTestResults(IDataContext context)
        {
            // TODO:fixme: context.Add(EntityFileTemp, context.Get(EntityFileExporter.File));
            // context.Add(SharedRepositoryFileTemp, context.Get(SharedFileExporter.File));
        }
        #endregion
    }
}