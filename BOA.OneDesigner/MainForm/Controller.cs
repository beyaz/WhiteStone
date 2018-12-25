using System.Collections.Generic;
using System.IO;
using System.Linq;
using BOA.CodeGeneration.Util;
using BOA.Common.Helpers;
using WhiteStone;
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
            var workingOnScreenListItems = SM.Get<InitialConfig>().SolutionFileNames.Select(x => new WorkingOnScreenListItem
            {
                Name = x
            }).ToList();

            Model = new Model
            {
                SolutionFileNames = SM.Get<InitialConfig>().SolutionFileNames,
                WorkingOnScreenList = workingOnScreenListItems,
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