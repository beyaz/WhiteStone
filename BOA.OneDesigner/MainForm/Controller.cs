using System.Collections.Generic;
using WhiteStone.UI.Container.Mvc;

namespace BOA.OneDesigner.MainForm
{
    public class Controller : ControllerBase<Model>
    {

        public void WorkingOnScreenListSelectionChanged()
        {

        }
        public void CreateNewForm()
        {
            Model.StartTabIsVisible = false;
            Model.ActionButtons = new List<ActionButtonInfo>
            {
                new ActionButtonInfo
                {
                    ActionName = "Next",
                    Text       = "İleri"
                },
                new ActionButtonInfo
                {
                    ActionName = "Back",
                    Text       = "Vazgeç"
                }
            };
        }
        public override void OnViewLoaded()
        {
            Model = new Model
            {
                WorkingOnScreenList = new List<WorkingOnScreenListItem>
                {
                    new WorkingOnScreenListItem
                    {
                        Name = "Test"
                    }
                },
                StartTabIsVisible = true,
                ActionButtons = new List<ActionButtonInfo>
                {
                    new ActionButtonInfo
                    {
                        ActionName = nameof(CreateNewForm),
                        Text       = "Yeni Ekran Tasarla"
                    }
                }
            };
        }
    }
}