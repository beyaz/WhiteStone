using System.Collections;
using System.Collections.Generic;
using System.Linq;
using BOA.Common.Helpers;
using BOA.OneDesigner.AppModel;
using BOA.OneDesigner.Helpers;
using FeserWard.Controls;
using WhiteStone.UI.Container;

namespace BOA.OneDesigner.WpfControls
{



        class RequestIntellisenseTextBox : IntellisenseTextBox
    {
        #region Constructors
        public RequestIntellisenseTextBox()
        {
            QueryProvider = new RequestPropertyIntellisenseProvider(this);
        }
        #endregion

        #region Public Properties
        public bool SearchByCurrentSelectedDataGridDataSourceContract { get; set; }

        public bool ShowOnlyBooleanProperties { get; set; }

        public bool ShowOnlyCollectionProperties { get; set; }
        public bool ShowOnlyOrchestrationMethods { get; set; }

        
        public bool ShowOnlyStringProperties     { get; set; }
        public bool ShowOnlyNotNullInt32Properties { get; set; }
        #endregion

        class RequestPropertyIntellisenseProvider : IIntelliboxResultsProvider
        {
            #region Fields
            readonly RequestIntellisenseTextBox _requestIntellisenseTextBox;
            #endregion

            #region Constructors
            public RequestPropertyIntellisenseProvider(RequestIntellisenseTextBox requestIntellisenseTextBox)
            {
                _requestIntellisenseTextBox = requestIntellisenseTextBox;
            }
            #endregion

            #region Public Properties
            public RequestIntellisenseData Data { get; set; } = SM.Get<Host>().RequestIntellisenseData;
            public Host                    Host { get; set; } = SM.Get<Host>();
            #endregion

            #region Public Methods
            public IEnumerable DoSearch(string searchTerm, int maxResults, object tag)
            {
                if (_requestIntellisenseTextBox.SearchByCurrentSelectedDataGridDataSourceContract)
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

                    return Host.RequestIntellisenseData.Collections[bindingPath].Where(term => term.ToUpperEN().Contains(searchTerm.ToUpperEN())).Select(t => t).Take(maxResults);
                }

                if (_requestIntellisenseTextBox.ShowOnlyOrchestrationMethods)
                {
                    return Data.OrchestrationMethods.Where(term => term.ToUpperEN().Contains(searchTerm.ToUpperEN())).Select(t => t).Take(maxResults);
                }

                if (_requestIntellisenseTextBox.ShowOnlyStringProperties)
                {
                    return Data.RequestStringPropertyIntellisense.Where(term => term.ToUpperEN().Contains(searchTerm.ToUpperEN())).Select(t => t).Take(maxResults);
                }

                if (_requestIntellisenseTextBox.ShowOnlyNotNullInt32Properties)
                {
                    return Data.RequestNotNullInt32PropertyIntellisense.Where(term => term.ToUpperEN().Contains(searchTerm.ToUpperEN())).Select(t => t).Take(maxResults);
                }

                if (_requestIntellisenseTextBox.ShowOnlyBooleanProperties)
                {
                    return Data.RequestBooleanPropertyIntellisense.Where(term => term.ToUpperEN().Contains(searchTerm.ToUpperEN())).Select(t => t).Take(maxResults);
                }

                if (_requestIntellisenseTextBox.ShowOnlyCollectionProperties)
                {
                    return Data.RequestCollectionPropertyIntellisense.Where(term => term.ToUpperEN().Contains(searchTerm.ToUpperEN())).Select(t => t).Take(maxResults);
                }

                return Data.RequestPropertyIntellisense.Where(term => term.ToUpperEN().Contains(searchTerm.ToUpperEN())).Select(t => t).Take(maxResults);
            }
            #endregion

            #region Methods
            protected IEnumerable<string> Sort(IEnumerable<string> preResults)
            {
                return preResults.OrderByDescending(s => s.Length);
            }
            #endregion
        }
    }
}