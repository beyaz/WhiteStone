using System.Collections.Generic;
using System.Windows;
using BOA.Common.Helpers;
using BOA.OneDesigner.Helpers;
using BOAPlugins.TypescriptModelGeneration;

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

                (Database as JsonFile)?.SaveTfsFolderNames(tfsFolderNames);
            }
        }
        #endregion

        #region Public Properties
        public IDatabase  Database                  { get; }
        public UIElement  DraggingElement           { get; set; }
        public Point      DraggingElementStartPoint { get; set; }
        public DragHelper DragHelper                { get; }
        public EventBus   EventBus                  { get; } = new EventBus();
        public UIElement  LastSelectedUIElement     { get; set; }

        

        public UIElement SelectedElement => DraggingElement;


        public RequestIntellisenseData RequestIntellisenseData { get; set; }
        #endregion

        #region Public Methods
        public T Create<T>() where T : IHostItem, new()
        {
            return new T
            {
                Host = this
            };
        }

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