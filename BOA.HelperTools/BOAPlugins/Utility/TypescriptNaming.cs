using System.Linq;
using BOA.Common.Helpers;
using Newtonsoft.Json.Serialization;

namespace BOAPlugins.Utility
{
    public static class TypescriptNaming
    {
        #region Static Fields
        static readonly CamelCasePropertyNamesContractResolver CamelCasePropertyNamesContractResolver = new CamelCasePropertyNamesContractResolver();
        #endregion

        #region Public Methods
        public static string GetResolvedPropertyName(string propertyNameInCSharp)
        {
            return CamelCasePropertyNamesContractResolver.GetResolvedPropertyName(propertyNameInCSharp);
        }

        public static string NormalizeBindingPath(string propertyNameInCSharp)
        {
            return string.Join(".", propertyNameInCSharp.SplitAndClear(".").ToList().ConvertAll(GetResolvedPropertyName));
        }
        #endregion
    }
}