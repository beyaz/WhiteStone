using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Text;

namespace BOA.Common.Helpers
{
    /// <summary>
    ///     The random value
    /// </summary>
    public class RandomValue
    {
        #region Static Fields
        /// <summary>
        ///     The random
        /// </summary>
        static readonly Random _random = new Random();

        /// <summary>
        ///     The supported collection types for list
        /// </summary>
        static readonly IReadOnlyList<Type> SupportedCollectionTypesForList = new List<Type>
        {
            typeof(List<>),
            typeof(IReadOnlyList<>),
            typeof(IReadOnlyCollection<>)
        };

        /// <summary>
        ///     The supported types
        /// </summary>
        static readonly Dictionary<Type, Func<Type, object>> SupportedTypes = new Dictionary<Type, Func<Type, object>>
        {
            {typeof(int), type => Int32()},
            {typeof(string), type => String()},
            {typeof(decimal), type => Decimal()},
            {typeof(double), type => Double()},
            {typeof(bool), type => Boolean()},
            {typeof(byte), type => Byte()},
            {typeof(char), type => Char()},
            {typeof(float), type => Single()},
            {typeof(long), type => Int64()},
            {typeof(sbyte), type => SByte()},
            {typeof(short), type => Int16()},
            {typeof(uint), type => UInt32()},
            {typeof(ulong), type => UInt64()},
            {typeof(ushort), type => UInt16()},
            {typeof(Guid), type => Guid()},
            {typeof(DateTime), type => DateTime()},
            {typeof(TimeSpan), type => TimeSpan()},
            {typeof(DateTimeOffset), type => DateTimeOffset()}
        };
        #endregion

        #region Fields
        /// <summary>
        ///     The object creation stack
        /// </summary>
        internal readonly Stack<object> _objectCreationStack = new Stack<object>();
        #endregion

        #region Public Events
        /// <summary>
        ///     Occurs when [property value assigned].
        /// </summary>
        public event EventHandler<PropertyValueAssignedEventArgs> PropertyValueAssigned;
        #endregion

        #region Enums
        /// <summary>
        ///     The support type
        /// </summary>
        internal enum SupportType
        {
            /// <summary>
            ///     The not supported
            /// </summary>
            NotSupported,

            /// <summary>
            ///     The user defined
            /// </summary>
            UserDefined,

            /// <summary>
            ///     The basic
            /// </summary>
            Basic,

            /// <summary>
            ///     The enum
            /// </summary>
            Enum,

            /// <summary>
            ///     The collection
            /// </summary>
            Collection,

            /// <summary>
            ///     The nullable
            /// </summary>
            Nullable
        }
        #endregion

