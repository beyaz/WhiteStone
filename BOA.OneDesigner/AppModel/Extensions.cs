using System.Linq;
using BOA.Common.Helpers;
using BOA.OneDesigner.JsxElementModel;

namespace BOA.OneDesigner.AppModel
{
    static class Extensions
    {
        #region Public Methods
        public static void CopyTo(this Aut_ResourceAction from, Aut_ResourceAction to)
        {
           


            to.IsVisibleBindingPath = from.IsVisibleBindingPath;
            to.IsEnableBindingPath  = from.IsEnableBindingPath;
            to.OnClickAction        = from.OnClickAction;
        }

       
        #endregion
    }
}