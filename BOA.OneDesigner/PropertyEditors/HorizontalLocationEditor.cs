namespace BOA.OneDesigner.PropertyEditors
{
    class HorizontalLocationEditor:WideEditorBase
    {
        protected override void UpdateLabel()
        {
            _textBlock.Text = "X: " + Value;
        }

        public HorizontalLocationEditor():base(0,11)
        {
            
        }
    }
}