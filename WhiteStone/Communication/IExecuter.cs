namespace WhiteStone.Communication
{
    /// <summary>
    ///     Defines a mechanism for Executing request
    /// </summary>
    public interface IExecuter
    {
        /// <summary>
        ///     Gets response of request.
        /// </summary>
        TResponse Execute<TRequest, TResponse>(TRequest request)
            where TRequest : RequestBase
            where TResponse : ResponseBase;
    }
}