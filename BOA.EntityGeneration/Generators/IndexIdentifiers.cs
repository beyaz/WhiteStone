using System.Linq;
using System.Text;
using BOA.EntityGeneration.Common;
using BOA.EntityGeneration.ScriptModel;

namespace BOA.EntityGeneration.Generators
{
    public class IndexIdentifiers 
    {
        #region Public Methods
        public string TransformText(GeneratorData data)
        {
            var sb = new StringBuilder();

            if (data.UniqueIndexIdentifiers.Any())
            {
                sb.AppendLine();
                sb.AppendLine("#region Unique Index Information");
                foreach (var item in data.UniqueIndexIdentifiers)
                {
                    sb.AppendLine();
                    sb.AppendLine("/// <summary>");
                    sb.AppendLine($"///{Padding.ForComment} ... WHERE {string.Join(" and ", item.IndexInfo.ColumnNames.Select(x => x + " = " + x.ToContractName()))}");
                    sb.AppendLine("/// </summary>");

                    sb.AppendLine($"public static readonly {item.TypeName} {item.Name} = new {item.TypeName}();");
                }

                sb.AppendLine();
                sb.AppendLine("#endregion");
            }

            if (data.NonUniqueIndexIdentifiers.Any())
            {
                sb.AppendLine();
                sb.AppendLine("#region Index Information");
                foreach (var item in data.NonUniqueIndexIdentifiers)
                {
                    sb.AppendLine();
                    sb.AppendLine("/// <summary>");
                    sb.AppendLine($"///{Padding.ForComment} ... WHERE {string.Join(" and ", item.IndexInfo.ColumnNames.Select(x => x + " = " + x.ToContractName()))}");
                    sb.AppendLine("/// </summary>");

                    sb.AppendLine($"public static readonly {item.TypeName} {item.Name} = new {item.TypeName}();");
                }

                sb.AppendLine();
                sb.AppendLine("#endregion");
            }

            return sb.ToString();
        }
        #endregion
    }
}