using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;
using BOA.OneDesigner.JsxElementModel;
using CustomUIMarkupLanguage.UIBuilding;

namespace BOA.OneDesigner.WpfControls
{
    class LabelEditor : GroupBox
    {
        
        #region Constructors
        public LabelEditor()
        {
            
            
            this.LoadJson(@"

{
 Header:'Label',
 Margin:5,
 Content:
 {
    ui:'Grid',
	    rows:[
		    {ui:'RadioButton', label:'From Messaging', IsChecked:'{Binding " + nameof(LabelInfo.IsFromMessaging) + @"}', Checked:'OnCheckedChanged'    },
		    {ui:'RadioButton', label:'Free Text',      IsChecked:'{Binding " + nameof(LabelInfo.IsFreeText) + @"}',      Checked:'OnCheckedChanged'    },
            {ui:'RadioButton', label:'Bind from Request',      IsChecked:'{Binding " + nameof(LabelInfo.IsRequestBindingPath) + @"}',      Checked:'OnCheckedChanged'    },
		    {ui:'Textbox',Text:'{Binding " + nameof(LabelInfo.FreeTextValue) + @"}',  IsVisible:'{Binding " + nameof(LabelInfo.IsFreeText) + @"}', KeyUp:'FirePropertyChanged'      },
            {
             ui:'MessagingIntellisenseTextBox',
             Text:'{Binding " + nameof(LabelInfo.MessagingValue) + @"}', 
             IsVisible:'{Binding " + nameof(LabelInfo.IsFromMessaging) + @"}',
             TextChanged:'FirePropertyChanged'
            },
            {
               ui:'RequestIntellisenseTextBox', Text:'{Binding RequestBindingPath}', Label:'Binding Path',
               IsVisible:'{Binding " + nameof(LabelInfo.IsRequestBindingPath) + @"}',
               KeyUp:'FirePropertyChanged'
            }

	    ]
 }

}


");
        }
        #endregion

        bool IsRefreshingDataContext;
        #region Public Methods
        public void OnCheckedChanged()
        {
            RefreshDataContext();
        }

        public void FirePropertyChanged()
        {
            EventBus. Publish(EventBus.OnComponentPropertyChanged);
        }

        public void RefreshDataContext()
        {
            if (IsRefreshingDataContext)
            {
                return;
            }

            EventBus. Publish(EventBus.OnComponentPropertyChanged);

            IsRefreshingDataContext = true;

            var dataContext = DataContext;
            DataContext = null;
            DataContext = dataContext;

            IsRefreshingDataContext = false;
        }

        
        #endregion
    }
}