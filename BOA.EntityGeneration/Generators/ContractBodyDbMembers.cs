using System;
using System.Text;
using BOA.Common.Helpers;
using BOA.EntityGeneration.Common;
using BOA.EntityGeneration.DbModel;

namespace BOA.EntityGeneration.Generators
{
    class ContractBodyDbMembers : GeneratorBase
    {
        #region Public Methods
        public override string ToString()
        {
            var sb = new StringBuilder();

            sb.AppendLine("#region Database Columns");
            foreach (var columnInfo in TableInfo.Columns)
            {
                sb.AppendLine();
                Write(sb, columnInfo);
            }

            sb.AppendLine();
            sb.AppendLine("#endregion");

            return sb.ToString();
        }
        #endregion

        #region Methods
        static void Write(StringBuilder sb, ColumnInfo data)
        {

            var comment = data.Comment;

            if (comment.HasValue())
            {
                sb.AppendLine("/// <summary>");
                var commentList    = comment.Split(Environment.NewLine.ToCharArray());
                var isFirstComment = true;
                foreach (var item in commentList)
                {
                    if (isFirstComment)
                    {
                        isFirstComment = false;
                        sb.AppendLine("///" + PaddingForComment + "" + item);
                    }
                    else
                    {
                        sb.AppendLine("///" + PaddingForComment + "<para> " + item + " </para>");
                    }
                }

                sb.AppendLine(@"/// </summary>");
            }

            var dotNetType = data.DotNetType;
            if (data.ColumnName == Names.VALID_FLAG)
            {
                dotNetType = DotNetTypeName.DotNetBool;
            }

            sb.AppendLine("public " + dotNetType + " " + data.ColumnName.ToContractName() + " { get; set; }");
        }
        #endregion
    }
}