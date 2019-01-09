using System.Windows;
using BOA.OneDesigner.Helpers;

namespace BOA.OneDesigner.AppModel
{
    public class Host
    {
        #region Constructors
        public Host()
        {
            DragHelper = new DragHelper(this);

            Database = new JsonFile();

            var tfsFolderNames = Database.GetTfsFolderNames();

            if (tfsFolderNames == null)
            {
                tfsFolderNames = TfsHelper.GetFolderNames();

                ((JsonFile) Database).SaveTfsFolderNames(tfsFolderNames);
            }
        }
        #endregion

        #region Public Properties
        public IDatabase  Database                                                { get; }
        public UIElement  DraggingElement                                         { get; set; }
        public Point      DraggingElementStartPoint                               { get; set; }
        public DragHelper DragHelper                                              { get; }
        public EventBus   EventBus                                                { get; } = new EventBus();
        public UIElement  LastSelectedUIElement                                   { get; set; }
        public string     LastSelectedUIElement_as_DataGrid_DataSourceBindingPath { get; set; }

        public RequestIntellisenseData RequestIntellisenseData { get; set; }

        public UIElement SelectedElement => DraggingElement;
        #endregion

        #region Public Methods
        public T Create<T>(object dataContext) where T : FrameworkElement, IHostItem, new()
        {
            return new T
            {
                Host        = this,
                DataContext = dataContext
            };
        }
        #endregion
    }
}