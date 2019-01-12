﻿using System.Linq;
using System.Windows.Controls;
using BOA.Common.Helpers;
using BOA.OneDesigner.JsxElementModel;
using BOA.OneDesigner.WpfControls;
using BOAPlugins.Utility;
using Host = BOA.OneDesigner.AppModel.Host;

namespace BOA.OneDesigner.CodeGeneration
{
    static class BTabBarRenderer
    {
        

        
        public static void Write(WriterContext writerContext, BTabBar data)
        {

            var        sb         = writerContext.Output;
            ScreenInfo screenInfo = writerContext.ScreenInfo;

            SnapNamingHelper.InitSnapName(data);





          
            writerContext.Imports.Add("import { BTabBar } from \"b-tab-bar\";");


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
                var activeTabIndexBindingPathInJs = TypescriptNaming.NormalizeBindingPath(BindingPrefix.Value +data.ActiveTabIndexBindingPath);

                sb.AppendLine("value={"+activeTabIndexBindingPathInJs+"}");
                sb.AppendLine($"onChange = {{(e: any, value: number) => {activeTabIndexBindingPathInJs} = value}}");
            }

            if (data.SizeInfo != null && data.SizeInfo.IsEmpty == false)
            {
                sb.AppendLine("size = {"+ RenderHelper.GetJsValue(data.SizeInfo) +"}");
            }

            sb.AppendLine("tabItems = {[");

            sb.PaddingCount += 2;

            for (var i = 0; i < data.Items.Count; i++)
            {
                var isLAstTab = i == data.Items.Count - 1;

                var bTabBarPage = data.Items[i];

                sb.AppendLine("{");
                sb.PaddingCount++;

                var title = RenderHelper.GetLabelValue(screenInfo, bTabBarPage.TitleInfo);
                if (title != null)
                {
                    sb.AppendLine($"text : {title},");
                }
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

                int rowIndex = 0;
                foreach (var dummy in divAsCardContainerWpf.RowDefinitions)
                {
                    sb.AppendLine("<div style={{flexWrap:'wrap',display:'flex'}}>");
                    sb.PaddingCount++;

                    var elementsInRow = divAsCardContainerWpf.Children.ToArray().Where(x => (int) x.GetValue(Grid.RowProperty) == rowIndex).ToList();
                    foreach (var uiElement in elementsInRow)
                    {
                        var bCardWpf = ((BCardWpf) uiElement);

                        var columnSpan = ((int) bCardWpf.GetValue(Grid.ColumnSpanProperty));

                        decimal width = (columnSpan / 12M) * 100M;

                        var bCard = bCardWpf.Model;

                        sb.AppendLine("<div style={{ width: '" + width + "%', padding: '15px' }}>");

                        sb.PaddingCount++;
                        BCardRenderer.Write(writerContext, bCard);
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

                if (isLAstTab)
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

    }
}