        #region Public Methods
        /// <summary>
        ///     Booleans this instance.
        /// </summary>
        public static bool Boolean()
        {
            var randomNumber = Int32(2);

            if (randomNumber == 0)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        ///     Bytes the specified maximum possible value.
        /// </summary>
        public static byte Byte(byte maxPossibleValue = byte.MaxValue)
        {
            return (byte) Int32(maxPossibleValue);
        }

        /// <summary>
        ///     Characters this instance.
        /// </summary>
        public static char Char()
        {
            var buffer = new byte[sizeof(char)];

            _random.NextBytes(buffer);

            return BitConverter.ToChar(buffer, 0);
        }

        /// <summary>
        ///     Dates the time.
        /// </summary>
        public static DateTime DateTime(DateTime? minDateTime = null, DateTime? maxDateTime = null)
        {
            if (minDateTime == null)
            {
                minDateTime = System.DateTime.Now.AddYears(-1);
            }

            if (maxDateTime == null)
            {
                maxDateTime = System.DateTime.Now;
            }

            var timeSinceStartOfDateTime        = maxDateTime.Value - minDateTime.Value;
            var timeInHoursSinceStartOfDateTime = (int) timeSinceStartOfDateTime.TotalHours;
            var hoursToSubtract                 = Int32(timeInHoursSinceStartOfDateTime) * -1;
            var timeToReturn                    = maxDateTime.Value.AddHours(hoursToSubtract);

            if (timeToReturn > minDateTime.Value && timeToReturn < maxDateTime.Value)
            {
                return timeToReturn;
            }

            return System.DateTime.Now;
        }

        /// <summary>
        ///     Dates the time offset.
        /// </summary>
        public static DateTimeOffset DateTimeOffset()
        {
            return new DateTimeOffset(DateTime());
        }

        /// <summary>
        ///     Decimals the specified maximum possible value.
        /// </summary>
        public static decimal Decimal(decimal maxPossibleValue = 1m)
        {
            return (decimal) _random.NextDouble() * maxPossibleValue;
        }

        /// <summary>
        ///     Doubles this instance.
        /// </summary>
        public static double Double()
        {
            return _random.NextDouble();
        }

        /// <summary>
        ///     Enums this instance.
        /// </summary>
        public static T Enum<T>()
        {
            return (T) Enum(typeof(T));
        }

        /// <summary>
        ///     Enums the specified enum type.
        /// </summary>
        public static object Enum(Type enumType)
        {
            var fields = enumType.GetFields().Where(x => x.IsPublic && x.IsStatic).ToArray();

            var index = _random.Next(fields.Length);

            return System.Enum.Parse(enumType, fields[index].Name, false);
        }

        /// <summary>
        ///     GUIDs this instance.
        /// </summary>
        public static Guid Guid()
        {
            return System.Guid.NewGuid();
        }

        /// <summary>
        ///     Int16s the specified maximum possible value.
        /// </summary>
        public static short Int16(short maxPossibleValue = short.MaxValue)
        {
            return (short) Int32(maxPossibleValue);
        }

        /// <summary>
        ///     Int32s the specified maximum possible value.
        /// </summary>
        public static int Int32(int maxPossibleValue = int.MaxValue, int minPossibleValue = 0)
        {
            if (minPossibleValue > maxPossibleValue || minPossibleValue < 0)
            {
                minPossibleValue = 0;
            }

            var max = Math.Abs(maxPossibleValue);

            return _random.Next(max - minPossibleValue) + minPossibleValue;
        }

        /// <summary>
        ///     Int64s the specified maximum possible value.
        /// </summary>
        public static long Int64(long maxPossibleValue = long.MaxValue)
        {
            return (long) UInt64((ulong) maxPossibleValue);
        }

        /// <summary>
        ///     Lists the of.
        /// </summary>
        public static List<T> ListOf<T>(int? optionalLength = null)
        {
            return new RandomValue().List<T>(optionalLength);
        }

        /// <summary>
        ///     Objects this instance.
        /// </summary>
        public static T Object<T>() where T : new()
        {
            var randomValue = new RandomValue();

            return (T) randomValue.Object(typeof(T));
        }

        /// <summary>
        ///     Objects the specified after random object created.
        /// </summary>
        public static T Object<T>(Action<T> afterRandomObjectCreated) where T : new()
        {
            var randomObject = Object<T>();

            afterRandomObjectCreated(randomObject);

            return randomObject;
        }

        /// <summary>
        ///     ses the byte.
        /// </summary>
        public static sbyte SByte(sbyte maxPossibleValue = sbyte.MaxValue)
        {
            return (sbyte) Int32(maxPossibleValue);
        }

        /// <summary>
        ///     Singles this instance.
        /// </summary>
        public static float Single()
        {
            return (float) _random.NextDouble();
        }

        /// <summary>
        ///     Strings this instance.
        /// </summary>
        public static string String()
        {
            return Guid().ToString();
        }

        /// <summary>
        ///     Strings the specified string length.
        /// </summary>
        public static string String(int stringLength)
        {
            var sb = new StringBuilder();

            sb.Append(String());

            while (sb.Length <= stringLength)
            {
                sb.Append(String());
            }

            sb.Length = stringLength;

            return sb.ToString();
        }

        /// <summary>
        ///     Times the span.
        /// </summary>
        public static TimeSpan TimeSpan()
        {
            var date1 = DateTime();
            var date2 = DateTime();

            return date1 > date2 ? date1.Subtract(date2) : date2.Subtract(date1);
        }

        /// <summary>
        ///     us the int16.
        /// </summary>
        public static ushort UInt16(ushort maxPossibleValue = ushort.MaxValue)
        {
            return (ushort) Int32(maxPossibleValue);
        }

        /// <summary>
        ///     us the int32.
        /// </summary>
        public static uint UInt32(uint maxPossibleValue = uint.MaxValue)
        {
            var buffer = new byte[sizeof(uint)];
            _random.NextBytes(buffer);

            var generatedUint = BitConverter.ToUInt32(buffer, 0);

            while (generatedUint > maxPossibleValue)
            {
                generatedUint = generatedUint >> 1;
            }

            return generatedUint;
        }

        /// <summary>
        ///     us the int64.
        /// </summary>
        public static ulong UInt64(ulong maxPossibleValue = ulong.MaxValue)
        {
            var buffer = new byte[sizeof(ulong)];

            _random.NextBytes(buffer);

            var generatedULongs = BitConverter.ToUInt64(buffer, 0);

            while (generatedULongs > maxPossibleValue)
            {
                generatedULongs = generatedULongs >> 1;
            }

            return generatedULongs;
        }

        /// <summary>
        ///     Arrays the specified optional length.
        /// </summary>
        public T[] Array<T>(int? optionalLength = null)
        {
            return Collection<T>(optionalLength).ToArray();
        }

        /// <summary>
        ///     Collections the specified optional length.
        /// </summary>
        public Collection<T> Collection<T>(int? optionalLength = null)
        {
            return (Collection<T>) ICollection<T>(optionalLength);
        }

        /// <summary>
        ///     Dictionaries the specified optional length.
        /// </summary>
        public Dictionary<TKey, TValue> Dictionary<TKey, TValue>(int? optionalLength = null)
        {
            return (Dictionary<TKey, TValue>) IDictionary<TKey, TValue>(optionalLength);
        }

        /// <summary>
        ///     is the collection.
        /// </summary>
        public ICollection<T> ICollection<T>(int? optionalLength = null)
        {
            var collection = ICollection(typeof(T), optionalLength);

            return new Collection<T>(collection.ConvertAll(x => (T) x));
        }

        /// <summary>
        ///     is the dictionary.
        /// </summary>
        public IDictionary<TKey, TValue> IDictionary<TKey, TValue>(int? optionalLength = null)
        {
            return (IDictionary<TKey, TValue>) IDictionary(typeof(TKey), typeof(TValue), optionalLength);
        }

        /// <summary>
        ///     is the enumerable.
        /// </summary>
        public IEnumerable<T> IEnumerable<T>()
        {
            return IEnumerable<T>(null);
        }

        /// <summary>
        ///     is the enumerable.
        /// </summary>
        public IEnumerable<T> IEnumerable<T>(int? optionalLength)
        {
            var numberOfItems = CreateRandomLengthIfOptionLengthIsNull(optionalLength);

            return LazyIEnumerable<T>().Take(numberOfItems);
        }

        /// <summary>
        ///     is the list.
        /// </summary>
        public IList<T> IList<T>(int? optionalLength = null)
        {
            return ICollection<T>(optionalLength).ToList();
        }

        /// <summary>
        ///     Lazies the i enumerable.
        /// </summary>
        public IEnumerable<T> LazyIEnumerable<T>()
        {
            var type = typeof(T);

            var supportType = GetSupportType(type);

            // ReSharper disable once LoopVariableIsNeverChangedInsideLoop
            while (supportType != SupportType.NotSupported)
            {
                var method = GetMethodCallAssociatedWithType(type);

                yield return (T) method;
            }
        }

        /// <summary>
        ///     Lazies the i enumerable.
        /// </summary>
        public IEnumerable<object> LazyIEnumerable(Type type)
        {
            var supportType = GetSupportType(type);

            // ReSharper disable once LoopVariableIsNeverChangedInsideLoop
            while (supportType != SupportType.NotSupported)
            {
                var method = GetMethodCallAssociatedWithType(type);

                yield return method;
            }
        }

        /// <summary>
        ///     Lists the specified optional length.
        /// </summary>
        public List<T> List<T>(int? optionalLength = null)
        {
            return ICollection<T>(optionalLength).ToList();
        }

        /// <summary>
        ///     Objects the specified type.
        /// </summary>
        public object Object(Type type)
        {
            var genericObject = Activator.CreateInstance(type);

            var properties = type.GetProperties();

            if (properties.Length == 0)
            {
                return genericObject;
            }

            _objectCreationStack.Push(genericObject);

            foreach (var prop in properties)
            {
                TryInitProperty(prop, genericObject);
            }

            _objectCreationStack.Pop();

            return genericObject;
        }

        /// <summary>
        ///     Observables the collection.
        /// </summary>
        public ObservableCollection<T> ObservableCollection<T>(int? optionalLength = null)
        {
            return new ObservableCollection<T>(List<T>(optionalLength));
        }
        #endregion

        #region Methods
        /// <summary>
        ///     Creates the random length if option length is null.
        /// </summary>
        static int CreateRandomLengthIfOptionLengthIsNull(int? optionalLength)
        {
            return optionalLength ?? _random.Next(1, 10);
        }

        /// <summary>
        ///     Gets the type of the support.
        /// </summary>
        static SupportType GetSupportType(Type type)
        {
            if (SupportedTypes.ContainsKey(type))
            {
                return SupportType.Basic;
            }

            if (type.IsEnum)
            {
                return SupportType.Enum;
            }

            if (IsSupportedCollection(type))
            {
                return SupportType.Collection;
            }

            if (IsNullableType(type))
            {
                return SupportType.Nullable;
            }

            if (type.IsClass)
            {
                return SupportType.UserDefined;
            }

            return SupportType.NotSupported;
        }

        /// <summary>
        ///     Determines whether [is nullable type] [the specified property type].
        /// </summary>
        static bool IsNullableType(Type propertyType)
        {
            return propertyType.IsGenericType && propertyType.GetGenericTypeDefinition() == typeof(Nullable<>);
        }

        /// <summary>
        ///     Determines whether [is supported collection] [the specified property type].
        /// </summary>
        static bool IsSupportedCollection(Type propertyType)
        {
            var hasImplementedICollection = typeof(ICollection).IsAssignableFrom(propertyType);

            return hasImplementedICollection
                   || propertyType.IsGenericType &&
                   (propertyType.IsArray
                    || propertyType.GetGenericTypeDefinition() == typeof(ICollection<>)
                    || propertyType.GetGenericTypeDefinition() == typeof(IList<>)
                    || propertyType.GetGenericTypeDefinition() == typeof(IEnumerable<>)
                    || propertyType.GetGenericTypeDefinition() == typeof(IDictionary<,>)
                    || SupportedCollectionTypesForList.Contains(propertyType.GetGenericTypeDefinition())
                   );
        }

        /// <summary>
        ///     Properties the has no setter.
        /// </summary>
        static bool PropertyHasNoSetter(PropertyInfo prop)
        {
            return prop.SetMethod == null;
        }

        /// <summary>
        ///     Gets the list method of collections.
        /// </summary>
        object GetListMethodOfCollections(Type type, Type genericArgumentType)
        {
            if (type.IsArray)
            {
                var elementType = type.GetElementType();
                if (elementType == null)
                {
                    throw new InvalidOperationException(type.FullName);
                }

                var array = ICollection(elementType).ToArray();

                var typedArray = System.Array.CreateInstance(elementType, array.Length);

                System.Array.Copy(array, 0, typedArray, 0, array.Length);

                return typedArray;
            }

            if (type.IsGenericType &&
                SupportedCollectionTypesForList.Contains(type.GetGenericTypeDefinition())
            )
            {
                var instance = (IList) Activator.CreateInstance(typeof(List<>).MakeGenericType(genericArgumentType));
                foreach (var item in ICollection(genericArgumentType))
                {
                    instance.Add(item);
                }

                return instance;
            }

            if (type.GetGenericTypeDefinition() == typeof(ObservableCollection<>))
            {
                var instance = (IList) Activator.CreateInstance(typeof(ObservableCollection<>).MakeGenericType(genericArgumentType));
                foreach (var item in (IEnumerable) IList(genericArgumentType))
                {
                    instance.Add(item);
                }

                return instance;
            }

            if (type.GetGenericTypeDefinition() == typeof(IList<>))
            {
                return IList(genericArgumentType);
            }

            if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Collection<>))
            {
                return ICollection(genericArgumentType).ToList();
            }

