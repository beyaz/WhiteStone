using System;
using System.Collections.Generic;
using BOA.Common.Helpers;
using BOA.OneDesigner.Helpers;
using BOA.OneDesigner.JsxElementModel;
using BOAPlugins.Utility;

namespace BOA.OneDesigner.CodeGeneration
{
    static class BCardRenderer
    {
        #region Public Methods
        public static void Write(WriterContext writerContext, BCard data)
        {
            var sb         = writerContext.Output;
            var screenInfo = writerContext.ScreenInfo;

            if (data.IsVisibleBindingPath.HasValue())
            {
                var isVisibleBindingPath = TypescriptNaming.NormalizeBindingPath(BindingPrefix.Value + data.IsVisibleBindingPath);

                sb.AppendLine("{ " + isVisibleBindingPath + " &&");
                sb.PaddingCount++;
            }

            sb.AppendWithPadding("<BCard context={context}");

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

            sb.PaddingCount--;
            sb.AppendLine("</BCard>");

            if (data.IsVisibleBindingPath.HasValue())
            {
                sb.PaddingCount--;
                sb.AppendLine("}");

            }
        }
        #endregion
    }
}