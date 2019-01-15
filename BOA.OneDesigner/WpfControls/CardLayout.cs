using System.Linq;
using System.Windows;
using System.Windows.Controls;
using BOA.OneDesigner.AppModel;
using BOA.OneDesigner.Helpers;
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
            var pushRow = false;

            var elements = grid.Children.ToArray();

            var length = elements.Length;

            for (var i = 0; i < length; i++)
            {
                var item        = elements[i];
                var isLast = i == length - 1;
               
                var nextItem = isLast ? null: elements[i+1];
                var nextItemLayoutProps = (nextItem as BCardWpf)?.Model?.LayoutProps;


                var layoutProps = (item as BCardWpf)?.Model?.LayoutProps;
                if (layoutProps == null)
                {
                    throw Error.InvalidOperation();
                }

                

                var span = layoutProps.Wide;

                item.SetValue(Grid.RowProperty, rowIndex);
                item.SetValue(Grid.ColumnProperty,  layoutProps.X);
                item.SetValue(Grid.ColumnSpanProperty, span);

                columnIndex += span;

                if ( isLast || columnIndex >= 12 )
                {
                    pushRow = true;
                }

                if (!pushRow)
                {
                    if (  nextItemLayoutProps != null)
                    {
                        if (nextItemLayoutProps.X ==0 || columnIndex + nextItemLayoutProps.Wide >12)
                        {
                            pushRow = true;
                        }
                    }    
                }
                

                if (pushRow)
                {
                    grid.RowDefinitions.Add(new RowDefinition {Height = GridLength.Auto});

                    columnIndex = 0;
                    rowIndex++;
                    pushRow = false;
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
                if (sizeInfo == null )
                {
                    sizeInfo = new SizeInfo{ IsLarge = true};
                    //throw Error.InvalidOperation();
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

            return  12;
        }
        #endregion
    }
}