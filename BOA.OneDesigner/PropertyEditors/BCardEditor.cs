﻿using System.Windows.Controls;
using BOA.Common.Helpers;
using BOA.OneDesigner.AppModel;
using BOA.OneDesigner.JsxElementModel;
using CustomUIMarkupLanguage.UIBuilding;

namespace BOA.OneDesigner.PropertyEditors
{
    class BCardEditor : StackPanel, IHostItem
    {
        #region Fields
        public LabelEditor _labelEditor;
        #endregion

        #region Constructors
        public BCardEditor()
        {
            this.LoadJson(@"

{ 
    Margin : 10,
    Spacing: 10,
	Childs : [

		{ui:'LabelEditor',Name:'_labelEditor', Header:'Title', MarginTop:10, DataContext:'{Binding " + nameof(Model.TitleInfo) + @"}'},
        
        
        {ui:'WideEditor',               MarginTop:10, Value:'{Binding " + Model.AccessPathOf(m => m.LayoutProps.Wide) + @"}' },
        {ui:'HorizontalLocationEditor', MarginTop:10, Value:'{Binding " + Model.AccessPathOf(m => m.LayoutProps.X)    + @"}' },

        {ui:'RequestIntellisenseTextBox', ShowOnlyBooleanProperties:true, Margin:5, Text:'{Binding " + nameof(Model.IsVisibleBindingPath) + @"}', Label:'Is Visible Binding Path' },
        {ui:'RequestIntellisenseTextBox', ShowOnlyBooleanProperties:true, Margin:5, Text:'{Binding " + nameof(Model.IsDisabledBindingPath) + @"}', Label:'Is Disabled Binding Path' },
        {ui:'RequestIntellisenseTextBox', ShowOnlyBooleanProperties:true, Margin:5, Text:'{Binding " + nameof(Model.IsExpandableBindingPath) + @"}', Label:'Is Expandable Binding Path' },
        {ui:'RequestIntellisenseTextBox', ShowOnlyBooleanProperties:true, Margin:5, Text:'{Binding " + nameof(Model.IsExpandedBindingPath) + @"}', Label:'Is Expanded Binding Path' },
        

        {ui:'Button', Text:'Delete',Click:'" + nameof(Delete) + @"'}
	]
}

");

            Loaded += (s, e) => { _labelEditor.Host = Host; };
        }
        #endregion

        #region Public Properties
        public Host  Host  { get; set; }
        public BCard Model => (BCard) DataContext;
        #endregion

        #region Public Methods
        public void Delete()
        {
            Host.EventBus.Publish(EventBus.ComponentDeleted);
        }

       
        #endregion
    }
}