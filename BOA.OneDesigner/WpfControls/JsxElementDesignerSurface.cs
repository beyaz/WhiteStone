using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using BOA.OneDesigner.JsxElementModel;

namespace BOA.OneDesigner.WpfControls
{
    public class JsxElementDesignerSurface : StackPanel
    {
        #region Constructors
        public JsxElementDesignerSurface()
        {
            Background = Brushes.WhiteSmoke;
            VerticalAlignment = VerticalAlignment.Stretch;

            EventBus.DragStarted        += EnterDropLocationMode;
            EventBus.AfterDropOperation += ExitDropLocationMode;
        }
        #endregion

        #region Methods
        protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
        {
            if (e.Property == DataContextProperty)
            {
                Refresh();
            }

            base.OnPropertyChanged(e);
        }

        void EnterDropLocationMode()
        {
            if (Children.Count == 0)
            {
                var dropLocation = new DropLocation
                {
                    OnDropAction        = OnDrop,
                    TargetLocationIndex = 0,
                    Width               = 50,
                    Height              = 50
                };

                Children.Add(dropLocation);
            }
        }

        void ExitDropLocationMode()
        {
            var items = Children.ToArray();

            Children.Clear();

            foreach (var control in items)
            {
                if (control is DropLocation)
                {
                    continue;
                }

                Children.Add(control);
            }
        }

        void OnDrop(DropLocation dropLocation)
        {
            var bInput = UIContext.DraggingElement as BCardWpf;
            if (bInput != null)
            {
                bInput.Data.RemoveFromParent();

                DataContext = new BCardSection
                {
                    Items = new List<BCard>
                    {
                        bInput.Data
                    }
                };

                this.RefreshDataContext();

                return;
            }

            throw new ArgumentException();
        }

        void Refresh()
        {
            Children.Clear();

            if (DataContext == null)
            {
                return;
            }

            var cardSection = DataContext as BCardSection;

            if (cardSection != null)
            {
                var bCardSection = new BCardSectionWpf
                {
                    DataContext = cardSection
                };

                Children.Add(bCardSection);
                return;
            }

            throw new ArgumentException();
        }
        #endregion
    }
}