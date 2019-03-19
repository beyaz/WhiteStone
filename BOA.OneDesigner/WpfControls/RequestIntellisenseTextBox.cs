using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;
using BOA.Common.Helpers;
using BOA.OneDesigner.AppModel;
using BOA.OneDesigner.Helpers;
using DotNetKit.Windows.Controls;
using WhiteStone.UI.Container;

namespace BOA.OneDesigner.WpfControls
{
    

    public class RequestIntellisenseTextBox : LabeledComboBox
    {

        #region Constructors
        public RequestIntellisenseTextBox()
        {
            ItemsSource = new List<string>();

            // TextChanged += RequestIntellisenseTextBox_TextChanged;

            Loaded += (s, e) => { ItemsSource = RefreshItemsSource("").ToList(); };
            
        }
        #endregion

        #region Public Properties
        public RequestIntellisenseData Data { get; set; } = SM.Get<Host>().RequestIntellisenseData;

        public Host Host { get; set; } = SM.Get<Host>();

        public bool SearchByCurrentSelectedDataGridDataSourceContract { get; set; }

        public bool ShowOnlyBooleanProperties { get; set; }

        public bool ShowOnlyClassProperties { get; set; }

        public bool ShowOnlyCollectionProperties { get; set; }

        public bool ShowOnlyDotnetCoreTypes { get; set; }

        public bool ShowOnlyNotNullInt32Properties { get; set; }

        public bool ShowOnlyOrchestrationMethods { get; set; }

        public bool ShowOnlyStringProperties { get; set; }
        #endregion

        #region Methods
        IEnumerable<string> RefreshItemsSource(string query)
        {
            if (SearchByCurrentSelectedDataGridDataSourceContract)
            {
                var bindingPath = Host.LastSelectedUIElement_as_DataGrid_DataSourceBindingPath;
                if (string.IsNullOrWhiteSpace(bindingPath))
                {
                    return new[] {"Önce gridin data source bilgisi  girilmelidir."};
                }

                if (Host.RequestIntellisenseData.Collections.ContainsKey(bindingPath) == false)
                {
                    return new[] {"ilgili data source path bulunamadı."};
                }

                return Host.RequestIntellisenseData.Collections[bindingPath].Where(term => term.ToUpperEN().Contains(query.ToUpperEN())).Select(t => t);
            }

            if (Data == null)
            {
                return new[] {@"İlgili request sizin d:\boa\server\bin\ dizininde bulunamadı."};
            }

            if (ShowOnlyOrchestrationMethods)
            {
                return Data.OrchestrationMethods.Where(term => term.ToUpperEN().Contains(query.ToUpperEN())).Select(t => t);
            }

            if (ShowOnlyClassProperties)
            {
                return Data.RequestClassPropertyIntellisense.Where(term => term.ToUpperEN().Contains(query.ToUpperEN())).Select(t => t);
            }

            if (ShowOnlyStringProperties)
            {
                return Data.RequestStringPropertyIntellisense.Where(term => term.ToUpperEN().Contains(query.ToUpperEN())).Select(t => t);
            }

            if (ShowOnlyNotNullInt32Properties)
            {
                return Data.RequestNotNullInt32PropertyIntellisense.Where(term => term.ToUpperEN().Contains(query.ToUpperEN())).Select(t => t);
            }

            if (ShowOnlyBooleanProperties)
            {
                return Data.RequestBooleanPropertyIntellisense.Where(term => term.ToUpperEN().Contains(query.ToUpperEN())).Select(t => t);
            }

            if (ShowOnlyCollectionProperties)
            {
                return Data.RequestCollectionPropertyIntellisense.Where(term => term.ToUpperEN().Contains(query.ToUpperEN())).Select(t => t);
            }

            if (ShowOnlyDotnetCoreTypes)
            {
                return Data.RequestJsSupportTypesPropertyIntellisense.Where(term => term.ToUpperEN().Contains(query.ToUpperEN())).Select(t => t);
            }

            


            return Data.RequestPropertyIntellisense.Where(term => term.ToUpperEN().Contains(query.ToUpperEN())).Select(t => t);
        }

        void RequestIntellisenseTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            ItemsSource =  RefreshItemsSource(Text).Take(10).ToList();
        }
        #endregion
    }
}