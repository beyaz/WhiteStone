using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace BOA.OneDesigner.WpfControls
{
    static class Extensions
    {
        #region Public Methods
        /// <summary>
        ///     Analyzes both visual and logical tree in order to find all elements
        ///     of a given type that are descendants of the <paramref name="source" />
        ///     item.
        /// </summary>
        /// <typeparam name="T">The type of the queried items.</typeparam>
        /// <param name="source">
        ///     The root element that marks the source of the
        ///     search. If the source is already of the requested type, it will not
        ///     be included in the result.
        /// </param>
        /// <returns>
        ///     All descendants of <paramref name="source" /> that match the
        ///     requested type.
        /// </returns>
        public static IEnumerable<T> FindChildren<T>(this DependencyObject source)
            where T : DependencyObject
        {
            if (source != null)
            {
                var children = GetChildObjects(source);
                foreach (var child in children)
                {
                    //analyze if children match the requested type
                    if (child != null && child is T)
                    {
                        yield return (T) child;
                    }

                    //recurse tree
                    foreach (var descendant in FindChildren<T>(child))
                    {
                        yield return descendant;
                    }
                }
            }
        }

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

        /// <summary>
        ///     This method is an alternative to WPF's
        ///     <see cref="VisualTreeHelper.GetChild" /> method, which also
        ///     supports content elements. Do note, that for content elements,
        ///     this method falls back to the logical tree of the element.
        /// </summary>
        /// <param name="parent">The item to be processed.</param>
        /// <returns>The submitted item's child elements, if available.</returns>
        public static IEnumerable<DependencyObject> GetChildObjects(
            this DependencyObject parent)
        {
            if (parent == null)
            {
                yield break;
            }

            if (parent is ContentElement || parent is FrameworkElement)
            {
                //use the logical tree for content / framework elements
                foreach (var obj in LogicalTreeHelper.GetChildren(parent))
                {
                    var depObj = obj as DependencyObject;
                    if (depObj != null)
                    {
                        yield return (DependencyObject) obj;
                    }
                }
            }
            else
            {
                //use the visual tree per default
                var count = VisualTreeHelper.GetChildrenCount(parent);
                for (var i = 0; i < count; i++)
                {
                    yield return VisualTreeHelper.GetChild(parent, i);
                }
            }
        }

        public static void RaiseLoadedEvent(this FrameworkElement element)
        {
            element.RaiseEvent(new RoutedEventArgs(FrameworkElement.LoadedEvent));
        }

        public static void RaisePreviewMouseLeftButtonDownEvent(this UIElement element)
        {
            element.RaiseEvent(new MouseButtonEventArgs(Mouse.PrimaryDevice, Environment.TickCount, MouseButton.Left)
            {
                RoutedEvent = UIElement.PreviewMouseLeftButtonDownEvent,
                Source      = element
            });
        }

        public static void RefreshDataContext(this FrameworkElement element)
        {
            element.GetBindingExpression(FrameworkElement.DataContextProperty)?.UpdateTarget();
        }

        public static void RemoveAll(this UIElementCollection elementCollection)
        {
            elementCollection.Clear();
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