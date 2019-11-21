using System.Collections.Generic;
using BOA.DataFlow;
using BOA.EntityGeneration.UI.MainForm;

namespace BOA.EntityGeneration.UI.Container
{

    static class Data
    {
        public static IDataConstant<List<string>> SchemaNames = DataConstant.Create<List<string>>(nameof(SchemaNames));
        public static IDataConstant<MainWindowModel> Model = DataConstant.Create<MainWindowModel>(nameof(Model));
    }

    public partial class App
    {
        public static readonly IDataContext Context;

        static App()
        {
            var context = new DataContext();

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

        public static string CheckInComment => Data.Model[Context].CheckinComment;
    }
}
