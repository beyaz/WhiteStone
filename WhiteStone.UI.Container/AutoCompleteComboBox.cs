using System;
using System.Windows;
using BOA.Common.Helpers;
using CustomUIMarkupLanguage.Markup;
using CustomUIMarkupLanguage.UIBuilding;

namespace WhiteStone.UI.Container
{
    class AutoCompleteComboBox : DotNetKit.Windows.Controls.AutoCompleteComboBox
    {
        static void A()
        {


            ResourceDictionary myResourceDictionary = new ResourceDictionary {Source = new Uri("pack://application:,,,/DotNetKit.Wpf.AutoCompleteComboBox;component/windows/controls/autocompletecombobox.xaml",UriKind.RelativeOrAbsolute)};

            Application.Current.Resources.MergedDictionaries.Add(myResourceDictionary);
        }
        #region Public Methods
        public static UIElement On(Builder builder, Node node)
        {
            if (node.UI == nameof(AutoCompleteComboBox).ToUpperEN())
            {
                A();

                return new AutoCompleteComboBox();
            }

            return null;
        }
        #endregion
    }
}