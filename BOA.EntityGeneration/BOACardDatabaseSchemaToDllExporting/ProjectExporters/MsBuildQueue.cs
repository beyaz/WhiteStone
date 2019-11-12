using System.Collections.Generic;
using BOA.EntityGeneration.BOACardDatabaseSchemaToDllExporting.Util;
using BOA.Tasks;
using Ninject;
using static ___Company___.EntityGeneration.DataFlow.DataContext;
using ___Company___.EntityGeneration.DataFlow;

namespace BOA.EntityGeneration.BOACardDatabaseSchemaToDllExporting.ProjectExporters
{
    public class MsBuildQueue
    {
        #region Fields
        readonly List<MSBuildData> Queue = new List<MSBuildData>();
        #endregion

        #region Public Properties
        [Inject]
        public Config Config { get; set; }

     

        #endregion

        #region Public Methods
        public void Build()
        {
            var progress = Context.Get(Data.SchemaGenerationProcess);

            if (!Config.BuildAfterCodeGenerationIsCompleted)
            {
                return;
            }

            foreach (var data in Queue)
            {
                progress.Text = "Compile started." + data.ProjectFilePath;
                MSBuild.Build(data);
                progress.Text = "Compile finished." + data.ProjectFilePath;
            }
        }

        public void Push(MSBuildData data)
        {
            Queue.Add(data);
        }
        #endregion
    }
}