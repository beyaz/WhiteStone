using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BOA.Common.Helpers;
using BOAPlugins.ExportingModel;

namespace BOAPlugins.FormApplicationGenerator
{
    public static class Map
    {
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
                ValueAccessPath        = Exporter.GetResolvedPropertyName(dataBField.Name)
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
