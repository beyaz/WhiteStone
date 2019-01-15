using System.Collections;
using System.Collections.Generic;
using System.Linq;
using BOA.Common.Helpers;
using BOA.OneDesigner.AppModel;
using BOA.OneDesigner.Helpers;
using BOA.UnitTestHelper;
using FeserWard.Controls;
using WhiteStone.UI.Container;

namespace BOA.OneDesigner.WpfControls
{
    class ResourceCodeTextBox : IntellisenseTextBox
    {
        #region Constructors
        public ResourceCodeTextBox()
        {
            QueryProvider = new RequestPropertyIntellisenseProvider(this);
        }
        #endregion

        #region Public Properties
        public bool SearchByCurrentSelectedDataGridDataSourceContract { get; set; }

        public bool ShowOnlyBooleanProperties { get; set; }

        public bool ShowOnlyCollectionProperties { get; set; }
        public bool ShowOnlyOrchestrationMethods { get; set; }

        
        public bool ShowOnlyStringProperties       { get; set; }
        public bool ShowOnlyNotNullInt32Properties { get; set; }
        #endregion

        class RequestPropertyIntellisenseProvider : IIntelliboxResultsProvider
        {
            static BOATestContextDev Dev => SM.Get<BOATestContextDev>();


            #region Fields
            readonly ResourceCodeTextBox _requestIntellisenseTextBox;
            #endregion

            #region Constructors
            public RequestPropertyIntellisenseProvider(ResourceCodeTextBox requestIntellisenseTextBox)
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

                if (Dev == null)
                {
                    return new List<string> {"Wait..."};
                }

               var items = Dev.GetRecords<Aut_Resource>("SELECT TOP 5 Name,ResourceCode from AUT.Resource WITH(NOLOCK) WHERE Name LIKE '%"+searchTerm+"%'");

                return items.Where(x => x.Name.ToUpperEN().Contains(searchTerm.ToUpperEN()) ||
                                        x.ResourceCode.ToUpperEN().Contains(searchTerm.ToUpperEN())).Select(t => t).Take(maxResults);
                
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