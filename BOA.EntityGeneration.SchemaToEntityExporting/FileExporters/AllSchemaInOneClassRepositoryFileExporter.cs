using BOA.Common.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BOA.EntityGeneration.SchemaToEntityExporting.FileExporters
{
    class AllSchemaInOneClassRepositoryFileExporter: ContextContainer
    {
          readonly PaddedStringBuilder file = new PaddedStringBuilder();


         public void AttachEvents()
        {
                 SchemaExportStarted += WriteUsingList;

        }

         void WriteUsingList()
        {
            foreach (var line in NamingPattern.AllSchemaInOneClassRepositoryUsingLines)
            {
                file.AppendLine(line);
            }
        }
    }
}
