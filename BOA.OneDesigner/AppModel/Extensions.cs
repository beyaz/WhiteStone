using System.Linq;
using BOA.Common.Helpers;

namespace BOA.OneDesigner.AppModel
{
    static class Extensions
    {
        #region Public Methods
        public static void CopyTo(this Aut_ResourceAction from, Aut_ResourceAction to)
        {

            if (from.OpenFormWithResourceCode.HasValue())
            {
                var realResourceCode = @from.OpenFormWithResourceCode.SplitAndClear("-")?.FirstOrDefault();
                if (realResourceCode.HasValue())
                {
                    from.OpenFormWithResourceCode = realResourceCode;
                }
            }

            to.IsVisibleBindingPath                             = from.IsVisibleBindingPath;
            to.IsEnableBindingPath                              = from.IsEnableBindingPath;
            to.OrchestrationMethodName                          = from.OrchestrationMethodName;
            to.OpenFormWithResourceCode                         = from.OpenFormWithResourceCode;
            to.OpenFormWithResourceCodeDataParameterBindingPath = from.OpenFormWithResourceCodeDataParameterBindingPath;
        }
        #endregion
    }
}