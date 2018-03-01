using System.IO;

namespace WhiteStone.Tasks
{
    public class ReplaceInFile : TaskBase
    {
        #region Properties
        string   Source         => GetKey(nameof(Source));
        string OldValue => GetKey(nameof(OldValue));
        string NewValue => GetKey(nameof(NewValue));
        
        #endregion

        #region Public Methods
        public override void Run()
        {
            var fileContent = File.ReadAllText(Source);

            fileContent = fileContent.Replace(OldValue, NewValue);

            File.WriteAllText(Source,fileContent);
        }
        #endregion
    }
}