using System.IO;
using ___Company___.DataFlow;
using ___Company___.EntityGeneration.DataFlow;
using BOA.Common.Helpers;
using BOA.EntityGeneration.BOACardDatabaseSchemaToDllExporting.AllInOne;
using BOA.EntityGeneration.BOACardDatabaseSchemaToDllExporting.ClassWriters;
using BOA.EntityGeneration.BOACardDatabaseSchemaToDllExporting.DataAccess;
using BOA.EntityGeneration.BOACardDatabaseSchemaToDllExporting.Exporters;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using static ___Company___.EntityGeneration.DataFlow.Data;
using static ___Company___.EntityGeneration.DataFlow.DataEvent;

namespace BOA.EntityGeneration.DbModel.SqlServerDataAccess
{
    [TestClass]
    public class SampleDatabaseTest
    {
        #region Constants
        const string ExpectedResultsDirectory = @"D:\github\WhiteStone\BOA.EntityGeneration.Test\SampleDatabaseTest.ExpectedResults\";
        #endregion

        #region Static Fields
        static readonly IDataConstant<PaddedStringBuilder> EntityFileTemp  = DataConstant.Create<PaddedStringBuilder>();
        static readonly IDataConstant<PaddedStringBuilder> SharedRepositoryFileTemp = DataConstant.Create<PaddedStringBuilder>();
        #endregion

        #region Fields
        readonly IDataContext context;
        #endregion

        #region Constructors
        public SampleDatabaseTest()
        {
            context = new TestDataContextCreator().Create();

            context.CreateTables();

            BOACardDatabaseExporter.Export(context, "ERP");
        }
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
            var expected = File.ReadAllText(ExpectedResultsDirectory + @"ERP\BOA.Business.Kernel.Card.ERP\All.cs.txt");
            var value    = context.Get(SharedRepositoryFileTemp).ToString();

            StringHelper.IsEqualAsData(value, expected).Should().BeTrue();
        }
        #endregion

        #region Methods
        static void HoldSomeDataForCheckingTestResults(IDataContext context)
        {
            context.Add(EntityFileTemp, context.Get(EntityFile));
            context.Add(SharedRepositoryFileTemp, context.Get(SharedRepositoryFile));
        }
        #endregion

        class TestDataContextCreator : DataContextCreator
        {
            #region Constructors
            public TestDataContextCreator()
            {
                ConfigFilePath      = @"D:\github\WhiteStone\BOA.EntityGeneration.Test\BOA.EntityGeneration.json";
                IsFileAccessWithTfs = false;
            }
            #endregion

            #region Methods
            protected override void AttachEvents(IDataContext context)
            {
                context.AttachEvent(StartToExportTable, GeneratorOfTypeClass.WriteClass);
                context.AttachEvent(StartToExportTable, GeneratorOfBusinessClass.CreateBusinessClassWriterContext);
                context.AttachEvent(StartToExportTable, GeneratorOfBusinessClass.WriteClass);
                context.AttachEvent(StartToExportTable, SharedDalClassWriter.Write);
                context.AttachEvent(StartToExportTable, GeneratorOfBusinessClass.RemoveBusinessClassWriterContext);

                context.AttachEvent(StartToExportSchema, SharedDalClassWriter.WriteUsingList);
                context.AttachEvent(StartToExportSchema, GeneratorOfTypeClass.WriteUsingList);
                context.AttachEvent(StartToExportSchema, GeneratorOfBusinessClass.WriteUsingList);
                context.AttachEvent(StartToExportSchema, GeneratorOfTypeClass.BeginNamespace);
                context.AttachEvent(StartToExportSchema, AllBusinessClassesInOne.BeginNamespace);
                context.AttachEvent(StartToExportSchema, Events.OnSchemaStartedToExport);
                context.AttachEvent(StartToExportSchema, SharedDalClassWriter.EndNamespace);
                context.AttachEvent(StartToExportSchema, GeneratorOfTypeClass.EndNamespace);
                context.AttachEvent(StartToExportSchema, GeneratorOfBusinessClass.EndNamespace);

                context.AttachEvent(StartToExportSchema, HoldSomeDataForCheckingTestResults);
            }
            #endregion
        }
    }
}