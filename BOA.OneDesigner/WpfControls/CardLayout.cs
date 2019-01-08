using System.Linq;
using System.Windows;
using System.Windows.Controls;
using BOA.OneDesigner.AppModel;
using BOA.OneDesigner.JsxElementModel;

namespace BOA.OneDesigner.WpfControls
{
    static class CardLayout
    {


        public static void ApplyForCardsContainer(Grid grid)
        {
            WpfHelper.Apply12Column(grid);

            var rowIndex    = 0;
            var columnIndex = 0;

            var elements = grid.Children.ToArray();

            foreach (var item in elements)
            {
                var layoutProps = (item as BCardWpf)?.Model?.LayoutProps;
                if (layoutProps == null )
                {
                    throw Error.InvalidOperation();
                }

                var isLast = item == elements.Last();

                var span = layoutProps.Wide;

                item.SetValue(Grid.RowProperty, rowIndex);
                item.SetValue(Grid.ColumnProperty, columnIndex+layoutProps.X);
                item.SetValue(Grid.ColumnSpanProperty, span);

                columnIndex += span;

                if (isLast || columnIndex >= 12)
                {
                    grid.RowDefinitions.Add(new RowDefinition {Height = GridLength.Auto});

                    columnIndex = 0;
                    rowIndex++;
                }
            }
        }

        #region Public Methods
        public static void Apply(Grid grid)
        {
            WpfHelper.Apply12Column(grid);

            var rowIndex    = 0;
            var columnIndex = 0;

            var elements = grid.Children.ToArray();

            foreach (var item in elements)
            {
                var sizeInfo = (item as ISupportSizeInfo)?.SizeInfo;
                if (sizeInfo == null || sizeInfo.IsEmpty)
                {
                    throw Error.InvalidOperation();
                }

                var isLast = item == elements.Last();

                var span = GetSpan(sizeInfo);

                item.SetValue(Grid.RowProperty, rowIndex);
                item.SetValue(Grid.ColumnProperty, columnIndex);
                item.SetValue(Grid.ColumnSpanProperty, span);

                columnIndex += span;

                if (isLast || columnIndex >= 12)
                {
                    grid.RowDefinitions.Add(new RowDefinition {Height = GridLength.Auto});

                    columnIndex = 0;
                    rowIndex++;
                }
            }
        }

        public static void ApplyWithDropLocationMode(Grid grid)
        {
            WpfHelper.Apply12Column(grid);

            var rowIndex    = 0;
            var columnIndex = 0;

            var elements = grid.Children.ToArray();

            foreach (var item in elements)
            {
                var isLast = item == elements.Last();

                item.SetValue(Grid.RowProperty, rowIndex);
                item.SetValue(Grid.ColumnProperty, columnIndex);
                item.SetValue(Grid.ColumnSpanProperty, 4);

                columnIndex += 4;

                if (isLast || columnIndex >= 12)
                {
                    grid.RowDefinitions.Add(new RowDefinition {Height = GridLength.Auto});
                    columnIndex = 0;
                    rowIndex++;
                }
            }
        }
        #endregion

        #region Methods
        static int GetSpan(SizeInfo sizeInfo)
        {
            if (sizeInfo.IsLarge)
            {
                return 12;
            }

            if (sizeInfo.IsMedium)
            {
                return 6;
            }

            if (sizeInfo.IsExtraSmall)
            {
                return 3;
            }

            if (sizeInfo.IsSmall)
            {
                return 4;
            }

            throw Error.InvalidOperation();
        }
        #endregion
    }
}