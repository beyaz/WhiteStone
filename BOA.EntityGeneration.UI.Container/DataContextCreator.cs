using System.Collections.Generic;
using BOA.DataFlow;
using static BOA.EntityGeneration.UI.Container.Data;

namespace BOA.EntityGeneration.UI.Container
{
    class DataContextCreator
    {
        #region Public Methods
        public IDataContext Create()
        {
            var dataContext = new DataContext();

            SchemaNames[dataContext] = new List<string>
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

            return dataContext;
        }
        #endregion
    }
}