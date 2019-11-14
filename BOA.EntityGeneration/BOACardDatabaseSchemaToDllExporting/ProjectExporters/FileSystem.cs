using BOA.TfsAccess;

namespace BOA.EntityGeneration.BOACardDatabaseSchemaToDllExporting.ProjectExporters
{
    public class FileSystem
    {
        public ConfigContract Config { get; set; }

        public FileAccess FileAccess { get; set; }

        public void WriteAllText(string path, string content)
        {
            if (!Config.IntegrateWithBOATfs)
            {
                var result = new FileAccessWriteResult();

                FileAccess.WriteToFileSystem(path,content,result);
                if (result.Exception != null)
                {
                    throw result.Exception;
                }
                return;
            }

            FileAccess.WriteAllText(path,content);
        }
    }
}