﻿using System;
using BOA.Common.Helpers;

namespace BOA.EntityGeneration.ConstantsProjectGeneration
{
    [Serializable]
    public class ConstantsProjectGenerationConfig
    {
        #region Public Properties
        public string ConnectionString                        { get; set; }
        public bool   IntegrateWithTFSAndCheckInAutomatically { get; set; }
        public string NamespaceName { get; set; }
        public string ProjectDirectory { get; set; }
        #endregion

        public static ConstantsProjectGenerationConfig CreateFromFile()
        {
            return YamlHelper.DeserializeFromFile<ConstantsProjectGenerationConfig>( nameof(ConstantsProjectGenerationConfig) + ".yaml");
        }
    }
}