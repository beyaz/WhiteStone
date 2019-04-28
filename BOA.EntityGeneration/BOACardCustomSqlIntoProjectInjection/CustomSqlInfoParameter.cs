using System;

namespace BOA.EntityGeneration.BOACardCustomSqlIntoProjectInjection
{
    [Serializable]
    public class CustomSqlInfoParameter
    {
        #region Public Properties
        public string DataType         { get; set; }
        public string DataTypeInDotnet { get; set; }
        public string Name             { get; set; }

        public string NameInDotnet        { get; set; }
        public string SqlDatabaseTypeName { get; set; }
        public bool IsNullable { get; set; }
        #endregion
    }
}