﻿using System;
using System.Windows;
using System.Windows.Controls;
using BOA.Common.Helpers;
using BOA.OneDesigner.JsxElementModel;
using CustomUIMarkupLanguage.UIBuilding;

namespace BOA.OneDesigner.WpfControls
{
    [Serializable]
    public class BDataGridEditorModel
    {
        #region Public Properties
        public BDataGrid           Info                        { get; set; }
        public bool                RemoveColumnButtonIsVisible { get; set; }
        public BDataGridColumnInfo SelectedColumn              { get; set; }
        #endregion
    }

    class BDataGridEditor : StackPanel
    {
        public ListBox _listBox;
        public Button _removeButton;

        #region Constructors
        public BDataGridEditor()
        {
            this.LoadJson(@"
{ 
  MinHeight:300,
   Margin:5,
	Childs:[  
		{ui:'RequestIntellisenseTextBox', Text:'{Binding DataSourceBindingPath}', Label:'Data Source Binding' },
        {ui:'Button', Text:'Add Column',Click:'AddColumn'},
        {ui:'Button', Name:'_removeButton', Text:'Remove Column',IsVisible:'{Binding RemoveColumnButtonIsVisible,Mode:OneWay}'},
        {ui:'ListBox', Name:'_listBox',
             ItemsSource:'{Binding Info.Columns,Mode:OneWay}', 
             SelectionChanged:'SelectedColumnChanged', 
             DisplayMemberPath:'LabelText',
             SelectedValue:'{Binding SelectedColumn}'

        }
	]
}

");

        }
        #endregion

        #region Public Properties
        public BDataGridEditorModel Model { get; set; }
        #endregion

        #region Public Methods
        public void AddColumn()
        {
            Model.Info.Columns.Add(new BDataGridColumnInfo
            {
                Label = new LabelInfo
                {
                    IsFreeText    = true,
                    FreeTextValue = "??"
                }
            });

            _listBox.GetBindingExpression(ItemsControl.ItemsSourceProperty)?.UpdateSource();

            EventBus.Publish(EventBus.OnComponentPropertyChanged);
        }
        #endregion

        #region Methods
        void SelectedColumnChanged()
        {
            if (Model.SelectedColumn == null)
            {
                Model.RemoveColumnButtonIsVisible = false;
            }
            else
            {
                Model.RemoveColumnButtonIsVisible = true;
            }

            this.RefreshDataContext();

            _listBox.GetBindingExpression(ItemsControl.ItemsSourceProperty)?.UpdateTarget();
            _removeButton.GetBindingExpression(UIElement.VisibilityProperty)?.UpdateTarget();
        }
        #endregion
    }
}