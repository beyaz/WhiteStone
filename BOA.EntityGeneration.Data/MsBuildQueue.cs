using System;
using System.Collections.Generic;
using BOA.Tasks;

namespace BOA.EntityGeneration.BOACardDatabaseSchemaToDllExporting.ProjectExporters
{
    public class MsBuildQueue
    {
        #region Fields
        readonly List<MSBuildData> Queue = new List<MSBuildData>();
        #endregion

        #region Public Properties
        public bool           BuildAfterCodeGenerationIsCompleted { get; set; }
        public Action<string> Trace                               { get; set; }
        #endregion

        #region Public Methods
        public void Build()
        {
            if (!BuildAfterCodeGenerationIsCompleted)
            {
                return;
            }

            foreach (var data in Queue)
            {
                Trace("Compile started." + data.ProjectFilePath);
                MSBuild.Build(data);
                Trace("Compile finished." + data.ProjectFilePath);
            }
        }

        public void Push(string csprojFilePath)
        {
            Queue.Add(new MSBuildData {ProjectFilePath = csprojFilePath});
        }
        #endregion
    }
}