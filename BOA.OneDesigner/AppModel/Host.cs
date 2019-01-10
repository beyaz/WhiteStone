using System;
using System.Windows;
using System.Windows.Controls;
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
        public UIElement  SelectedElement                                         { get; set; }
        public Point      DraggingElementStartPoint                               { get; set; }
        public DragHelper DragHelper                                              { get; }
        public EventBus   EventBus                                                { get; } = new EventBus();
        public UIElement  LastSelectedUIElement                                   { get; set; }
        public string     LastSelectedUIElement_as_DataGrid_DataSourceBindingPath { get; set; }

        public RequestIntellisenseData RequestIntellisenseData { get; set; }

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


        public void AttachToEventBus(IEventBusListener listener, IEventBusListener parent=null)
        {
            if (listener == null)
            {
                return;
            }

            listener.AttachToEventBus();

            if (parent != null)
            {
                parent.OnDeAttachToEventBus += listener.DeAttachToEventBus;    
            }
        }

        public void DeAttachToEventBus(IEventBusListener listener)
        {
            listener?.DeAttachToEventBus();
        }

        public void DeAttachToEventBus(UIElementCollection uiElementCollection)
        {
            foreach (UIElement  element in uiElementCollection)
            {
                DeAttachToEventBus(element as IEventBusListener);
            }
        }

        public void AttachToEventBus(UIElementCollection uiElementCollection)
        {
            foreach (UIElement  element in uiElementCollection)
            {
                AttachToEventBus(element as IEventBusListener);
            }
        }

        
    }


    public interface IEventBusListener
    {
        event Action OnAttachToEventBus;
        event Action OnDeAttachToEventBus;

        void AttachToEventBus();
        void DeAttachToEventBus();
    }
}