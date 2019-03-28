using BOA.OneDesigner.CodeGenerationComponentGetValueModels;
using BOA.OneDesigner.CodeGenerationHelper;
using BOA.OneDesigner.CodeGenerationModel;
using BOA.OneDesigner.JsxElementModel;

namespace BOA.OneDesigner.CodeGeneration
{
    static class BBranchComponentRenderer
    {
        #region Public Methods
        public static void Write(WriterContext writerContext, ComponentInfo data)
        {
            var sb = writerContext.Output;

            SnapNamingHelper.InitSnapName(writerContext, data);

            writerContext.Imports.Add("import { BBranchComponent } from \"b-branch-component\"");

            var jsBindingPath = new JsBindingPathCalculatorData(writerContext, data.ValueBindingPath)
            {
                EvaluateInsStateVersion = true
            };
            JsBindingPathCalculator.CalculateBindingPathInRenderMethod(jsBindingPath);

            
            writerContext.GrabValuesToRequest(new ComponentGetValueInfoBranchComponent { JsBindingPath = jsBindingPath.FullBindingPathInJs,SnapName = data.SnapName});

            sb.AppendLine($"<BBranchComponent selectedBranchId = {{{jsBindingPath.BindingPathInJsInState}}}");
            sb.PaddingCount++;

            sb.AppendLine($"onBranchSelect = {{(selectedBranch: BOA.Common.Types.BranchContract) => {jsBindingPath.BindingPathInJs} = selectedBranch ? selectedBranch.branchId : null}}");
            sb.AppendLine("mode = {\"horizontal\"}");
            sb.AppendLine("sortOption = {BBranchComponent.name}");
            sb.AppendLine("ref = {(r: any) => this.snaps." + data.SnapName + " = r}");

            RenderHelper.WriteLabelInfo(writerContext, data.LabelTextInfo, sb.AppendLine, "labelText");

            RenderHelper.WriteIsVisible(writerContext, data.IsVisibleBindingPath, sb);
            RenderHelper.WriteIsDisabled(writerContext, data.IsDisabledBindingPath, sb);
            RenderHelper.WriteSize(data.SizeInfo,sb.AppendLine);

            sb.AppendLine("context = {context}/>");

            sb.PaddingCount--;
        }
        #endregion
    }
}