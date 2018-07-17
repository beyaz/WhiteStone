using System.Collections.Generic;
using System.Linq;
using HtmlAgilityPack;

namespace BOA.LanguageTranslations
{
    static class Extensions
    {
        #region Public Methods
        public static IEnumerable<HtmlNode> GetElementbyClass(this HtmlNode node, string className)
        {
            return node.Descendants().Where(d => d.Attributes.Contains("class") && d.Attributes["class"].Value.Split(' ').Contains(className));
        }
        #endregion
    }
}