using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Newtonsoft.Json.Serialization;

namespace BOA.UI.Types
{
    /// <summary>
    ///     The pair
    /// </summary>
    [Serializable]
    public class Pair
    {
        #region Constructors
        /// <summary>
        ///     Initializes a new instance of the <see cref="Pair" /> class.
        /// </summary>
        public Pair()
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="Pair" /> class.
        /// </summary>
        public Pair(string key, string value)
        {
            Key   = key;
            Value = value;
        }
        #endregion

        #region Public Properties
        /// <summary>
        ///     Gets or sets the key.
        /// </summary>
        public string Key { get; set; }

        /// <summary>
        ///     Gets or sets the value.
        /// </summary>
        public string Value { get; set; }
        #endregion
    }

    /// <summary>
    ///     The data grid column information
    /// </summary>
    [Serializable]
    public class DataGridColumnInfo
    {
        #region Public Properties
        /// <summary>
        ///     Gets or sets the binding path.
        /// </summary>
        public string BindingPath { get; set; }

        /// <summary>
        ///     Gets or sets the is boolean.
        /// </summary>
        public bool? IsBoolean { get; set; }

        /// <summary>
        ///     Gets or sets the is date.
        /// </summary>
        public bool? IsDate { get; set; }

        /// <summary>
        ///     Gets or sets the is decimal.
        /// </summary>
        public bool? IsDecimal { get; set; }

        /// <summary>
        ///     Gets or sets the is int32.
        /// </summary>
        public bool? IsInt32 { get; set; }

        /// <summary>
        ///     Gets or sets the is string.
        /// </summary>
        public bool? IsString { get; set; }

        /// <summary>
        ///     Gets or sets the label.
        /// </summary>
        public string Label { get; set; }
        #endregion

        #region Methods
        /// <summary>
        ///     Initializes the type of the data.
        /// </summary>
        internal static void InitializeDataType(DataGridColumnInfo columnInfo, PropertyInfo propertyInfo)
        {
            var propertyType = propertyInfo.PropertyType;

            if (propertyType == typeof(int) ||
                propertyType == typeof(int?) ||
                propertyType == typeof(short) ||
                propertyType == typeof(short?) ||
                propertyType == typeof(byte) ||
                propertyType == typeof(byte?))
            {
                columnInfo.IsInt32 = true;
                return;
            }

            if (propertyType == typeof(bool) ||
                propertyType == typeof(bool?))
            {
                columnInfo.IsBoolean = true;
                return;
            }

            if (propertyType == typeof(DateTime) ||
                propertyType == typeof(DateTime?))
            {
                columnInfo.IsDate = true;
                return;
            }

            if (propertyType == typeof(decimal) ||
                propertyType == typeof(decimal?))
            {
                columnInfo.IsDecimal = true;
                return;
            }

            columnInfo.IsString = true;
        }
        #endregion
    }

    /// <summary>
    ///     The data grid row background information
    /// </summary>
    [Serializable]
    public sealed class DataGridRowBackgroundInfo
    {
        #region Public Properties
        /// <summary>
        ///     Gets or sets the binding path.
        /// </summary>
        public string BindingPath { get; set; }

        /// <summary>
        ///     Gets or sets the color.
        /// </summary>
        public string Color { get; set; }
        #endregion
    }

    /// <summary>
    ///     The data grid information
    /// </summary>
    [Serializable]
    public class DataGridInfo
    {
        #region Fields
        /// <summary>
        ///     The constant messages class
        /// </summary>
        [NonSerialized] internal Type ConstMessagesClass;

        /// <summary>
        ///     The messaging pattern
        /// </summary>
        [NonSerialized] internal Func<string, string> MessagingPattern;

        /// <summary>
        ///     The record type
        /// </summary>
        [NonSerialized] internal Type recordType;
        #endregion

        #region Public Properties
        /// <summary>
        ///     Gets or sets the columns.
        /// </summary>
        public IReadOnlyList<DataGridColumnInfo> Columns { get; set; }

        /// <summary>
        ///     Gets or sets the type of the record.
        /// </summary>
        public string RecordType { get; set; }

