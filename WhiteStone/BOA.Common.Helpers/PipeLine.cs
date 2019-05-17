using System;

namespace BOA.Common.Helpers
{
    /// <summary>
    ///     The pipe line
    /// </summary>
    public static class PipeLine
    {
        #region Public Methods
        /// <summary>
        ///     Runs the specified parameter.
        /// </summary>
        public static void Run<TParameter>(TParameter parameter, params Action<TParameter>[] methods)
        {
            foreach (var method in methods)
            {
                method(parameter);
            }
        }
        #endregion
    }
}