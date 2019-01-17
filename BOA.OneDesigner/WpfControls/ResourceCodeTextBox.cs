using System.Collections;
using System.Collections.Generic;
using System.Linq;
using BOA.Common.Helpers;
using BOA.DatabaseAccess;
using BOA.OneDesigner.AppModel;
using FeserWard.Controls;
using WhiteStone.UI.Container;

namespace BOA.OneDesigner.WpfControls
{
    class ResourceCodeTextBox : IntellisenseTextBox
    {
        #region Constructors
        public ResourceCodeTextBox()
        {
            QueryProvider = new RequestPropertyIntellisenseProvider();
        }
        #endregion

        #region Public Properties
        public bool SearchByCurrentSelectedDataGridDataSourceContract { get; set; }

        public bool ShowOnlyBooleanProperties { get; set; }

        public bool ShowOnlyCollectionProperties   { get; set; }
        public bool ShowOnlyNotNullInt32Properties { get; set; }
        public bool ShowOnlyOrchestrationMethods   { get; set; }

        public bool ShowOnlyStringProperties { get; set; }
        #endregion

        class RequestPropertyIntellisenseProvider : IIntelliboxResultsProvider
        {
            #region Static Fields
            static readonly DevelopmentDatabase Database = new DevelopmentDatabase();
            #endregion

            #region Public Methods
            public IEnumerable DoSearch(string searchTerm, int maxResults, object tag)
            {
                var items = Database.GetRecords<Aut_Resource>("SELECT TOP 5 Name,ResourceCode from AUT.Resource WITH(NOLOCK) WHERE Name LIKE '%" + searchTerm + "%' OR ResourceCode = '" + searchTerm + "'");

                return items.Where(x => IsMatch(x, searchTerm)).Select(t => t).Take(maxResults);
            }
            #endregion

            #region Methods
            protected IEnumerable<string> Sort(IEnumerable<string> preResults)
            {
                return preResults.OrderByDescending(s => s.Length);
            }

            static bool IsMatch(Aut_Resource x, string searchTerm)
            {
                if (x == null)
                {
                    return false;
                }

                if (x.Name == null)
                {
                    return false;
                }

                if (x.ResourceCode == null)
                {
                    return false;
                }

                return x.Name.ToUpperEN().Contains(searchTerm.ToUpperEN()) || x.ResourceCode.ToUpperEN().Contains(searchTerm.ToUpperEN());
            }
            #endregion
        }
    }
}