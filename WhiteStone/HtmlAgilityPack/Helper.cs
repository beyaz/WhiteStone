using System.Net;

namespace HtmlAgilityPack
{
    /// <summary>
    ///     The helper
    /// </summary>
    public static class Helper
    {
        #region Public Methods
        /// <summary>
        ///     Gets the HTML document.
        /// </summary>
        public static HtmlDocument GetHtmlDocument(string url)
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;

            var web = new HtmlWeb();

            // web.UserAgent = "Mozilla/5.0 (Windows; U; Windows NT 5.1; en-US; rv:1.9.2) Gecko/20100115 Firefox/3.6";
            var doc = web.Load(url, "10.13.50.100", 8080, "beyaztas", "sokrates_aristo_48");

            return doc;
        }
        #endregion
    }
}