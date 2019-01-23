namespace BOA.OneDesigner.AppModel
{
    static class Extensions
    {
        #region Public Methods
        public static void CopyTo(this Aut_ResourceAction from, Aut_ResourceAction to)
        {
            to.IsVisibleBindingPath                             = from.IsVisibleBindingPath;
            to.OrchestrationMethodName                          = from.OrchestrationMethodName;
            to.OpenFormWithResourceCode                         = from.OpenFormWithResourceCode;
            to.OpenFormWithResourceCodeDataParameterBindingPath = from.OpenFormWithResourceCodeDataParameterBindingPath;
        }
        #endregion
    }
}