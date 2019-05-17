using System;
using System.Collections.Generic;
using BOA.Common.Helpers;

namespace CustomSqlInjectionToProject
{
    
    public class Program
    {
        #region Methods
        static void Main(string[] args)
        {
           

            Injection.Inject(args);
            // Publisher.Publish(new PublisherData());

            
        }
        #endregion
    }
}