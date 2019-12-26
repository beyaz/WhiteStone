using System.Collections.Generic;
using BOA.OneDesigner.Helpers;
using WhiteStone.Helpers;

namespace BOA.OneDesigner.WpfControls
{
    static class InsertHelper
    {
        #region Public Methods
        public static void Move<T>(List<T> items, T item, int target)
        {
            var index = items.IndexOf(item);

            if (index < 0)
            {
                throw Error.InvalidOperation();
            }

            if (index == target)
            {
                return;
            }

            if (target >= items.Count)
            {
                target = items.Count ;
            }

            if (index == target)
            {
                return;
            }

            items.Insert(target,item);

            if (target>index)
            {
                items.RemoveAt(index);
            }
            else
            {
                items.RemoveAt(index+1);
            }

           
        }
        #endregion
    }
}