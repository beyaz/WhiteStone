using System;
using BOA.Common.Helpers;

namespace CustomSqlInjectionToProject
{
    public class Program
    {
        #region Methods
        [STAThread]
        static void Main(string[] args)
        {
            Log.Push(args, nameof(args));

            // TODO  remove
            args = new[] {"CC_OPERATIONS"};

            Injection.Inject(args);
        }
        #endregion
    }
}