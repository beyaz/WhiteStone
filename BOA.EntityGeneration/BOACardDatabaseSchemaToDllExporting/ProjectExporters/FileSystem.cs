using BOA.TfsAccess;
using Ninject;

namespace BOA.EntityGeneration.BOACardDatabaseSchemaToDllExporting.ProjectExporters
{
    public class FileSystem
    {
        [Inject]
        public Config Config { get; set; }
        [Inject]
        public FileAccess FileAccess { get; set; }

        public void WriteAllText(string path, string content)
        {
            if (!Config.IntegrateWithBOATfs)
            {
                FileAccess.WriteToFileSystem(path,content,new FileAccessWriteResult());
                return;
            }

            FileAccess.WriteAllText(path,content);
        }
    }
}