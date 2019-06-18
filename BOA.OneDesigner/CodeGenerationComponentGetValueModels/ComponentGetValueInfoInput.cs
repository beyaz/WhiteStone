using BOA.Common.Helpers;

namespace BOA.OneDesigner.CodeGenerationComponentGetValueModels
{
    public class ComponentGetValueInfoInput : ComponentGetValueInfo
    {
        #region Public Methods
        public override string GetCode()
        {
            return $"{ComponentGetValueInfo.VariableNameOfComponent}.getInstance().getValue()";
        }
        #endregion
    }

    public class ComponentGetValueInfoInputMask : ComponentGetValueInfo
    {
        #region Public Methods
        public override string GetCode()
        {
            return $"{ComponentGetValueInfo.VariableNameOfComponent}.getInstance().getValue().saltValue";
        }
        #endregion
    }

    public class ComponentGetValueInfoExcelBrowser : ComponentGetValueInfo
    {
        #region Public Properties
        public string ValueBindingPathInDotNet { get; set; }
        #endregion

        #region Public Methods
        public override string GetCode()
        {
            var sb = new PaddedStringBuilder();
            Write(sb);
            return sb.ToString();
        }

        public void Write(PaddedStringBuilder sb)
        {
            sb.AppendLine($"// #region read excel records to {JsBindingPath}");
            sb.AppendLine($"const excelData = snaps.{SnapName}.getInstance().getValue();");
            sb.AppendLine();
            sb.AppendLine("const $excelData: any = [];");
            sb.AppendLine();
            sb.AppendLine("for (let i = 0; i < excelData.length; i++)");
            sb.AppendLine("{");
            sb.AppendLine("    const excelRow = excelData[i];");
            sb.AppendLine();
            sb.AppendLine("    const record = [];");
            sb.AppendLine();
            sb.AppendLine("    let columnIndex = 0;");
            sb.AppendLine("    while (true)");
            sb.AppendLine("    {");
            sb.AppendLine("        const cellValue = excelRow[columnIndex];");
            sb.AppendLine("        if (cellValue === undefined)");
            sb.AppendLine("        {");
            sb.AppendLine("            break;");
            sb.AppendLine("        }");
            sb.AppendLine();
            sb.AppendLine("        record.push(cellValue);");
            sb.AppendLine();
            sb.AppendLine("        columnIndex++;");
            sb.AppendLine("    }");
            sb.AppendLine();
            sb.AppendLine("    $excelData.push(record);");
            sb.AppendLine("}");
            sb.AppendLine();
            sb.AppendLine($"{JsBindingPath} = $excelData;");
            sb.AppendLine("// #endregion");
        }
        #endregion
    }
}