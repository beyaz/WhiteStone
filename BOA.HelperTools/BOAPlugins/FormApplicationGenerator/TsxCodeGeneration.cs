using System;
using System.Collections.Generic;
using System.Linq;
using BOA.Common.Helpers;
using BOAPlugins.ExportingModel;

namespace BOAPlugins.FormApplicationGenerator
{
    class TsxCodeInfo
    {
        #region Public Properties
        public bool   HasSnap          { get; set; }
        public string RenderCodeForJsx { get; set; }
        public string SnapDeclaration  { get; set; }
        public string SnapDefinition   { get; set; }
        #endregion
    }

    static class TsxCodeGeneration
    {
        #region Public Methods
        public static TsxCodeInfo EvaluateTSCodeInfo(Model model, bool isDefinitionForm)
        {
            var fields = isDefinitionForm ? model.FormDataClassFields : model.ListFormSearchFields;

            var snapDeclaration = "";

            var snapDefinition = new PaddedStringBuilder();

            var hasSnap = fields.Any(x => x.HasSnapName());
            if (hasSnap)
            {
                snapDefinition.AppendLine("interface ISnaps");
                snapDefinition.AppendLine("{");
                snapDefinition.PaddingCount++;

                foreach (var dataField in fields.Where(x => x.HasSnapName()))
                {
                    snapDefinition.AppendLine($"{dataField.GetSnapName()}: {dataField.ComponentType};");
                }

                snapDefinition.PaddingCount--;
                snapDefinition.AppendLine("}");

                snapDeclaration = "snaps: ISnaps;";
            }

            if (isDefinitionForm)
            {
                return new TsxCodeInfo
                {
                    HasSnap          = hasSnap,
                    SnapDeclaration  = snapDeclaration,
                    SnapDefinition   = snapDefinition.ToString(),
                    RenderCodeForJsx = GetJSXElementForRenderDefinitionPage(model)
                };
            }

            return new TsxCodeInfo
            {
                HasSnap          = hasSnap,
                SnapDeclaration  = snapDeclaration,
                SnapDefinition   = snapDefinition.ToString(),
                RenderCodeForJsx = GetJSXElementForRenderBrowsePage(fields, model.UserRawStringForMessaging)
            };
        }
        #endregion

        #region Methods
        static string GetJSXElementForRenderBrowsePage(IReadOnlyCollection<BField> fields, bool userRawStringForMessaging)
        {
            var template = new JSXElementForRenderBrowsePage
            {
                Components = fields.Select(dataField=> GetRenderComponent(dataField, userRawStringForMessaging)).ToList()
            };
            template.PushIndent("            ");
            return template.TransformText();
        }

        static string GetJSXElementForRenderDefinitionPage(Model model)
        {
            var renderCodes = new PaddedStringBuilder
            {
                PaddingLength = 4,
                PaddingCount  = 3
            };

            if (model.IsTabForm)
            {
                var isFirst = true;
                foreach (var tab in model.Tabs)
                {
                    if (isFirst)
                    {
                        isFirst = false;
                    }
                    else
                    {
                        renderCodes.AppendLine(",");
                    }

                    renderCodes.AppendLine("{");

                    renderCodes.PaddingCount++;
                    renderCodes.AppendLine("text: Message." + tab.Title + ",");
                    renderCodes.AppendWithPadding("content:");

                    renderCodes.PaddingCount++;
                    RenderCards(renderCodes, tab.Cards, model.UserRawStringForMessaging);
                    renderCodes.PaddingCount--;

                    renderCodes.PaddingCount--;

                    renderCodes.AppendLine("}");
                }
            }
            else
            {
                renderCodes.AppendWithPadding("");
                RenderCards(renderCodes, model.Cards, model.UserRawStringForMessaging);
            }

            return renderCodes.ToString();
        }

        static void RenderCard(PaddedStringBuilder renderCodes, BCard card, int? columnIndex, bool userRawStringForMessaging)
        {
            var titleAttribute       = card.Title.HasValue() ? " title={Message." + card.Title + "}" : "";
            var columnIndexAttribute = columnIndex.HasValue ? " column={" + columnIndex + "}" : "";

            renderCodes.AppendLine("<BCard context={context}" + titleAttribute + columnIndexAttribute + ">");

            renderCodes.PaddingCount++;
            foreach (var dataField in card.Fields)
            {
                renderCodes.PaddingCount++;

                renderCodes.AppendLine("");

                RenderComponent(renderCodes, dataField, userRawStringForMessaging);

                renderCodes.PaddingCount--;
            }

            renderCodes.PaddingCount--;

            renderCodes.AppendLine("</BCard>");
        }

        static void RenderCards(PaddedStringBuilder renderCodes, IReadOnlyCollection<BCard> cards, bool userRawStringForMessaging)
        {
            var thresholdColumnCount = "";
            if (cards.Count > 1)
            {
                thresholdColumnCount = " thresholdColumnCount={3}";
            }

            renderCodes.Append("<BCardSection context={context}" + thresholdColumnCount + ">" + Environment.NewLine);
            renderCodes.PaddingCount++;

            int? columnIndex = 0;

            foreach (var card in cards)
            {
                if (columnIndex == 3)
                {
                    columnIndex = 0;
                }

                if (cards.Count == 1)
                {
                    columnIndex = null;
                }

                RenderCard(renderCodes, card, columnIndex, userRawStringForMessaging);

                columnIndex++;
            }

            renderCodes.PaddingCount--;
            renderCodes.AppendLine("</BCardSection>");
        }

        static void RenderComponent(PaddedStringBuilder output, BField dataBField, bool userRawStringForMessaging)
        {
            var template = GetRenderComponent(dataBField, userRawStringForMessaging);

            output.AppendAll(template.TransformText());
        }

        static BoaJsxComponentRenderTemplate GetRenderComponent( BField dataBField, bool userRawStringForMessaging)
        {
            var valueAccessPath = Exporter.GetResolvedPropertyName(dataBField.Name);

            var componentLabel = "Message." + dataBField.Name;
            if (userRawStringForMessaging)
            {
                componentLabel = '"' + dataBField.Name + '"';
            }

            return new BoaJsxComponentRenderTemplate
            {

                Label                  = componentLabel,
                IsBDateTimePicker      = dataBField.ComponentType == ComponentType.BDateTimePicker,
                IsBInput               = dataBField.ComponentType == ComponentType.BInput,
                IsBInputNumericDecimal = dataBField.ComponentType == ComponentType.BInputNumeric && dataBField.DotNetType == DotNetType.Decimal,
                IsBInputNumeric        = dataBField.ComponentType == ComponentType.BInputNumeric,
                IsBAccountComponent    = dataBField.ComponentType == ComponentType.BAccountComponent,
                SnapName               = dataBField.GetSnapName(),
                IsBCheckBox            = dataBField.ComponentType == ComponentType.BCheckBox,
                IsBParameterComponent  = dataBField.ComponentType == ComponentType.BParameterComponent,
                ValueTypeIsInt32       = dataBField.DotNetType == DotNetType.Int32,
                ParamType              = dataBField.ParamType ?? "GENDER",
                IsBBranchComponent     = dataBField.ComponentType == ComponentType.BBranchComponent,
                ValueAccessPath        = valueAccessPath
            };
            
        }
        #endregion
    }
}