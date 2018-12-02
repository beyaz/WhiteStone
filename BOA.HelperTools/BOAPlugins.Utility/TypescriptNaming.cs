using Newtonsoft.Json.Serialization;

namespace BOAPlugins.Utility
{
    public static class TypescriptNaming
    {
        static readonly CamelCasePropertyNamesContractResolver CamelCasePropertyNamesContractResolver = new CamelCasePropertyNamesContractResolver();

        public static string GetResolvedPropertyName(string propertyNameInCSharp)
        {
            return TypescriptNaming.CamelCasePropertyNamesContractResolver.GetResolvedPropertyName(propertyNameInCSharp);
        }
    }
}