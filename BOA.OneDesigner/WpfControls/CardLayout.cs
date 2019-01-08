using System.Linq;
using System.Windows;
using System.Windows.Controls;
using BOA.OneDesigner.AppModel;

namespace BOA.OneDesigner.WpfControls
{
    static class CardLayout
    {
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

                if (sizeInfo.IsLarge)
                {

                    item.SetValue(Grid.RowProperty, rowIndex);
                    item.SetValue(Grid.ColumnProperty, columnIndex);
                    item.SetValue(Grid.ColumnSpanProperty, 12);

                    columnIndex += 12;
                }

                if (sizeInfo.IsMedium)
                {

                    item.SetValue(Grid.RowProperty, rowIndex);
                    item.SetValue(Grid.ColumnProperty, columnIndex);
                    item.SetValue(Grid.ColumnSpanProperty, 6);

                    columnIndex += 6;
                }

                if (sizeInfo.IsExtraSmall)
                {

                    item.SetValue(Grid.RowProperty, rowIndex);
                    item.SetValue(Grid.ColumnProperty, columnIndex);
                    item.SetValue(Grid.ColumnSpanProperty, 3);

                    columnIndex += 3;
                }

                if (sizeInfo.IsSmall)
                {

                    item.SetValue(Grid.RowProperty, rowIndex);
                    item.SetValue(Grid.ColumnProperty, columnIndex);
                    item.SetValue(Grid.ColumnSpanProperty, 4);

                    columnIndex += 4;
                }

                if ( isLast || columnIndex >= 12)
                {
                    grid.RowDefinitions.Add(new RowDefinition{Height = GridLength.Auto});
                    
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
                    grid.RowDefinitions.Add(new RowDefinition{Height = GridLength.Auto});
                    columnIndex = 0;
                    rowIndex++;
                }
            }


            
        }
        #endregion
    }
}