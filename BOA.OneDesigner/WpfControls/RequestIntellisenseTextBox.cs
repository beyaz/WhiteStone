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
        public bool SearchByCurrentSelectedDataGridDataSourceContract { get; set; }

        #region Constructors
        public RequestIntellisenseTextBox()
        {
            QueryProvider = new RequestPropertyIntellisenseProvider(this);
        }
        #endregion

        #region Public Properties
        public bool ShowOnlyStringProperties { get; set; }

        public bool ShowOnlyCollectionProperties { get; set; }
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
            #endregion

            #region Public Methods
            public IEnumerable DoSearch(string searchTerm, int maxResults, object tag)
            {

                if (_requestIntellisenseTextBox.SearchByCurrentSelectedDataGridDataSourceContract)
                {
                    return new[] { "Önce gridin data source bilgisi  girilmelidir."};

                    //var column = ((BDataGridColumnWpf) Host.LastSelectedUIElement);
                    
                    //var bindingPath = column.BDataGridInfoWpf?.BData?.DataSourceBindingPath;
                    //if (string.IsNullOrWhiteSpace(bindingPath) )
                    //{
                    //    return new[] { "Önce gridin data source bilgisi  girilmelidir."};
                    //}

                    //return CecilHelper.GetAllBindProperties(Host.TypeAssemblyPathInServerBin, Host.RequestName, bindingPath);


                }

                

                if (_requestIntellisenseTextBox.ShowOnlyStringProperties)
                {
                    return Data.RequestStringPropertyIntellisense.Where(term => term.ToUpperEN().Contains(searchTerm.ToUpperEN())).Select(t => t).Take(maxResults);
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