using WhiteStone.UI.Container;

namespace BOA.OneDesigner.WpfControls
{
    class RequestIntellisenseTextBox :IntellisenseTextBox
    {
        public RequestIntellisenseTextBox()
        {
            QueryProvider = new RequestPropertyIntellisenseProvider();
        }

        
        
    }
}