using System;
using System.Collections.Generic;

namespace BOAPlugins.Utility
{
    [Serializable]
    public class CheckInInformation
    {
        #region Public Properties
        public Dictionary<string, string> SolutionCheckInComments { get; set; } = new Dictionary<string, string>();
        #endregion
    }

    [Serializable]
    public class Configuration
    {
        #region Public Properties
        public bool CheckInSolutionIsEnabled { get; set; }
        #endregion
    }
}