﻿using System;
using System.Threading;
using BOA.EntityGeneration.BOACardCustomSqlIntoProjectInjection.Injectors;
using BOA.EntityGeneration.BOACardDatabaseSchemaToDllExporting.Exporters;
using BOA.TfsAccess;
using Ninject;

namespace CustomSqlInjectionToProject
{
    public static class Injection
    {
        #region Methods
        public static void Inject(string[] args)
        {
            if (args == null || args.Length == 0)
            {
                throw new ArgumentException(nameof(args));
            }

            using (var kernel = new Kernel())
            {
                kernel.Bind<FileAccess>().To<FileAccessWithAutoCheckIn>();
                kernel.Get<ProjectInjector>().Inject(args[0].Trim());
            }

            Thread.Sleep(3000);
        }
        #endregion
    }
}