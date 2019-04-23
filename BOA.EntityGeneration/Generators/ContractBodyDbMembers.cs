using System;
using System.Collections.Generic;
using System.Linq;
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
                var extraComment = string.Empty;

                var indexInfo = TableInfo.IndexInfoList.FirstOrDefault(x => x.ColumnNames.Contains(columnInfo.ColumnName));

                if (indexInfo != null)
                {
                    extraComment = indexInfo.ToString();
                }

                sb.AppendLine();
                Write(sb, columnInfo, extraComment);
            }

            sb.AppendLine();
            sb.AppendLine("#endregion");

            return sb.ToString();
        }
        #endregion

        #region Methods
        static void Write(StringBuilder sb, ColumnInfo data, string extraComment)
        {
            var comment = data.Comment;

            var commentList = new List<string>();

            if (comment.HasValue())
            {
                commentList.AddRange(comment.Split(Environment.NewLine.ToCharArray()));
            }

            if (extraComment.HasValue())
            {
                commentList.Add(extraComment);
            }

            if (commentList.Any())
            {
                sb.AppendLine("/// <summary>");

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