using System;

namespace BOA.EntityGeneration.ConstantsProjectGeneration
{
    [Serializable]
    public class ConfigurationContract
    {
        #region Public Properties
        public string ConnectionString                        { get; set; }
        public bool   IntegrateWithTFSAndCheckInAutomatically { get; set; }
        public string NamespaceName { get; set; }
        public string ProjectDirectory { get; set; }
        #endregion
    }
}