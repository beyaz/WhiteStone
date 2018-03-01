namespace WhiteStone.Configuration
{
    /// <summary>
    ///     Defines a methods for accessing config reading operations.
    /// </summary>
    public interface IConfigReader
    {
        /// <summary>
        ///     Gets config value by given key.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        string this[string key] { get; }
    }
}