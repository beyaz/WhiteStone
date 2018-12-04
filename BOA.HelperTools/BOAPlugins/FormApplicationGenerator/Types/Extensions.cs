using BOA.CodeGeneration.Generators;
using BOA.Common.Helpers;

namespace BOAPlugins.FormApplicationGenerator.Types
{
    public static class Extensions
    {
        #region Methods
        internal static string GetSnapName(this BField bField)
        {
            return $"{bField.GetType().Name.RemoveFromStart("B").MakeLowerCaseFirstChar()}{bField.BindingPath}";
        }

        internal static bool HasSnapName(this BField bField)
        {
            return bField is BAccountComponent ||
                   bField is BParameterComponent;
        }

        static string MakeLowerCaseFirstChar(this string value)
        {
            if (value.IsNullOrEmpty())
            {
                return value;
            }

            return ContractBodyGenerator.GetPropertyFieldName("", value);
        }
        #endregion
    }
}