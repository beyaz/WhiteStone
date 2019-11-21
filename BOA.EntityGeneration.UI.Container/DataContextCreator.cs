using System.Collections.Generic;
using BOA.DataFlow;
using BOA.EntityGeneration.UI.MainForm;
using static BOA.EntityGeneration.UI.Container.Data;

namespace BOA.EntityGeneration.UI.Container
{
    class DataContextCreator
    {
        #region Public Methods
        public IDataContext Create()
        {
            var context = new DataContext();

            SchemaNames[context] = new List<string>
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
            };





            Model[context] = new MainWindowModel
            {
                CheckinComment = CheckInCommentAccess.GetCheckInComment()
            };

            return context;
        }
        #endregion
    }
}