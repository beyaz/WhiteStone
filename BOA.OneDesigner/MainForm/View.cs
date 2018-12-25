using System.IO;
using System.Windows;
using BOA.OneDesigner.WpfControls;
using CustomUIMarkupLanguage.UIBuilding;
using WhiteStone.UI.Container.Mvc;
using BCard = BOA.OneDesigner.JsxElementModel.BCard;
using BCardSection = BOA.OneDesigner.JsxElementModel.BCardSection;
using BInput = BOA.OneDesigner.JsxElementModel.BInput;

namespace BOA.OneDesigner.MainForm
{
    public class View : WindowBase<Model, Controller>
    {
        #region Fields
        public JsxElementDesignerSurface DesignSurface;
        #endregion

        #region Constructors
        public View()
        {
            Builder.RegisterElementCreation("Surface",typeof(JsxElementDesignerSurface));
            Builder.RegisterElementCreation("ToolBox",typeof(ToolBox));
            Builder.RegisterElementCreation("PropertyEditorContainer",typeof(PropertyEditorContainer));

            this.LoadJsonFile(nameof(MainForm) + Path.DirectorySeparatorChar + nameof(View) + ".json");

            DesignSurface.VerticalAlignment = VerticalAlignment.Stretch;

            var card = new BCard
            {
                Title = "Aloha"
            };

            card.InsertItem(0, new BInput
            {
                Label       = "User Name",
                BindingPath = "Request.DataContract.UserName"
            });
            card.InsertItem(1, new BInput
            {
                Label       = "User Password",
                BindingPath = "Request.DataContract.Password"
            });

            card.InsertItem(2, new BInput
            {
                Label       = "System Date",
                BindingPath = "Request.DataContract.SystemDate"
            });

            var cardSection = new BCardSection();
            cardSection.InsertItem(0,card);


            card = new BCard
            {
                Title = "Aloha2"
            };

            card.InsertItem(0, new BInput
            {
                Label       = "User Name2",
                BindingPath = "Request.DataContract.UserName2"
            });
            card.InsertItem(1, new BInput
            {
                Label       = "User Password2",
                BindingPath = "Request.DataContract.Password2"
            });

            card.InsertItem(2, new BInput
            {
                Label       = "System Date2",
                BindingPath = "Request.DataContract.SystemDate2"
            });

            cardSection.InsertItem(0,card);

           

            DesignSurface.DataContext = cardSection;

           
        }
        #endregion
    }
}