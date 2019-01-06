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
                var element = item as ISupportSizeInfo;
                if (element == null)
                {
                    continue;
                }

                var isLast = item == elements.Last();

                if (element.SizeInfo.IsLarge)
                {
                    row.Add(item);

                    item.SetValue(Grid.RowProperty, rowIndex);
                    item.SetValue(Grid.ColumnProperty, columnIndex);
                    item.SetValue(Grid.ColumnSpanProperty, 12);

                    pushRow = true;
                }

                if (element.SizeInfo.IsMedium)
                {
                    row.Add(item);

                    item.SetValue(Grid.RowProperty, rowIndex);
                    item.SetValue(Grid.ColumnProperty, columnIndex);
                    item.SetValue(Grid.ColumnSpanProperty, 6);

                    columnIndex += 6;
                }

                if (element.SizeInfo.IsExtraSmall)
                {
                    row.Add(item);

                    item.SetValue(Grid.RowProperty, rowIndex);
                    item.SetValue(Grid.ColumnProperty, columnIndex);
                    item.SetValue(Grid.ColumnSpanProperty, 3);

                    columnIndex += 3;
                }

                if (element.SizeInfo.IsSmall)
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
        #endregion
    }
}