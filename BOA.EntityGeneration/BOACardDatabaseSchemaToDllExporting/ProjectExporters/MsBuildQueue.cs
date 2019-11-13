using System.Collections.Generic;
using ___Company___.DataFlow;
using ___Company___.EntityGeneration.DataFlow;
using BOA.Tasks;

namespace BOA.EntityGeneration.BOACardDatabaseSchemaToDllExporting.ProjectExporters
{
    public class MsBuildQueue
    {
        #region Fields
        readonly List<MSBuildData> Queue = new List<MSBuildData>();
        #endregion

        #region Public Methods
         void BuildInternal(IDataContext context)
        {
            var config   = context.Get(Data.Config);
            var progress = context.Get(Data.SchemaGenerationProcess);

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

        public static void Build(IDataContext context)
        {
            context.Get(Data.MsBuildQueue).BuildInternal(context);
        }
        #endregion
    }
}