        /// <summary>
        ///     Gets or sets the row backgrounds.
        /// </summary>
        public IReadOnlyList<DataGridRowBackgroundInfo> RowBackgrounds { get; set; }
        #endregion

        #region Public Methods
        /// <summary>
        ///     Creates the specified record type.
        /// </summary>
        public static DataGridInfo Create(Type recordType)
        {
            const string Comma = ",";

            return new DataGridInfo
            {
                recordType = recordType,
                Columns    = new List<DataGridColumnInfo>(),
                RecordType = recordType.FullName + Comma + recordType.Assembly.GetName().Name
            };
        }

        /// <summary>
        ///     Adds the column.
        /// </summary>
        public DataGridInfo AddColumn<T>(Expression<Func<T>> bindingPath)
        {
            AddColumn(bindingPath, null);
            return this;
        }

        /// <summary>
        ///     Adds the column.
        /// </summary>
        public DataGridInfo AddColumn<T>(Expression<Func<T>> bindingPath, string label)
        {
            AddColumn(BindingPathExpressionHelper.GetBindingPath(bindingPath), label);

            return this;
        }

        /// <summary>
        ///     Adds the columns.
        /// </summary>
        public DataGridInfo AddColumns(params string[] columnNames)
        {
            foreach (var columnName in columnNames)
            {
                AddColumn(columnName);
            }

            return this;
        }

        /// <summary>
        ///     Adds the row background.
        /// </summary>
        public void AddRowBackground<T>(Expression<Func<T>> bindingPath, string color)
        {
            if (bindingPath == null)
            {
                throw new ArgumentNullException(nameof(bindingPath));
            }

            if (color == null)
            {
                throw new ArgumentNullException(nameof(color));
            }

            if (RowBackgrounds == null)
            {
                RowBackgrounds = new List<DataGridRowBackgroundInfo>();
            }

            var propertyList = PropertyPathResolver.Resolve(recordType, BindingPathExpressionHelper.GetBindingPath(bindingPath));

            var item = new DataGridRowBackgroundInfo
            {
                BindingPath = NamingHelper.GetBindingPath(propertyList),
                Color       = color
            };

            ((List<DataGridRowBackgroundInfo>) RowBackgrounds).Add(item);
        }

        /// <summary>
        ///     Sets the messaging.
        /// </summary>
        public DataGridInfo SetMessaging(Type constMessagesClass, Func<string, string> messagingPattern)
        {
            if (constMessagesClass == null)
            {
                throw new ArgumentNullException(nameof(constMessagesClass));
            }

            if (messagingPattern == null)
            {
                throw new ArgumentNullException(nameof(messagingPattern));
            }

            ConstMessagesClass = constMessagesClass;
            MessagingPattern   = messagingPattern;

            return this;
        }

        /// <summary>
        ///     Sets the messaging.
        /// </summary>
        public DataGridInfo SetMessaging(Func<string, string> messagingPattern)
        {
            if (messagingPattern == null)
            {
                throw new ArgumentNullException(nameof(messagingPattern));
            }

            MessagingPattern = messagingPattern;

            return this;
        }
        #endregion

        #region Methods
        /// <summary>
        ///     Adds the column.
        /// </summary>
        void AddColumn(string bindingPath, string label = null)
        {
            var columnInfo = new DataGridColumnInfo
            {
                BindingPath = bindingPath,
                Label       = label
            };

            DataGridInfoHelper.Initialize(this, columnInfo);

            ((List<DataGridColumnInfo>) Columns).Add(columnInfo);
        }
        #endregion
    }

