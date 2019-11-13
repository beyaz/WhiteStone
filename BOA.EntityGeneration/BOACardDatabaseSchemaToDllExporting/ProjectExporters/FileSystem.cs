using BOA.TfsAccess;

namespace BOA.EntityGeneration.BOACardDatabaseSchemaToDllExporting.ProjectExporters
{
    public class FileSystem
    {
        public Config Config { get; set; }

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