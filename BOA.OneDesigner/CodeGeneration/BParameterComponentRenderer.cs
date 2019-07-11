using BOA.OneDesigner.CodeGenerationComponentGetValueModels;
using BOA.OneDesigner.CodeGenerationHelper;
using BOA.OneDesigner.CodeGenerationModel;
using BOA.OneDesigner.JsxElementModel;

namespace BOA.OneDesigner.CodeGeneration
{
    static class BParameterComponentRenderer
    {
        #region Public Methods
        public static void Write(WriterContext writerContext, ComponentInfo data)
        {
            var sb = writerContext.Output;

            writerContext.Imports.Add("import { BParameterComponent } from \"b-parameter-component\"");

            SnapNamingHelper.InitSnapName(writerContext, data);

            var jsBindingPath = new JsBindingPathInfo(data.ValueBindingPath)
            {
                EvaluateInsStateVersion = true
            };
            JsBindingPathCalculator.CalculateBindingPathInRenderMethod(jsBindingPath);
            writerContext.PushVariablesToRenderScope(jsBindingPath);
            writerContext.GrabValuesToRequest(new ComponentGetValueInfoParameterComponent { JsBindingPath = jsBindingPath.FullBindingPathInJs,SnapName = data.SnapName});

            sb.AppendLine($"<BParameterComponent  selectedParamCode = {{{jsBindingPath.FullBindingPathInJs}+\"\"}}");
            sb.PaddingCount++;
            // sb.AppendLine($" onParameterSelect = {{(selectedParameter: BOA.Types.Kernel.General.ParameterContract) => {{{jsBindingPath.BindingPathInJs}}} = selectedParameter ? selectedParameter.paramCode : null}}");
            sb.AppendLine("ref = {(r: any) => this.snaps." + data.SnapName + " = r}");
            sb.AppendLine("paramType = \"" + data.ParamType + "\"");
            sb.AppendLine("displayMemberPath={'paramDescription'}");

            RenderHelper.WriteLabelInfo(writerContext, data.LabelTextInfo, sb.AppendLine, "hintText");
            RenderHelper.WriteLabelInfo(writerContext, data.LabelTextInfo, sb.AppendLine, "labelText");
            RenderHelper.WriteLabelInfo(writerContext, data.ParamValue2FilterInto, sb.AppendLine, "paramValue2Filter");


            if (data.IsAllOptionIncluded)
            {
                sb.AppendLine("isAllOptionIncluded={true}");
            }
            else
            {
                sb.AppendLine("isAllOptionIncluded={false}");
            }

            sb.AppendLine("paramColumns={[");
            sb.AppendLine("{    name: \"paramCode\",        header: 'Kod',       visible: false },");
            sb.AppendLine("{    name: \"paramDescription\", header: 'Açıklama',  width: 200 }");
            sb.AppendLine("]}");

            RenderHelper.WriteIsVisible(writerContext, data.IsVisibleBindingPath, sb);
            RenderHelper.WriteIsDisabled(writerContext, data.IsDisabledBindingPath, sb);

            
            RenderHelper.WriteSize(data.SizeInfo,sb.AppendLine);

            sb.AppendLine("context = {context}/>");

            sb.PaddingCount--;
        }
        #endregion
    }
}