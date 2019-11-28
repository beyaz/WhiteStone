﻿using System;
using System.Collections.Generic;
using System.IO;
using BOA.Tasks;

namespace BOA.EntityGeneration.BOACardDatabaseSchemaToDllExporting.ProjectExporters
{
    public class MsBuildQueue
    {
        #region Fields
        readonly List<MSBuildData> Queue = new List<MSBuildData>();
        #endregion

        #region Public Properties
        public bool           BuildAfterCodeGenerationIsCompleted { get; set; } = true;
        public Action<string> Trace                               { get; set; }
        public Action<Exception> OnError { get; set; }
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
                var fileName = Path.GetFileName(data.ProjectFilePath);

                Trace("Compile started." + fileName);
                MSBuild.Build(data);
                if (data.BuildError == null)
                {
                    Trace($"Compile successfully finished. {fileName}");
                }
                else
                {
                    OnError(data.BuildError);
                }
            }
        }

        public void Push(string csprojFilePath)
        {
            Queue.Add(new MSBuildData {ProjectFilePath = csprojFilePath});
        }
        #endregion
    }
}