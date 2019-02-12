using System;
using System.Windows.Controls;
using BOA.Common.Helpers;
using BOA.OneDesigner.AppModel;
using BOA.OneDesigner.JsxElementModel;
using CustomUIMarkupLanguage.UIBuilding;

namespace BOA.OneDesigner.WpfControls
{
    class ComponentModel
    {
        #region Public Properties
        public ComponentInfo Info        { get; set; }
        public bool          IsInToolbox { get; set; }
        #endregion
    }

    class Component2 : StackPanel, IEventBusListener, ISupportSizeInfo
    {

        public static Component2 Create(Host host, ComponentInfo info)
        {
            return new Component2
            {
                Host = host,
                DataContext = new ComponentModel
                {
                    Info = info
                }
            };
        }

        #region Constructors
        public Component2()
        {
            var template = @"
{
    Childs:
    [
        {
            ui          : 'StackPanel',
            IsVisible   : '{Binding " + Model.AccessPathOf(m => m.Info.Type.IsDivider) + @"}',
            Background  : 'LightGray',
            MinHeight   : 10,
            Childs      :
            [
                {
                    ui          : 'Label',
                    IsVisible   : '{Binding " + Model.AccessPathOf(m => m.IsInToolbox) + @"}',
                    Content     : 'b-divider'

                }
            ]
        }        
    ]
}

";
            this.LoadJson(template);
        }
        #endregion

        #region Public Properties
        public Host           Host  { get; set; }
        public ComponentModel Model => (ComponentModel) DataContext;

        public SizeInfo SizeInfo => Model.Info?.SizeInfo;
        #endregion

        #region IEventBusListener
        public event Action OnAttachToEventBus;
        public event Action OnDeAttachToEventBus;

        public void AttachToEventBus()
        {
        }

        public void DeAttachToEventBus()
        {
        }
        #endregion
    }
}