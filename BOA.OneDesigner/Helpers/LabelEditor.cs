﻿using System.Windows.Controls;
using BOA.OneDesigner.AppModel;
using BOA.OneDesigner.JsxElementModel;
using BOA.OneDesigner.WpfControls;
using CustomUIMarkupLanguage.UIBuilding;

namespace BOA.OneDesigner.Helpers
{
    class LabelEditor : GroupBox,IHostItem
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
               ui:'RequestIntellisenseTextBox', Text:'{Binding RequestBindingPath}', 
               Label:'Binding Path',
               ShowOnlyStringProperties:true,
               IsVisible:'{Binding " + nameof(LabelInfo.IsRequestBindingPath) + @"}',
               KeyUp:'FirePropertyChanged'
            }

	    ]
 }

}


");
        }
        #endregion

        #region Public Methods
        public void FirePropertyChanged()
        {
            if (Host == null)
            {
                return;
                 // TODO:??
                // throw Error.InvalidOperation();
            }
            Host.EventBus.Publish(EventBus.OnComponentPropertyChanged);
        }

        public void OnCheckedChanged()
        {
            if (!IsLoaded)
            {
                return;
            }
            this.RefreshDataContext();
            Host.EventBus.Publish(EventBus.OnComponentPropertyChanged);
        }
        #endregion

        public Host Host { get; set; }
    }
}