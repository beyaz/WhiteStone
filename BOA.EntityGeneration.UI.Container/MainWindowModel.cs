using System;
using BOA.EntityGeneration.BOACardDatabaseSchemaToDllExporting.Util;

namespace BOA.EntityGeneration.UI.Container
{
    [Serializable]
    public class MainWindowModel
    {
        public EntityGenerationModel EntityGeneration { get; set; } = new EntityGenerationModel();

        public string SchemaGenerationProcessText { get; set; }
        public bool StartTimer { get; set; }
        public bool FinishTimer { get; set; }


        public string SelectedSchemaName { get; set; }

        public bool EntityGeneratingIsStarted { get; set; }

        public bool EntityGeneratingIsEnabled => !EntityGeneratingIsStarted;


    }

    [Serializable]
    public class EntityGenerationModel
    {
        public ProcessContract SecondaryProcess { get; set; } = new ProcessContract();
        public ProcessContract PrimaryProcess { get; set; } = new ProcessContract();

        public string SelectedSchemaName { get; set; }

        public bool IsEnabled { get; set; } = true;




    }


}