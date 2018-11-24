using System.Collections.Generic;
using System.Linq;
using BOA.Common.Helpers;
using HtmlAgilityPack;

namespace BOA.LanguageTranslations
{
    static class Extensions
    {
        #region Public Methods
        public static IEnumerable<HtmlNode> GetElementByClass(this HtmlNode node, string className)
        {
            return node.Descendants().Where(x => x.IsClassNameMatch(className.Split(' ').Trim()));
        }
        #endregion

        #region Methods
        static bool IsClassNameMatch(this HtmlNode node, string[] searchClassName)
        {
            var classNames = node.Attributes["class"]?.Value?.Split(' ').Trim();

            if (classNames == null)
            {
                return false;
            }

            foreach (var className in searchClassName)
            {
                if (classNames.Contains(className) == false)
                {
                    return false;
                }
            }

            return true;
        }

        static string[] Trim(this IEnumerable<string> values)
        {
            return values.Where(x => x.IsNullOrWhiteSpace() == false).Select(x => x.Trim()).ToArray();
        }
        #endregion
    }
}