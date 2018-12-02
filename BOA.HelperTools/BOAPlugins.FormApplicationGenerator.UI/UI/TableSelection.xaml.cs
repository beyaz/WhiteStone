using BOAPlugins.FormApplicationGenerator.UI;

namespace BOAPlugins.FormApplicationGenerator.UI
{
    /// <summary>
    ///     Interaction logic for TableSelection.xaml
    /// </summary>
    public partial class TableSelection
    {
        #region Constructors
        public TableSelection()
        {
            InitializeComponent();
        }
        #endregion

        void IsReady()
        {
            
            NamingInfo = NamingInfo.Create(solutionFilePath, tableNameIndDatabase);

            TableNameInDatabase = tableNameIndDatabase;
        }
       
    }
}