using System;
using System.Collections.Generic;
using System.Linq;
using BOA.OneDesigner.JsxElementModel;

namespace BOA.OneDesigner.CodeGeneration
{
    static class JsxModelHelper
    {


        #region Public Methods
        public static bool HasComponent<TComponentType>(this DivAsCardContainer divAsCardContainer)
        {
            var items = new List<object>();

            divAsCardContainer.VisitAllBComponents(c =>
            {
                if (c.GetType() == typeof(TComponentType))
                {
                    items.Add(c);
                }
            });

            return items.Any();
        }

        public static void VisitAllBComponents(this DivAsCardContainer divAsCardContainer, Action<object> action)
        {
            foreach (var card in divAsCardContainer.Items)
            {
                foreach (var item in card.Items)
                {
                    action(item);
                }

                action(card);
            }
        }

        

        

        
        #endregion

        

        #region Methods
         

        
        #endregion
    }
}