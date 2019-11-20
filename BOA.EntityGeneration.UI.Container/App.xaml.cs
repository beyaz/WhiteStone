using System.Collections.Generic;
using System.Windows;
using BOA.DataFlow;

namespace BOA.EntityGeneration.UI.Container
{

    static class Data
    {
        public static IDataConstant<List<string>> SchemaNames = DataConstant.Create<List<string>>(nameof(SchemaNames));
    }

    public partial class App 
    {
        public static IDataContext Context = new DataContextCreator().Create();
    }
}
