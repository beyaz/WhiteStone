﻿using BOA.EntityGeneration.BOACardCustomSqlIntoProjectInjection.AllInOne;
using BOA.EntityGeneration.BOACardCustomSqlIntoProjectInjection.DataAccess;
using BOA.EntityGeneration.BOACardCustomSqlIntoProjectInjection.Models;
using BOA.EntityGeneration.BOACardDatabaseSchemaToDllExporting.Util;
using BOA.TfsAccess;
using Ninject;

namespace BOA.EntityGeneration.BOACardCustomSqlIntoProjectInjection.Injectors
{
    /// <summary>
    ///     The project injector
    /// </summary>
    public class ProjectInjector
    {
        #region Public Properties
        /// <summary>
        ///     Gets or sets all in one for business DLL.
        /// </summary>
        [Inject]
        public AllInOneForBusinessDll AllInOneForBusinessDll { get; set; }

        /// <summary>
        ///     Gets or sets all in one for type DLL.
        /// </summary>
        [Inject]
        public AllInOneForTypeDll AllInOneForTypeDll { get; set; }

        /// <summary>
        ///     Gets or sets the file access.
        /// </summary>
        [Inject]
        public FileAccess FileAccess { get; set; }

        /// <summary>
        ///     Gets or sets the project custom SQL information data access.
        /// </summary>
        [Inject]
        public ProjectCustomSqlInfoDataAccess ProjectCustomSqlInfoDataAccess { get; set; }

        /// <summary>
        ///     Gets or sets the tracer.
        /// </summary>
        [Inject]
        public Tracer Tracer { get; set; }
        #endregion

        #region Public Methods
        /// <summary>
        ///     Injects the specified profile identifier.
        /// </summary>
        public void Inject(string profileId)
        {
            Inject(ProjectCustomSqlInfoDataAccess.GetByProfileId(profileId));
        }
        #endregion

        #region Methods
        /// <summary>
        ///     Injects the specified data.
        /// </summary>
        void Inject(ProjectCustomSqlInfo data)
        {
            for (var i = 0; i < data.CustomSqlInfoList.Count; i++)
            {
                data.CustomSqlInfoList[i].SwitchCaseIndex = i;
            }

            Tracer.CustomSqlGenerationOfProfileIdProcess.Text = "Generating types...";
            var typeCode = AllInOneForTypeDll.GetCode(data);

            Tracer.CustomSqlGenerationOfProfileIdProcess.Text = "Generating business...";
            var businessCode = AllInOneForBusinessDll.GetCode(data);

            Tracer.CustomSqlGenerationOfProfileIdProcess.Text = "Writing to files...";

            FileAccess.WriteAllText(data.TypesProjectPath + "\\Generated\\CustomSql.cs", typeCode);
            FileAccess.WriteAllText(data.BusinessProjectPath + "\\Generated\\CustomSql.cs", businessCode);

            //if (data.ProfileId == "CC_OPERATIONS")
            //{
            //    var typesFilePath = $@"D:\work\BOA.CardModules\Dev\AutoGeneratedCodes\{data.ProfileId}\BOA.Types.Dal.Card.{data.ProfileId}\All.cs";
            //    var businessFilePath = $@"D:\work\BOA.CardModules\Dev\AutoGeneratedCodes\{data.ProfileId}\BOA.Business.Dal.Card.{data.ProfileId}\All.cs";

            //    FileAccess.WriteAllText(typesFilePath, typeCode);
            //    FileAccess.WriteAllText(businessFilePath, businessCode);
            //}
        }
        #endregion
    }
}