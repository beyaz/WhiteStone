using System.Windows;

namespace BOA.OneDesigner.AppModel
{
    public class Host
    {
        #region Constructors
        public Host()
        {
            DragHelper = new DragHelper(this);
        }
        #endregion

        #region Public Properties

        public  UIElement SelectedElement => DraggingElement;
        public UIElement  DraggingElement           { get; set; }
        public Point      DraggingElementStartPoint { get; set; }
        public DragHelper DragHelper                { get; }
        public EventBus   EventBus                  { get; } = new EventBus();
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