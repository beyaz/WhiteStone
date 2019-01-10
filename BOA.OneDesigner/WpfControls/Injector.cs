using BOA.OneDesigner.AppModel;
using BOA.OneDesigner.JsxElementModel;

namespace BOA.OneDesigner.WpfControls
{
    static class Injector
    {
        public static BCardWpf CreateBCardWpf(this Host host, BCard dataContext)
        {
            var item = host.Create<BCardWpf>(dataContext);
            item.Refresh();
            return item;
        }
    }
}