using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace BOA.OneDesigner.WpfControls
{
    static class Extensions
    {
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

        public static void RaiseLoadedEvent(this FrameworkElement element)
        {
            element.RaiseEvent(new RoutedEventArgs(FrameworkElement.LoadedEvent));
        }

        public static void RefreshDataContext(this FrameworkElement element)
        {
            element.GetBindingExpression(FrameworkElement.DataContextProperty)?.UpdateTarget();
        }

        public static void RemoveAll(this UIElementCollection elementCollection)
        {
            elementCollection.UnSubscribeFromEventBus();

            elementCollection.Clear();
        }

        public static UIElement[] ToArray(this UIElementCollection uiElementCollection)
        {
            var items = new UIElement[uiElementCollection.Count];

            uiElementCollection.CopyTo(items, 0);

            return items;
        }

        public static void UnSubscribeFromEventBus(this UIElementCollection elementCollection)
        {
            foreach (var element in elementCollection)
            {
                (element as IEventBusDisposable)?.DeAttachToEventBus();
            }
        }
        #endregion
    }
}