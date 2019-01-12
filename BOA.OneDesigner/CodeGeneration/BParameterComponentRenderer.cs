using BOA.Common.Helpers;
using BOA.OneDesigner.JsxElementModel;

namespace BOA.OneDesigner.CodeGeneration
{
    static class BParameterComponentRenderer
    {
        public static void Write(PaddedStringBuilder sb, ScreenInfo screenInfo, BParameterComponent data)
        {
            SnapNamingHelper.InitSnapName(data);

            sb.AppendLine($"<BParameterComponent  selectedParamCode = {{{data.BindingPathInTypeScript}}}");
            if (data.ValueTypeIsInt32)
            {
                sb.AppendLine($"            onParameterSelect = {{(selectedParameter: BOA.Types.Kernel.General.ParameterContract) => {{{data.BindingPathInTypeScript}}} = selectedParameter ? selectedParameter.paramCode|0 : null}}");    
            }
            else
            {
                sb.AppendLine($"            onParameterSelect = {{(selectedParameter: BOA.Types.Kernel.General.ParameterContract) => {{{data.BindingPathInTypeScript}}} = selectedParameter ? selectedParameter.paramCode : null}}");
            }
            
            sb.AppendLine("             ref = {(r: any) => this.snaps."+data.SnapName+" = r}");
            sb.AppendLine("paramType = \""+data.ParamType+"\"");

            var labelValue = RenderHelper.GetLabelValue(screenInfo, data.LabelInfo);
            if (labelValue != null)
            {
                sb.AppendLine($"hintText = {labelValue}");
                sb.AppendLine($"labelText = {labelValue}");
            }

            sb.AppendLine("isAllOptionIncluded={true}");

            sb.AppendLine("paramColumns={[");
            sb.AppendLine("{ name: \"paramCode\",        header: Message.Code,        visible: false },");
            sb.AppendLine("{ name: \"paramDescription\", header: Message.Description, width:   200 }");
            sb.AppendLine("]}");


            sb.AppendLine("             context = {context}/>");
        }
    }
}