using System;
using System.Collections.Generic;
using BOAPlugins.FormApplicationGenerator.Types;
using BOAPlugins.Utility;
using BOAPlugins.Utility.TypescriptModelGeneration;

namespace BOAPlugins.FormApplicationGenerator.UI
{
    [Serializable]
    public class Model
    {
        #region Constructors
        public Model(string solutionFilePath, string tableNameIndDatabase)
        {
            SolutionInfo = SolutionInfo.CreateFrom(solutionFilePath);

            NamingInfo = NamingInfo.Create(solutionFilePath, tableNameIndDatabase);

            TableNameInDatabase = tableNameIndDatabase;
        }
        #endregion

        #region Public Properties
        public IReadOnlyCollection<BField> FormDataClassFields
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

        public IReadOnlyCollection<BField> ListFormSearchFields { get; set; } = new List<BField>();

        public NamingInfo   NamingInfo   { get; set; }
        public SolutionInfo SolutionInfo { get; set; }

        public string TableNameInDatabase     { get; }
        public string TsxFilePathOfDetailForm => SolutionInfo.OneProjectFolder + @"ClientApp\pages\" + NamingInfo.TsxFileNameForDetailForm;

        public string TsxFilePathOfListForm => SolutionInfo.OneProjectFolder + @"ClientApp\pages\" + NamingInfo.TsxFileNameForListForm;

        public bool UserRawStringForMessaging { get; set; }
        #endregion
    }
}