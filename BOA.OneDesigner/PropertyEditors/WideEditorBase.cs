using System.Windows;
using System.Windows.Controls;
using BOA.Common.Helpers;
using BOA.OneDesigner.AppModel;
using CustomUIMarkupLanguage.UIBuilding;

namespace BOA.OneDesigner.PropertyEditors
{
    class WideEditorBase : StackPanel
    {
        #region Fields
        protected readonly TextBlock _textBlock;
        protected readonly Slider    _slider;
        #endregion

        #region Constructors
        public WideEditorBase( int min, int max)
        {
            _textBlock = new TextBlock().LoadJson("{isBold:true}");
            _slider    = new Slider();

            Children.Add(_textBlock);
            Children.Add(_slider);

            _slider.Minimum = min;
            _slider.Maximum = max;

            _slider.LoadJson("{TickPlacement:'BottomRight', TickFrequency:1, IsSnapToTickEnabled:true,Margin:10,Value:'{Binding Value}'}");
            _slider.DataContext = this;

            _slider.ValueChanged += SliderValueChanged;

            Loaded += (s, e) => { UpdateLabel(); };
        }
        #endregion

        #region Methods
        void SliderValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (!IsLoaded)
            {
                return;
            }
            UpdateLabel();
            SM.Get<Host>().EventBus.Publish(EventBus.WideChanged);
        }

        protected virtual void UpdateLabel()
        {
            _textBlock.Text = "Wide: " + Value;
        }
        #endregion

        #region Value
        public static readonly DependencyProperty ValueProperty = DependencyProperty.Register("Value", typeof(int), typeof(WideEditorBase), new PropertyMetadata(default(int), OnValueChanged));

        static void OnValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as WideEditorBase)?.UpdateLabel();
        }

        public int Value
        {
            get { return (int) GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }
        #endregion
    }
}