using System.IO;

namespace BOA.EntityGeneration.SchemaToEntityExporting
{
    class ConfigBase
    {
        #region Static Fields
        protected static string ConfigDirectory = Path.GetDirectoryName(typeof(ConfigBase).Assembly.Location) + Path.DirectorySeparatorChar + "SchemaToEntityExportingConfigFiles" + Path.DirectorySeparatorChar;
        #endregion
    }
}