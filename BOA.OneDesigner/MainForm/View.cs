using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using BOA.OneDesigner.DragAndDrop;
using BOA.OneDesigner.JsxElementModel;
using BOA.OneDesigner.JsxElementRender;
using CustomUIMarkupLanguage.UIBuilding;
using WhiteStone.UI.Container;
using WhiteStone.UI.Container.Mvc;

namespace BOA.OneDesigner.MainForm
{
    public class View:WindowBase<Model,Controller>
    {
        public StackPanel DesignSurface;

        public View()
        {

            

            this.LoadJsonFile(nameof(MainForm) + Path.DirectorySeparatorChar + nameof(View) + ".json");


            DesignSurface.VerticalAlignment = VerticalAlignment.Stretch;

            var bCard = new BCard
            {
                Title = "Aloha",
                Fields = new List<BField>
                {
                    new BInput
                    {
                        Label = "User Name",
                        BindingPath = "Request.DataContract.UserName"
                    },

                    new BInput
                    {
                        Label       = "User Password",
                        BindingPath = "Request.DataContract.Password"
                    }
                }
            };
            DesignSurface.Children.Add(Visualizer.Visualize(bCard));
        }

       
    }
}
