using System;
using BOA.EntityGeneration.DbModel;

namespace BOA.EntityGeneration.BOACardCustomSqlIntoProjectInjection.Models
{
    [Serializable]
    public class CustomSqlInfoResult
    {
        #region Public Properties
        public string DataType { get; set; }

        public string DataTypeInDotnet { get; set; }
        public string Name             { get; set; }

        public string           NameInDotnet    { get; set; }
        public SqlReaderMethods SqlReaderMethod { get; set; }
        public bool IsNullable { get; set; }
        #endregion
    }
}