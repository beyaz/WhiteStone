using System.Collections.Generic;
using System.Linq;
using BOAPlugins.FormApplicationGenerator.Templates;
using BOAPlugins.FormApplicationGenerator.Types;
using BOAPlugins.FormApplicationGenerator.UI;
using BOAPlugins.Utility;

namespace BOAPlugins.FormApplicationGenerator.Logic
{
    public static class Map
    {
        public static OrchestrationFileForDetailForm ToOrchestrationFileForDefinitionForm(this MainWindowModel mainWindowModel)
        {
            return  new OrchestrationFileForDetailForm
            {
                //TODO:
            };
        }
        public static OrchestrationFileForListForm ToOrchestrationFileForListForm(this MainWindowModel mainWindowModel)
        {
            return  new OrchestrationFileForListForm
            {
                NamespaceNameForType        = mainWindowModel.NamingInfo.NamespaceNameForType,
                NamespaceName               = mainWindowModel.NamingInfo.NamespaceNameForOrchestration,
                ClassName                   = mainWindowModel.TableNameInDatabase + "ListForm",
                RequestName                 = mainWindowModel.NamingInfo.RequestNameForList,
                DefinitionFormDataClassName = mainWindowModel.NamingInfo.DefinitionFormDataClassName,
                GridColumnFields            = mainWindowModel.FormDataClassFields.Select(fieldInfo => fieldInfo.Name).ToArray()
            };
        }
        public static TransactionPageTemplate ToTransactionPageTemplate(this MainWindowModel mainWindowModel)
        {
            var template= new TransactionPageTemplate
            {
                NamespaceNameForType = mainWindowModel.NamingInfo.NamespaceNameForType,
                RequestName          = mainWindowModel.NamingInfo.RequestNameForList,
                ClassName            = mainWindowModel.TableNameInDatabase + @"Form",
                Snaps                = mainWindowModel.FormDataClassFields.GetSnaps(),
                IsTabForm = mainWindowModel.IsTabForm,
            };

            if (mainWindowModel.IsTabForm)
            {
                template.IsTabForm = true;
                template.ContentAsTabControl = new BTabControlTemplate
                {
                    TabPages = new List<TabPageTemplate>
                    {
                        new TabPageTemplate
                        {
                            content = new BCardSectionTemplate
                            {
                                Cards = new List<BCardTemplate>
                                {
                                    new BCardTemplate
                                    {
                                        Components = mainWindowModel.FormDataClassFields.Select(Map.GetRenderComponent).ToList()
                                    }
                                }
                            }
                        }
                    }
                };
            }
            else
            {
                template.ContentAsBCardSection = new BCardSectionTemplate
                {
                    Cards = new List<BCardTemplate>
                    {
                        new BCardTemplate
                        {
                            Components = mainWindowModel.FormDataClassFields.Select(Map.GetRenderComponent).ToList()
                        }
                    }
                };
            }

            return template;

        }
        public static BrowsePageTemplate ToBrowsePageTemplate(this MainWindowModel mainWindowModel)
        {
            return new BrowsePageTemplate
            {
                NamespaceNameForType = mainWindowModel.NamingInfo.NamespaceNameForType,
                RequestName          = mainWindowModel.NamingInfo.RequestNameForList,
                ClassName            = mainWindowModel.TableNameInDatabase + @"ListForm",
                DetailFormClassName  = mainWindowModel.TableNameInDatabase + @"Form",
                Snaps                = mainWindowModel.ListFormSearchFields.GetSnaps(),
                Components           = mainWindowModel.ListFormSearchFields.Select(Map.GetRenderComponent).ToList()
            };
            
        }

        public static IReadOnlyList<SnapInfo> GetSnaps(this IReadOnlyCollection<BField> fields)
        {
            return fields.Where(x => x.HasSnapName()).Select(dataField => new SnapInfo
            {
                Name              = dataField.GetSnapName(),
                ComponentTypeName = dataField.ComponentType.GetValueOrDefault().ToString()
            }).ToList();
        }

        public static BFieldTemplate GetRenderComponent(BField dataBField)
        {
            return new BFieldTemplate
            {

                Label                  = dataBField.Label,
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
                ValueAccessPath        = TypescriptNaming.GetResolvedPropertyName(dataBField.Name)
            };

        }


        public static BCardSectionTemplate ToBCardSectionTemplate(IReadOnlyCollection<BCard> cards)
        {
            return new BCardSectionTemplate
            {
                Cards = cards.Select(card => new BCardTemplate
                {
                    Title      = card.Title,
                    Components = card.Fields.Select(Map.GetRenderComponent).ToList()
                }).ToList()
            };


        }
    }
}
