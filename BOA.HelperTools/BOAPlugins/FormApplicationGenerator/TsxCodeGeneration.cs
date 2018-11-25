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
                RenderCodeForJsx = GetJSXElementForRenderBrowsePage(fields,model.UserRawStringForMessaging)
            };
        }
        #endregion

        #region Methods
        static string GetJSXElementForRenderBrowsePage(IReadOnlyCollection<BField> fields, bool userRawStringForMessaging)
        {
            var renderCodes = new PaddedStringBuilder
            {
                PaddingLength = 4,
                PaddingCount  = 3
            };

            renderCodes.AppendLine("<BGridSection context={context}>");
            renderCodes.PaddingCount++;
            foreach (var dataField in fields)
            {
                renderCodes.AppendLine("<BGridRow context={context}>");

                renderCodes.PaddingCount++;

                RenderComponent(renderCodes, dataField, userRawStringForMessaging);

                renderCodes.PaddingCount--;

                renderCodes.AppendLine("</BGridRow>");
            }

            renderCodes.PaddingCount--;
            renderCodes.AppendLine("</BGridSection>");

            return renderCodes.ToString();
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
                    RenderCards(renderCodes, tab.Cards,model.UserRawStringForMessaging);
                    renderCodes.PaddingCount--;

                    renderCodes.PaddingCount--;

                    renderCodes.AppendLine("}");
                }
            }
            else
            {
                renderCodes.AppendWithPadding("");
                RenderCards(renderCodes, model.Cards,model.UserRawStringForMessaging);
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

                RenderComponent(renderCodes, dataField,userRawStringForMessaging);

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

                RenderCard(renderCodes, card, columnIndex,userRawStringForMessaging);

                columnIndex++;
            }

            renderCodes.PaddingCount--;
            renderCodes.AppendLine("</BCardSection>");
        }

        static void RenderComponent(PaddedStringBuilder output, BField dataBField,bool userRawStringForMessaging)
        {
            var valueAccessPath = Exporter.GetResolvedPropertyName(dataBField.Name);

            var label = "Message." + dataBField.Name;
            if (userRawStringForMessaging)
            {
                label = '"'+dataBField.Name+ '"';
            }


            if (dataBField.ComponentType == ComponentType.BDateTimePicker)
            {
                output.AppendLine("<BDateTimePicker format = \"DDMMYYYY\"");
                output.AppendLine("                 value = {data." + valueAccessPath + "}");
                output.AppendLine("                 dateOnChange = {(e: any, value: Date) => data." + valueAccessPath + " = value}");
                output.AppendLine("                 floatingLabelTextDate = {" + label + "}");
                output.AppendLine("                 context = {context}/>");
                return;
            }

            if (dataBField.ComponentType == ComponentType.BInput)
            {
                output.AppendLine("<BInput value = {data." + valueAccessPath + "}");
                output.AppendLine("        onChange = {(e: any, value: string) => data." + valueAccessPath + " = value}");
                output.AppendLine("        floatingLabelText = {" + label + "}");
                output.AppendLine("        context = {context}/>");
                return;
            }

            if (dataBField.ComponentType == ComponentType.BInputNumeric)
            {
                output.AppendLine("<BInputNumeric value = {data." + valueAccessPath + "}");
                output.AppendLine("               onChange = {(e: any, value: any) => data." + valueAccessPath + " = value}");
                output.AppendLine("               floatingLabelText = {" + label + "}");
                if (dataBField.DotNetType == DotNetType.Decimal)
                {
                    output.AppendLine("               format = {\"D\"}");
                    output.AppendLine("               maxLength = {22}");
                }
                else
                {
                    output.AppendLine("               maxLength = {10}");
                }

                output.AppendLine("               context = {context}/>");
                return;
            }

            if (dataBField.ComponentType == ComponentType.BAccountComponent)
            {
                output.AppendLine("<BAccountComponent accountNumber = {data." + valueAccessPath + "}");
                output.AppendLine("                   onAccountSelect = {(selectedAccount: any) => data." + valueAccessPath + " = selectedAccount ? selectedAccount.accountNumber : null}");
                output.AppendLine("                   isVisibleBalance={false}");
                output.AppendLine("                   isVisibleAccountSuffix={false}");
                output.AppendLine("                   enableShowDialogMessagesInCallback={false}");
                output.AppendLine("                   isVisibleIBAN={false}");
                output.AppendLine("                   ref={(r: any) => this.snaps." + dataBField.GetSnapName() + " = r}");
                output.AppendLine("                   context = {context}/>");
                return;
            }

            if (dataBField.ComponentType == ComponentType.BCheckBox)
            {
                output.AppendLine("<BCheckBox checked = {data." + valueAccessPath + "}");
                output.AppendLine("           onCheck = {(e: Object, isChecked: boolean) => data." + valueAccessPath + " = isChecked}");
                output.AppendLine("           label   = {" + label + "}");
                output.AppendLine("           context = {context}/>");
                return;
            }

            if (dataBField.ComponentType == ComponentType.BParameterComponent)
            {
                if (dataBField.DotNetType == DotNetType.Int32)
                {
                    output.AppendLine("<BParameterComponent selectedParamCode = {Helper.numberToString(data." + valueAccessPath + ")}");
                    output.AppendLine("                     onParameterSelect = {(selectedParameter: BOA.Types.Kernel.General.ParameterContract) => data." + valueAccessPath + " = selectedParameter ? Helper.stringToNumber(selectedParameter.paramCode) : null}");
                }
                else
                {
                    output.AppendLine("<BParameterComponent selectedParamCode = {data." + valueAccessPath + "}");
                    output.AppendLine("                     onParameterSelect = {(selectedParameter: BOA.Types.Kernel.General.ParameterContract) => data." + valueAccessPath + " = selectedParameter ? selectedParameter.paramCode : null}");
                }

                if (dataBField.ParamType.IsNullOrWhiteSpace())
                {
                    output.AppendLine("                     paramType=\"GENDER\"");
                }
                else
                {
                    output.AppendLine("                     paramType=\"" + dataBField.ParamType + "\"");
                }

                output.AppendLine("                     hintText = {" + label + "}");
                output.AppendLine("                     labelText = {" + label + "}");
                output.AppendLine("                     isAllOptionIncluded={true}");
                output.AppendLine("                     paramColumns={[");
                output.AppendLine("                            { name: \"paramCode\",        header: Message.Code,        visible: false },");
                output.AppendLine("                            { name: \"paramDescription\", header: Message.Description, width:   200 }");
                output.AppendLine("                     ]}");
                output.AppendLine("                     ref={(r: any) => this.snaps." + dataBField.GetSnapName() + " = r}");
                output.AppendLine("                     context = {context}/>");
                return;
            }

            if (dataBField.ComponentType == ComponentType.BBranchComponent)
            {
                output.AppendLine("<BBranchComponent selectedBranchId = {data." + valueAccessPath + "}");
                output.AppendLine("                  onBranchSelect = {(selectedBranch: BOA.Common.Types.BranchContract) => data." + valueAccessPath + " = selectedBranch ? selectedBranch.branchId : null}");
                output.AppendLine("                  mode={\"horizontal\"}");
                output.AppendLine("                  labelText = {" + label + "}");
                output.AppendLine("                  sortOption={BBranchComponent.name}");
                output.AppendLine("                  context = {context}/>");
            }
        }
        #endregion
    }
}