using System.IO;
using System.Text;

namespace System.Web
{
    /// <summary>
    ///     The white stone extensions
    /// </summary>
    public static class WhiteStoneExtensions
    {
        #region Public Methods
        /// <summary>
        ///     Inputs the stream as json.
        /// </summary>
        public static string InputStreamAsJson(this HttpRequest request)
        {
            string documentContents;
            using (var receiveStream = request.InputStream)
            {
                using (var readStream = new StreamReader(receiveStream, Encoding.UTF8))
                {
                    documentContents = readStream.ReadToEnd();
                }
            }

            return documentContents;
        }
        #endregion
    }
}