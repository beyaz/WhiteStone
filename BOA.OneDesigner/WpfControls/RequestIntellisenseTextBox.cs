using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using BOA.Common.Helpers;
using FeserWard.Controls;
using WhiteStone.UI.Container;

namespace BOA.OneDesigner.WpfControls
{
    class RequestIntellisenseTextBox : IntellisenseTextBox
    {
        public bool ShowOnlyStringProperties { get; set; }

        #region Constructors
        public RequestIntellisenseTextBox()
        {
            QueryProvider = new RequestPropertyIntellisenseProvider(this);
        }
        #endregion

        

        class RequestPropertyIntellisenseProvider : IIntelliboxResultsProvider
        {
            #region Fields
            readonly RequestIntellisenseTextBox m_requestIntellisenseTextBox;
            #endregion

            #region Constructors
            public RequestPropertyIntellisenseProvider(RequestIntellisenseTextBox requestIntellisenseTextBox)
            {
                m_requestIntellisenseTextBox = requestIntellisenseTextBox;
            }
            #endregion

            #region Public Methods
            public IEnumerable DoSearch(string searchTerm, int maxResults, object tag)
            {
                if (m_requestIntellisenseTextBox.ShowOnlyStringProperties)
                {
                    return UIContext.RequestStringPropertyIntellisense.Where(term => term.ToUpperEN().Contains(searchTerm.ToUpperEN())).Select(t => t).Take(maxResults);
                }

                return UIContext.RequestPropertyIntellisense.Where(term => term.ToUpperEN().Contains(searchTerm.ToUpperEN())).Select(t => t).Take(maxResults);
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