using System.Linq;
using BOA.Common.Helpers;
using BOA.OneDesigner.CodeGenerationHelper;
using BOA.OneDesigner.CodeGenerationModel;
using BOA.OneDesigner.JsxElementModel;

namespace BOA.OneDesigner.CodeGeneration
{
    class RenderFunctionDefinition
    {
        readonly WriterContext      writerContext;
        readonly DivAsCardContainer data;
        readonly string _windowRequestAccessPath;

        public RenderFunctionDefinition(WriterContext writerContext, DivAsCardContainer data, string windowRequestAccessPath)
        {
            this.writerContext = writerContext;
            this.data          = data;
            _windowRequestAccessPath = windowRequestAccessPath;
        }

            
        public string GetCode()
        {
            var temp = writerContext.Output;

            var jsxBuilder = new PaddedStringBuilder {PaddingCount = 1};

            jsxBuilder.AppendLine("return (");
            jsxBuilder.PaddingCount++;
            writerContext.Output = jsxBuilder;
            DivAsCardContainerRenderer.Write(writerContext, data);
            jsxBuilder.PaddingCount--;
            jsxBuilder.AppendLine(");");

            jsxBuilder.PaddingCount--;
            jsxBuilder.AppendLine("}");

            var sb = new PaddedStringBuilder();
            if (RenderHelper.IsCommentEnabled)
            {
                sb.AppendLine("/**");
                sb.AppendLine("  *  Renders the component.");
                sb.AppendLine("  */");
            }

            sb.AppendLine("render()");
            sb.AppendLine("{");
            sb.PaddingCount++;



            var requestDefaultValue = BindingPathHelper.EvaluateWindowRequestDefaultCreationInRenderFunction(from x in writerContext.UsedBindingPathInRenderMethod
                                                                                                             select StringHelper.RemoveFromStart(x.FullBindingPathInJs, Config.BindingPrefixInJs));

            sb.AppendLine();
            sb.AppendLine("const context = this.state.context;");
            sb.AppendLine();
            sb.AppendLine("const request = "+_windowRequestAccessPath+" || "+requestDefaultValue+";");
            sb.AppendLine();

            writerContext.Output = temp;

            return sb + jsxBuilder.ToString();
        }

        
    }
}