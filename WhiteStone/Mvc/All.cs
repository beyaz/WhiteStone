namespace WhiteStone.Mvc
{
    /// <summary>
    ///     Base class of controllers
    /// </summary>
    public class ControllerBase<TModel>
    {
        /// <summary>
        ///     The model.
        /// </summary>
        public TModel Model { get; set; }
    }
}