            if (type.GetGenericTypeDefinition() == typeof(ICollection<>))
            {
                return ICollection(genericArgumentType);
            }

            if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Dictionary<,>))
            {
                var genericArguments = type.GetGenericArguments();

                return IDictionary(genericArguments[0], genericArguments[1]);
            }

            if (type.GetGenericTypeDefinition() == typeof(IEnumerable<>))
            {
                return IEnumerable(genericArgumentType, null);
            }

            return null;
        }

        /// <summary>
        ///     Gets the type of the method call associated with.
        /// </summary>
        object GetMethodCallAssociatedWithType(Type propertyType)
        {
            var supportType = GetSupportType(propertyType);

            switch (supportType)
            {
                case SupportType.Basic:
                    return SupportedTypes[propertyType].Invoke(propertyType);
                case SupportType.Enum:
                    return Enum(propertyType);
                case SupportType.Collection:
                {
                    var collectionType = propertyType.IsArray
                        ? propertyType.GetElementType()
                        : propertyType.GetGenericArguments()[0];
                    return GetListMethodOfCollections(propertyType, collectionType);
                }

                case SupportType.Nullable:
                    return NullableMethodCall(propertyType);
                case SupportType.UserDefined:
                    return Object(propertyType);
                default:
                    return null;
            }
        }

        /// <summary>
        ///     is the collection.
        /// </summary>
        List<object> ICollection(Type type, int? optionalLength = null)
        {
            var numberOfItems = CreateRandomLengthIfOptionLengthIsNull(optionalLength);

            var enumerable = LazyIEnumerable(type).Take(numberOfItems);

            return enumerable.ToList();
        }

        /// <summary>
        ///     is the dictionary.
        /// </summary>
        IDictionary IDictionary(Type keyType, Type valueType, int? optionalLength = null)
        {
            var length = CreateRandomLengthIfOptionLengthIsNull(optionalLength);

            var keys = LazyIEnumerable(keyType).Distinct().Take(length);

            var values = ICollection(valueType, length);

            var dictionaryInstance = (IDictionary) Activator.CreateInstance(typeof(Dictionary<,>).MakeGenericType(keyType, valueType));

            var i = 0;
            foreach (var key in keys)
            {
                dictionaryInstance.Add(key, values[i++]);
            }

            return dictionaryInstance;
        }

        /// <summary>
        ///     is the enumerable.
        /// </summary>
        object IEnumerable(Type type, int? optionalLength)
        {
            var numberOfItems = CreateRandomLengthIfOptionLengthIsNull(optionalLength);

            return LazyIEnumerable(type).Take(numberOfItems);
        }

        /// <summary>
        ///     is the list.
        /// </summary>
        object IList(Type typeOfList, int? optionalLength = null)
        {
            return ICollection(typeOfList, optionalLength).ToList();
        }

        /// <summary>
        ///     Nullables the method call.
        /// </summary>
        object NullableMethodCall(Type propertyType)
        {
            var baseType = propertyType.GetGenericArguments()[0];
            return GetMethodCallAssociatedWithType(baseType);
        }

        /// <summary>
        ///     Tries the initialize property.
        /// </summary>
        void TryInitProperty<T>(PropertyInfo prop, T genericObject) where T : new()
        {
            if (PropertyHasNoSetter(prop))
            {
                return;
            }

            var isAbstract = false;

            if (prop.PropertyType.IsAbstract)
            {
                isAbstract = true;
                if (prop.PropertyType.IsGenericType)
                {
                    var genericTypeDefinition = prop.PropertyType.GetGenericTypeDefinition();

                    if (genericTypeDefinition != null)
                    {
                        if (IsSupportedCollection(genericTypeDefinition))
                        {
                            isAbstract = false;
                        }
                    }
                }
            }

            if (isAbstract)
            {
                return;
            }

            var countInCreationStack = _objectCreationStack.Count(item => item.GetType() == prop.PropertyType);
            if (countInCreationStack > 0)
            {
                return;
            }

            countInCreationStack = _objectCreationStack.Count(item => prop.PropertyType.Equals(item));
            if (countInCreationStack > 0)
            {
                return;
            }

            var valueInObject = prop.GetValue(genericObject);
            var isValueType   = prop.PropertyType.IsValueType;

            if (isValueType)
            {
                var defaultValue = Activator.CreateInstance(prop.PropertyType);

                if (valueInObject != defaultValue)
                {
                    return;
                }
            }

            if (!isValueType && valueInObject != null)
            {
                return;
            }

            _objectCreationStack.Push(prop.PropertyType);

            var propertyValue = GetMethodCallAssociatedWithType(prop.PropertyType);

            _objectCreationStack.Pop();

            prop.SetValue(genericObject, propertyValue, null);

            PropertyValueAssigned?.Invoke(genericObject, new PropertyValueAssignedEventArgs(prop));
        }
        #endregion
    }

    /// <summary>
    ///     The property value assigned event arguments
    /// </summary>
    [Serializable]
    public class PropertyValueAssignedEventArgs : EventArgs
    {
        #region Constructors
        /// <summary>
        ///     Initializes a new instance of the <see cref="PropertyValueAssignedEventArgs" /> class.
        /// </summary>
        public PropertyValueAssignedEventArgs(PropertyInfo propertyInfo)
        {
            PropertyInfo = propertyInfo;
        }
        #endregion

        #region Public Properties
        /// <summary>
        ///     Gets the property information.
        /// </summary>
        public PropertyInfo PropertyInfo { get; private set; }
        #endregion
    }
}