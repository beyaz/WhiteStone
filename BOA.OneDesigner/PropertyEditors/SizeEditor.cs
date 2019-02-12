using System.Windows.Controls;
using BOA.Common.Helpers;
using BOA.OneDesigner.AppModel;
using BOA.OneDesigner.JsxElementModel;
using CustomUIMarkupLanguage.UIBuilding;

namespace BOA.OneDesigner.PropertyEditors
{
    class SizeEditor : GroupBox, IHostItem
    {
        #region Constructors
        public SizeEditor()
        {
            this.LoadJson(@"

{
 Header:'Size',
 Margin:5,
 Content:
 {
    ui:'Grid',
	    rows:[
		    {ui:'RadioButton', label:'Large',        IsChecked:'{Binding " + nameof(SizeInfo.IsLarge) + @"}',        Checked:'OnCheckedChanged'    },
		    {ui:'RadioButton', label:'Medium',       IsChecked:'{Binding " + nameof(SizeInfo.IsMedium) + @"}',       Checked:'OnCheckedChanged'    },
            {ui:'RadioButton', label:'Small',        IsChecked:'{Binding " + nameof(SizeInfo.IsSmall) + @"}',        Checked:'OnCheckedChanged'    },
            {ui:'RadioButton', label:'Extra Small',  IsChecked:'{Binding " + nameof(SizeInfo.IsExtraSmall) + @"}',   Checked:'OnCheckedChanged'    }
	    ]
 }

}


");
        }
        #endregion

        #region Public Properties
        public Host Host { get; set; }
        #endregion

        #region Public Methods
        public void OnCheckedChanged()
        {
            if (!IsLoaded)
            {
                return;
            }

            if (Host == null)
            {
                Host = SM.Get<Host>();
            }

            Host.EventBus.Publish(EventBus.SizeChanged);
        }
        #endregion
    }
}