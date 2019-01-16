using System.Linq;
using BOA.Common.Helpers;
using BOA.OneDesigner.JsxElementModel;

namespace BOA.OneDesigner.CodeGeneration
{
    public static class SnapNamingHelper
    {

        public static string GetLastPropertyName(string propertyPath)
        {
            return propertyPath.SplitAndClear(".").Last();
        }

        #region Public Methods
        public static void InitSnapName(BTabBar data)
        {
            // TODO: snap  olayını hallet
            if (string.IsNullOrWhiteSpace(data.ActiveTabIndexBindingPath))
            {
            }
        }
        public static void InitSnapName(BParameterComponent data)
        {
            data.SnapName = GetComponentTypeName(data) + data.BindingPathInTypeScript;
        }
        public static void InitSnapName(BComboBox data)
        {
            var lastPropertyName = GetLastPropertyName(data.SelectedValueBindingPath);

            data.SnapName =  lastPropertyName.MakeLowerCaseFirstCharacter() + data.GetType().Name.RemoveFromStart("B");

            data.TypeScriptMethodNameOfGetGridColumns = $"get{lastPropertyName}Columns";
        }
        public static void InitSnapName(BAccountComponent data)
        {
            data.SnapName = GetComponentTypeName(data) + data.BindingPathInTypeScript;
        }
        
        public static void InitSnapName(BBranchComponent data)
        {
            data.SnapName = GetComponentTypeName(data) + data.BindingPathInTypeScript;
        }
        public static void InitSnapName(BDataGrid data)
        {
            data.SnapName = GetComponentTypeName(data) + data.BindingPathInTypeScript;
        }
        

        public static void InitSnapName(BCheckBox data)
        {
            data.SnapName = GetComponentTypeName(data) + data.BindingPathInTypeScript;
        }
        
        public static void InitSnapName(BDateTimePicker data)
        {
            data.SnapName = GetComponentTypeName(data) + data.BindingPathInTypeScript;
        }
        #endregion

        #region Methods
        static string GetComponentTypeName(object componentModel)
        {
            var typeName = componentModel.GetType().Name.RemoveFromStart("B");

            return typeName[0].ToString().ToLowerTR() + typeName.Substring(1);
        }

        static string MakeLowerCaseFirstCharacter(this string value)
        {
            return value[0].ToString().ToLowerTR() + value.Substring(1);
        }
        #endregion
    }
}