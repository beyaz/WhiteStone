using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;
using BOA.Common.Helpers;
using CustomUIMarkupLanguage.UIBuilding;
using FeserWard.Controls;
using WhiteStone.UI.Container;

namespace BOA.OneDesigner.WpfControls
{
    class RequestIntellisenseTextBox : IntellisenseTextBox
    {
        #region Constructors
        public RequestIntellisenseTextBox()
        {
            QueryProvider = new RequestPropertyIntellisenseProvider();
        }
        #endregion

        class RequestPropertyIntellisenseProvider : IIntelliboxResultsProvider
        {
            #region Public Methods
            public IEnumerable DoSearch(string searchTerm, int maxResults, object tag)
            {
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


    [Serializable]
    public class LabelInfo
    {
        public bool IsFreeText { get; set; }
        public bool IsFromMessaging { get; set; }
        public string FreeTextValue { get; set; }
        public string MessagingValue { get; set; }
        public string DesignerText { get; set; }
    }

    class LabelEditor2 : Grid
    {


        public LabelEditor2()
        {
            this.LoadJson(@"

{
	rows:[
		{ui:'RadioButton', label:'From Messaging', IsChecked:'{Binding IsFromMessaging}'},
		{ui:'RadioButton', label:'Free Text', IsChecked:'{Binding IsFreeText}'},
		{ui:'Textbox',Text:'Aloha'}

	]
}


");
        }
    }
}