    /// <summary>
    ///     The property path resolver
    /// </summary>
    static class PropertyPathResolver
    {
        #region Public Methods
        /// <summary>
        ///     Resolves the property path.
        /// </summary>
        public static IReadOnlyList<PropertyInfo> Resolve(Type type, string propertyPath)
        {
            if (type == null)
            {
                const string ArgumentName = "type";
                throw new ArgumentNullException(ArgumentName);
            }

            if (propertyPath == null)
            {
                const string ArgumentName = "propertyPath";
                throw new ArgumentNullException(ArgumentName);
            }

            var propertyInfos = new List<PropertyInfo>();

            const char Dot = '.';

            foreach (var propertyName in propertyPath.Split(Dot).Where(x => string.IsNullOrWhiteSpace(x) == false))
            {
                var propertyInfo = type.GetProperty(propertyName, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
                if (propertyInfo == null)
                {
                    throw new MissingMemberException(type.FullName + Dot + propertyName);
                }

                propertyInfos.Add(propertyInfo);
                type = propertyInfo.PropertyType;
            }

            return propertyInfos;
        }
        #endregion
    }

    /// <summary>
    ///     The binding path expression helper
    /// </summary>
    static class BindingPathExpressionHelper
    {
        #region Public Methods
        /// <summary>
        ///     Gets the binding path.
        /// </summary>
        public static string GetBindingPath<T>(Expression<Func<T>> propertyAccessor)
        {
            var memberExpression = propertyAccessor.Body as MemberExpression;
            if (memberExpression == null)
            {
                throw new ArgumentException(propertyAccessor.ToString());
            }

            return NameofAllPath(memberExpression);
        }
        #endregion

        #region Methods
        /// <summary>
        ///     Nameofs all path.
        /// </summary>
        static string NameofAllPath(MemberExpression memberExpression)
        {
            var path = new List<string>();

            while (memberExpression != null)
            {
                path.Add(memberExpression.Member.Name);

                memberExpression = memberExpression.Expression as MemberExpression;
            }

            if (path.Count == 0)
            {
                return null;
            }

            path.RemoveAt(path.Count - 1);

            path.Reverse();

            const string Separator = ".";

            return string.Join(Separator, path);
        }
        #endregion
    }

    /// <summary>
    ///     The naming helper
    /// </summary>
    static class NamingHelper
    {
        #region Static Fields
        /// <summary>
        ///     The camel case property names contract resolver
        /// </summary>
        static readonly CamelCasePropertyNamesContractResolver CamelCasePropertyNamesContractResolver = new CamelCasePropertyNamesContractResolver();
        #endregion

        #region Public Methods
        /// <summary>
        ///     Gets the binding path.
        /// </summary>
        public static string GetBindingPath(IReadOnlyList<PropertyInfo> propertyList)
        {
            const string separator = ".";
            return string.Join(separator, from propertyInfo in propertyList select GetResolvedPropertyName(propertyInfo.Name));
        }

        /// <summary>
        ///     Gets the name of the resolved property.
        /// </summary>
        public static string GetResolvedPropertyName(string propertyName)
        {
            return CamelCasePropertyNamesContractResolver.GetResolvedPropertyName(propertyName);
        }
        #endregion
    }

    /// <summary>
    ///     The data grid information helper
    /// </summary>
    static class DataGridInfoHelper
    {
        #region Public Methods
        /// <summary>
        ///     Initializes the specified grid information.
        /// </summary>
        public static void Initialize(DataGridInfo gridInfo, DataGridColumnInfo columnInfo)
        {
            var ConstMessagesClass = gridInfo.ConstMessagesClass;
            var MessagingPattern   = gridInfo.MessagingPattern;

            var propertyList = PropertyPathResolver.Resolve(gridInfo.recordType, columnInfo.BindingPath);

            columnInfo.BindingPath = NamingHelper.GetBindingPath(propertyList);

            var lastPropertyInfo = propertyList.Last();

            DataGridColumnInfo.InitializeDataType(columnInfo, lastPropertyInfo);

            if (columnInfo.Label == null)
            {
                var propertyNameForLabel = lastPropertyInfo.Name;

                if (ConstMessagesClass != null)
                {
                    columnInfo.Label = ConstMessagesClass.GetFields(BindingFlags.Public | BindingFlags.Static).FirstOrDefault(f => f.Name == propertyNameForLabel)?.GetRawConstantValue().ToString();
                }

                if (columnInfo.Label == null && MessagingPattern != null)
                {
                    columnInfo.Label = MessagingPattern(propertyNameForLabel);
                }
            }
        }
        #endregion
    }
}