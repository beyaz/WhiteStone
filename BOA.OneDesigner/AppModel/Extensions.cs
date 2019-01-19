namespace BOA.OneDesigner.AppModel
{
    static class Extensions
    {
        public static void CopyTo(this Aut_ResourceAction from, Aut_ResourceAction to)
        {
            to.IsVisibleBindingPath    = from .IsVisibleBindingPath;
            to.OrchestrationMethodName = from .OrchestrationMethodName;
        }
    }
}