using System.Collections.Generic;

namespace BOA.EntityGeneration.SchemaToDllExporting
{
    public class SchemaExporterDataForBOACard : SchemaExporterData
    {
        #region Constructors
        public SchemaExporterDataForBOACard()
        {
            CatalogName     = "BOACard";
            TableNameFilter = IsReadyToExport;
            ReferencedAssemblies = new List<string>
            {
                @"d:\boa\server\bin\BOA.Types.Kernel.Card.dll",
                @"d:\boa\server\bin\BOA.Common.dll"
            };
        }
        #endregion

        #region Methods
        static bool IsReadyToExport(string schemaName, string tableName)
        {
            if ($"{schemaName}.{tableName}" == "MRC.SENDING_REPORT_PARAMS_")
            {
                return false;
            }

            if ($"{schemaName}.{tableName}" == "POS.POS_INVENTORY_")
            {
                return false;
            }

            if ($"{schemaName}.{tableName}" == "STM.BUCKET_CLOSE_DETAIL") // todo aynı indexden iki tane var ?  sormak lazım
            {
                return false;
            }

            return true;
        }
        #endregion
    }
}