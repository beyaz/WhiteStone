using System.Collections.Generic;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Text;
using WhiteStone.Helpers;

namespace System
{
    /// <summary>
    ///     The extensions
    /// </summary>
    public static class Extensions
    {
        #region Properties
        /// <summary>
        ///     Gets the default format provider.
        /// </summary>
        static IFormatProvider DefaultFormatProvider => CultureInfo.CurrentCulture;
        #endregion

        #region Public Methods
        /// <summary>
        ///     Asserts the not null.
        /// </summary>
        /// <exception cref="System.ArgumentNullException"></exception>
        public static T AssertNotNull<T>(this T value, string valueName = null, [CallerMemberName] string callerMemberName = null, [CallerFilePath] string callerFilePath = null, [CallerLineNumber] int callerLineNumber = 0)
        {
            if (value == null)
            {
                var sb = new StringBuilder();
                sb.Append(valueName ?? "value");
                sb.AppendLine(" is null.");

                sb.Append("@callerMemberName:");
                sb.AppendLine(callerMemberName);

                sb.Append("@callerFilePath:");
                sb.AppendLine(callerFilePath);

                sb.Append("@callerLineNumber:");
                sb.AppendLine(callerLineNumber.ToString(GlobalizationUtility.EnglishCulture));

                throw new ArgumentNullException(sb.ToString());
            }

            return value;
        }

        /// <summary>
        ///     Compares the specified right.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <param name="formatProvider">The format provider.</param>
        /// <returns></returns>
        public static int Compare(this object left, object right, IFormatProvider formatProvider = null)
        {
            if (ReferenceEquals(left, null) && ReferenceEquals(right, null))
            {
                return 0;
            }

            if (!left.IsNumeric())
            {
                throw ValueMustbeNumeric(left);
            }

            if (!right.IsNumeric())
            {
                throw ValueMustbeNumeric(right);
            }

            return Convert.ToDecimal(left, formatProvider).CompareTo(Convert.ToDecimal(right, formatProvider));
        }

        /// <summary>
        ///     Determines whether [is bigger than] [the specified right].
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <param name="formatProvider">The format provider.</param>
        /// <returns>
        ///     <c>true</c> if [is bigger than] [the specified right]; otherwise, <c>false</c>.
        /// </returns>
        /// <exception cref="System.ArgumentException"></exception>
        public static bool IsBiggerThan(this object left, object right, IFormatProvider formatProvider = null)
        {
            if (ReferenceEquals(left, null) || ReferenceEquals(right, null))
            {
                return false;
            }

            if (left.IsNumeric() && right.IsNumeric())
            {
                return Convert.ToDecimal(left, formatProvider) > Convert.ToDecimal(right, formatProvider);
            }
            throw new ArgumentException(left.ToString());
        }

        /// <summary>
        ///     Determines whether [is not null].
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>
        ///     <c>true</c> if [is not null] [the specified value]; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsNotNull(this object value)
        {
            return !ReferenceEquals(value, null);
        }

        /// <summary>
        ///     Determines whether this instance is null.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>
        ///     <c>true</c> if the specified value is null; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsNull(this object value)
        {
            if (value == DBNull.Value)
            {
                return true;
            }
            return ReferenceEquals(value, null);
        }

