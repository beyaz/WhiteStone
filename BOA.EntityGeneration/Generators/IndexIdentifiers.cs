using System.Linq;
using System.Text;
using BOA.EntityGeneration.Common;
using BOA.EntityGeneration.ScriptModel;

namespace BOA.EntityGeneration.Generators
{
    class IndexIdentifiers : GeneratorBase
    {
        #region Public Methods
        public override string ToString()
        {
            var sb = new StringBuilder();

            if (Data.UniqueIndexIdentifiers.Any())
            {
                sb.AppendLine();
                sb.AppendLine("#region Unique Index Information");
                foreach (var item in Data.UniqueIndexIdentifiers)
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

            if (Data.NonUniqueIndexIdentifiers.Any())
            {
                sb.AppendLine();
                sb.AppendLine("#region Index Information");
                foreach (var item in Data.NonUniqueIndexIdentifiers)
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