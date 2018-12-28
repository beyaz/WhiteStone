using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace BOA.OneDesigner.WpfControls
{
    static class Extensions
    {
        public static void RefreshDataContext(this FrameworkElement element )
        {

            var dataContext = element.DataContext;
            element.DataContext = null;
            element.DataContext = dataContext;
        }

        #region Public Methods
        public static T FindParent<T>(this DependencyObject child) where T : DependencyObject
        {
            //get parent item
            var parentObject = VisualTreeHelper.GetParent(child);

            //we've reached the end of the tree
            if (parentObject == null)
            {
                return null;
            }

            //check if the parent matches the type we're looking for
            var parent = parentObject as T;
            if (parent != null)
            {
                return parent;
            }

            return FindParent<T>(parentObject);
        }

        public static UIElement[] ToArray(this UIElementCollection uiElementCollection)
        {
            var items = new UIElement[uiElementCollection.Count];

            uiElementCollection.CopyTo(items, 0);

            return items;
        }
        #endregion
    }
}