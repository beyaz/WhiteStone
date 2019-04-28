using System;
using System.Collections.Generic;

namespace BOA.EntityGeneration.BOACardCustomSqlIntoProjectInjection
{
    [Serializable]
    public class CustomSqlInfo
    {
        #region Public Properties
        public string Name { get; set; }

        public string                                ParameterContractName => Name.ToContractName() + "Request";
        public IReadOnlyList<CustomSqlInfoParameter> Parameters            { get; set; }
        public IReadOnlyList<CustomSqlInfoResult>    ResultColumns         { get; set; }

        public string ResultContractName => Name.ToContractName() + "Contract";

        public string BusinessClassName => Name.ToContractName();

        public string BusinessClassNamespace { get; set; }

        public string Sql { get; set; }
        #endregion
    }
}