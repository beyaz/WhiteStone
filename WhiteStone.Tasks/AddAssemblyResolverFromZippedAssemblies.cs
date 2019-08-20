using System;
using BOA.Common.Helpers;

namespace WhiteStone.Tasks
{

    [Serializable]
    public class AddAssemblyResolverFromZippedAssembliesData
    {
        #region Public Properties
        public string ZipFilePath { get; set; }
        #endregion
    }

    public static class AddAssemblyResolverFromZippedAssemblies
    {
        #region Public Methods
        public static void Run(AddAssemblyResolverFromZippedAssembliesData data)
        {
            new ZipAssemblyResolver(data.ZipFilePath).AttachToCurrentDomain();
        }
        #endregion
    }
}