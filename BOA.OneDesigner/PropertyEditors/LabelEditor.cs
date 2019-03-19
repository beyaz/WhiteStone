using System.Windows.Controls;
using BOA.Common.Helpers;
using BOA.OneDesigner.AppModel;
using BOA.OneDesigner.JsxElementModel;
using CustomUIMarkupLanguage.UIBuilding;

namespace BOA.OneDesigner.PropertyEditors
{
    class LabelEditor : GroupBox, IHostItem
    {
        #region Constructors
        public LabelEditor()
        {
            LoadUI();
        }
        #endregion

        #region Public Properties
        public Host Host { get; set; }
        #endregion

        #region Public Methods
        public void FirePropertyChanged()
        {
            if (!IsLoaded)
            {
                return;
            }

            var host = Host ?? SM.Get<Host>();

            host.EventBus.Publish(EventBus.LabelChanged);
        }

        public void OnCheckedChanged()
        {
            if (!IsLoaded)
            {
                return;
            }

            LoadUI();

            var host = Host ?? SM.Get<Host>();
            host.EventBus.Publish(EventBus.LabelChanged);
        }
        #endregion

        #region Methods
        void LoadUI()
        {
            Content = null;
            this.LoadJson(@"

{
 Header:'Label',
 Margin:5,
 Content:
 {
    ui:'Grid',
	    rows:
        [
		    {ui:'RadioButton', label:'From Messaging', IsChecked:'{Binding " + nameof(LabelInfo.IsFromMessaging) + @"}', Checked:'OnCheckedChanged'    },
		    {ui:'RadioButton', label:'Free Text',      IsChecked:'{Binding " + nameof(LabelInfo.IsFreeText) + @"}',      Checked:'OnCheckedChanged'    },
            {ui:'RadioButton', label:'Bind from Request',      IsChecked:'{Binding " + nameof(LabelInfo.IsRequestBindingPath) + @"}',      Checked:'OnCheckedChanged'    },
		    
            {   
                ui          : 'Textbox',
                Text        : '{Binding " + nameof(LabelInfo.FreeTextValue) + @"}',  
                IsVisible   : '{Binding " + nameof(LabelInfo.IsFreeText) + @"}', 
                KeyUp       : 'FirePropertyChanged'
            }
            ,
            {
                ui          : 'MessagingIntellisenseTextBox',
                Text        : '{Binding " + nameof(LabelInfo.MessagingValue) + @"}', 
                IsVisible   : '{Binding " + nameof(LabelInfo.IsFromMessaging) + @"}',
                TextChanged : 'FirePropertyChanged'
            }
            ,
            {
                ui                      : 'RequestIntellisenseTextBox',
                Text                    : '{Binding RequestBindingPath}', 
                Label                   : 'Binding Path',
                IsVisible               : '{Binding " + nameof(LabelInfo.IsRequestBindingPath) + @"}',
                KeyUp                   : 'FirePropertyChanged',
                ShowOnlyDotnetCoreTypes : true
            }

	    ]
 }

}


");
        }
        #endregion
    }
}