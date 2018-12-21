using System.Windows.Forms;
using BOAPlugins.SearchProcedure;
using BOAPlugins.ViewClassDependency;
using Handler = BOAPlugins.SearchProcedure.Handler;

namespace BOAPlugins.VSIntegration
{
    /// <summary>
    ///     The communication
    /// </summary>
    public class Communication
    {
        #region Fields
        /// <summary>
        ///     The visual studio layer
        /// </summary>
        readonly IVisualStudioLayer _visualStudioLayer;
        #endregion

        #region Constructors
        /// <summary>
        ///     Initializes a new instance of the <see cref="Communication" /> class.
        /// </summary>
        public Communication(IVisualStudioLayer visualStudioLayer)
        {
            _visualStudioLayer = visualStudioLayer;
        }
        #endregion

        #region Public Methods
        /// <summary>
        ///     Sends the specified input.
        /// </summary>
        public void Send(Input input)
        {
            var handler = new Handler(input);

            handler.Handle();

            Process(handler.Result);
        }

      
        #endregion

        #region Methods
        /// <summary>
        ///     Processes the specified result.
        /// </summary>
        void Process(Result result)
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
    }
}