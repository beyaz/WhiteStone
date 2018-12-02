using System.Collections.Generic;
using System.Linq;
using BOAPlugins.FormApplicationGenerator.Templates;
using BOAPlugins.FormApplicationGenerator.Types;
using BOAPlugins.FormApplicationGenerator.UI;
 using BOAPlugins.TypescriptModelGeneration;
using BOAPlugins.Utility;

namespace BOAPlugins.FormApplicationGenerator.Logic
{
    public static class Map
    {
        public static OrchestrationFileForDetailForm ToOrchestrationFileForDefinitionForm(this Model Model)
        {
            return  new OrchestrationFileForDetailForm
            {
                //TODO:
            };
        }
        public static OrchestrationFileForListForm ToOrchestrationFileForListForm(this Model Model)
        {
            return  new OrchestrationFileForListForm
            {
                NamespaceNameForType        = Model.NamespaceNameForType,
                NamespaceName               = Model.NamespaceNameForOrchestration,
                ClassName                   = Model.TableNameInDatabase + "ListForm",
                RequestName                 = Model.RequestNameForList,
                DefinitionFormDataClassName = Model.NamingInfo.DefinitionFormDataClassName,
                GridColumnFields            = Model.FormDataClassFields.Select(fieldInfo => fieldInfo.Name).ToArray()
            };
        }
        public static TransactionPageTemplate ToTransactionPageTemplate(this Model model)
        {
            var template= new TransactionPageTemplate
            {
                NamespaceNameForType = model.NamespaceNameForType,
                RequestName          = model.RequestNameForList,
                ClassName            = model.TableNameInDatabase + @"Form",
                Snaps                = model.FormDataClassFields.GetSnaps(),
                IsTabForm = model.IsTabForm,
            };

            if (model.IsTabForm)
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
                                        Components = model.FormDataClassFields.Select(Map.GetRenderComponent).ToList()
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
                            Components = model.FormDataClassFields.Select(Map.GetRenderComponent).ToList()
                        }
                    }
                };
            }

            return template;

        }
        public static BrowsePageTemplate ToBrowsePageTemplate(this Model model)
        {
            return new BrowsePageTemplate
            {
                NamespaceNameForType = model.NamespaceNameForType,
                RequestName          = model.RequestNameForList,
                ClassName            = model.TableNameInDatabase + @"ListForm",
                DetailFormClassName  = model.TableNameInDatabase + @"Form",
                Snaps                = model.ListFormSearchFields.GetSnaps(),
                Components           = model.ListFormSearchFields.Select(Map.GetRenderComponent).ToList()
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
