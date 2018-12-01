using System.Collections.Generic;
using System.Linq;
using BOA.Common.Helpers;
using BOAPlugins.ExportingModel;
using BOAPlugins.ViewClassDependency;

namespace BOAPlugins.FormApplicationGenerator
{
    

   
    static class TsxCodeGeneration
    {

        public static IReadOnlyList<SnapInfo> GetSnaps(this IReadOnlyCollection<BField> fields)
        {
            return fields.Where(x => x.HasSnapName()).Select(dataField => new SnapInfo
            {
                Name              = dataField.GetSnapName(),
                ComponentTypeName = dataField.ComponentType.GetValueOrDefault().ToString()
            }).ToList();
        }

     

        #region Methods
        

        static string GetJSXElementForRenderDefinitionPage(Model model)
        {
            var renderCodes = new PaddedStringBuilder
            {
                PaddingLength = 4,
                PaddingCount  = 3
            };

            new TransactionPageTemplate
            {
                IsTabForm = model.IsTabForm,

            };



            if (model.IsTabForm)
            {
                var isFirst = true;
                foreach (var tab in model.Tabs)
                {
                    if (isFirst)
                    {
                        isFirst = false;
                    }
                    else
                    {
                        renderCodes.AppendLine(",");
                    }

                    renderCodes.AppendLine("{");

                    renderCodes.PaddingCount++;
                    renderCodes.AppendLine("text: Message." + tab.Title + ",");
                    renderCodes.AppendWithPadding("content:");

                    renderCodes.PaddingCount++;
                    // RenderCards(renderCodes, tab.Cards, model.UserRawStringForMessaging);
                    renderCodes.PaddingCount--;

                    renderCodes.PaddingCount--;

                    renderCodes.AppendLine("}");
                }
            }
            else
            {
                renderCodes.AppendWithPadding("");
                //RenderCards(renderCodes, model.Cards, model.UserRawStringForMessaging);
            }

            return renderCodes.ToString();
        }

        

        

        
        #endregion
    }
}