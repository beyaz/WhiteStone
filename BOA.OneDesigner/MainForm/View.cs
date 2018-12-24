﻿using System.IO;
using System.Windows;
using BOA.OneDesigner.DragAndDrop;
using BOA.OneDesigner.JsxElementModel;
using CustomUIMarkupLanguage.UIBuilding;
using WhiteStone.UI.Container.Mvc;

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

            this.LoadJsonFile(nameof(MainForm) + Path.DirectorySeparatorChar + nameof(View) + ".json");

            DesignSurface.VerticalAlignment = VerticalAlignment.Stretch;

            var card = new BCard
            {
                Title = "Aloha"
            };

            card.InsertField(0, new BInput
            {
                Label       = "User Name",
                BindingPath = "Request.DataContract.UserName"
            });
            card.InsertField(1, new BInput
            {
                Label       = "User Password",
                BindingPath = "Request.DataContract.Password"
            });

            card.InsertField(2, new BInput
            {
                Label       = "System Date",
                BindingPath = "Request.DataContract.SystemDate"
            });

            var cardSection = new BCardSection();
            cardSection.InsertCard(0,card);


            card = new BCard
            {
                Title = "Aloha2"
            };

            card.InsertField(0, new BInput
            {
                Label       = "User Name2",
                BindingPath = "Request.DataContract.UserName2"
            });
            card.InsertField(1, new BInput
            {
                Label       = "User Password2",
                BindingPath = "Request.DataContract.Password2"
            });

            card.InsertField(2, new BInput
            {
                Label       = "System Date2",
                BindingPath = "Request.DataContract.SystemDate2"
            });

            cardSection.InsertCard(0,card);

           

            DesignSurface.DataContext = cardSection;

           
        }
        #endregion
    }
}