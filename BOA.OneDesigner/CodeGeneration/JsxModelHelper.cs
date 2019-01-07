using System;
using System.Collections.Generic;
using System.Linq;
using BOA.Common.Helpers;
using BOA.OneDesigner.AppModel;
using BOA.OneDesigner.Helpers;
using BOA.OneDesigner.JsxElementModel;
using BOAPlugins.TypescriptModelGeneration;

namespace BOA.OneDesigner.CodeGeneration
{
    static class JsxModelHelper
    {
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
                sb.Write(screenInfo,bCard);

                sb.AppendLine(string.Empty);
            }

            sb.PaddingCount--;

            sb.AppendLine("</div>");
        }


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
            if(data.IsRequestBindingPath)
            {
                return "request." + data.RequestBindingPath;
            }
            if(data.IsFromMessaging)
            {
                return $"getMessage(\"{screenInfo.MessagingGroupName}\", \"{data.MessagingValue}\")";
            }

            throw Error.InvalidOperation();
        }


        public static void Write(this PaddedStringBuilder sb, ScreenInfo screenInfo, BCard data)
        {
            sb.AppendWithPadding("<BCard context={context}");

            var labelValue = GetLabelValue(screenInfo,data.TitleInfo);
            if (labelValue != null)
            {
                sb.Append($" title = {labelValue}");  
            }
            
            sb.Append(">");
            sb.Append(Environment.NewLine);

            sb.PaddingCount++;

            foreach (var item in data.Items)
            {
                var bInput = item as BInput;
                if (bInput != null)
                {
                    sb.Write(screenInfo,bInput);
                    continue;
                }

                throw Error.InvalidOperation();
            }

            sb.PaddingCount--;
            sb.AppendLine("</BCard>");
        }

        public static void Write(this PaddedStringBuilder sb, ScreenInfo screenInfo ,BInput data)
        {
            
            var solutionInfo = SolutionInfo.CreateFromTfsFolderPath(screenInfo.TfsFolderName);

            var propertyDefinition = CecilHelper.FindPropertyInfo(solutionInfo.TypeAssemblyPathInServerBin,screenInfo.RequestName,data.BindingPath);

            var isDecimal = CecilHelper.FullNameOfNullableDecimal == propertyDefinition.PropertyType.FullName ||
                            propertyDefinition.PropertyType.FullName == typeof(decimal).FullName;


            var isString = propertyDefinition.PropertyType.FullName == typeof(string).FullName;


            if (isString)
            {

                sb.AppendLine($"<BInput value = {{request.{data.BindingPath}}}");
                sb.PaddingCount++;

                sb.AppendLine($"onChange = {{(e: any, value: string) => request.{data.BindingPath} = value}}");

            }
            else if(isDecimal)
            {
                sb.AppendLine($"<BInputNumeric value = {{request.{data.BindingPath}}}");
                sb.PaddingCount++;

                sb.AppendLine($"onChange = {{(e: any, value: any) => request.{data.BindingPath} = value}}");
                sb.AppendLine("format = {\"D\"}");
                sb.AppendLine("maxLength = {22}");
            }
            else
            {
                sb.AppendLine($"<BInputNumeric value = {{request.{data.BindingPath}}}");
                sb.PaddingCount++;

                sb.AppendLine($"onChange = {{(e: any, value: any) => request.{data.BindingPath} = value}}");
                sb.AppendLine("maxLength = {10}");
            }
            


            var labelValue = GetLabelValue(screenInfo,data.LabelInfo);
            if (labelValue != null)
            {
                sb.AppendLine($"floatingLabelText = {labelValue}");  
            }

            sb.AppendLine("context = {context}/>");

            sb.PaddingCount--;
        }
        #endregion
    }
}