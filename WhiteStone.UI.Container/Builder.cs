namespace WhiteStone.UI.Container
{
    public class Builder : CustomUIMarkupLanguage.UIBuilding.Builder
    {
        #region Constructors
        static Builder()
        {
            RegisterElementCreation(LabeledTextBox.On);

            RegisterElementCreation("Tile",typeof(MahApps.Metro.Controls.Tile));
        }
        #endregion
    }
}