using System.Diagnostics;
using System.IO;
using System.Threading;
using BOAPlugins.Utility;
using WhiteStone.IO;

namespace BOAPlugins.ViewClassDependency
{
    public class ViewTypeDependencyData
    {
        #region Public Properties
        public string CsprojFilePath { get; set; }
        public string ErrorMessage   { get; set; }
        public string GraphFilePath  { get; set; }
        #endregion
    }

    public static class ViewTypeDependency
    {
        #region Public Methods
        public static void Execute(ViewTypeDependencyData data)
        {
            var filePath = data.CsprojFilePath;
            if (filePath == null)
            {
                return;
            }

            var graphFilePath = filePath + ".dgml";

            var FS = new FileService();

            FS.TryDelete(graphFilePath);

            var arguments = string.Format(@"graph={1} source={0} {0}", filePath, graphFilePath);

            Process.Start(ConstConfiguration.BOAPluginDirectory_DeepEnds + "DeepEnds.Console.exe", arguments);

            var count = 0;
            // wait for process finih
            while (true)
            {
                if (!FS.Exists(graphFilePath))
                {
                    Thread.Sleep(300);
                    continue;
                }

                var fi = new FileInfo(graphFilePath);
                if (fi.Length > 0)
                {
                    DgmlHelper.SetDirectionLeftToRight(graphFilePath);
                    data.GraphFilePath = graphFilePath;

                    return;
                }

                Thread.Sleep(300);
                if (count++ > 50)
                {
                    data.ErrorMessage = "EvaluationTimeout";
                    return;
                }
            }
        }
        #endregion
    }
}