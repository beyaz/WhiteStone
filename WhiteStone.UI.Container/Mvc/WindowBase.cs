using System;
using System.Windows;
using System.Windows.Input;
using BOA.Common.Helpers;
using CustomUIMarkupLanguage;

namespace WhiteStone.UI.Container.Mvc
{
    public class WindowBase<TModel, TController> : WindowBase where TModel : ModelBase, new() where TController : ControllerBase<TModel>, new()
    {
        #region Fields
        TController controller;
        TModel model;
        #endregion

        #region Constructors
        /// <summary>
        ///     Initializes a new instance of the <see cref="MainWindow" /> class.
        /// </summary>
        public WindowBase()
        {
            Controller =  new TController();
            Loaded     += OnViewLoaded;
            Closed     += OnViewClose;
        }
        #endregion

        #region Public Properties
        public bool CloseOnEscapeCharacter { get; set; }

        public TController Controller
        {
            get { return controller; }
            set
            {
                controller = value;
                OnPropertyChanged();
            }
        }

        
        public  TModel Model
        {
            get { return model; }
            set
            {
                model = value;
                OnPropertyChanged();
            }
        }
        #endregion

        #region Public Methods
        public virtual void FireAction(string controllerPublicMethodName)
        {
            if (Model != null)
            {
                Model.ViewMessageTypeIsError = false;
                Model.ViewMessage            = null;
                Model.ViewShouldBeClose      = false;

                Controller.Model = Model.Clone();
            }

            try
            {
                var methodInfo = Controller.GetType().GetMethod(controllerPublicMethodName, true, false, true, true);

                methodInfo.Invoke(Controller, null);
            }
            catch (Exception e)
            {
                App.ShowErrorNotification(e.Message);
                Log.Push(e);
                return;
            }
          

            if (Controller.Model == null)
            {
                DataContext = default(TModel);
                return;
            }

            Model = Controller.Model.Clone();

           

            if (Model.ViewMessage != null)
            {
                if (Model.ViewMessageTypeIsError)
                {
                    App.ShowErrorNotification(Model.ViewMessage);
                }
                else
                {
                    App.ShowSuccessNotification(Model.ViewMessage);
                }
            }

            if (Model.ViewShouldBeClose)
            {
                Close();
                return;
            }

            DataContext = Model;
        }
        #endregion

        #region Methods
        void OnViewClose(object sender, EventArgs e)
        {
            FireAction(nameof(OnViewClose));
        }

        void OnViewLoaded(object sender, RoutedEventArgs eventArgs)
        {
            if (CloseOnEscapeCharacter)
            {
                KeyDown += (s, e) =>
                {
                    if (e.Key == Key.Escape)
                    {
                        Close();
                    }
                };
            }

            FireAction(nameof(OnViewLoaded));
        }
        #endregion
    }
}