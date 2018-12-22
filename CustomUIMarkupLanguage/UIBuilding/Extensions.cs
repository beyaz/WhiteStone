using System.IO;
using System.Windows;

namespace CustomUIMarkupLanguage.UIBuilding
{
    /// <summary>
    ///     The extensions
    /// </summary>
    public static class Extensions
    {
        #region Public Methods
        /// <summary>
        ///     Loads the json.
        /// </summary>
        public static T LoadJson<T>(this T element, string json) where T : UIElement
        {
            var builder = new Builder
            {
                Caller = element
            };

            builder.Load(json);

            return element;
        }

        /// <summary>
        ///     Loads the json file.
        /// </summary>
        public static T LoadJsonFile<T>(this T element, string jsonFilePath) where T : UIElement
        {
            return element.LoadJson(File.ReadAllText(jsonFilePath));
        }
        #endregion
    }
}