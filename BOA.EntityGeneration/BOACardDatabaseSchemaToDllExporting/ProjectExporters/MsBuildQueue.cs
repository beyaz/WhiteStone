using System.Collections.Generic;
using ___Company___.EntityGeneration.DataFlow;
using BOA.Tasks;
using static ___Company___.EntityGeneration.DataFlow.DataContext;

namespace BOA.EntityGeneration.BOACardDatabaseSchemaToDllExporting.ProjectExporters
{
    public class MsBuildQueue
    {
        #region Fields
        readonly List<MSBuildData> Queue = new List<MSBuildData>();
        #endregion

        #region Public Methods
        public void Build()
        {
            var config   = Context.Get(Data.Config);
            var progress = Context.Get(Data.SchemaGenerationProcess);

            if (!config.BuildAfterCodeGenerationIsCompleted)
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