using System;

namespace BOA.OneDesigner.JsxElementModel
{
    public static class VisitHelper
    {
        #region Public Methods
        public static void ConvertToNewComponent(object component)
        {
            var ci = component as ComponentInfo;
            if (ci == null)
            {
                return;
            }

            if (ci.ButtonClickedActionInfo != null)
            {
                return;
            }

            ci.ButtonClickedActionInfo = new ActionInfo
            {
                OrchestrationMethodName                          = ci.ButtonClickedOrchestrationMethod,
                ExtensionMethodName                              = ci.ExtensionMethodName,
                DialogTitleInfo                                  = ci.OpenFormWithResourceCodeTitle ?? new LabelInfo(),
                YesNoQuestionInfo                                = ci.YesNoQuestion ?? new LabelInfo(),
                YesNoQuestionAfterYesOrchestrationCall           = ci.YesNoQuestionAfterYesOrchestrationCall,
                OpenFormWithResourceCode                         = ci.OpenFormWithResourceCode,
                OpenFormWithResourceCodeDataParameterBindingPath = ci.OpenFormWithResourceCodeDataParameterBindingPath,
                OpenFormWithResourceCodeIsInDialogBox            = ci.OpenFormWithResourceCodeIsInDialogBox,
                CssOfDialog                                      = ci.CssOfDialog,
                OrchestrationMethodOnDialogResponseIsOK          = ci.OrchestrationMethodOnDialogResponseIsOK
            };

            // clear old values.
            ci.ButtonClickedOrchestrationMethod                 = null;
            ci.ExtensionMethodName                              = null;
            ci.OpenFormWithResourceCodeTitle                    = null;
            ci.YesNoQuestion                                    = null;
            ci.YesNoQuestionAfterYesOrchestrationCall           = null;
            ci.OpenFormWithResourceCode                         = null;
            ci.OpenFormWithResourceCodeDataParameterBindingPath = null;
            ci.OpenFormWithResourceCodeIsInDialogBox            = false;
            ci.CssOfDialog                                      = null;
            ci.OrchestrationMethodOnDialogResponseIsOK          = null;
        }

        public static void VisitComponents(ScreenInfo instance, Action<object> on)
        {
            Visit(instance.JsxModel, on);
        }
        #endregion

        #region Methods
        static void Visit(object component, Action<object> on)
        {
            if (component == null)
            {
                return;
            }

            if (component is ComponentInfo componentInfo)
            {
                on(componentInfo);
                return;
            }

            if (component is BComboBox comboBox)
            {
                on(comboBox);
                return;
            }

            if (component is BDataGrid dataGrid)
            {
                on(dataGrid);
                return;
            }

            if (component is BTabBar tabBar)
            {
                on(tabBar);

                foreach (var item in tabBar.Items)
                {
                    Visit(item, on);
                }

                return;
            }

            if (component is BTabBarPage tabBarPage)
            {
                on(tabBarPage);

                Visit(tabBarPage.DivAsCardContainer, on);

                return;
            }

            if (component is DivAsCardContainer divAsCardContainer)
            {
                on(divAsCardContainer);

                foreach (var bCard in divAsCardContainer.Items)
                {
                    Visit(bCard, on);
                }

                return;
            }

            if (component is BCard card)
            {
                on(card);

                foreach (var item in card.Items)
                {
                    Visit(item, on);
                }

                return;
            }

            throw new InvalidOperationException();
        }
        #endregion
    }
}