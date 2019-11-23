using System.Collections.Generic;
using BOA.DataFlow;
using BOA.EntityGeneration.BOACardDatabaseSchemaToDllExporting.Util;
using BOA.Tasks;

namespace BOA.EntityGeneration.BOACardDatabaseSchemaToDllExporting.ProjectExporters
{
    public class MsBuildQueue
    {
        #region Static Fields
        public static readonly IProperty<bool>            BuildAfterCodeGenerationIsCompleted = Property.Create<bool>(nameof(BuildAfterCodeGenerationIsCompleted));
        public static readonly IProperty<MsBuildQueue>    MsBuildQueueId                      = Property.Create<MsBuildQueue>();
        public static readonly IProperty<ProcessContract> ProcessInfo                         = Property.Create<ProcessContract>(nameof(ProcessInfo));
        #endregion

        #region Fields
        readonly List<MSBuildData> Queue = new List<MSBuildData>();
        #endregion

        #region Public Methods
        public static void Build(IContext context)
        {
            context.Get(MsBuildQueueId).BuildInternal(context);
        }

        public void Push(MSBuildData data)
        {
            Queue.Add(data);
        }
        #endregion

        #region Methods
        void BuildInternal(IContext context)
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
        #endregion
    }
}