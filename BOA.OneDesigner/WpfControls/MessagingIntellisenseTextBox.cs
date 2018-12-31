using System.Collections;
using System.Collections.Generic;
using System.Linq;
using BOA.Common.Helpers;
using FeserWard.Controls;
using WhiteStone.UI.Container;

namespace BOA.OneDesigner.WpfControls
{
    class MessagingIntellisenseTextBox : IntellisenseTextBox
    {
        static IReadOnlyList<string> MessagingPropertyNames => UIContext.MessagingPropertyNames.Select(x => x.PropertyName).ToList();

        #region Constructors
        public MessagingIntellisenseTextBox()
        {
            QueryProvider = new RequestPropertyIntellisenseProvider();
        }
        #endregion

        class RequestPropertyIntellisenseProvider : IIntelliboxResultsProvider
        {
            #region Public Methods
            public IEnumerable DoSearch(string searchTerm, int maxResults, object tag)
            {
                return MessagingPropertyNames.Where(term => term.ToUpperEN().Contains(searchTerm.ToUpperEN())).Select(t => t).Take(maxResults);
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