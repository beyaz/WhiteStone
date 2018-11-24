using System;
using System.IO;

namespace BOAPlugins.HideSuccessCheck
{
    public static class Log
    {
        #region Properties
        static string FilePath => Path.GetDirectoryName(typeof(Log).Assembly.Location) + Path.DirectorySeparatorChar + "Log.txt";
        #endregion

        #region Public Methods
        public static void Push(Exception exception)
        {
            try
            {
                var fs = new FileStream(FilePath, FileMode.Append);

                var sw = new StreamWriter(fs);
                sw.Write(exception.ToString());
                sw.Write(Environment.NewLine);
                sw.Close();
                fs.Close();
            }
            catch
            {
                // ignored
            }
        }
        #endregion
    }
}