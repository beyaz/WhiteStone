﻿using System;
using System.ComponentModel.Design;
using BOA.CodeGeneration.Util;
using BOA.Common.Helpers;
using BOA.TfsAccess;
using BOAPlugins.SearchProcedure;
using BOAPlugins.Utility;
using BOAPlugins.VSIntegration;
using BOAPlugins.VSIntegration.MainForm;
using Microsoft.VisualStudio.Shell;

namespace BOASpSearch
{
    /// <summary>
    ///     Command handler
    /// </summary>
    internal sealed partial class Command1
    {
        #region Constants
        /// <summary>
        ///     Command ID.
        /// </summary>
        public const int CommandId = 0x0100;

        public const int CommandId12 = 12;
        public const int CommandId13 = 13;

        #endregion

        #region Static Fields
        /// <summary>
        ///     Command menu group (command set GUID).
        /// </summary>
        public static readonly Guid CommandSet = new Guid("02e7ce5d-c034-4f2b-b45a-c89807ed1c77");
        #endregion

        #region Fields
        /// <summary>
        ///     VS Package that provides this command, not null.
        /// </summary>
        readonly Package package;
        #endregion

        #region Constructors
        /// <summary>
        ///     Initializes a new instance of the <see cref="Command1" /> class.
        ///     Adds our command handlers for menu (commands must exist in the command table file)
        /// </summary>
        /// <param name="package">Owner package, not null.</param>
        Command1(Package package)
        {
            if (package == null)
            {
                throw new ArgumentNullException(nameof(package));
            }

            this.package = package;

            var commandService = ServiceProvider.GetService(typeof(IMenuCommandService)) as OleMenuCommandService;
            if (commandService == null)
            {
                Log.Push("commandService is null.");
                return;
            }

            commandService.AddCommand(new MenuCommand(SearchProcedure, new CommandID(CommandSet, CommandId)));
            commandService.AddCommand(new MenuCommand(DocumentFile, new CommandID(CommandSet, CommandId12)));
            commandService.AddCommand(new MenuCommand(OpenMainForm, new CommandID(CommandSet, CommandId13)));

            Factory.InitializeApplicationServices();
        }
        #endregion

        #region Public Properties
        /// <summary>
        ///     Gets the instance of the command.
        /// </summary>
        public static Command1 Instance { get; private set; }
        #endregion

        #region Properties
        static Configuration Configuration => SM.Get<Configuration>();
        Communication        Communication => Factory.GetCommunication(VisualStudio);

        /// <summary>
        ///     Gets the service provider from the owner package.
        /// </summary>
        IServiceProvider ServiceProvider => package;

        TFSAccessForBOA TFSAccessForBOA => new TFSAccessForBOA();

        IVisualStudioLayer VisualStudio => new VisualStudioLayer(ServiceProvider);
        #endregion

        #region Public Methods
        /// <summary>
        ///     Initializes the singleton instance of the command.
        /// </summary>
        /// <param name="package">Owner package, not null.</param>
        public static void Initialize(Package package)
        {
            Instance = new Command1(package);
        }
        #endregion

        #region Methods
        void OpenMainForm(object sender, EventArgs e)
        {
            try
            {
                SM.Set(VisualStudio);

                var mainForm = new View();

                mainForm.ShowDialog();
            }
            catch (Exception exception)
            {
                Log.Push(exception);
            }
        }

        void SearchProcedure(object sender, EventArgs e)
        {
            var selectedText = VisualStudio.CursorSelectedText;
            if (selectedText == null)
            {
                return;
            }

            var input = new Input
            {
                ProcedureName = selectedText
            };

            Communication.Send(input);
        }

        
        #endregion
    }



}