using BOAPlugins.ViewClassDependency;

namespace BOAPlugins.VSIntegration
{
    public interface ICommunication
    {
        #region Public Methods

        /// <summary>
        ///     Sends the specified input.
        /// </summary>
        /// <param name="input">The input.</param>
        void Send(SearchProcedure.Input input);

    


        void Send(Data input);

        /// <summary>
        ///     Shows the property generator.
        /// </summary>
        void ShowPropertyGenerator();

        void ShowTranslateHelperForLabels(); // TODO remove  unused method
        #endregion
    }
}