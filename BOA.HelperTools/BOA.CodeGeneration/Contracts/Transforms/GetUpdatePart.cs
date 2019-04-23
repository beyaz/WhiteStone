using System;
using System.Linq;
using BOA.CodeGeneration.Common;
using BOA.Common.Helpers;
using BOA.EntityGeneration.Common;

namespace BOA.CodeGeneration.Contracts.Transforms
{

        class GetSavePart:GeneratorBase
    {

       


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

            if (TableInfo.PrimaryKeyColumns.Count ==1)
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


        #region Public Methods
        #endregion
    }


    class GetUpdatePart:GeneratorBase
    {

        public override string ToString()
        {
            var sb = new PaddedStringBuilder();

            sb.AppendAll(GetUpdateSql());
            sb.AppendLine();

            sb.AppendLine();
            sb.AppendAll(GetUpdateParameters());

            return sb.ToString();
        }


        string GetUpdateSql()
        {
            var sb = new PaddedStringBuilder();

            sb.AppendLine($"string {Names.ISupportDmlOperationUpdate}.UpdateSql");
            sb.AppendLine("{");
            sb.PaddingCount++;

            sb.AppendLine("get");
            sb.AppendLine("{");
            sb.PaddingCount++;

            #region body
            sb.AppendLine("return @\"");

            sb.AppendLine($"UPDATE [{TableInfo.SchemaName}].[{TableInfo.TableName}] SET");
            sb.PaddingCount++;

            sb.AppendAll(string.Join("," + Environment.NewLine, TableInfo.GetColumnsWillBeUpdate().Select(c => $"[{c.ColumnName}] = @{c.ColumnName}")));
            sb.AppendLine();

            sb.PaddingCount--;
            sb.AppendLine("WHERE");
            sb.PaddingCount++;

            sb.AppendAll(string.Join("," + Environment.NewLine, TableInfo.GetWhereParameters().Select(c => $"[{c.ColumnName}] = @{c.ColumnName}")));
            sb.AppendLine();

            sb.PaddingCount--;

            sb.AppendLine("\";");
            #endregion

            sb.PaddingCount--;
            sb.AppendLine("}");

            sb.PaddingCount--;
            sb.AppendLine("}");
            

            return sb.ToString();
        }


        #region Public Methods
        string GetUpdateParameters()
        {
            var sb = new PaddedStringBuilder();

            sb.AppendLine($"IReadOnlyList<Parameter> {Names.ISupportDmlOperationUpdate}.GetUpdateSqlParameters(ExecutionScope context)");
            sb.AppendLine("{");
            sb.PaddingCount++;

            sb.AppendLine("return new List<Parameter>");
            sb.AppendLine("{");
            sb.PaddingCount++;

            sb.AppendAll(string.Join("," + Environment.NewLine, TableInfo.GetSqlInputParameters().Select(ParameterHelper.ConvertToParameterDeclarationCode)));
            sb.AppendLine();

            sb.PaddingCount--;
            sb.AppendLine("};");

            sb.PaddingCount--;
            sb.AppendLine("}");

            return sb.ToString();
        }
        #endregion
    }
}