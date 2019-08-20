using System;
using BOA.CodeGeneration.Util;

namespace BOA.TfsAccess.Tasks
{
    [Serializable]
    public sealed class CheckoutFileData
    {
        #region Public Properties
        public string Path { get; set; }
        #endregion
    }

    public class CheckoutFile
    {
        #region Public Methods
        public static void Run(CheckoutFileData data)
        {
            TFSAccessForBOA.CheckoutFile(data.Path);
        }
        #endregion
    }
}