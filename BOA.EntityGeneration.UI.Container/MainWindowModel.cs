using System;

namespace BOA.EntityGeneration.UI.Container
{
    [Serializable]
    public class MainWindowModel
    {
        public string SchemaGenerationProcessText { get; set; }
        public bool StartTimer { get; set; }
        public bool FinishTimer { get; set; }
    }
}