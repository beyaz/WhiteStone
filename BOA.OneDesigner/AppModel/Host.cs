using System.Windows;

namespace BOA.OneDesigner.AppModel
{
    public interface IHostItem
    {
        #region Public Properties
        Host Host { get; set; }
        #endregion
    }

    public class Host
    {
        #region Public Properties
        public EventBus EventBus { get; } = new EventBus();
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