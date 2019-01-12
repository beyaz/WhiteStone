using System.Linq;
using System.Text;

namespace BOA.Common.Helpers
{
    /// <summary>
    ///     The padded string builder
    /// </summary>
    public class PaddedStringBuilder
    {
        #region Fields
        readonly StringBuilder sb = new StringBuilder();
        #endregion

        #region Public Properties
        /// <summary>
        ///     Gets or sets the padding count.
        /// </summary>
        public int PaddingCount { get; set; }

        /// <summary>
        ///     Gets or sets the length of the padding.
        /// </summary>
        public int PaddingLength { get; set; } = 4;
        #endregion

        #region Properties
        /// <summary>
        ///     Gets the padding.
        /// </summary>
        string Padding
        {
            get { return "".PadRight(PaddingLength * PaddingCount); }
        }
        #endregion

        #region Public Methods
        /// <summary>
        ///     Appends the specified value.
        /// </summary>
        public void Append(string value)
        {
            sb.Append(value);
        }

        /// <summary>
        ///     Appends all.
        /// </summary>
        public void AppendAll(string multipleLines)
        {
            var strings = multipleLines.SplitToLines().ToArray();

            var length = strings.Length - 1;
            for (var i = 0; i < length; i++)
            {
                AppendLine(strings[i]);
            }

            AppendWithPadding(strings[length]);
        }

        /// <summary>
        ///     Appends the line.
        /// </summary>
        public void AppendLine(string line)
        {
            if (string.IsNullOrEmpty(line))
            {
                sb.AppendLine();
                return;
            }
            sb.Append(Padding);
            sb.AppendLine(line);
        }

        /// <summary>
        ///     Appends the line.
        /// </summary>
        public void AppendLine()
        {
            sb.AppendLine();
        }

        /// <summary>
        ///     Appends the with padding.
        /// </summary>
        public void AppendWithPadding(string value)
        {
            sb.Append(Padding);
            sb.Append(value);
        }

        /// <summary>
        ///     Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        public override string ToString()
        {
            return sb.ToString();
        }
        #endregion
    }
}