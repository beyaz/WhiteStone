using System;
using System.Collections.Generic;
using BOAPlugins.FormApplicationGenerator.Types;
using BOAPlugins.Utility;
using BOAPlugins.Utility.TypescriptModelGeneration;

namespace BOAPlugins.FormApplicationGenerator.UI
{
    [Serializable]
    public class MainWindowModel
    {
       

        #region Public Properties
        public IReadOnlyCollection<BField_Kaldırılacak> FormDataClassFields
        {
            get
            {
                throw new NotImplementedException();
                //if (IsTabForm)
                //{
                //    return Tabs.GetAllFields();
                //}

                //return Cards.GetAllFields();
            }
        }

        public bool IsTabForm { get; set; }

        public IReadOnlyCollection<BField_Kaldırılacak> ListFormSearchFields { get; set; } = new List<BField_Kaldırılacak>();

        public NamingInfo   NamingInfo   { get; set; }
        public SolutionInfo SolutionInfo { get; set; }

        public string TableNameInDatabase     { get; }
        public string TsxFilePathOfDetailForm => SolutionInfo.OneProjectFolder + @"ClientApp\pages\" + NamingInfo.TsxFileNameForDetailForm;

        public string TsxFilePathOfListForm => SolutionInfo.OneProjectFolder + @"ClientApp\pages\" + NamingInfo.TsxFileNameForListForm;

        public bool UserRawStringForMessaging { get; set; }
        #endregion
    }
}