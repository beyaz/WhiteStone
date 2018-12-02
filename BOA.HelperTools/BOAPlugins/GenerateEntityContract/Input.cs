namespace BOAPlugins.GenerateEntityContract
{
    public class Input
    {
        #region Public Properties
        public string SelectedText { get; set; }
        #endregion
    }

    public class Result
    {
        #region Public Properties
        public string ContractClassBody { get; set; }
        public string ErrorMessage      { get; set; }
        #endregion
    }
}