using System.Collections.Generic;
using BOA.EntityGeneration.BOACardDatabaseSchemaToDllExporting.Util;
using Ninject;
using BOA.Tasks;

namespace BOA.EntityGeneration.BOACardDatabaseSchemaToDllExporting.ProjectExporters
{
    public class MsBuildQueue
    {
        #region Fields
        readonly List<MSBuildData> Queue = new List<MSBuildData>();
        #endregion

        #region Public Properties
        [Inject]
        public Tracer Tracer { get; set; }
        #endregion

        #region Public Methods
        public void Build()
        {
            foreach (var data in Queue)
            {
                Tracer.SchemaGenerationProcess.Text = "Compile started." + data.ProjectFilePath;
                MSBuild.Build(data);
                Tracer.SchemaGenerationProcess.Text = "Compile finished." + data.ProjectFilePath;
            }
        }

        public void Push(MSBuildData data)
        {
            Queue.Add(data);
        }
        #endregion
    }
}