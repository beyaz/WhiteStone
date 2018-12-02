using System;
using System.Windows.Forms;
using BOAPlugins.ViewClassDependency;

namespace BOAPlugins.VSIntegration
{
    /// <summary>
    ///     .
    /// </summary>
    public class Communication : ICommunication
    {
        #region Fields
        #region Field
        readonly IVisualStudioLayer _visualStudioLayer;
        #endregion
        #endregion

        #region Constructors
        #region Constructor
        public Communication(IVisualStudioLayer visualStudioLayer)
        {
            _visualStudioLayer = visualStudioLayer;
        }
        #endregion
        #endregion

        #region Public Methods

        /// <summary>
        ///     Sends the specified input.
        /// </summary>
        /// <param name="input">The input.</param>
        public void Send(SearchProcedure.Input input)
        {
            var handler = new SearchProcedure.Handler(input);

            handler.Handle();

            Process(handler.Result);
        }

        public void Send(Data input)
        {
            var result = new ViewClassDependency.Handler().Handle(input);

            if (result.ErrorMessage != null)
            {
                MessageBox.Show(result.ErrorMessage);
                return;
            }

            _visualStudioLayer.OpenFile(input.OutputFileFullPath);
        }

        #region ShowPropertyGenerator
        /// <summary>
        ///     Shows the property generator.
        /// </summary>
        public void ShowPropertyGenerator()
        {
            System.Diagnostics.Process.Start(ContainerHelper, typeof(View).FullName);
        }
        #endregion
        #endregion

        #region Methods


        /// <summary>
        ///     Processes the specified result.
        /// </summary>
        /// <param name="result">The result.</param>
        void Process(SearchProcedure.Result result)
        {
            if (result.ErrorMessage != null)
            {
                MessageBox.Show(result.ErrorMessage);
                return;
            }

            foreach (var sqlFileInfo in result.SqlFileList)
            {
                _visualStudioLayer.CreateNewSQLFile(sqlFileInfo.Content, sqlFileInfo.FileName);
            }
        }
        #endregion

        #region ShowTranslateHelperForLabels
        static string ContainerHelper => ConstConfiguration.PluginDirectory + "UI.ContainerHelper.exe";

        public void ShowTranslateHelperForLabels()
        {
            System.Diagnostics.Process.Start(ContainerHelper, typeof(BOA.Tools.Translator.UI.TranslateHelper.View).FullName);
        }
        #endregion
    }
}