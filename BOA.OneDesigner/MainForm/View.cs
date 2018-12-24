using System.IO;
using System.Windows;
using System.Windows.Controls;
using BOA.OneDesigner.JsxElementModel;
using CustomUIMarkupLanguage.UIBuilding;
using WhiteStone.UI.Container.Mvc;

namespace BOA.OneDesigner.MainForm
{
    public class View : WindowBase<Model, Controller>
    {
        #region Fields
        public StackPanel DesignSurface;
        #endregion

        #region Constructors
        public View()
        {
            this.LoadJsonFile(nameof(MainForm) + Path.DirectorySeparatorChar + nameof(View) + ".json");

            DesignSurface.VerticalAlignment = VerticalAlignment.Stretch;

            var bCard = new BCard
            {
                Title = "Aloha"
            };

            bCard.InsertField(0, new BInput
            {
                Label       = "User Name",
                BindingPath = "Request.DataContract.UserName"
            });
            bCard.InsertField(1, new BInput
            {
                Label       = "User Password",
                BindingPath = "Request.DataContract.Password"
            });

            bCard.InsertField(2, new BInput
            {
                Label       = "System Date",
                BindingPath = "Request.DataContract.SystemDate"
            });

            DesignSurface.Children.Add(new WpfControls.BCard
            {
                DataContext = bCard
            });
        }
        #endregion
    }
}