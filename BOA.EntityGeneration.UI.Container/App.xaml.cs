using System.Collections.Generic;
using BOA.EntityGeneration.UI.MainForm;

namespace BOA.EntityGeneration.UI.Container
{
    public partial class App
    {
        #region Static Fields
        public static readonly MainWindowModel Model;
        #endregion

        #region Constructors
        static App()
        {
            Model = new MainWindowModel
            {
                CheckinComment = CheckInCommentAccess.GetCheckInComment(),

                SchemaNames = new List<string>
                {
                    "BKM",
                    "BNS",
                    "CCA",
                    "CFG",
                    "CIS",
                    "CLR",
                    "COR",
                    "CRD",
                    "DBT",
                    "DLV",
                    "EMC",
                    "EMV",
                    "ESW",
                    "FRD",
                    "KMT",
                    "LOG",
                    "MRC",
                    "POS",
                    "PPD",
                    "PRM",
                    "RKL",
                    "STM",
                    "SWC",
                    "TMS",
                    "TRN",
                    "VIS",
                    "EMB",
                    "SYSOP",
                    "DIS",
                    "*"
                },
                ProfileNames = new List<string>
                {
                    "CC_OPERATIONS",
                    "CC_OPERATIONS_KERNEL",
                    "DLV_COURIER",
                    "*"
                }
            };
        }
        #endregion
    }
}