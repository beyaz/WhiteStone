using System.Collections.Generic;

namespace BOA.OneDesigner.AppModel
{
    public static class FormType
    {
        #region Constants
        public const string BrowsePage                  = "Browse Page (Listelme)";
        public const string TransactionPage             = "Transaction Page";
        public const string TransactionPageWithWorkflow = "TransactionPage with Workflow";
        #endregion

        #region Public Methods
        public static IReadOnlyList<string> GetAll()
        {
            return new List<string> {BrowsePage, TransactionPage, TransactionPageWithWorkflow};
        }
        #endregion
    }
}