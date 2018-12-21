using System;
using System.Collections.Generic;
using System.IO;
using BOA.Common.Helpers;
using WhiteStone;

namespace BOAPlugins.Utility
{
    [Serializable]
    public class Configuration
    {

        
            

        public bool CheckInSolutionIsEnabled { get; set; }
        public Dictionary<string,string> SolutionCheckInComments { get; set; }
        #region Public Properties
        public bool                        DeepEndsAssembliesDownloaded { get; set; }
        public IReadOnlyCollection<string> ServerFiles                  { get; set; }
        #endregion

        #region Public Methods
        public static void LoadFromFile()
        {
            var configurationAsString = File.ReadAllText(ConstConfiguration.ConfigurationJsonFilePath);

            SM.Set(JsonHelper.Deserialize<Configuration>(configurationAsString));
        }

        public static void SaveToFile()
        {
            File.WriteAllText(ConstConfiguration.ConfigurationJsonFilePath, JsonHelper.Serialize(SM.Get<Configuration>()));
        }
        #endregion
    }
}