using System.Collections.Generic;
using BOA.OneDesigner.JsxElementModel;
using BOAPlugins.Messaging;

namespace BOA.OneDesigner.AppModel
{
    public interface IDatabase
    {
        #region Public Methods
        List<string> GetDefaultRequestNames();

        IReadOnlyList<string> GetMessagingGroupNames();
        ScreenInfo            GetScreenInfo(string requestName);
        IReadOnlyList<string> GetTfsFolderNames();
        void                  Save(ScreenInfo screenInfo);

        IList<PropertyInfo> GetPropertyNames(string groupName);

        #endregion
    }
}