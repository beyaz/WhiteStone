using System;
using System.Windows.Controls;
using System.Windows.Input;
using BOA.Common.Helpers;
using BOA.OneDesigner.AppModel;
using BOA.OneDesigner.JsxElementModel;
using CustomUIMarkupLanguage.UIBuilding;
using MahApps.Metro.Controls;

namespace BOA.OneDesigner.WpfControls
{
    class ComponentWpfModel
    {
        #region Public Properties
        public ComponentInfo Info { get; set; }

        public bool IsInToolbox { get; set; }

        public double ValueBindingPathTextBoxMinHeight { get; set; }
        #endregion
    }

    class ComponentWpf : StackPanel, IEventBusListener, ISupportSizeInfo
    {
        #region Constructors
        ComponentWpf()
        {
            MouseEnter += (s, e) => { Cursor = Cursors.Hand; };
            MouseLeave += (s, e) => { Cursor = Cursors.Arrow; };
        }
        #endregion

        #region Public Properties
        public Host Host { get; set; }

        public ComponentWpfModel Model => (ComponentWpfModel) DataContext;

        public SizeInfo SizeInfo => Model.Info?.SizeInfo;
        #endregion

        #region Properties
        bool SelectedElementIsNotThisElement => Host.SelectedElement != this;
        #endregion

        #region Public Methods
        public static ComponentWpf Create(Host host, ComponentInfo info, bool isInToolbox = false)
        {
            var wpf = new ComponentWpf
            {
                Host = host,
                DataContext = new ComponentWpfModel
                {
                    Info        = info,
                    IsInToolbox = isInToolbox
                }
            };

            wpf.LoadUI();

            return wpf;
        }
        #endregion

        #region Methods
        void EvaluateRowCount()
        {
            var rowCount = Model.Info.RowCount;

            if (rowCount > 0)
            {
                Model.ValueBindingPathTextBoxMinHeight = rowCount.Value * 10;
            }
            else
            {
                Model.ValueBindingPathTextBoxMinHeight = 10;
            }
        }

