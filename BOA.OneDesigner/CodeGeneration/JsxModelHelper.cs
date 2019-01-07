using System;
using System.Collections.Generic;
using System.Linq;
using BOA.Common.Helpers;
using BOA.OneDesigner.AppModel;
using BOA.OneDesigner.JsxElementModel;

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

        public static void Write(this PaddedStringBuilder sb, DivAsCardContainer data)
        {
            sb.AppendLine("<div>");

            sb.PaddingCount++;

            foreach (var bCard in data.Items)
            {
                sb.Write(bCard);

                sb.AppendLine(string.Empty);
            }

            sb.PaddingCount--;

            sb.AppendLine("</div>");
        }

        public static void Write(this PaddedStringBuilder sb, BCard data)
        {
            sb.Append("<BCard context={context}");
            var hasTitle = string.IsNullOrWhiteSpace(data.Title) == false;
            if (hasTitle)
            {
                sb.AppendLine($" title = {{{data.Title}}}");
            }

            sb.AppendLine(">");

            sb.PaddingCount++;

            foreach (var item in data.Items)
            {
                var bInput = item as BInput;
                if (bInput != null)
                {
                    sb.Write(bInput);
                    continue;
                }

                throw Error.InvalidOperation();
            }

            sb.PaddingCount--;
            sb.Append("</BCard>");
        }

        public static void Write(this PaddedStringBuilder sb, BInput data)
        {
            sb.AppendLine($"<BInput value = {{request.{data.BindingPath}}}");
            sb.PaddingCount += 2;

            sb.AppendLine($"onChange = {{(e: any, value: string) => {data.BindingPath} = value}}");
            sb.AppendLine($"floatingLabelTextDate = {{{data.Label}}}");
            sb.AppendLine("context = {context}/>");

            sb.PaddingCount -= 2;
        }
        #endregion
    }
}