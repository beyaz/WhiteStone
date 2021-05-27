using System;
using System.Windows;
using System.Windows.Controls;
using CustomUIMarkupLanguage.Markup;

namespace CustomUIMarkupLanguage.UIBuilding
{
    sealed class ActionButton : Border
    {
        #region Constants
        const string UITemplate = @"
{
    cornerRadius: 10, 
    border: '1px solid #A4ADB2',
	Child: { ui: 'TextBlock', Text: '{Binding Text}', vAlignIsCenter: true, hAlignIsCenter: true, margin: 10 }
}";
        #endregion

        #region Fields
        string initialText;
        #endregion

        #region Constructors
        public ActionButton()
        {
            DataContext = this;

            this.LoadJson(UITemplate);

            void onFinish()
            {
                WpfExtra.UpdateUiAfterSleep(Dispatcher, 10, () => Text = initialText);
            }

            MouseLeftButtonDown += (s, e) =>
            {
                initialText = Text;

                Text = TextWhileProcessIsRunning;

                onClick?.Invoke(onFinish);
            };
        }
        #endregion

        #region Public Events
        public event Action<Action> onClick;
        #endregion

        #region Public Methods
        public static UIElement HandleCreation(Builder builder, Node node)
        {
            if (node.UI == nameof(ActionButton).ToUpperEN())
            {
                return new ActionButton();
            }

            return null;
        }
        #endregion

        #region string Text
        public static readonly DependencyProperty TextProperty = DependencyProperty.Register(nameof(Text), typeof(string), typeof(ActionButton), new PropertyMetadata(default(string)));

        public string Text
        {
            get { return (string) GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }
        #endregion

        #region string TextWhileProcessIsRunning
        public static readonly DependencyProperty TextWhileProcessIsRunningProperty = DependencyProperty.Register(
                                                                                                                  "TextWhileProcessIsRunning", typeof(string), typeof(ActionButton), new PropertyMetadata(default(string)));

        public string TextWhileProcessIsRunning
        {
            get { return (string) GetValue(TextWhileProcessIsRunningProperty); }
            set { SetValue(TextWhileProcessIsRunningProperty, value); }
        }
        #endregion
    }
}