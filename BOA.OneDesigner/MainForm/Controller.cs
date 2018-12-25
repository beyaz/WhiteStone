using System.Collections.Generic;
using BOA.Common.Helpers;
using WhiteStone.UI.Container.Mvc;

namespace BOA.OneDesigner.MainForm
{
    public class Controller : ControllerBase<Model>
    {
        #region Public Methods
        public void Next()
        {
            if (string.IsNullOrWhiteSpace(Model.SelectedTfsFolderName))
            {
                Model.ViewMessage = "Tfs Project boş olamaz.";
                Model.ViewMessageTypeIsError = true;
               return;
            }

            Model.SelectedTfsFolderNameIsIsEnabled = false;

            Model.SelectedRequestNameIsIsEnabled = true;


            
        }

        public override void OnViewLoaded()
        {
            Model = new Model
            {
                SelectedTfsFolderNameIsIsEnabled = true,
                TfsFolderNames    = SM.Get<InitialConfig>().TfsFolderNames,
                StartTabIsVisible = true,
                ActionButtons = new List<ActionButtonInfo>
                {
                    new ActionButtonInfo
                    {
                        ActionName = nameof(Next),
                        Text       = "İleri"
                    }
                }
            };
        }
        #endregion
    }
}