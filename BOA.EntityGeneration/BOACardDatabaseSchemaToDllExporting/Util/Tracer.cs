using System;

namespace BOA.EntityGeneration.BOACardDatabaseSchemaToDllExporting.Util
{
    [Serializable]
    public class ProcessInfo
    {
        #region Public Properties
        public int Current { get; set; }

        public int PercentageOfCompletion
        {
            get
            {
                if (Current == 0)
                {
                    return 0;
                }

                return (int) (Current / (double) Total * 100);
            }
        }

        public string Text  { get; set; }
        public int    Total { get; set; }
        #endregion
    }

    public class Tracer
    {
        #region Public Properties
        public ProcessInfo SchemaGenerationProcess { get; set; } = new ProcessInfo();

        public ProcessInfo AllSchemaGenerationProcess { get; set; } = new ProcessInfo();

        public ProcessInfo CustomSqlGenerationOfProfileIdProcess { get; set; } = new ProcessInfo();



        
        #endregion

        #region Public Methods
        public void Trace(string message)
        {
            Console.WriteLine(message);
        }
        #endregion
    }
}