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


    public class ComponentGetValueInfoPosTerminalComponent: ComponentGetValueInfo
    {
        #region Public Methods
        public override string GetCode()
        {
            return $"{ComponentGetValueInfo.VariableNameOfComponent}.getInstance().getValue() && {ComponentGetValueInfo.VariableNameOfComponent}.getInstance().getValue().terminalNumber";
        }
        #endregion
    }

    public class ComponentGetValueInfoPosMerchantComponent: ComponentGetValueInfo
    {
        #region Public Methods
        public override string GetCode()
        {
            return $"{ComponentGetValueInfo.VariableNameOfComponent}.getInstance().getValue() && {ComponentGetValueInfo.VariableNameOfComponent}.getInstance().getValue().merchantNumber";
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
            sb.AppendLine($"{JsBindingPath} = this.readExcel(snaps.{SnapName});");
        }
        #endregion
    }
}