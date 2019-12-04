using BOA.EntityGeneration.UI.Container.Bootstrapper.Infrastructure;

namespace BOA.EntityGeneration.UI.Container.Bootstrapper
{
    class Program
    {
      


        #region Public Methods
        public static void Main(string[] args)
        {
           ModuleLoader.Load();

           new Launcher().Start();
            
        }
        #endregion

        
    }
}