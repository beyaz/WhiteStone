using System;
using System.Collections.Generic;
using BOA.EntityGeneration.DbModel;


namespace BOA.EntityGeneration.ScriptModel
{
    public static class Padding
    {
        public const string ForComment = "     ";
    }
}
namespace BOA.EntityGeneration.ScriptModel
{

    [Serializable]
    public class DeleteInfo
    {
        #region Public Properties
        public string                    Sql           { get; set; }
        public IReadOnlyList<ColumnInfo> SqlParameters { get; set; }
        #endregion
    }


    [Serializable]
    public class ContractCommentInfo
    {
        #region Public Properties
        public string                    Comment           { get; set; }
        #endregion
    }
}