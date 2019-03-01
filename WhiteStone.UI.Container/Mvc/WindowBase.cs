using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using BOA.Common.Helpers;
using CustomUIMarkupLanguage;

namespace WhiteStone.UI.Container.Mvc
{
    public class WindowBase<TModel, TController> : WindowBase where TModel : ModelBase, new() where TController : ControllerBase<TModel>, new()
    {
        readonly bool CloneModelOnEveryAction;

        #region Fields
        TController controller;
        TModel      model;
        #endregion

        #region Constructors       
        /// <summary>
        /// Initializes a new instance of the <see cref="WindowBase{TModel, TController}"/> class.
        /// </summary>
        public WindowBase()
        {
            Controller =  new TController();
            Loaded     += OnViewLoaded;
            Closed     += OnViewClose;

            TitleAlignment = HorizontalAlignment.Center;

            CloneModelOnEveryAction = ConfigHelper.GetFromAppSetting("WindowBase.CloneModelOnEveryAction").To<bool?>()??true;

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

        public TModel Model
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

                Controller.Model = CloneModelOnEveryAction ? Model.Clone() : Model;
            }

            try
            {
                var methodInfo = Controller.GetType().GetMethod(controllerPublicMethodName, true, false, true, true);

                methodInfo.Invoke(Controller, null);
            }
            catch (Exception e)
            {
                App.HandleException(e);
                Log.Push(e);
                return;
            }

            if (Controller.Model == null)
            {
                DataContext = default(TModel);
                return;
            }

            Model = CloneModelOnEveryAction ? Controller.Model.Clone() : Controller.Model;

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

            Apply(Model.ActionButtons);

            DataContext = Model;
        }
        #endregion

        public Panel ActionPanel;

        #region Methods
        void Apply(IReadOnlyCollection<ActionButtonInfo> actionButtons)
        {
            ActionPanel?.Children.Clear();
            
            if (actionButtons == null)
            {
                return;
            }

            if (ActionPanel == null)
            {
                throw new ArgumentException(nameof(ActionPanel));
            }


            foreach (var info in actionButtons)
            {
                var button = new Button
                {
                    Content = info.Text,
                    FontWeight = FontWeights.Bold,
                    Margin = new Thickness(10),
                    MinWidth = 150
                };

                button.Click += (s, e) => { FireAction(info.ActionName); };

                ActionPanel.Children.Add(button);
            }
            
        }

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