        void LoadUI()
        {
            var template = @"
{
    Margin: 10,
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
                    Text        :'{Binding " + Model.AccessPathOf(m => m.Info.LabelText) + @", Mode = OneWay}', 
                    MarginBottom:5, 
                    IsBold      :true
                }
                ,                
                {
                    view        :'TextBox', 
                    Text        :'{Binding " + Model.AccessPathOf(m => m.Info.ValueBindingPath) + @", Mode = OneWay}', 
                    IsReadOnly  :true
                }    
            ]
        }
        ,
        {
            ui          : 'StackPanel',
            IsVisible   : '{Binding " + Model.AccessPathOf(m => m.Info.Type.IsCreditCardComponent) + @"}',
            Childs      :
            [
                
                {
                    view        :'TextBlock',
                    Text        :'Credit Card Component', 
                    MarginBottom:5, 
                    IsBold      :true
                }
                ,                
                {
                    view        :'TextBox', 
                    Text        :'{Binding " + Model.AccessPathOf(m => m.Info.ValueBindingPath) + @", Mode = OneWay}', 
                    IsReadOnly  :true
                }    
            ]
        }
        ,
        {
            ui          : 'StackPanel',
            IsVisible   : '{Binding " + Model.AccessPathOf(m => m.Info.Type.IsAccountComponent) + @"}',
            Childs      :
            [
                
                {
                    view        :'TextBlock',
                    Text        :'{Binding " + Model.AccessPathOf(m => m.Info.LabelText) + @", Mode = OneWay}', 
                    MarginBottom:5, 
                    IsBold      :true
                }
                ,                
                {
                    view        :'TextBox', 
                    Text        :'{Binding " + Model.AccessPathOf(m => m.Info.ValueBindingPath) + @", Mode = OneWay}', 
                    IsReadOnly  :true
                }    
            ]
        }
        ,
        {
            ui          : 'StackPanel',
            IsVisible   : '{Binding " + Model.AccessPathOf(m => m.Info.Type.IsParameterComponent) + @"}',
            Childs      :
            [
                
                {
                    view        : 'TextBlock',
                    Text        : '{Binding " + Model.AccessPathOf(m => m.Info.LabelText) + @",Mode = OneWay}', 
                    MarginBottom: 5, 
                    IsBold      : true
                }
                ,                
                {
                    view        : 'TextBox', 
                    Text        : '{Binding " + Model.AccessPathOf(m => m.Info.ValueBindingPath) + @", Mode = OneWay}', 
                    IsReadOnly  : true
                }    
            ]
        }
        ,
        {
            ui          : 'WrapPanel',
            MarginTop   : 20,
            IsVisible   : '{Binding " + Model.AccessPathOf(m => m.Info.Type.IsInformationText) + @"}',
            Childs      :
            [
                
                {
                    view        : 'TextBlock',
                    Text        : '{Binding " + Model.AccessPathOf(m => m.Info.LabelText) + @",Mode = OneWay}', 
                    IsBold      : true
                }
                ,
                {
                    view        : 'TextBlock',
                    Text        : ' : ', 
                    MarginLeft  : 5,
                    IsBold      : true
                }
                ,                
                {
                    view        : 'TextBlock', 
                    Text        : '{Binding " + Model.AccessPathOf(m => m.Info.InfoTextValue) + @", Mode = OneWay}', 
                    MarginLeft  : 5
                }    
            ]
        }
        ,
        {
            ui          : 'StackPanel',
            IsVisible   : '{Binding " + Model.AccessPathOf(m => m.Info.Type.IsLabel) + @"}',
            Childs      :
            [
                
                {
                    view        : 'TextBlock',
                    Text        : '{Binding " + Model.AccessPathOf(m => m.Info.Text) + @",Mode = OneWay}', 
                    MarginTop   : 20,
                    IsBold      : '{Binding " + Model.AccessPathOf(m => m.Info.IsBold) + @",Mode = OneWay}'
                }
            ]
        }
        ,
        {
            ui          : 'StackPanel',
            IsVisible   : '{Binding " + Model.AccessPathOf(m => m.Info.Type.IsInput) + @"}',
            Childs      :
            [   
                {
                    view        : 'TextBlock',
                    Text        : '{Binding " + Model.AccessPathOf(m => m.Info.LabelText) + @", Mode = OneWay}', 
                    IsBold      : true
                }
                ,
                {
                    view        : 'TextBox',
                    MarginTop   : 5,
                    Text        : '{Binding " + Model.AccessPathOf(m => m.Info.ValueBindingPath) + @", Mode = OneWay}', 
                    IsReadOnly  : true,
                    MinHeight   : '{Binding " + Model.AccessPathOf(m => m.ValueBindingPathTextBoxMinHeight) + @", Mode = OneWay}'
                }
            ]
        }
        ,
        {
            ui          : 'StackPanel',
            IsVisible   : '{Binding " + Model.AccessPathOf(m => m.Info.Type.IsExcelBrowser) + @"}',
            Childs      :
            [   
                {
                    view        : 'TextBlock',
                    Text        : '{Binding " + Model.AccessPathOf(m => m.Info.LabelText) + @", Mode = OneWay}', 
                    IsBold      : true
                }
                ,
                {
                    view        : 'TextBox',
                    MarginTop   : 5,
                    Text        : '{Binding " + Model.AccessPathOf(m => m.Info.ValueBindingPath) + @", Mode = OneWay}', 
                    IsReadOnly  : true
                }
            ]
        }
        ,
        {
            ui          : 'StackPanel',
            IsVisible   : '{Binding " + Model.AccessPathOf(m => m.Info.Type.IsButton) + @"}',
            Childs      :
            [
                {
                    view        : 'Button',
                    Content     : '{Binding " + Model.AccessPathOf(m => m.Info.Text) + @", Mode = OneWay}'
                }
            ]
        }
    ]
}

";
            this.LoadJson(template);
        }

        void OnRowCountChanged()
        {
            if (SelectedElementIsNotThisElement)
            {
                return;
            }

            EvaluateRowCount();

            foreach (var textBox in this.FindChildren<TextBox>())
            {
                textBox.GetBindingExpression(MinHeightProperty)?.UpdateTarget();
            }
        }

        void UpdateLabel()
        {
            if (SelectedElementIsNotThisElement)
            {
                return;
            }

            foreach (var textBlock in this.FindChildren<TextBlock>())
            {
                textBlock.GetBindingExpression(TextBlock.TextProperty)?.UpdateTarget();

                textBlock.GetBindingExpression(TextBlock.FontWeightProperty)?.UpdateTarget();
            }

            foreach (var textBlock in this.FindChildren<Button>())
            {
                textBlock.GetBindingExpression(ContentControl.ContentProperty)?.UpdateTarget();
            }
        }
        #endregion

        #region IEventBusListener
        public event Action OnAttachToEventBus;
        public event Action OnDeAttachToEventBus;

        public void AttachToEventBus()
        {
            OnAttachToEventBus?.Invoke();

            Host.EventBus.Subscribe(EventBus.LabelChanged, UpdateLabel);
            Host.EventBus.Subscribe(EventBus.RowCountChanged, OnRowCountChanged);
        }

        public void DeAttachToEventBus()
        {
            OnDeAttachToEventBus?.Invoke();

            Host.EventBus.UnSubscribe(EventBus.LabelChanged, UpdateLabel);
            Host.EventBus.UnSubscribe(EventBus.RowCountChanged, OnRowCountChanged);
        }
        #endregion
    }
}