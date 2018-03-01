using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BOA.Common.Helpers;

namespace WhiteStone.Tasks
{
    public class SemanticUIIconNamesEvaluater
    {
        #region Public Methods
        public void Run()
        {
            var url = "https://semantic-ui.com/elements/icon.html";

            var content = FileHelper.DownloadString(url);

            var values = new Dictionary<string, string>();
            foreach (var line in content.Split(Environment.NewLine.ToCharArray()))
            {
                var prefix = "<i class=\"";
                var sufix = " icon\"></i>";
                if (line.Contains(prefix) && line.Contains(sufix))
                {
                    var index = line.IndexOf(prefix, StringComparison.Ordinal) + prefix.Length;
                    var endIndex = line.IndexOf(sufix, StringComparison.Ordinal);

                    var key = line.Substring(index, endIndex - index);

                    if (key == "lock")
                    {
                        key = "Lock";
                    }
                    if (key== "birthday:")
                    {
                        key = "birthday";
                    }

                    if (key== "500px")
                    {
                        continue;
                    }
                    if (key== "free.code.camp")
                    {
                        continue;
                        
                    }


                    values[key] = key;
                }
            }

            var indent = "    ";

            var enumBody = string.Join<string>("," + Environment.NewLine, from x in values.Keys select indent + indent + x.Replace(" ", "_"));

            var targetPath = @"D:\github\Bridge.CustomUIMarkup\Src\Libraries\SemanticUI\IconType.cs";

            var sb = new StringBuilder();

            sb.AppendLine("namespace Bridge.CustomUIMarkup.Libraries.SemanticUI");
            sb.AppendLine("{");

            sb.AppendLine(indent + "public enum IconType");
            sb.AppendLine(indent + "{");
            sb.AppendLine(enumBody);
            sb.AppendLine(indent + "}");

            sb.AppendLine("}");

            FileHelper.WriteAllText(targetPath, sb.ToString());
        }
        #endregion
    }
}