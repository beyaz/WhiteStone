using System;
using System.Windows.Controls;
using BOA.Common.Helpers;
using BOA.OneDesigner.AppModel;
using BOA.OneDesigner.JsxElementModel;
using CustomUIMarkupLanguage.UIBuilding;

namespace BOA.OneDesigner.WpfControls
{
    class ComponentWpfModel
    {
        #region Public Properties
        public ComponentInfo Info        { get; set; }
        public bool          IsInToolbox { get; set; }
        #endregion
    }

    class ComponentWpf : StackPanel, IEventBusListener, ISupportSizeInfo
    {
        #region Constructors
        public ComponentWpf()
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
        ,
        {
            ui          : 'StackPanel',
            IsVisible   : '{Binding " + Model.AccessPathOf(m => m.Info.Type.IsBranchComponent) + @"}',
            Childs      :
            [
                
                {
                    view        :'TextBlock',
                    Text        :'{Binding " + Model.AccessPathOf(m=>m.Info.LabelText) + @",Mode = OneWay}', 
                    MarginBottom:5, 
                    IsBold      :true
                }
                ,                
                {
                    view        :'TextBox', 
                    Text        :'{Binding " + Model.AccessPathOf(m=>m.Info.ValueBindingPath) + @", Mode = OneWay}', 
                    IsReadOnly  :true
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
        public Host              Host  { get; set; }
        public ComponentWpfModel Model => (ComponentWpfModel) DataContext;

        public SizeInfo SizeInfo => Model.Info?.SizeInfo;
        #endregion

        #region Public Methods
        public static ComponentWpf Create(Host host, ComponentInfo info)
        {
            return new ComponentWpf
            {
                Host = host,
                DataContext = new ComponentWpfModel
                {
                    Info = info
                }
            };
        }
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