using System;
using System.Globalization;
using System.Linq;
using System.Text;

namespace BOA.Infrastructure
{
    /// <summary>
    ///     The space case insensitive comparator
    /// </summary>
    sealed class SpaceCaseInsensitiveComparator
    {
        #region Static Fields
        internal static readonly char[] ExceptCharacters = {' ', '\t', '\n', '\r'};
        #endregion

        #region Fields
        /// <summary>
        ///     The culture
        /// </summary>
        readonly CultureInfo _culture;

        /// <summary>
        ///     The ignore lines
        /// </summary>
        Func<string, bool> _ignoreLines;
        #endregion

        #region Constructors
        /// <summary>
        ///     Initializes a new instance of the <see cref="SpaceCaseInsensitiveComparator" /> class.
        /// </summary>
        public SpaceCaseInsensitiveComparator(CultureInfo cultureInfo)
        {
            _culture = cultureInfo;
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="SpaceCaseInsensitiveComparator" /> class.
        /// </summary>
        public SpaceCaseInsensitiveComparator()
        {
            _culture = CultureInfo.CurrentCulture;
        }
        #endregion

        #region Public Methods
        /// <summary>
        ///     Compares the specified left.
        /// </summary>
        public bool Compare(string left, string right)
        {
            if (left == null)
            {
                if (right == null)
                {
                    return true;
                }

                return false;
            }

            if (right == null)
            {
                return false;
            }

            if (_ignoreLines != null)
            {
                right = string.Join(string.Empty, right.Split(Environment.NewLine.ToCharArray()).Where(line => !_ignoreLines(line)));
                left  = string.Join(string.Empty, left.Split(Environment.NewLine.ToCharArray()).Where(line => !_ignoreLines(line)));
            }

            return ExceptChars(left.ToLower(_culture), ExceptCharacters).Equals(ExceptChars(right.ToLower(_culture), ExceptCharacters));
        }

        /// <summary>
        ///     Ignores the lines.
        /// </summary>
        public SpaceCaseInsensitiveComparator IgnoreLines(Func<string, bool> func)
        {
            _ignoreLines = func;
            return this;
        }
        #endregion

        #region Methods
        /// <summary>
        ///     Excepts the chars.
        /// </summary>
        internal static string ExceptChars(string str, char[] toExclude)
        {
            var sb = new StringBuilder();
            foreach (var c in str)
            {
                if (!toExclude.Contains(c))
                {
                    sb.Append(c);
                }
            }

            return sb.ToString();
        }
        #endregion
    }
}