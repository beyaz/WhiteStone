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

        

        static void RenderCards(PaddedStringBuilder renderCodes, IReadOnlyCollection<BCard> cards, bool userRawStringForMessaging)
        {
            var cardSectionTemplate = new BCardSectionTemplate
            {
                Cards = cards.Select(card=> new BCardTemplate
                {
                    Title      = card.Title.HasValue() ? "Message." + card.Title : null,
                    Components = card.Fields.Select(dataBField => GetRenderComponent(dataBField, userRawStringForMessaging)).ToList()
                }).ToList()
            };
            renderCodes.AppendAll(cardSectionTemplate.TransformText());

            
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