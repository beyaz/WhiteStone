using System.Linq;
using BOA.Common.Helpers;

namespace BOA.CodeGeneration.Contracts.Transforms
{
    static class Names
    {
        public const string Parameter = "Parameter";

        public static string ToContractName(this string dbObjectName)
        {
            if (string.IsNullOrEmpty(dbObjectName))
            {
                return dbObjectName;
            }

            if (dbObjectName.Length == 1)
            {
                return dbObjectName.ToUpper();
            }

            var names = dbObjectName.SplitAndClear("_");

            return string.Join(string.Empty, names.Select(name => name.Substring(0, 1).ToUpper(new System.Globalization.CultureInfo("EN-US")) + name.Substring(1).ToLowerInvariant()));

            
        }

    }
}