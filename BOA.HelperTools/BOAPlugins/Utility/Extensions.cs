using Mono.Cecil;

namespace BOAPlugins.Utility
{
    public static class Extensions
    {
        public static TypeDefinition FindType( this AssemblyDefinition definition, string classFullName)
        {
            foreach (var module in definition.Modules)
            {
                var typeDefinition = module.GetType(classFullName);

                if (typeDefinition != null)
                {
                    return typeDefinition;
                }
            }

            return null;
        }
    }
}