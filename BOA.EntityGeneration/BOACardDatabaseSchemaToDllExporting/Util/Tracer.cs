using System;

namespace BOA.EntityGeneration.BOACardDatabaseSchemaToDllExporting.Util
{
    public class Tracer
    {
        #region Public Properties
        public static string LastTrace { get; set; }
        #endregion

        #region Public Methods
        public void Trace(string message)
        {
            LastTrace = message;
            Console.WriteLine(message);
        }
        #endregion
    }
}