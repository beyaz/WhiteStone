using BOA.Common.Helpers;
using BOA.OneDesigner.AppModel;
using WhiteStone.UI.Container;

namespace BOA.OneDesigner.PropertyEditors
{
    class OrchestrationIntellisense:LabeledComboBox
    {
        public OrchestrationIntellisense()
        {
            ItemsSource = SM.Get<Host>().RequestIntellisenseData.OrchestrationMethods;
        }
    }
}