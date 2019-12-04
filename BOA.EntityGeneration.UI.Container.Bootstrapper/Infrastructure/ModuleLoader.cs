using BOA.Common.Helpers;

namespace BOA.EntityGeneration.UI.Container.Bootstrapper.Infrastructure
{
    static class ModuleLoader
    {
        public static void Load()
        {
            EmbeddedCompressedAssemblyReferencesResolver.Resolve("EmbeddedReferences.zip");
        }
    }
}