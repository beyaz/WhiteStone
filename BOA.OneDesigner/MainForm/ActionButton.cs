using System.Windows;
using System.Windows.Controls;
using BOA.OneDesigner.AppModel;
using BOA.OneDesigner.JsxElementModel;

namespace BOA.OneDesigner.MainForm
{
    class ActionButton : Button
    {
        #region Constructors
        public ActionButton(Aut_ResourceAction action)
        {
            Model = action;

            Content = action.Name;

            Padding = new Thickness(5);

            Margin = new Thickness(5);
        }
        #endregion

        #region Public Properties
        public Aut_ResourceAction Model { get; }
        #endregion
    }
}