using System;

namespace BOA.EntityGeneration.BOACardCustomSqlIntoProjectInjection.Model
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