using System.Windows;
using System.Windows.Controls;

namespace BOA.OneDesigner.WpfControls
{
    static class Extensions
    {
        public static UIElement[] ToArray(this UIElementCollection uiElementCollection)
        {
            var items = new UIElement[uiElementCollection.Count];

            uiElementCollection.CopyTo(items,0);

            return items;
        }
    }
}