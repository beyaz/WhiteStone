namespace BOA.OneDesigner.PropertyEditors
{
    class WideEditor : WideEditorBase
    {
      

        #region Constructors
        public WideEditor():base(2,12)
        {
           
        }
        #endregion

        #region Methods
        

        protected override void UpdateLabel()
        {
            _textBlock.Text = "Wide: " + Value;
        }
        #endregion

        
    }
}