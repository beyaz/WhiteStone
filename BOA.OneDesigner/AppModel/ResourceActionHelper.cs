using System;
using System.Collections.Generic;
using System.Linq;
using BOA.Common.Helpers;
using BOA.OneDesigner.JsxElementModel;

namespace BOA.OneDesigner.AppModel
{
    static class ResourceActionHelper
    {
        public static void InitOnClickActionForPreviousRecords(Aut_ResourceAction boaResourceInfo)
        {

            if (boaResourceInfo.OpenFormWithResourceCode.HasValue())
            {
                var realResourceCode = boaResourceInfo.OpenFormWithResourceCode.SplitAndClear("-")?.FirstOrDefault();
                if (realResourceCode.HasValue())
                {
                    boaResourceInfo.OpenFormWithResourceCode = realResourceCode;
                }
            }

            if (boaResourceInfo.OnClickAction == null)
            {
                boaResourceInfo.OnClickAction = new ActionInfo
                {
                    OrchestrationMethodName                          = boaResourceInfo.OrchestrationMethodName,
                    ExtensionMethodName                              = boaResourceInfo.ExtensionMethodName,
                    OpenFormWithResourceCode                         = boaResourceInfo.OpenFormWithResourceCode,
                    OpenFormWithResourceCodeDataParameterBindingPath = boaResourceInfo.OpenFormWithResourceCodeDataParameterBindingPath,
                    DialogTitleInfo                                  = new LabelInfo(),
                    YesNoQuestionInfo                                = new LabelInfo()
                };

                boaResourceInfo.OrchestrationMethodName                          = null;
                boaResourceInfo.ExtensionMethodName                              = null;
                boaResourceInfo.OpenFormWithResourceCode                         = null;
                boaResourceInfo.OpenFormWithResourceCodeDataParameterBindingPath = null;
            }
        }

        public static void UpdateOldStyleResourceAction(ScreenInfo data)
        {
            if (data.ResourceActions == null)
            {
                return;
            }

            foreach (var item in data.ResourceActions)
            {
                InitOnClickActionForPreviousRecords(item);    
            }
        }
        public static List<Aut_ResourceAction> GetResourceActions(List<Aut_ResourceAction> resourceActionsInDesigner , string resourceCode)
        {
            if (resourceCode == null)
            {
                throw new ArgumentNullException(nameof(resourceCode));
            }

            var resourceActions = GetResourceActionsFromDb(resourceCode);
            if (resourceActions.Count == 0)
            {
                return resourceActions;
            }

            if (resourceActionsInDesigner == null)
            {
                return resourceActions;
            }

            // merge
            foreach (var item in resourceActionsInDesigner)
            {
                var existingRecord = resourceActions.FirstOrDefault(x => x.CommandName == item.CommandName);
                if (existingRecord == null)
                {
                    continue;
                }

                item.CopyTo(existingRecord);
            }

            return resourceActions;
        }

        static List<Aut_ResourceAction> GetResourceActionsFromDb(string resourceCode)
        {
            using (var database = new DevelopmentDatabase())
            {
                return database.GetResourceActions(resourceCode);
            }

        }
    }
}