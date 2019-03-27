using System.Linq;
using BOA.Common.Helpers;
using BOA.OneDesigner.CodeGenerationModel;
using BOA.OneDesigner.Helpers;
using BOA.OneDesigner.JsxElementModel;

namespace BOA.OneDesigner.CodeGenerationHelper
{
    public static class SnapNamingHelper
    {
        #region Public Methods
        public static string ForceUniqueName(this WriterContext writerContext, string name)
        {
            var useCount = false;
            var count    = 1;
            while (true)
            {
                if (writerContext.AllNames.ContainsKey(name) == false)
                {
                    break;
                }

                if (writerContext.AllNames.ContainsKey(name + count) == false)
                {
                    useCount = true;
                    break;
                }

                count++;
            }

            if (useCount)
            {
                name += count;
            }

            writerContext.AllNames[name] = true;

            return name;
        }

        public static string GetLastPropertyName(string propertyPath)
        {
            return propertyPath.SplitAndClear(".")?.Last();
        }

        public static void InitSnapName(WriterContext writerContext, BTabBar data)
        {
            data.SnapName = GetComponentTypeName(data);
            data.SnapName = writerContext.ForceUniqueName(data.SnapName);
        }

        public static void InitSnapName(WriterContext writerContext,ComponentInfo data)
        {

            var lastPropertyName = GetLastPropertyName(data.ValueBindingPathInTypeScript);

            if (data.Type.IsAccountComponent)
            {
                data.SnapName = lastPropertyName.MakeLowerCaseFirstCharacter() + "AccountComponent";
            }
            else  if (data.Type.IsBranchComponent)
            {
                data.SnapName = lastPropertyName.MakeLowerCaseFirstCharacter() + "BranchComponent";
            }
            else  if (data.Type.IsParameterComponent)
            {
                data.SnapName = lastPropertyName.MakeLowerCaseFirstCharacter() + "ParameterComponent";
            }
            else  if (data.Type.IsCreditCardComponent)
            {
                data.SnapName = lastPropertyName.MakeLowerCaseFirstCharacter() + "CreditCardComponent";
            }
            else  if (data.Type.IsInformationText)
            {
                data.SnapName = lastPropertyName.MakeLowerCaseFirstCharacter() + "InformationText";
            }
            else  if (data.Type.IsDivider)
            {
                data.SnapName = lastPropertyName.MakeLowerCaseFirstCharacter() + "Divider";
            }
            else  if (data.Type.IsInput)
            {
                data.SnapName = lastPropertyName.MakeLowerCaseFirstCharacter() + "Input";
            }
            else
            {
                throw Error.InvalidOperation();
            }

            data.SnapName = writerContext.ForceUniqueName(data.SnapName);

            
        }

        public static void InitSnapName(WriterContext writerContext, BComboBox data)
        {
            var lastPropertyName = GetLastPropertyName(data.SelectedValueBindingPath);

            data.SnapName = lastPropertyName.MakeLowerCaseFirstCharacter() + data.GetType().Name.RemoveFromStart("B");
            data.SnapName = writerContext.ForceUniqueName(data.SnapName);

            data.TypeScriptMethodNameOfGetGridColumns = $"get{data.SnapName.MakeUpperCaseFirstCharacter()}Columns";
        }

        

        

        public static void InitSnapName(BDataGrid data)
        {
            data.SnapName = GetComponentTypeName(data) + data.ValueBindingPathInTypeScript;
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

        static string MakeUpperCaseFirstCharacter(this string value)
        {
            return value[0].ToString().ToUpperEN() + value.Substring(1);
        }
        #endregion
    }
}