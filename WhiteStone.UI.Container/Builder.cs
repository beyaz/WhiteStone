namespace WhiteStone.UI.Container
{
    public class Builder : CustomUIMarkupLanguage.UIBuilding.Builder
    {
        #region Constructors
        static Builder()
        {
            RegisterElementCreation(LabeledTextBox.On);
        }
        #endregion
    }
}