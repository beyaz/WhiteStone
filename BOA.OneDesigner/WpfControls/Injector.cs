using System;
using BOA.OneDesigner.AppModel;
using BOA.OneDesigner.JsxElementModel;

namespace BOA.OneDesigner.WpfControls
{
    static class Injector
    {
        public static BTabBarWpf CreateBTabBarWpf(this Host host, BTabBar dataContext)
        {
            if (host == null)
            {
                throw new ArgumentNullException(nameof(host));
            }

            if (dataContext == null)
            {
                throw new ArgumentNullException(nameof(dataContext));
            }

            var item = host.Create<BTabBarWpf>(dataContext);
            item.Refresh();
            return item;
        }
        

        public static BDataGridInfoWpf CreateBDataGridInfoWpf(this Host host, BDataGrid dataContext)
        {
            if (host == null)
            {
                throw new ArgumentNullException(nameof(host));
            }

            if (dataContext == null)
            {
                throw new ArgumentNullException(nameof(dataContext));
            }

            var item = host.Create<BDataGridInfoWpf>(dataContext);
            item.Refresh();
            return item;
        }
        

        #region Public Methods
        public static BCardWpf CreateBCardWpf(this Host host, BCard dataContext)
        {
            if (host == null)
            {
                throw new ArgumentNullException(nameof(host));
            }

            if (dataContext == null)
            {
                throw new ArgumentNullException(nameof(dataContext));
            }

            var item = host.Create<BCardWpf>(dataContext);
            item.Refresh();
            return item;
        }

        public static DivAsCardContainerWpf CreateDivAsCardContainerWpf(this Host host, DivAsCardContainer dataContext)
        {
            if (host == null)
            {
                throw new ArgumentNullException(nameof(host));
            }

            if (dataContext == null)
            {
                throw new ArgumentNullException(nameof(dataContext));
            }

            var item = host.Create<DivAsCardContainerWpf>(dataContext);
            item.Refresh();
            return item;
        }
        #endregion
    }
}