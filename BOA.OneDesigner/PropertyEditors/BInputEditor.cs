using System.Windows;
using System.Windows.Controls;
using BOA.OneDesigner.AppModel;
using BOA.OneDesigner.JsxElementModel;
using CustomUIMarkupLanguage.UIBuilding;
using WhiteStone.UI.Container;

namespace BOA.OneDesigner.PropertyEditors
{
    class BInputEditor : StackPanel, IHostItem
    {
        public void OnValueBindingPathChanged()
        {
            var isStringProperty = Host.RequestIntellisenseData.RequestStringPropertyIntellisense.Contains(Model.ValueBindingPath);
            if (isStringProperty)
            {
                _rowCount.Visibility= Visibility.Visible;
                _mask.Visibility = Visibility.Visible;
            }
            else
            {
                _rowCount.Visibility = Visibility.Collapsed;
                _mask.Visibility     = Visibility.Collapsed;
            }

            var isNullableInt = Host.RequestIntellisenseData.RequestNullableInt32PropertyIntellisense.Contains(Model.ValueBindingPath);
            var isInt = Host.RequestIntellisenseData.RequestNotNullInt32PropertyIntellisense.Contains(Model.ValueBindingPath);


            if (isNullableInt || isInt)
            {
                _isAccountNumber.Visibility = Visibility.Visible;
            }
            else
            {
                _isAccountNumber.Visibility = Visibility.Collapsed;
            }

        }

        public void OnRowCountChanged()
        {
            Host.EventBus.Publish(EventBus.RowCountChanged);
        }

        #region Fields
        public LabeledTextBox _rowCount,_mask;
        public LabelEditor _labelEditor;
        public SizeEditor  _sizeEditor;
        public CheckBox _isAccountNumber;
        #endregion

        #region Constructors
        public BInputEditor()
        {
            this.LoadJson(@"

{ 
    Margin:10,
	Childs:[
		{ui:'RequestIntellisenseTextBox', Margin:5, Text:'{Binding " + nameof(BInput.ValueBindingPath) + @"}', Label:'Binding Path', TextChanged:'"+nameof(OnValueBindingPathChanged)+@"' },
		{ui:'LabelEditor', Name:'" + nameof(_labelEditor) + @"', DataContext:'{Binding " + nameof(BInput.LabelInfo) + @"}'},
        {ui:'SizeEditor',Name:'" + nameof(_sizeEditor) + @"',   Header:'Size', MarginTop:10, DataContext:'{Binding " + nameof(BInput.SizeInfo) + @"}'},

        {ui:'RequestIntellisenseTextBox', ShowOnlyBooleanProperties:true, Margin:5, Text:'{Binding " + nameof(BInput.IsVisibleBindingPath) + @"}', Label:'Is Visible' },
        {ui:'RequestIntellisenseTextBox', ShowOnlyBooleanProperties:true, Margin:5, Text:'{Binding " + nameof(BInput.IsDisabledBindingPath) + @"}', Label:'Is Disabled' },


        {   
            ui       :'LabeledTextBox', 
            Label    :'Row Count', 
            MarginTop: 10, 
            Text     : '{Binding " + nameof(Model.RowCount) + @",Converter=WhiteStone.UI.Container.StringToNullableInt32Converter}',
            Name     :'"+nameof(_rowCount)+@"',
          TextChanged:'"+nameof(OnRowCountChanged)+@"'
        },
        {   
            ui       :'LabeledTextBox', 
            Label    :'Mask', 
            MarginTop: 10, 
            Text     : '{Binding " + nameof(Model.Mask) + @"}',
            Name     :'"+nameof(_mask)+@"'          
        },

       {   
            ui       :'CheckBox', 
            Content  :'Is Account Number', 
            MarginTop: 10, 
            IsChecked: '{Binding " + nameof(Model.IsAccountComponent) + @"}', 
            Checked  : '" + nameof(OnIsAccountComponentChanged) + @"',
            Unchecked: '" + nameof(OnIsAccountComponentChanged) + @"',
            Name     : '"+nameof(_isAccountNumber)+@"'
        },

        {ui:'Button', Text:'Delete',Click:'" + nameof(Delete) + @"'}
	]
}

");

            Loaded += (s, e) =>
            {
                _labelEditor.Host = Host;
                _sizeEditor.Host  = Host;
            };
        }
        #endregion

        #region Public Properties
        public Host Host { get; set; }
        #endregion

        #region Properties
        BInput Model => (BInput) DataContext;
        #endregion

        #region Public Methods
        public void Delete()
        {
            Host.EventBus.Publish(EventBus.ComponentDeleted);
        }

        public void OnIsAccountComponentChanged()
        {
            // clear label
            Model.LabelInfo = new LabelInfo();
        }
        #endregion
    }
}