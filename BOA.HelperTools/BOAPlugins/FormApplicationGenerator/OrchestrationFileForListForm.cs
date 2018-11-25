using System;
using System.Linq;

namespace BOAPlugins.FormApplicationGenerator
{
    static class OrchestrationFileForListForm
    {
        #region Public Methods
        public static string GenerateCode(Model Model)
        {
            var resultGridColumnNames = string.Join("," + Environment.NewLine, Model.FormDataClassFields.Select(fieldInfo => $"nameof({Model.DefinitionFormDataClassName}.{fieldInfo.Name})"));

            return @"
using System.Collections.Generic;
using BOA.Base;
using BOA.Common.Types;
using BOA.Common.Helpers;
using " + Model.NamespaceNameForType + @";

namespace " + Model.NamespaceNameForOrchestration + @"
{   
    public class " + Model.FormName + @"ListForm
    {
        /// <summary>
        ///     Loads the data.
        /// </summary>
        public GenericResponse<" + Model.RequestNameForList + @"> LoadData(" + Model.RequestNameForList + @" request, ObjectHelper objectHelper)
        {
            var returnObject = objectHelper.InitializeResponse(request);
            var dataSource   = request.DataSource;


            dataSource.DataGridInfo = GetDataGridInfo();
            dataSource.Records      = new List<" + Model.DefinitionFormDataClassName + @">();

            return returnObject;
        }

        /// <summary>
        ///     Gets the information.
        /// </summary>
        public GenericResponse<" + Model.RequestNameForList + @"> GetInfo(" + Model.RequestNameForList + @" request, ObjectHelper objectHelper)
        {
            var returnObject = objectHelper.InitializeResponse(request);
            var data         = request.Data;
            var dataSource   = request.DataSource;
           

            #region TODO: Expects code
            dataSource.Records = RandomValue.ListOf<" + Model.DefinitionFormDataClassName + @">();
            #endregion

            request.StatusMessage = dataSource.Records.Count + Message.RecordsWereBrought;

            return returnObject;
        }

        /// <summary>
        ///     Gets the data grid information.
        /// </summary>
        static DataGridInfo GetDataGridInfo()
        {
            return DataGridInfo.Create(typeof(" + Model.DefinitionFormDataClassName + @"), new[]
            {
                " + resultGridColumnNames + @"
            });
        }        
    }
}
";
        }
        #endregion
    }
}