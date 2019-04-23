using System;
using BOA.Common.Helpers;
using BOA.EntityGeneration.Common;

namespace BOA.EntityGeneration.Generators
{
    class GetSavePart : GeneratorBase
    {
        #region Public Methods
        public override string ToString()
        {
            var sb = new PaddedStringBuilder();

            sb.AppendLine($"bool {Names.ISupportDmlOperationSave}.IsReadyToUpdate");
            sb.AppendLine("{");
            sb.PaddingCount++;

            sb.AppendLine("get");
            sb.AppendLine("{");
            sb.PaddingCount++;

            #region body
            if (TableInfo.PrimaryKeyColumns.Count == 1)
            {
                var keyColumn = TableInfo.PrimaryKeyColumns[0];

                if (keyColumn.DotNetType == DotNetTypeName.DotNetInt16 ||
                    keyColumn.DotNetType == DotNetTypeName.DotNetInt32 ||
                    keyColumn.DotNetType == DotNetTypeName.DotNetInt64)
                {
                    sb.AppendLine($"return {keyColumn.ColumnName.ToContractName()} > 0;");
                }
                else if (keyColumn.DotNetType == DotNetTypeName.DotNetStringName)
                {
                    sb.AppendLine($"return !string.IsNullOrWhiteSpace({keyColumn.ColumnName.ToContractName()});");
                }
                else
                {
                    throw new NotImplementedException();
                }
            }
            else
            {
                throw new NotImplementedException();
            }
            #endregion

            sb.PaddingCount--;
            sb.AppendLine("}");

            sb.PaddingCount--;
            sb.AppendLine("}");

            return sb.ToString();
        }
        #endregion
    }
}