using System;
using System.Collections.Generic;
using Ninject;

namespace BOA.EntityGeneration.BOACardCustomSqlIntoProjectInjection
{
    [Serializable]
    public class UIModel
    {
        #region Public Properties
        public IReadOnlyList<string> Profiles { get; set; }

        public string SelectedProfile { get; set; }

        public ProjectCustomSqlInfo SelectedProfileCustomSqlList { get; set; }
        #endregion
    }

    public class UIController
    {
        #region Public Properties
        [Inject]
        public DataAccess DataAccess { get; set; }
        #endregion

        #region Public Methods
        public UIModel CreateModel()
        {
            return new UIModel
            {
                Profiles = DataAccess.GetByProfileIdList()
            };
        }

        public void SelectedProfileChange(UIModel model)
        {
            model.SelectedProfileCustomSqlList = DataAccess.GetByProfileId(model.SelectedProfile);
        }
        #endregion
    }
}