using System.Linq;
using BOA.Common.Helpers;
using BOA.OneDesigner.CodeGenerationHelper;
using BOA.OneDesigner.CodeGenerationModel;
using BOA.OneDesigner.JsxElementModel;

namespace BOA.OneDesigner.CodeGeneration
{
    /// <summary>
    ///     The render function definition
    /// </summary>
    class RenderFunctionDefinition
    {
        #region Fields
        /// <summary>
        ///     The data
        /// </summary>
        public DivAsCardContainer Data;

        /// <summary>
        ///     The window request access path
        /// </summary>
        public string WindowRequestAccessPath;

        /// <summary>
        ///     The writer context
        /// </summary>
        public WriterContext WriterContext;
        #endregion

        #region Public Methods
        /// <summary>
        ///     Gets the code.
        /// </summary>
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