using System;
using WhiteStone.UI.Container;

namespace CustomSqlInjectionToProject
{
    public static class Injection
    {
        #region Static Fields
        public static string ProfileId;
        #endregion

        #region Public Methods
        public static void Inject(string[] args)
        {
            if (args == null || args.Length == 0)
            {
                throw new ArgumentException(nameof(args));
            }

            ProfileId = args[0];

            App.Main();
        }
        #endregion
    }
}