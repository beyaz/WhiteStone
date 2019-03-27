using System.Collections.Generic;
using BOA.OneDesigner.AppModel;
using BOA.OneDesigner.JsxElementModel;

namespace BOA.OneDesigner.WpfControls
{
    static class TestData
    {
        #region Public Methods
        public static BCard CreateBCardWithTwoInput()
        {
            var bCard = new BCard
            {
                Items = new List<BField>
                {
                    new ComponentInfo
                    {
                        Type             = new ComponentType {IsInput = true},
                        ValueBindingPath = "A"
                    },
                    new ComponentInfo
                    {
                        Type             = new ComponentType {IsInput = true},
                        ValueBindingPath = "B"
                    }
                }
            };
            return bCard;
        }

        public static BTabBarWpf CreateBTabBarWpfWithTwoTab(this Host host)
        {
            var bTabBar = new BTabBar
            {
                Items = new List<BTabBarPage>
                {
                    new BTabBarPage
                    {
                        DivAsCardContainer = CreateDivAsCardContainer()
                    },
                    new BTabBarPage
                    {
                        DivAsCardContainer = CreateDivAsCardContainer()
                    }
                }
            };

            return host.CreateBTabBarWpf(bTabBar);
        }

        public static DivAsCardContainer CreateDivAsCardContainer()
        {
            return new DivAsCardContainer
            {
                Items = new List<BCard>
                {
                    CreateBCardWithTwoInput(),
                    CreateBCardWithTwoInput()
                }
            };
        }
        #endregion
    }
}