        /// <summary>
        ///     Determines whether this instance is numeric.
        /// </summary>
        public static bool IsNumeric(this object value)
        {
            if (ReferenceEquals(value, null))
            {
                return false;
            }

            if (value is byte ||
                value is sbyte ||
                value is ushort ||
                value is uint ||
                value is ulong ||
                value is short ||
                value is int ||
                value is long ||
                value is decimal ||
                value is double ||
                value is float)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        ///     Determines whether this instance is numeric.
        /// </summary>
        public static bool IsNumeric(this Type type)
        {
            if (type == null)
            {
                throw new ArgumentNullException(nameof(type));
            }
            

            if (type == typeof( byte) ||
                type == typeof( sbyte) ||
                type == typeof( ushort) ||
                type == typeof( uint) ||
                type == typeof( ulong) ||
                type == typeof( short) ||
                type == typeof( int) ||
                type == typeof( long) ||
                type == typeof( decimal) ||
                type == typeof( double) ||
                type == typeof( float))
            {
                return true;
            }

            return false;
        }

        /// <summary>
        ///     Determines whether the specified right is same.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <param name="formatProvider">The format provider.</param>
        /// <returns>
        ///     <c>true</c> if the specified right is same; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsSame(this object left, object right, IFormatProvider formatProvider = null)
        {
            if (ReferenceEquals(left, null))
            {
                return ReferenceEquals(right, null);
            }

            var leftAsString = left as string;
            if (leftAsString != null)
            {
                return leftAsString.Equals(right);
            }
            if (right is string)
            {
                return false;
            }

            if (left.IsNumeric() && right.IsNumeric())
            {
                return Convert.ToDecimal(left, formatProvider) == Convert.ToDecimal(right, formatProvider);
            }

            return left.Equals(right);
        }

        /// <summary>
        ///     Determines whether this instance is string.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static bool? IsString(this object value)
        {
            return value is string;
        }

        /// <summary>
        ///     To the boolean.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static bool ToBoolean(this object value)
        {
            return ToBoolean(value, DefaultFormatProvider);
        }

        /// <summary>
        ///     To the boolean.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="formatProvider">The format provider.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException">value</exception>
        public static bool ToBoolean(this object value, IFormatProvider formatProvider)
        {
            if (value.IsNull())
            {
                throw new ArgumentNullException(nameof(value));
            }
            return Convert.ToBoolean(value, formatProvider);
        }

        /// <summary>
        ///     To the boolean nullable.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static bool? ToBooleanNullable(this object value)
        {
            return ToBooleanNullable(value, DefaultFormatProvider);
        }

        /// <summary>
        ///     To the boolean nullable.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="formatProvider">The format provider.</param>
        /// <returns></returns>
        public static bool? ToBooleanNullable(this object value, IFormatProvider formatProvider)
        {
            if (value.IsNull())
            {
                return null;
            }

            var valueAsString = value as string;
            if (valueAsString != null)
            {
                if (valueAsString.Trim() == "1")
                {
                    return true;
                }
                if (valueAsString.Trim() == "0")
                {
                    return false;
                }
            }

            return Convert.ToBoolean(value, formatProvider);
        }

        /// <summary>
        ///     To the decimal.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="formatProvider">The format provider.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException">value</exception>
        public static decimal ToDecimal(this object value, IFormatProvider formatProvider)
        {
            if (value.IsNull())
            {
                throw new ArgumentNullException(nameof(value));
            }
            return Convert.ToDecimal(value, formatProvider);
        }

        /// <summary>
        ///     To the decimal.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException">value</exception>
        public static decimal ToDecimal(this object value)
        {
            if (value.IsNull())
            {
                throw new ArgumentNullException(nameof(value));
            }
            return ToDecimal(value, DefaultFormatProvider);
        }

        /// <summary>
        ///     To the decimal nullable.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="formatProvider">The format provider.</param>
        /// <returns></returns>
        public static decimal? ToDecimalNullable(this object value, IFormatProvider formatProvider)
        {
            if (value.IsNull())
            {
                return null;
            }
            return Convert.ToDecimal(value, formatProvider);
        }

        /// <summary>
        ///     To the decimal nullable.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static decimal? ToDecimalNullable(this object value)
        {
            return ToDecimalNullable(value, DefaultFormatProvider);
        }

        /// <summary>
        ///     To the int32.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException">value</exception>
        public static int ToInt32(this object value)
        {
            if (value.IsNull())
            {
                throw new ArgumentNullException(nameof(value));
            }
            return Convert.ToInt32(value, DefaultFormatProvider);
        }

        /// <summary>
        ///     To the int32 nullable.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="formatProvider">The format provider.</param>
        /// <returns></returns>
        public static int? ToInt32Nullable(this object value, IFormatProvider formatProvider)
        {
            if (value.IsNull())
            {
                return null;
            }
            return Convert.ToInt32(value, formatProvider);
        }

        /// <summary>
        ///     To the int32 nullable.
        /// </summary>
        public static int? ToInt32Nullable(this object value)
        {
            return ToInt32Nullable(value, CultureInfo.CurrentCulture);
        }
        #endregion

        #region Methods
        /// <summary>
        ///     Values the mustbe numeric.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        static ArgumentException ValueMustbeNumeric(object value)
        {
            return new ArgumentException(value.ToString());
        }
        #endregion
    }
}

namespace System.ComponentModel
{
    /// <summary>
    ///     The bag changed event arguments
    /// </summary>
    /// <seealso cref="System.ComponentModel.PropertyChangedEventArgs" />
    public class BagChangedEventArgs : PropertyChangedEventArgs
    {
        #region Constructors
        /// <summary>
        ///     Initializes a new instance of the <see cref="BagChangedEventArgs" /> class.
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        public BagChangedEventArgs(string propertyName) : base(propertyName)
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="BagChangedEventArgs" /> class.
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        /// <param name="newValue">The new value.</param>
        public BagChangedEventArgs(string propertyName, object newValue) : base(propertyName)
        {
            NewValue = newValue;
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="BagChangedEventArgs" /> class.
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        /// <param name="newValue">The new value.</param>
        /// <param name="oldValue">The old value.</param>
        public BagChangedEventArgs(string propertyName, object newValue, object oldValue) : base(propertyName)
        {
            NewValue = newValue;
            OldValue = oldValue;
        }
        #endregion

        #region Public Properties
        /// <summary>
        ///     Gets the new value.
        /// </summary>
        public object NewValue { get; }

        /// <summary>
        ///     Gets the old value.
        /// </summary>
        public object OldValue { get; }
        #endregion
    }

