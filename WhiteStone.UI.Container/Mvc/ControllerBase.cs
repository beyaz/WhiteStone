namespace WhiteStone.UI.Container.Mvc
{
    public class ControllerBase<TModel> where TModel : ModelBase,new()
    {
        #region Public Properties
        public TModel Model { get; set; }
        #endregion

        #region Public Methods
        public virtual bool CanViewClose()
        {
            return true;
        }

        public virtual void OnViewClose()
        {
        }

        public virtual void OnViewLoaded()
        {
        }
        #endregion
    }
}