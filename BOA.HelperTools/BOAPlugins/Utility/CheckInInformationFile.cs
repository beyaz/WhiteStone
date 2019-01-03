using System;
using System.IO;

namespace BOAPlugins.Utility
{
    public class CheckInInformationFile : FileBase
    {
        #region Public Methods
        public CheckInInformation Load()
        {
            return base.Load<CheckInInformation>();
        }
        #endregion

        #region Methods
        protected override string GetFilePath()
        {
            if (DirectoryPath == null)
            {
                DirectoryPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + Path.DirectorySeparatorChar +
                                "BOAPlugin" + Path.DirectorySeparatorChar;
            }

            const string FileName = "CheckInInformation.json";

            return DirectoryPath + FileName;
        }
        #endregion
    }
}