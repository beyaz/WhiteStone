using System.Collections.Generic;
using BOA.DataFlow;
using BOA.EntityGeneration.UI.MainForm;

namespace BOA.EntityGeneration.UI.Container
{
    static class Data
    {
        #region Static Fields
        public static Property<MainWindowModel> Model       = Property.Create<MainWindowModel>(nameof(Model));
        public static Property<List<string>>    SchemaNames = Property.Create<List<string>>(nameof(SchemaNames));
        #endregion
    }

    public partial class App
    {
        #region Static Fields
        public static readonly IContext Context;
        #endregion

        #region Constructors
        static App()
        {
            var context = new Context();

            Data.SchemaNames[context] = new List<string>
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

            Data.Model[context] = new MainWindowModel
            {
                CheckinComment = CheckInCommentAccess.GetCheckInComment()
            };

            Context = context;
        }
        #endregion

        #region Public Properties
        public static string CheckInComment => Data.Model[Context].CheckinComment;
        #endregion
    }
}