    /// <summary>
    ///     The bag
    /// </summary>
    /// <seealso cref="System.ComponentModel.INotifyPropertyChanged" />
    [Serializable]
    public class Bag : INotifyPropertyChanged
    {
        #region Fields
        /// <summary>
        ///     The entries
        /// </summary>
        readonly Dictionary<string, object> _entries = new Dictionary<string, object>();
        #endregion

        #region Public Indexers
        /// <summary>
        ///     Gets or sets the <see cref="System.Object" /> with the specified property name.
        /// </summary>
        /// <value>
        ///     The <see cref="System.Object" />.
        /// </value>
        /// <param name="propertyName">Name of the property.</param>
        /// <returns></returns>
        public object this[string propertyName]
        {
            get { return GetValue(propertyName); }
            set { SetValue(propertyName, value); }
        }
        #endregion

        #region Public Methods
        /// <summary>
        ///     Determines whether the specified property name contains key.
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        /// <returns>
        ///     <c>true</c> if the specified property name contains key; otherwise, <c>false</c>.
        /// </returns>
        public bool ContainsKey(string propertyName)
        {
            return _entries.ContainsKey(propertyName);
        }

        /// <summary>
        ///     Determines whether the specified property name contains key.
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        /// <returns>
        ///     <c>true</c> if the specified property name contains key; otherwise, <c>false</c>.
        /// </returns>
        public bool ContainsKey(Enum propertyName)
        {
            return ContainsKey(propertyName.ToString());
        }

        /// <summary>
        ///     Gets the value.
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        /// <returns></returns>
        public object GetValue(string propertyName)
        {
            object value = null;
            _entries.TryGetValue(propertyName, out value);
            return value;
        }

        /// <summary>
        ///     Sets the value.
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        /// <param name="value">The value.</param>
        public void SetValue(string propertyName, object value)
        {
            var oldValue = GetValue(propertyName);

            if (!ReferenceEquals(oldValue, null))
            {
                if (oldValue.Equals(value))
                {
                    return;
                }
            }

            _entries[propertyName] = value;

            OnPropertyChanged(propertyName, value, oldValue);
        }
        #endregion

        #region INotifyPropertyChanged Members
        /// <summary>
        ///     Notifies clients that a property value has changed.
        /// </summary>
        [field: NonSerialized]
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        ///     Called when [property changed].
        /// </summary>
        /// <param name="prop">The property.</param>
        /// <param name="newValue">The new value.</param>
        /// <param name="oldValue">The old value.</param>
        void OnPropertyChanged(string prop, object newValue, object oldValue)
        {
            PropertyChanged?.Invoke(this, new BagChangedEventArgs(prop, newValue, oldValue));
        }

        /// <summary>
        ///     Called when [property changed].
        /// </summary>
        /// <param name="prop">The property.</param>
        public virtual void OnPropertyChanged(string prop)
        {
            PropertyChanged?.Invoke(this, new BagChangedEventArgs(prop));
        }
        #endregion
    }
}