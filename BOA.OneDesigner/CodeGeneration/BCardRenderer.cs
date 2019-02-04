using System;
using System.Collections.Generic;
using BOA.Common.Helpers;
using BOA.OneDesigner.Helpers;
using BOA.OneDesigner.JsxElementModel;

namespace BOA.OneDesigner.CodeGeneration
{
    static class BCardRenderer
    {
        #region Public Methods
        public static void Write(WriterContext writerContext, BCard data)
        {
            var sb         = writerContext.Output;
            var screenInfo = writerContext.ScreenInfo;


            var doNotWriteTag = data.IsBrowsePageDataGridContainer || data.IsBrowsePageCriteria;

            if (doNotWriteTag == false)
            {
                writerContext.Imports.Add("import { BCard } from \"b-card\""); 

                sb.AppendWithPadding("<BCard context={context}");

                if (data.IsVisibleBindingPath.HasValue())
                {
                    var isVisibleBindingPath = RenderHelper.NormalizeBindingPathInRenderMethod(writerContext, data.IsVisibleBindingPath);

                    sb.Append(" style={{ display: " + isVisibleBindingPath + " ? 'inherit' : 'none' }} ");
                }

                var labelValue = RenderHelper.GetLabelValue(screenInfo, data.TitleInfo);
                if (labelValue != null)
                {
                    sb.Append($" title = {{{labelValue}}}");
                }

                if (data.LayoutProps != null)
                {
                    sb.Append(" layoutProps = {{w:" + data.LayoutProps.Wide + ", x:" + data.LayoutProps.X + "}}");
                }

                sb.Append(">");
                sb.Append(Environment.NewLine);

                sb.PaddingCount++;
            }


            var subComponents = new List<string>();

            foreach (var item in data.Items)
            {
                var bInput = item as BInput;
                if (bInput != null)
                {
                    writerContext.Output = new PaddedStringBuilder();

                    BInputRenderer.Write(writerContext, bInput);

                    subComponents.Add(writerContext.Output.ToString());

                    writerContext.Output = sb;

                    continue;
                }

                var bLabel = item as BLabel;
                if (bLabel != null)
                {
                    writerContext.Output = new PaddedStringBuilder();

                    BLabelRenderer.Write(writerContext, bLabel);

                    subComponents.Add(writerContext.Output.ToString());

                    writerContext.Output = sb;

                    continue;
                }

                


                var bTabBar = item as BTabBar;
                if (bTabBar != null)
                {
                    writerContext.Output = new PaddedStringBuilder();

                    BTabBarRenderer.Write(writerContext, bTabBar);

                    subComponents.Add(writerContext.Output.ToString());

                    writerContext.Output = sb;
                    continue;
                }

                var bComboBox = item as BComboBox;
                if (bComboBox != null)
                {
                    writerContext.Output = new PaddedStringBuilder();

                    BComboBoxRenderer.Write(writerContext, bComboBox);

                    subComponents.Add(writerContext.Output.ToString());

                    writerContext.Output = sb;

                    continue;
                }

                var bDataGrid = item as BDataGrid;
                if (bDataGrid != null)
                {
                    writerContext.Output = new PaddedStringBuilder();

                    BDataGridRenderer.Write(writerContext, bDataGrid);

                    subComponents.Add(writerContext.Output.ToString());

                    writerContext.Output = sb;

                    continue;
                }

                throw Error.InvalidOperation();
            }

            foreach (var subComponent in subComponents)
            {
                sb.AppendLine();
                sb.AppendAll(subComponent);
                sb.AppendLine();
            }

            if (doNotWriteTag == false)
            {
                sb.PaddingCount--;
                sb.AppendLine("</BCard>");
            }
        }
        #endregion
    }
}