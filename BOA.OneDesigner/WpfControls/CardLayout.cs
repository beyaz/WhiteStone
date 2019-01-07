using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using BOA.OneDesigner.JsxElementModel;

namespace BOA.OneDesigner.WpfControls
{
    static class CardLayout
    {
        #region Public Methods
        public static void Apply(SizeInfo cardSizeInfo, Grid grid)
        {
            WpfHelper.Apply12Column(grid);

            var row         = new List<UIElement>();
            var rowIndex    = 0;
            var columnIndex = 0;
            var pushRow     = false;

            var elements = grid.Children.ToArray();

            grid.Children.Clear();

            foreach (var item in elements)
            {
                var sizeInfo = (item as ISupportSizeInfo)?.SizeInfo;
                if (sizeInfo == null)
                {
                    sizeInfo = cardSizeInfo;
                }

                var isLast = item == elements.Last();

                if (sizeInfo.IsLarge)
                {
                    row.Add(item);

                    item.SetValue(Grid.RowProperty, rowIndex);
                    item.SetValue(Grid.ColumnProperty, columnIndex);
                    item.SetValue(Grid.ColumnSpanProperty, 12);

                    pushRow = true;
                }

                if (sizeInfo.IsMedium)
                {
                    row.Add(item);

                    item.SetValue(Grid.RowProperty, rowIndex);
                    item.SetValue(Grid.ColumnProperty, columnIndex);
                    item.SetValue(Grid.ColumnSpanProperty, 6);

                    columnIndex += 6;
                }

                if (sizeInfo.IsExtraSmall)
                {
                    row.Add(item);

                    item.SetValue(Grid.RowProperty, rowIndex);
                    item.SetValue(Grid.ColumnProperty, columnIndex);
                    item.SetValue(Grid.ColumnSpanProperty, 3);

                    columnIndex += 3;
                }

                if (sizeInfo.IsSmall)
                {
                    row.Add(item);

                    item.SetValue(Grid.RowProperty, rowIndex);
                    item.SetValue(Grid.ColumnProperty, columnIndex);
                    item.SetValue(Grid.ColumnSpanProperty, 4);

                    columnIndex += 4;
                }

                if (pushRow || isLast || columnIndex >= 12)
                {
                    pushRow = false;

                    foreach (var uiElement in row)
                    {
                        grid.Children.Add(uiElement);
                    }

                    columnIndex = 0;
                    rowIndex++;
                    row.Clear();
                }
            }
        }

        public static void ApplyWithDropLocationMode(Grid grid)
        {
            WpfHelper.Apply12Column(grid);

            var row         = new List<UIElement>();
            var rowIndex    = 0;
            var columnIndex = 0;

            var elements = grid.Children.ToArray();

            grid.Children.Clear();

            foreach (var item in elements)
            {
                var isLast = item == elements.Last();

                row.Add(item);

                item.SetValue(Grid.RowProperty, rowIndex);
                item.SetValue(Grid.ColumnProperty, columnIndex);
                item.SetValue(Grid.ColumnSpanProperty, 4);

                columnIndex += 4;

                if (isLast || columnIndex >= 12)
                {
                    foreach (var uiElement in row)
                    {
                        grid.Children.Add(uiElement);
                    }

                    columnIndex = 0;
                    rowIndex++;
                    row.Clear();
                }
            }
        }
        #endregion
    }
}