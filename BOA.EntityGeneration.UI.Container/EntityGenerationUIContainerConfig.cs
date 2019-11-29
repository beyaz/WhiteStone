using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BOA.Common.Helpers;
using BOA.EntityGeneration.SchemaToEntityExporting.Exporters;

namespace BOA.EntityGeneration.UI.Container
{
    public class EntityGenerationUIContainerConfig
    {
        public bool IntegrateWithTFSAndCheckInAutomatically { get; set; }

        public static EntityGenerationUIContainerConfig CreateFromFile()
        {
            return YamlHelper.DeserializeFromFile<EntityGenerationUIContainerConfig>(nameof(EntityGenerationUIContainerConfig) + ".yaml");
        }
    }
}
