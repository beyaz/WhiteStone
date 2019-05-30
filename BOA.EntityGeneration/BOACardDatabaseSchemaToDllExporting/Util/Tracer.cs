using System;

namespace BOA.EntityGeneration.BOACardDatabaseSchemaToDllExporting.Util
{
    public class ProcessInfo
    {
        #region Public Properties
        public int    PercentageOfCompletion { get; set; }
        public string Text                   { get; set; }
        #endregion
    }

    public class Tracer
    {
        #region Public Properties
        public  ProcessInfo CurrentSchemaProcess { get; set; } = new ProcessInfo();

        public  ProcessInfo GenerateAllSchemaProcess { get; set; } = new ProcessInfo();
        #endregion

        #region Public Methods
        public void Trace(string message)
        {
            Console.WriteLine(message);
        }
        #endregion
    }
}