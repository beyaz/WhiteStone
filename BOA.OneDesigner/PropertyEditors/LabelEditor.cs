using System.Windows;
using System.Windows.Controls;
using BOA.Common.Helpers;
using BOA.OneDesigner.AppModel;
using BOA.OneDesigner.JsxElementModel;
using CustomUIMarkupLanguage.UIBuilding;
using WhiteStone.UI.Container;

namespace BOA.OneDesigner.PropertyEditors
{
    class LabelEditor : GroupBox, IHostItem
    {
        public LabeledTextBox _dateFormatInput;


        void RefreshDateFormatVisibility( )
        {
            var data = (LabelInfo) DataContext;
            if (data == null || _dateFormatInput == null)
            {
                return;
            }

            if (data.IsRequestBindingPath)
            {
                var isBooleanProperty = ( Host ?? SM.Get<Host>())?.RequestIntellisenseData?.RequestDatePropertyIntellisense?.Contains(data.RequestBindingPath) == true;
                
                if (isBooleanProperty)
                {
                    _dateFormatInput.Visibility = Visibility.Visible;
                }
                else
                {
                    _dateFormatInput.Visibility = Visibility.Collapsed;
                }

            }
            else
            {
                _dateFormatInput.Visibility = Visibility.Collapsed;
            }
        }

        #region Constructors
        public LabelEditor()
        {
            LoadUI();
            DataContextChanged += (s, e) => { RefreshDateFormatVisibility(); };
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

            RefreshDateFormatVisibility();
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
            if (Header == null || (Header as string)?.HasValue() == false)
            {
                Header = "Label";
            }

            Content = null;
            this.LoadJson(@"

{
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
            },
            {
                ui          : 'TextBox',
                Name       : '_dateFormatInput',
                Text        : '{Binding DateFormat}', 
                Label       : 'Date Format (Örn:DD/MM//YYYY)'
            }

	    ]
 }

}


");
        }
        #endregion
    }
}