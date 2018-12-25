using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using BOA.Common.Helpers;
using CustomUIMarkupLanguage;
using MahApps.Metro.Controls;

namespace WhiteStone.UI.Container.Mvc
{
    public class WindowBase<TModel, TController> : WindowBase where TModel : ModelBase, new() where TController : ControllerBase<TModel>, new()
    {
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

            Apply(Model.ActionButtons);

            DataContext = Model;
        }
        #endregion

        #region Methods
        void Apply(IReadOnlyCollection<ActionButtonInfo> actionButtons)
        {
            if (actionButtons == null)
            {
                LeftWindowCommands = new WindowCommands();
                return;
            }

            var itemsSource = new List<Button>();

            foreach (var info in actionButtons)
            {
                var button = new Button
                {
                    Content = info.Text,
                    FontWeight = FontWeights.ExtraBold,
                    FontSize = 17
                };

                button.Click += (s, e) => { FireAction(info.ActionName); };

                itemsSource.Add(button);
            }

            LeftWindowCommands = new WindowCommands
            {
                ItemsSource = itemsSource
            };
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