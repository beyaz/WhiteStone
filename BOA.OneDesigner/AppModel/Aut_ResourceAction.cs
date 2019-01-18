using System;

namespace BOA.OneDesigner.AppModel
{
    [Serializable]
    public class Aut_ResourceAction
    {
        public string Name        { get; set; }
        public string CommandName { get; set; }

        public string IsEnabledBindingPath { get; set; }

        public string IsVisibleBindingPath { get; set; }

    }
}