using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;
using BOA.Common.Helpers;
using BOA.OneDesigner.AppModel;
using BOA.OneDesigner.Helpers;
using BOA.OneDesigner.JsxElementModel;
using BOA.OneDesigner.WpfControls;
using BOAPlugins.TypescriptModelGeneration;
using BOAPlugins.Utility;
using Host = BOA.OneDesigner.AppModel.Host;

namespace BOA.OneDesigner.CodeGeneration
{
    static class JsxModelHelper
    {


        
        public static void Write(this PaddedStringBuilder sb, ScreenInfo screenInfo, BTabBar data)
        {

     
            

            
        

            


            sb.AppendLine("<BTabBar context={context}");
            sb.PaddingCount++;
            sb.AppendLine("value={this.state.activeTab}");
            sb.AppendLine("mode='secondary'");
            sb.AppendLine("onChange={(event, value) => { this.setState( { activeTab: value} ); }}");

            if (data.SizeInfo != null && data.SizeInfo.IsEmpty == false)
            {
                sb.AppendLine("size = {"+ GetJsValue(data.SizeInfo) +"}");
            }

            sb.AppendLine("tabItems = {[");

            sb.PaddingCount+=2;

            for (var i = 0; i < data.Items.Count; i++)
            {
                var isLAstTab = i == data.Items.Count - 1;

                var bTabBarPage = data.Items[i];

                sb.AppendLine("{");
                sb.PaddingCount++;

                var title = GetLabelValue(screenInfo, bTabBarPage.TitleInfo);
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

                        decimal width = ((decimal)columnSpan / 12M) * 100M;

                        var bCard = bCardWpf.Model;

                        sb.AppendLine("<div style={{ width: '" + width + "%', padding: '15px' }}>");

                        sb.PaddingCount++;
                        sb.Write(screenInfo, bCard);
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


        #region Public Methods
        public static bool HasComponent<TComponentType>(this DivAsCardContainer divAsCardContainer)
        {
            var items = new List<object>();

            divAsCardContainer.VisitAllBComponents(c =>
            {
                if (c.GetType() == typeof(TComponentType))
                {
                    items.Add(c);
                }
            });

            return items.Any();
        }

        public static void VisitAllBComponents(this DivAsCardContainer divAsCardContainer, Action<object> action)
        {
            foreach (var card in divAsCardContainer.Items)
            {
                foreach (var item in card.Items)
                {
                    action(item);
                }

                action(card);
            }
        }

        public static void Write(this PaddedStringBuilder sb, ScreenInfo screenInfo, DivAsCardContainer data)
        {
            sb.AppendLine("<div>");

            sb.PaddingCount++;

            foreach (var bCard in data.Items)
            {
                sb.Write(screenInfo, bCard);

                sb.AppendLine(string.Empty);
            }

            sb.PaddingCount--;

            sb.AppendLine("</div>");
        }

        public static void Write(this PaddedStringBuilder sb, ScreenInfo screenInfo, BCard data)
        {
            sb.AppendWithPadding("<BCard context={context}");

            var labelValue = GetLabelValue(screenInfo, data.TitleInfo);
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

            var subComponents= new List<string>();

            foreach (var item in data.Items)
            {
                var bInput = item as BInput;
                if (bInput != null)
                {
                    var stringBuilder = new PaddedStringBuilder();

                    stringBuilder.Write(screenInfo, bInput);

                    subComponents.Add(stringBuilder.ToString());

                    continue;
                }

                var bTabBar = item as BTabBar;
                if (bTabBar != null)
                {
                    var stringBuilder = new PaddedStringBuilder();

                    stringBuilder.Write(screenInfo, bTabBar);

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

        public static void Write(this PaddedStringBuilder sb, ScreenInfo screenInfo, BInput data)
        {
            var solutionInfo = SolutionInfo.CreateFromTfsFolderPath(screenInfo.TfsFolderName);

            var propertyDefinition = CecilHelper.FindPropertyInfo(solutionInfo.TypeAssemblyPathInServerBin, screenInfo.RequestName, data.BindingPath);

            var isDecimal = CecilHelper.FullNameOfNullableDecimal == propertyDefinition.PropertyType.FullName ||
                            propertyDefinition.PropertyType.FullName == typeof(decimal).FullName;

            var isString = propertyDefinition.PropertyType.FullName == typeof(string).FullName;

            var bindingPathInJs = NormalizeBindingPath($"request.{data.BindingPath}");

            if (isString)
            {
                sb.AppendLine($"<BInput value = {{{bindingPathInJs}}}");
                sb.PaddingCount++;

                sb.AppendLine($"onChange = {{(e: any, value: string) => {bindingPathInJs} = value}}");
            }
            else if (isDecimal)
            {
                sb.AppendLine($"<BInputNumeric value = {{{bindingPathInJs}}}");
                sb.PaddingCount++;

                sb.AppendLine($"onChange = {{(e: any, value: number) => {bindingPathInJs} = value}}");
                sb.AppendLine("format = {\"D\"}");
                sb.AppendLine("maxLength = {22}");
            }
            else
            {
                sb.AppendLine($"<BInputNumeric value = {{{bindingPathInJs}}}");
                sb.PaddingCount++;

                sb.AppendLine($"onChange = {{(e: any, value: number) => {bindingPathInJs} = value}}");
                sb.AppendLine("maxLength = {10}");
            }

            var labelValue = GetLabelValue(screenInfo, data.LabelInfo);
            if (labelValue != null)
            {
                sb.AppendLine($"floatingLabelText = {labelValue}");
            }

            if (!string.IsNullOrWhiteSpace(data.IsVisibleBindingPath))
            {
                sb.AppendLine($"isVisible = {{{NormalizeBindingPath("request." + data.IsVisibleBindingPath)}}}");
            }

            if (!string.IsNullOrWhiteSpace(data.IsDisabledBindingPath))
            {
                sb.AppendLine($"disabled = {{{NormalizeBindingPath("request." + data.IsDisabledBindingPath)}}}");
            }

            if (data.SizeInfo != null && data.SizeInfo.IsEmpty == false)
            {
                sb.AppendLine("size = {"+ GetJsValue(data.SizeInfo) +"}");
            }

            sb.AppendLine("context = {context}/>");

            sb.PaddingCount--;
        }
        #endregion

        static string GetJsValue(SizeInfo size)
        {
			
			
            if (size.IsLarge)
            {
                return "ComponentSize.LARGE";
            }
            if (size.IsMedium)
            {
                return "ComponentSize.MEDIUM";
            }
            if(size.IsSmall)
            {
                return "ComponentSize.SMALL";
            }
            if(size.IsExtraSmall)
            {
                return "ComponentSize.XSMALL";
            }

            throw Error.InvalidOperation();
        }

        #region Methods
        static string GetLabelValue(ScreenInfo screenInfo, LabelInfo data)
        {
            if (data == null)
            {
                return null;
            }

            if (data.IsFreeText)
            {
                return '"' + data.FreeTextValue + '"';
            }

            if (data.IsRequestBindingPath)
            {
                return NormalizeBindingPath("request." + data.RequestBindingPath);
            }

            if (data.IsFromMessaging)
            {
                return $"getMessage(\"{screenInfo.MessagingGroupName}\", \"{data.MessagingValue}\")";
            }

            throw Error.InvalidOperation();
        }

        static string NormalizeBindingPath(string propertyNameInCSharp)
        {
            return string.Join(".", propertyNameInCSharp.SplitAndClear(".").ToList().ConvertAll(TypescriptNaming.GetResolvedPropertyName));
        }
        #endregion
    }
}