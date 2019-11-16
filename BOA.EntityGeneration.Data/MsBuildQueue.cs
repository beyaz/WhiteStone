using System.Collections.Generic;
using BOA.DataFlow;
using BOA.EntityGeneration.BOACardDatabaseSchemaToDllExporting.Util;
using BOA.Tasks;

namespace BOA.EntityGeneration.BOACardDatabaseSchemaToDllExporting.ProjectExporters
{
    public class MsBuildQueue
    {
        public static readonly IDataConstant<MsBuildQueue> MsBuildQueueId = DataConstant.Create<MsBuildQueue>();
        public static readonly IDataConstant<bool> BuildAfterCodeGenerationIsCompleted = DataConstant.Create<bool>(nameof(BuildAfterCodeGenerationIsCompleted));
        public static readonly IDataConstant<ProcessInfo> ProcessInfo = DataConstant.Create<ProcessInfo>(nameof(ProcessInfo));

        #region Fields
        readonly List<MSBuildData> Queue = new List<MSBuildData>();
        #endregion

        #region Public Methods
         void BuildInternal(IDataContext context)
        {
      

            if (!context.TryGet(BuildAfterCodeGenerationIsCompleted))
            {
                return;
            }

            var progress = context.Get(ProcessInfo);

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
            context.Get<MsBuildQueue>(MsBuildQueueId).BuildInternal(context);
        }
        #endregion
    }
}