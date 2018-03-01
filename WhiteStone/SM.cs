using WhiteStone.Services;

namespace WhiteStone
{
    /// <summary>
    ///     Defines the Service Manager.
    /// </summary>
    public static class SM
    {
        /// <summary>
        ///     Gets  instance of T
        /// </summary>
        public static T Get<T>()
        {
            return ServiceManager.Instance.GetService<T>();
        }

        /// <summary>
        ///     Sets the specified Service..
        /// </summary>
        public static T Set<T>(T value)
        {
            ServiceManager.Instance.SetService(value);
            return value;
        }
    }
}