using System;
using System.Linq;
using System.Windows.Controls;
using BOA.Common.Helpers;
using BOA.OneDesigner.AppModel;
using BOA.OneDesigner.CodeGenerationHelper;
using BOA.OneDesigner.CodeGenerationModel;
using BOA.OneDesigner.JsxElementModel;
using BOA.OneDesigner.WpfControls;

namespace BOA.OneDesigner.CodeGeneration
{
    static class BTabBarRenderer
    {
        public static void DefineTabPage(WriterContext writerContext, BTabBarPage tabPage)
        {
            writerContext.Imports.Add("import { BPageModule, BPageModuleComposer } from \"b-page-module\";");

            var sb = new PaddedStringBuilder();

            sb.AppendLine($"class {tabPage.ClassName} extends BPageModule");
            sb.AppendLine("{");
            sb.PaddingCount++;



            sb.PaddingCount--;
            sb.AppendLine("}");
            sb.AppendLine($"var {tabPage.ComposedName} = BPageModuleComposer({tabPage.ClassName});");



            writerContext.Page.Add(sb.ToString());
        }
        #region Public Methods
        public static void Write(WriterContext writerContext, BTabBar data)
        {
            var sb = writerContext.Output;

            foreach (var tabPage in data.Items)
            {
                DefineTabPage(writerContext,tabPage);
            }


            SnapNamingHelper.InitSnapName(writerContext, data);



            writerContext.Imports.Add("import { BTabBar } from \"b-tab-bar\";");// TODO gerek olmayabilir.
            

            sb.AppendLine("<BTabBar context={context}");
            sb.PaddingCount++;

            sb.AppendLine("mode='secondary'");

            if (string.IsNullOrWhiteSpace(data.ActiveTabIndexBindingPath))
            {
                sb.AppendLine("value={this.state.activeTab}");
                sb.AppendLine("onChange={(event, value) => { this.setState( { activeTab: value} ); }}");
            }
            else
            {
                var activeTabIndexBindingPath = new JsBindingPathInfo(data.ActiveTabIndexBindingPath)
                {
                    EvaluateInsStateVersion = true
                };
                JsBindingPathCalculator.CalculateBindingPathInRenderMethod(activeTabIndexBindingPath);
                writerContext.PushVariablesToRenderScope(activeTabIndexBindingPath);

                sb.AppendLine("value={" + activeTabIndexBindingPath.FullBindingPathInJs + "}");
                sb.AppendLine($"onChange = {{(e: any, value: number) => {activeTabIndexBindingPath.FullBindingPathInJs} = value}}");
            }

            RenderHelper.WriteSize(data.SizeInfo,sb.AppendLine);

            sb.AppendLine("tabItems = {[");

            sb.PaddingCount += 2;

            for (var i = 0; i < data.Items.Count; i++)
            {
                var isLastTab = i == data.Items.Count - 1;

                var bTabBarPage = data.Items[i];

                sb.AppendLine("{");
                sb.PaddingCount++;

                RenderHelper.WriteLabelInfo(writerContext, bTabBarPage.TitleInfo, sb.AppendLine, "text:", ",");

                sb.AppendLine($"value : {i},");

                sb.AppendLine("content : (");

                #region Content
                var divAsCardContainerWpf = new DivAsCardContainerWpf
                {
                    DataContext = bTabBarPage.DivAsCardContainer,
                    Host        = new Host()
                };
                divAsCardContainerWpf.Refresh();

                sb.AppendLine("<div>");
                sb.PaddingCount++;

                var rowIndex = 0;
                foreach (var dummy in divAsCardContainerWpf.RowDefinitions)
                {
                    sb.AppendLine("<div style={{flexWrap:'wrap',display:'flex'}}>");
                    sb.PaddingCount++;

                    var elementsInRow = divAsCardContainerWpf.Children.ToArray().Where(x => (int) x.GetValue(Grid.RowProperty) == rowIndex).ToList();
                    foreach (var uiElement in elementsInRow)
                    {
                        var bCardWpf = (BCardWpf) uiElement;

                        var columnSpan = (int) bCardWpf.GetValue(Grid.ColumnSpanProperty);

                        var width = columnSpan / 12M * 100M;

                        width = Math.Round(width, 3);

                        var bCard = bCardWpf.Model;

                        sb.AppendLine("<div style={{ width: '" + width + "%', padding: '15px' }}>");

                        sb.PaddingCount++;
                        BCardRenderer.Write(writerContext, bCard,true);
                        sb.PaddingCount--;

                        sb.AppendLine("</div>");
                    }

                    sb.PaddingCount--;
                    sb.AppendLine("</div>");

                    rowIndex++;
                }

                sb.PaddingCount--;
                sb.AppendLine("</div>");
                #endregion

                sb.AppendLine(")");

                sb.AppendLine();

                sb.PaddingCount--;

                if (isLastTab)
                {
                    sb.AppendLine("}");
                }
                else
                {
                    sb.AppendLine("},");
                }
            }

            sb.AppendLine("]}");
            sb.PaddingCount -= 2;

            sb.PaddingCount--;
            sb.AppendLine(" />");
        }
        #endregion
    }
}