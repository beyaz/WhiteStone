using System.Collections.Generic;
using BOA.OneDesigner.AppModel;
using BOA.OneDesigner.JsxElementModel;

namespace BOA.OneDesigner.WpfControls
{
    static class Extensions
    {



        public  static BTabBarWpf CreateAndLoadBTabBarWpfWithTwoTab( this Host host)
        {

            var bTabBar = new BTabBar
            {
                Items = new List<BTabBarPage> {new BTabBarPage(), new BTabBarPage()}
            };

            var tabBarWpf = host.Create<BTabBarWpf>(bTabBar);
            
            tabBarWpf.RaiseLoadedEvent();

            return tabBarWpf;
        }



        public  static BDataGridInfoWpf CreateAndLoadDataGridWpfWithTwoColumn( this Host host)
        {

            var dataGrid = new BDataGrid
            {
                Columns = new List<BDataGridColumnInfo> {new BDataGridColumnInfo(), new BDataGridColumnInfo()}
            };

            var dataGridInfoWpf = host.Create<BDataGridInfoWpf>(dataGrid);
            
            dataGridInfoWpf.RaiseLoadedEvent();

            return dataGridInfoWpf;
        }

        public  static BDataGridInfoWpf CreateDataGridWpfWithTwoColumn( this Host host)
        {

            var dataGrid = new BDataGrid
            {
                Columns = new List<BDataGridColumnInfo> {new BDataGridColumnInfo(), new BDataGridColumnInfo()}
            };

            var dataGridInfoWpf = host.Create<BDataGridInfoWpf>(dataGrid);
            

            return dataGridInfoWpf;
        }
    }
}