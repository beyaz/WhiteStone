using System.Linq;
using BOA.Common.Helpers;
using BOA.OneDesigner.CodeGenerationHelper;
using BOA.OneDesigner.CodeGenerationModel;
using BOA.OneDesigner.JsxElementModel;

namespace BOA.OneDesigner.CodeGeneration
{
    class RenderFunctionDefinition
    {
        #region Fields
        readonly DivAsCardContainer Data;
        readonly string             WindowRequestAccessPath;
        readonly WriterContext      WriterContext;
        #endregion

        #region Constructors
        public RenderFunctionDefinition(WriterContext writerContext, DivAsCardContainer data, string windowRequestAccessPath)
        {
            WriterContext           = writerContext;
            Data                    = data;
            WindowRequestAccessPath = windowRequestAccessPath;
        }


        #endregion

        #region Public Methods
        public string GetCode()
        {
            var temp = WriterContext.Output;

            var jsxBuilder = new PaddedStringBuilder {PaddingCount = 1};

            jsxBuilder.AppendLine("return (");
            jsxBuilder.PaddingCount++;
            WriterContext.Output = jsxBuilder;
            DivAsCardContainerRenderer.Write(WriterContext, Data);
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

            var requestDefaultValue = BindingPathHelper.EvaluateWindowRequestDefaultCreationInRenderFunction(from x in WriterContext.UsedBindingPathInRenderMethod
                                                                                                             select x.FullBindingPathInJs.RemoveFromStart(Config.BindingPrefixInJs));

            sb.AppendLine();
            sb.AppendLine("const context = this.state.context;");
            sb.AppendLine();
            sb.AppendLine("const request = " + WindowRequestAccessPath + " || " + requestDefaultValue + ";");
            sb.AppendLine();

            WriterContext.Output = temp;

            return sb + jsxBuilder.ToString();
        }
        #endregion
    }
}