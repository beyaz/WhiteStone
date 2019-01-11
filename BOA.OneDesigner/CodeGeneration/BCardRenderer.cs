using System;
using System.Collections.Generic;
using BOA.Common.Helpers;
using BOA.OneDesigner.AppModel;
using BOA.OneDesigner.JsxElementModel;

namespace BOA.OneDesigner.CodeGeneration
{

    static class BAccountComponentRenderer
    {
        public static void Write(PaddedStringBuilder sb, ScreenInfo screenInfo, BAccountComponent data)
        {

        }
    }

    static class BCardRenderer
    {
        public static void Write(PaddedStringBuilder sb, ScreenInfo screenInfo, BCard data)
        {
            sb.AppendWithPadding("<BCard context={context}");

            var labelValue = RenderHelper.GetLabelValue(screenInfo, data.TitleInfo);
            if (labelValue != null)
            {
                sb.Append($" title = {{{labelValue}}}");
            }

            if (data.LayoutProps != null)
            {
                sb.Append(" layoutProps = {{w:"+data.LayoutProps.Wide+", x:"+data.LayoutProps.X+"}}");
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
                    var stringBuilder = new PaddedStringBuilder();

                    BInputRenderer.Write(stringBuilder, screenInfo, bInput);

                    subComponents.Add(stringBuilder.ToString());

                    continue;
                }

                var bTabBar = item as BTabBar;
                if (bTabBar != null)
                {
                    var stringBuilder = new PaddedStringBuilder();

                    BTabBarRenderer.Write(stringBuilder, screenInfo, bTabBar);

                    subComponents.Add(stringBuilder.ToString());

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
        }
    }
}