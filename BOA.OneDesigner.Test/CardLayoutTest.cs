using System.Windows.Controls;
using BOA.OneDesigner.JsxElementModel;
using BOA.OneDesigner.WpfControls;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BOA.OneDesigner
{
    [TestClass]
    public class CardLayoutTest
    {
        #region Public Methods
        [TestMethod]
        public void App_should_reorder_elements_according_to_size_information()
        {
            var input_0 = new BInput
            {
                SizeInfo =
                {
                    IsLarge = true
                }
            };

            var grid = new Grid();

            grid.Children.Add(new BInputWpf {DataContext = input_0});

            // ACT
            CardLayout.Apply(grid);

            // Large
            grid.Children[0].GetValue(Grid.ColumnSpanProperty).Should().Be(12);

            grid.Children.Add(Create_Medium());
            grid.Children.Add(Create_Medium());

            // ACT
            CardLayout.Apply(grid);

            // Large
            grid.Children[0].GetValue(Grid.RowProperty).Should().Be(0);
            grid.Children[0].GetValue(Grid.ColumnSpanProperty).Should().Be(12);

            // Medium + Medium
            grid.Children[1].GetValue(Grid.RowProperty).Should().Be(1);
            grid.Children[1].GetValue(Grid.ColumnProperty).Should().Be(0);
            grid.Children[1].GetValue(Grid.ColumnSpanProperty).Should().Be(6);

            grid.Children[2].GetValue(Grid.RowProperty).Should().Be(1);
            grid.Children[2].GetValue(Grid.ColumnProperty).Should().Be(6);
            grid.Children[2].GetValue(Grid.ColumnSpanProperty).Should().Be(6);

            // Medium + ExtraSmall + ExtraSmall
            grid.Children.Add(Create_Medium());
            grid.Children.Add(Create_ExtraSmall());
            grid.Children.Add(Create_ExtraSmall());

            // ACT
            CardLayout.Apply(grid);

            // Large
            grid.Children[0].GetValue(Grid.RowProperty).Should().Be(0);
            grid.Children[0].GetValue(Grid.ColumnSpanProperty).Should().Be(12);

            // Medium + Medium
            grid.Children[1].GetValue(Grid.RowProperty).Should().Be(1);
            grid.Children[1].GetValue(Grid.ColumnProperty).Should().Be(0);
            grid.Children[1].GetValue(Grid.ColumnSpanProperty).Should().Be(6);

            grid.Children[2].GetValue(Grid.RowProperty).Should().Be(1);
            grid.Children[2].GetValue(Grid.ColumnProperty).Should().Be(6);
            grid.Children[2].GetValue(Grid.ColumnSpanProperty).Should().Be(6);

            // Medium + ExtraSmall + ExtraSmall
            grid.Children[3].GetValue(Grid.RowProperty).Should().Be(2);
            grid.Children[3].GetValue(Grid.ColumnProperty).Should().Be(0);
            grid.Children[3].GetValue(Grid.ColumnSpanProperty).Should().Be(6);

            grid.Children[4].GetValue(Grid.RowProperty).Should().Be(2);
            grid.Children[4].GetValue(Grid.ColumnProperty).Should().Be(6);
            grid.Children[4].GetValue(Grid.ColumnSpanProperty).Should().Be(3);

            grid.Children[5].GetValue(Grid.RowProperty).Should().Be(2);
            grid.Children[5].GetValue(Grid.ColumnProperty).Should().Be(9);
            grid.Children[5].GetValue(Grid.ColumnSpanProperty).Should().Be(3);

            // ExtraSmall + ExtraSmall + ExtraSmall + ExtraSmall
            grid.Children.Add(Create_ExtraSmall());
            grid.Children.Add(Create_ExtraSmall());
            grid.Children.Add(Create_ExtraSmall());
            grid.Children.Add(Create_ExtraSmall());

            // ACT
            CardLayout.Apply(grid);

            // Large
            grid.Children[0].GetValue(Grid.RowProperty).Should().Be(0);
            grid.Children[0].GetValue(Grid.ColumnSpanProperty).Should().Be(12);

            // Medium + Medium
            grid.Children[1].GetValue(Grid.RowProperty).Should().Be(1);
            grid.Children[1].GetValue(Grid.ColumnProperty).Should().Be(0);
            grid.Children[1].GetValue(Grid.ColumnSpanProperty).Should().Be(6);

            grid.Children[2].GetValue(Grid.RowProperty).Should().Be(1);
            grid.Children[2].GetValue(Grid.ColumnProperty).Should().Be(6);
            grid.Children[2].GetValue(Grid.ColumnSpanProperty).Should().Be(6);

            // Medium + ExtraSmall + ExtraSmall
            grid.Children[3].GetValue(Grid.RowProperty).Should().Be(2);
            grid.Children[3].GetValue(Grid.ColumnProperty).Should().Be(0);
            grid.Children[3].GetValue(Grid.ColumnSpanProperty).Should().Be(6);

            grid.Children[4].GetValue(Grid.RowProperty).Should().Be(2);
            grid.Children[4].GetValue(Grid.ColumnProperty).Should().Be(6);
            grid.Children[4].GetValue(Grid.ColumnSpanProperty).Should().Be(3);

            grid.Children[5].GetValue(Grid.RowProperty).Should().Be(2);
            grid.Children[5].GetValue(Grid.ColumnProperty).Should().Be(9);
            grid.Children[5].GetValue(Grid.ColumnSpanProperty).Should().Be(3);

            // ExtraSmall + ExtraSmall + ExtraSmall + ExtraSmall
            grid.Children[6].GetValue(Grid.RowProperty).Should().Be(3);
            grid.Children[6].GetValue(Grid.ColumnProperty).Should().Be(0);
            grid.Children[6].GetValue(Grid.ColumnSpanProperty).Should().Be(3);

            grid.Children[7].GetValue(Grid.RowProperty).Should().Be(3);
            grid.Children[7].GetValue(Grid.ColumnProperty).Should().Be(3);
            grid.Children[7].GetValue(Grid.ColumnSpanProperty).Should().Be(3);

            grid.Children[8].GetValue(Grid.RowProperty).Should().Be(3);
            grid.Children[8].GetValue(Grid.ColumnProperty).Should().Be(6);
            grid.Children[8].GetValue(Grid.ColumnSpanProperty).Should().Be(3);

            grid.Children[9].GetValue(Grid.RowProperty).Should().Be(3);
            grid.Children[9].GetValue(Grid.ColumnProperty).Should().Be(9);
            grid.Children[9].GetValue(Grid.ColumnSpanProperty).Should().Be(3);

            // Small + Small + Small
            grid.Children.Add(Create_Small());
            grid.Children.Add(Create_Small());
            grid.Children.Add(Create_Small());

            // ACT
            CardLayout.Apply(grid);
            grid.Children.Count.Should().Be(13);

            // Large
            grid.Children[0].GetValue(Grid.RowProperty).Should().Be(0);
            grid.Children[0].GetValue(Grid.ColumnSpanProperty).Should().Be(12);

            // Medium + Medium
            grid.Children[1].GetValue(Grid.RowProperty).Should().Be(1);
            grid.Children[1].GetValue(Grid.ColumnProperty).Should().Be(0);
            grid.Children[1].GetValue(Grid.ColumnSpanProperty).Should().Be(6);

            grid.Children[2].GetValue(Grid.RowProperty).Should().Be(1);
            grid.Children[2].GetValue(Grid.ColumnProperty).Should().Be(6);
            grid.Children[2].GetValue(Grid.ColumnSpanProperty).Should().Be(6);

            // Medium + ExtraSmall + ExtraSmall
            grid.Children[3].GetValue(Grid.RowProperty).Should().Be(2);
            grid.Children[3].GetValue(Grid.ColumnProperty).Should().Be(0);
            grid.Children[3].GetValue(Grid.ColumnSpanProperty).Should().Be(6);

            grid.Children[4].GetValue(Grid.RowProperty).Should().Be(2);
            grid.Children[4].GetValue(Grid.ColumnProperty).Should().Be(6);
            grid.Children[4].GetValue(Grid.ColumnSpanProperty).Should().Be(3);

            grid.Children[5].GetValue(Grid.RowProperty).Should().Be(2);
            grid.Children[5].GetValue(Grid.ColumnProperty).Should().Be(9);
            grid.Children[5].GetValue(Grid.ColumnSpanProperty).Should().Be(3);

            // ExtraSmall + ExtraSmall + ExtraSmall + ExtraSmall
            grid.Children[6].GetValue(Grid.RowProperty).Should().Be(3);
            grid.Children[6].GetValue(Grid.ColumnProperty).Should().Be(0);
            grid.Children[6].GetValue(Grid.ColumnSpanProperty).Should().Be(3);

            grid.Children[7].GetValue(Grid.RowProperty).Should().Be(3);
            grid.Children[7].GetValue(Grid.ColumnProperty).Should().Be(3);
            grid.Children[7].GetValue(Grid.ColumnSpanProperty).Should().Be(3);

            grid.Children[8].GetValue(Grid.RowProperty).Should().Be(3);
            grid.Children[8].GetValue(Grid.ColumnProperty).Should().Be(6);
            grid.Children[8].GetValue(Grid.ColumnSpanProperty).Should().Be(3);

            grid.Children[9].GetValue(Grid.RowProperty).Should().Be(3);
            grid.Children[9].GetValue(Grid.ColumnProperty).Should().Be(9);
            grid.Children[9].GetValue(Grid.ColumnSpanProperty).Should().Be(3);

            // Small + Small + Small
            grid.Children[10].GetValue(Grid.RowProperty).Should().Be(4);
            grid.Children[10].GetValue(Grid.ColumnProperty).Should().Be(0);
            grid.Children[10].GetValue(Grid.ColumnSpanProperty).Should().Be(4);

            grid.Children[11].GetValue(Grid.RowProperty).Should().Be(4);
            grid.Children[11].GetValue(Grid.ColumnProperty).Should().Be(4);
            grid.Children[11].GetValue(Grid.ColumnSpanProperty).Should().Be(4);

            grid.Children[12].GetValue(Grid.RowProperty).Should().Be(4);
            grid.Children[12].GetValue(Grid.ColumnProperty).Should().Be(8);
            grid.Children[12].GetValue(Grid.ColumnSpanProperty).Should().Be(4);
        }

        [TestMethod]
        public void ApplyForCardsContainer_should_reorder_elements_according_to_layoutProps_information()
        {
            var grid = new Grid();

            // 4-4-4
            grid.Children.Add(CreateCard(4, 0));
            grid.Children.Add(CreateCard(4, 4));
            grid.Children.Add(CreateCard(4, 8));

            // ACT
            CardLayout.ApplyForCardsContainer(grid);

            // 4-4-4
            grid.Children[0].GetValue(Grid.RowProperty).Should().Be(0);
            grid.Children[0].GetValue(Grid.ColumnProperty).Should().Be(0);
            grid.Children[0].GetValue(Grid.ColumnSpanProperty).Should().Be(4);

            grid.Children[1].GetValue(Grid.RowProperty).Should().Be(0);
            grid.Children[1].GetValue(Grid.ColumnProperty).Should().Be(4);
            grid.Children[1].GetValue(Grid.ColumnSpanProperty).Should().Be(4);

            grid.Children[2].GetValue(Grid.RowProperty).Should().Be(0);
            grid.Children[2].GetValue(Grid.ColumnProperty).Should().Be(8);
            grid.Children[2].GetValue(Grid.ColumnSpanProperty).Should().Be(4);

            // 3-3-3-3
            grid.Children.Add(CreateCard(3, 0));
            grid.Children.Add(CreateCard(3, 3));
            grid.Children.Add(CreateCard(3, 6));
            grid.Children.Add(CreateCard(3, 9));

            // ACT
            CardLayout.ApplyForCardsContainer(grid);

            // 4-4-4
            grid.Children[0].GetValue(Grid.RowProperty).Should().Be(0);
            grid.Children[0].GetValue(Grid.ColumnProperty).Should().Be(0);
            grid.Children[0].GetValue(Grid.ColumnSpanProperty).Should().Be(4);

            grid.Children[1].GetValue(Grid.RowProperty).Should().Be(0);
            grid.Children[1].GetValue(Grid.ColumnProperty).Should().Be(4);
            grid.Children[1].GetValue(Grid.ColumnSpanProperty).Should().Be(4);

            grid.Children[2].GetValue(Grid.RowProperty).Should().Be(0);
            grid.Children[2].GetValue(Grid.ColumnProperty).Should().Be(8);
            grid.Children[2].GetValue(Grid.ColumnSpanProperty).Should().Be(4);

            // 3-3-3-3
            grid.Children[3].GetValue(Grid.RowProperty).Should().Be(1);
            grid.Children[3].GetValue(Grid.ColumnProperty).Should().Be(0);
            grid.Children[3].GetValue(Grid.ColumnSpanProperty).Should().Be(3);

            grid.Children[4].GetValue(Grid.RowProperty).Should().Be(1);
            grid.Children[4].GetValue(Grid.ColumnProperty).Should().Be(3);
            grid.Children[4].GetValue(Grid.ColumnSpanProperty).Should().Be(3);

            grid.Children[5].GetValue(Grid.RowProperty).Should().Be(1);
            grid.Children[5].GetValue(Grid.ColumnProperty).Should().Be(6);
            grid.Children[5].GetValue(Grid.ColumnSpanProperty).Should().Be(3);

            grid.Children[6].GetValue(Grid.RowProperty).Should().Be(1);
            grid.Children[6].GetValue(Grid.ColumnProperty).Should().Be(9);
            grid.Children[6].GetValue(Grid.ColumnSpanProperty).Should().Be(3);

            // w:7,x:5
            grid.Children.Add(CreateCard(7, 5));

            // ACT
            CardLayout.ApplyForCardsContainer(grid);

            // 4-4-4
            grid.Children[0].GetValue(Grid.RowProperty).Should().Be(0);
            grid.Children[0].GetValue(Grid.ColumnProperty).Should().Be(0);
            grid.Children[0].GetValue(Grid.ColumnSpanProperty).Should().Be(4);

            grid.Children[1].GetValue(Grid.RowProperty).Should().Be(0);
            grid.Children[1].GetValue(Grid.ColumnProperty).Should().Be(4);
            grid.Children[1].GetValue(Grid.ColumnSpanProperty).Should().Be(4);

            grid.Children[2].GetValue(Grid.RowProperty).Should().Be(0);
            grid.Children[2].GetValue(Grid.ColumnProperty).Should().Be(8);
            grid.Children[2].GetValue(Grid.ColumnSpanProperty).Should().Be(4);

            // 3-3-3-3
            grid.Children[3].GetValue(Grid.RowProperty).Should().Be(1);
            grid.Children[3].GetValue(Grid.ColumnProperty).Should().Be(0);
            grid.Children[3].GetValue(Grid.ColumnSpanProperty).Should().Be(3);

            grid.Children[4].GetValue(Grid.RowProperty).Should().Be(1);
            grid.Children[4].GetValue(Grid.ColumnProperty).Should().Be(3);
            grid.Children[4].GetValue(Grid.ColumnSpanProperty).Should().Be(3);

            grid.Children[5].GetValue(Grid.RowProperty).Should().Be(1);
            grid.Children[5].GetValue(Grid.ColumnProperty).Should().Be(6);
            grid.Children[5].GetValue(Grid.ColumnSpanProperty).Should().Be(3);

            grid.Children[6].GetValue(Grid.RowProperty).Should().Be(1);
            grid.Children[6].GetValue(Grid.ColumnProperty).Should().Be(9);
            grid.Children[6].GetValue(Grid.ColumnSpanProperty).Should().Be(3);

            // w:7,x:5
            grid.Children[7].GetValue(Grid.RowProperty).Should().Be(2);
            grid.Children[7].GetValue(Grid.ColumnProperty).Should().Be(5);
            grid.Children[7].GetValue(Grid.ColumnSpanProperty).Should().Be(7);
        }

        [TestMethod]
        public void ApplyWithDropLocationMode_should_reorder_elements_every_element_as_small()
        {
            var grid = new Grid();

            // row 0
            grid.Children.Add(Create_ExtraSmall());
            grid.Children.Add(Create_ExtraSmall());
            grid.Children.Add(Create_ExtraSmall());

            // row 1
            grid.Children.Add(Create_ExtraSmall());
            grid.Children.Add(Create_ExtraSmall());
            grid.Children.Add(Create_ExtraSmall());

            // ACT
            CardLayout.ApplyWithDropLocationMode(grid);

            // row 0
            grid.Children[0].GetValue(Grid.ColumnSpanProperty).Should().Be(4);
            grid.Children[1].GetValue(Grid.ColumnSpanProperty).Should().Be(4);
            grid.Children[2].GetValue(Grid.ColumnSpanProperty).Should().Be(4);

            grid.Children[0].GetValue(Grid.ColumnProperty).Should().Be(0);
            grid.Children[1].GetValue(Grid.ColumnProperty).Should().Be(4);
            grid.Children[2].GetValue(Grid.ColumnProperty).Should().Be(8);

            grid.Children[0].GetValue(Grid.RowProperty).Should().Be(0);
            grid.Children[1].GetValue(Grid.RowProperty).Should().Be(0);
            grid.Children[2].GetValue(Grid.RowProperty).Should().Be(0);

            // row 1
            grid.Children[3].GetValue(Grid.ColumnSpanProperty).Should().Be(4);
            grid.Children[4].GetValue(Grid.ColumnSpanProperty).Should().Be(4);
            grid.Children[5].GetValue(Grid.ColumnSpanProperty).Should().Be(4);

            grid.Children[3].GetValue(Grid.ColumnProperty).Should().Be(0);
            grid.Children[4].GetValue(Grid.ColumnProperty).Should().Be(4);
            grid.Children[5].GetValue(Grid.ColumnProperty).Should().Be(8);

            grid.Children[3].GetValue(Grid.RowProperty).Should().Be(1);
            grid.Children[4].GetValue(Grid.RowProperty).Should().Be(1);
            grid.Children[5].GetValue(Grid.RowProperty).Should().Be(1);
        }
        #endregion

        #region Methods
        static BInputWpf Create_ExtraSmall()
        {
            var bInput = new BInput
            {
                SizeInfo =
                {
                    IsMedium = true
                }
            };

            bInput.SizeInfo = new SizeInfo {IsExtraSmall = true};

            return new BInputWpf {DataContext = bInput};
        }

        static BInputWpf Create_Medium()
        {
            var bInput = new BInput
            {
                SizeInfo =
                {
                    IsMedium = true
                }
            };

            bInput.SizeInfo = new SizeInfo {IsMedium = true};

            return new BInputWpf {DataContext = bInput};
        }

        static BInputWpf Create_Small()
        {
            var bInput = new BInput
            {
                SizeInfo =
                {
                    IsMedium = true
                }
            };

            bInput.SizeInfo = new SizeInfo {IsSmall = true};

            return new BInputWpf {DataContext = bInput};
        }

        static BCardWpf CreateCard(int wide, int x)
        {
            return new BCardWpf {DataContext = new BCard {LayoutProps = new LayoutProps {Wide = wide, X = x}}};
        }
        #endregion
    }
}