using System;
using System.Linq;
using BOA.Common.Helpers;
using BOA.OneDesigner.Helpers;
using BOA.OneDesigner.JsxElementModel;
using BOAPlugins.Utility;

namespace BOA.OneDesigner.CodeGeneration
{
    static class RenderHelper
    {

        public static bool IsCommentEnabled => false;

        #region Public Methods
        public static string ConvertBindingPathToIncomingRequest(string bindingPathInJs)
        {
            return bindingPathInJs.Replace(Config.BindingPrefixInJs, Config.IncomingRequestVariableName + ".");
        }

        public static string GetJsValue(SizeInfo size)
        {
            if (size.IsLarge)
            {
                return "ComponentSize.LARGE";
            }

            if (size.IsMedium)
            {
                return "ComponentSize.MEDIUM";
            }

            if (size.IsSmall)
            {
                return "ComponentSize.SMALL";
            }

            if (size.IsExtraSmall)
            {
                return "ComponentSize.XSMALL";
            }

            throw Error.InvalidOperation();
        }

        public static string GetLabelValue(ScreenInfo screenInfo, LabelInfo data)
        {
            if (data == null)
            {
                return null;
            }

            if (data.IsFreeText)
            {
                if (data.FreeTextValue.IsNullOrWhiteSpace())
                {
                    return null;
                }

                return '"' + data.FreeTextValue + '"';
            }

            if (data.IsRequestBindingPath)
            {
                return TypescriptNaming.NormalizeBindingPath(Config.BindingPrefixInCSharp + data.RequestBindingPath);
            }

            if (data.IsFromMessaging)
            {
                return $"getMessage(\"{screenInfo.MessagingGroupName}\", \"{data.MessagingValue}\")";
            }

            return null;
        }

        public static bool HasValue(this SizeInfo size)
        {
            return size != null && size.IsEmpty == false;
        }

        public static string NormalizeBindingPathInRenderMethod(WriterContext writerContext, string bindingPathInCSharp)
        {
            var bindingPathInJs = TypescriptNaming.NormalizeBindingPath(Config.BindingPrefixInCSharp + bindingPathInCSharp);

            var list = bindingPathInJs.SplitAndClear(".");

            if (list.Count <= 2)
            {
                return bindingPathInJs;
            }

            var len = list.Count - 2;

            for (var i = 0; i < len; i++)
            {
                var assignments = new string[2];

                Array.Copy(list.ToArray(), i, assignments, 0, 2);

                var assignmentValue = string.Join(".", assignments);

                var variable = $"const {list[i + 1]} = {assignmentValue}||{{}};";

                if (writerContext.RenderMethodRequestRelatedVariables.Contains(variable)==false)
                {
                    writerContext.RenderMethodRequestRelatedVariables.Add(variable);    
                }
                
            }

            return string.Join(".", list.Reverse().Take(2).Reverse());
        }
        #endregion
    }
}