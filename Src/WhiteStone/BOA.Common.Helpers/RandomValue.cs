using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;

namespace BOA.Common.Helpers
{
    //https://github.com/RasicN/random-test-values
    /// <summary>
    ///     Defines the random value.
    /// </summary>
    public static class RandomValue
    {
        #region Static Fields
        /// <summary>
        ///     The object creation stack
        /// </summary>
        internal static readonly Stack<object> _objectCreationStack = new Stack<object>();

        /// <summary>
        ///     The random.
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

        #region Enums
        /// <summary>
        ///     Defines the support type.
        /// </summary>
        internal enum SupportType
        {
            /// <summary>
            ///     The not supported.
            /// </summary>
            NotSupported,

            /// <summary>
            ///     The user defined.
            /// </summary>
            UserDefined,

            /// <summary>
            ///     The basic.
            /// </summary>
            Basic,

            /// <summary>
            ///     The enum.
            /// </summary>
            Enum,

            /// <summary>
            ///     The collection.
            /// </summary>
            Collection,

            /// <summary>
            ///     The nullable.
            /// </summary>
            Nullable
        }
        #endregion

        #region Public Methods
        /// <summary>
        ///     Arrays the specified optional length.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="optionalLength">Length of the optional.</param>
        /// <returns></returns>
        public static T[] Array<T>(int? optionalLength = null)
        {
            return Collection<T>(optionalLength).ToArray();
        }

        /// <summary>
        ///     Booleans this instance.
        /// </summary>
        /// <returns></returns>
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
        ///     Use for getting a random Byte for your unit tests.
        /// </summary>
        /// <param name="maxPossibleValue">The maximum possible value.</param>
        /// <returns>
        ///     A random Byte
        /// </returns>
        public static byte Byte(byte maxPossibleValue = byte.MaxValue)
        {
            return (byte) Int32(maxPossibleValue);
        }

        /// <summary>
        ///     Characters this instance.
        /// </summary>
        /// <returns></returns>
        public static char Char()
        {
            var buffer = new byte[sizeof(char)];

            _random.NextBytes(buffer);

            return BitConverter.ToChar(buffer, 0);
        }

        /// <summary>
        ///     Collections the specified optional length.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="optionalLength">Length of the optional.</param>
        /// <returns></returns>
        public static Collection<T> Collection<T>(int? optionalLength = null)
        {
            return (Collection<T>) ICollection<T>(optionalLength);
        }

        /// <summary>
        ///     Use for getting a random DateTimes for your unit tests. Always returns a date in the past.
        /// </summary>
        /// <param name="minDateTime">The minimum date time.</param>
        /// <param name="maxDateTime">The maximum date time.</param>
        /// <returns>
        ///     A random DateTime
        /// </returns>
        public static DateTime DateTime(DateTime? minDateTime = null, DateTime? maxDateTime = null)
        {
            if (minDateTime == null)
            {
                minDateTime = new DateTime(1610, 1, 7); //discovery of galilean moons. Using system.DateTime.Min just made weird looking dates.
            }

            if (maxDateTime == null)
            {
                maxDateTime = System.DateTime.Now;
            }

            var timeSinceStartOfDateTime = maxDateTime.Value - minDateTime.Value;
            var timeInHoursSinceStartOfDateTime = (int) timeSinceStartOfDateTime.TotalHours;
            var hoursToSubtract = Int32(timeInHoursSinceStartOfDateTime) * -1;
            var timeToReturn = maxDateTime.Value.AddHours(hoursToSubtract);

            if (timeToReturn > minDateTime.Value && timeToReturn < maxDateTime.Value)
            {
                return timeToReturn;
            }

            return System.DateTime.Now;
        }

        /// <summary>
        ///     Dates the time offset.
        /// </summary>
        /// <returns></returns>
        public static DateTimeOffset DateTimeOffset()
        {
            return new DateTimeOffset(DateTime());
        }

        /// <summary>
        ///     Use for getting a random Decimal for your unit test
        /// </summary>
        /// <param name="maxPossibleValue">Maximum decimal value, defaults to 1</param>
        /// <returns></returns>
        public static decimal Decimal(decimal maxPossibleValue = 1m)
        {
            return (decimal) _random.NextDouble() * maxPossibleValue;
        }

        /// <summary>
        ///     Dictionaries the specified optional length.
        /// </summary>
        /// <typeparam name="TKey">The type of the key.</typeparam>
        /// <typeparam name="TValue">The type of the value.</typeparam>
        /// <param name="optionalLength">Length of the optional.</param>
        /// <returns></returns>
        public static Dictionary<TKey, TValue> Dictionary<TKey, TValue>(int? optionalLength = null)
        {
            return (Dictionary<TKey, TValue>) IDictionary<TKey, TValue>(optionalLength);
        }

        /// <summary>
        ///     Doubles this instance.
        /// </summary>
        /// <returns></returns>
        public static double Double()
        {
            return _random.NextDouble();
        }

        /// <summary>
        ///     Enums this instance.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T Enum<T>()
        {
            var fields = typeof(T).GetRuntimeFields().Where(x => x.IsPublic && x.IsStatic).ToArray();

            var index = _random.Next(fields.Length);

            return (T) System.Enum.Parse(typeof(T), fields[index].Name, false);
        }

        /// <summary>
        ///     GUIDs this instance.
        /// </summary>
        /// <returns></returns>
        public static Guid Guid()
        {
            return System.Guid.NewGuid();
        }

        /// <summary>
        ///     is the collection.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="optionalLength">Length of the optional.</param>
        /// <returns></returns>
        public static ICollection<T> ICollection<T>(int? optionalLength = null)
        {
            var numberOfItems = CreateRandomLengthIfOptionLengthIsNull(optionalLength);

            var enumerable = LazyIEnumerable<T>().Take(numberOfItems);

            var randomList = new Collection<T>(enumerable.ToList());

            return randomList;
        }

        /// <summary>
        ///     is the dictionary.
        /// </summary>
        /// <typeparam name="TKey">The type of the key.</typeparam>
        /// <typeparam name="TValue">The type of the value.</typeparam>
        /// <param name="optionalLength">Length of the optional.</param>
        /// <returns></returns>
        public static IDictionary<TKey, TValue> IDictionary<TKey, TValue>(int? optionalLength = null)
        {
            var length = CreateRandomLengthIfOptionLengthIsNull(optionalLength);

            var keys = LazyIEnumerable<TKey>().Distinct().Take(length);

            var values = ICollection<TValue>(length);

            var keyValuePairs = keys.Zip(values, (key, value) => new KeyValuePair<TKey, TValue>(key, value));

            return keyValuePairs.ToDictionary(key => key.Key, value => value.Value);
        }

        /// <summary>
        ///     is the enumerable.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static IEnumerable<T> IEnumerable<T>()
        {
            return IEnumerable<T>(null);
        }

        /// <summary>
        ///     is the enumerable.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="optionalLength">Length of the optional.</param>
        /// <returns></returns>
        public static IEnumerable<T> IEnumerable<T>(int? optionalLength)
        {
            var numberOfItems = CreateRandomLengthIfOptionLengthIsNull(optionalLength);

            return LazyIEnumerable<T>().Take(numberOfItems);
        }

        /// <summary>
        ///     is the list.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="optionalLength">Length of the optional.</param>
        /// <returns></returns>
        public static IList<T> IList<T>(int? optionalLength = null)
        {
            return ICollection<T>(optionalLength).ToList();
        }

        /// <summary>
        ///     Int16s the specified maximum possible value.
        /// </summary>
        /// <param name="maxPossibleValue">The maximum possible value.</param>
        /// <returns></returns>
        public static short Int16(short maxPossibleValue = short.MaxValue)
        {
            return (short) Int32(maxPossibleValue);
        }

        /// <summary>
        ///     Int32s the specified maximum possible value.
        /// </summary>
        /// <param name="maxPossibleValue">The maximum possible value.</param>
        /// <param name="minPossibleValue">The minimum possible value.</param>
        /// <returns></returns>
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
        /// <param name="maxPossibleValue">The maximum possible value.</param>
        /// <returns></returns>
        public static long Int64(long maxPossibleValue = long.MaxValue)
        {
            return (long) UInt64((ulong) maxPossibleValue);
        }

        /// <summary>
        ///     Lazies the i enumerable.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static IEnumerable<T> LazyIEnumerable<T>()
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
        ///     Lists the specified optional length.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="optionalLength">Length of the optional.</param>
        /// <returns></returns>
        public static List<T> List<T>(int? optionalLength = null)
        {
            return ICollection<T>(optionalLength).ToList();
        }

        /// <summary>
        ///     Objects this instance.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T Object<T>() where T : new()
        {
            var genericObject = (T) Activator.CreateInstance(typeof(T));

            var properties = typeof(T).GetRuntimeProperties().ToArray();

            if (properties.Length == 0)
            {
                // Prevent infinite loop when called recursively
                return genericObject;
            }

            _objectCreationStack.Push(genericObject);

            foreach (var prop in properties)
            {
                if (PropertyHasNoSetter(prop))
                {
                    // Property doesn't have a public setter so let's ignore it
                    continue;
                }

                var countInCreationStack = _objectCreationStack.Count(item => item.GetType() == prop.PropertyType);
                if (countInCreationStack > 0) // for avoid infinite call
                {
                    continue;
                }

                countInCreationStack = _objectCreationStack.Count(item => prop.PropertyType.Equals(item));
                if (countInCreationStack > 0) // for avoid infinite call
                {
                    continue;
                }

                var valueInObject = prop.GetValue(genericObject);
                var isValueType = prop.PropertyType.IsValueType;

                if (isValueType)
                {
                    var defaultValue = Activator.CreateInstance(prop.PropertyType);

                    if (valueInObject != defaultValue)
                    {
                        continue;
                    }
                }

                if (!isValueType && valueInObject != null)
                {
                    continue;
                }

                _objectCreationStack.Push(prop.PropertyType);

                var propertyValue = GetMethodCallAssociatedWithType(prop.PropertyType);

                _objectCreationStack.Pop();

                prop.SetValue(genericObject, propertyValue, null);
            }

            _objectCreationStack.Pop();

            return genericObject;
        }

        /// <summary>
        ///     Objects the specified after random object created.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="afterRandomObjectCreated">The after random object created.</param>
        /// <returns></returns>
        public static T Object<T>(Action<T> afterRandomObjectCreated) where T : new()
        {
            var randomObject = Object<T>();

            afterRandomObjectCreated(randomObject);

            return randomObject;
        }

        /// <summary>
        ///     Observables the collection.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="optionalLength">Length of the optional.</param>
        /// <returns></returns>
        public static ObservableCollection<T> ObservableCollection<T>(int? optionalLength = null)
        {
            return new ObservableCollection<T>(List<T>(optionalLength));
        }

        /// <summary>
        ///     Use for getting a random Signed Byte for your unit tests.
        /// </summary>
        /// <param name="maxPossibleValue">The maximum possible value.</param>
        /// <returns>
        ///     A random (positive) Signed Byte
        /// </returns>
        public static sbyte SByte(sbyte maxPossibleValue = sbyte.MaxValue)
        {
            return (sbyte) Int32(maxPossibleValue);
        }

        /// <summary>
        ///     Singles this instance.
        /// </summary>
        /// <returns></returns>
        public static float Single()
        {
            return (float) _random.NextDouble();
        }

        /// <summary>
        ///     Use for getting a random string for your unit tests.  This is basically a Guid.ToString() so it will
        ///     not have any formatting and it will have "-"
        /// </summary>
        /// <returns>
        ///     A random string the length of a Guid
        /// </returns>
        public static string String()
        {
            return Guid().ToString();
        }

        /// <summary>
        ///     Use for getting a random string of a specific length for your unit tests.
        /// </summary>
        /// <param name="stringLength">Length of desired random string</param>
        /// <returns>
        ///     A random string the length of a stringLength parameter
        /// </returns>
        public static string String(int stringLength) //Where did the tests go for this method? 
        {
            var randomString = String();

            while (randomString.Length <= stringLength)
            {
                randomString += String();
            }

            return randomString.Substring(0, stringLength);
        }

        /// <summary>
        ///     Times the span.
        /// </summary>
        /// <returns></returns>
        public static TimeSpan TimeSpan()
        {
            var date1 = DateTime();
            var date2 = DateTime();

            return date1 > date2 ? date1.Subtract(date2) : date2.Subtract(date1);
        }

        /// <summary>
        ///     UInt16s the specified maximum possible value.
        /// </summary>
        /// <param name="maxPossibleValue">The maximum possible value.</param>
        /// <returns></returns>
        public static ushort UInt16(ushort maxPossibleValue = ushort.MaxValue)
        {
            return (ushort) Int32(maxPossibleValue);
        }

        /// <summary>
        ///     UInt32s the specified maximum possible value.
        /// </summary>
        /// <param name="maxPossibleValue">The maximum possible value.</param>
        /// <returns></returns>
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
        ///     UInt64s the specified maximum possible value.
        /// </summary>
        /// <param name="maxPossibleValue">The maximum possible value.</param>
        /// <returns></returns>
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
        #endregion

        #region Methods
        /// <summary>
        ///     Arrays the method call.
        /// </summary>
        /// <param name="typeOfList">The type of list.</param>
        /// <returns></returns>
        static object ArrayMethodCall(Type typeOfList)
        {
            return InvokeCollectionMethod("Array", typeOfList);
        }

        /// <summary>
        ///     Collections the method call.
        /// </summary>
        /// <param name="typeOfList">The type of list.</param>
        /// <returns></returns>
        static object CollectionMethodCall(Type typeOfList)
        {
            return InvokeCollectionMethod("Collection", typeOfList);
        }

        /// <summary>
        ///     Creates the random length if option length is null.
        /// </summary>
        /// <param name="optionalLength">Length of the optional.</param>
        /// <returns></returns>
        static int CreateRandomLengthIfOptionLengthIsNull(int? optionalLength)
        {
            return optionalLength ?? _random.Next(1, 10);
        }

        /// <summary>
        ///     Dictionaries the method call.
        /// </summary>
        /// <param name="genericTypeArguments">The generic type arguments.</param>
        /// <returns></returns>
        static object DictionaryMethodCall(Type[] genericTypeArguments)
        {
            var method = GetMethod("Dictionary");

            return method
                .MakeGenericMethod(genericTypeArguments[0], genericTypeArguments[1])
                .Invoke(null, new object[] {null});
        }

        /// <summary>
        ///     Enums the method call.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        static object EnumMethodCall(Type type)
        {
            return GetMethod("Enum")
                .MakeGenericMethod(type)
                .Invoke(null, new object[] { });
        }

        /// <summary>
        ///     Gets the list method of collections.
        /// </summary>
        /// <param name="propertyType">Type of the property.</param>
        /// <param name="genericType">Type of the generic.</param>
        /// <returns></returns>
        static object GetListMethodOfCollections(Type propertyType, Type genericType)
        {
            var typeOfList = genericType;

            object listMethod = null;

            var type = propertyType;

            if (propertyType.IsArray)
            {
                listMethod = ArrayMethodCall(propertyType.GetElementType());
            }
            else if (type.GetTypeInfo().IsGenericType &&
                     SupportedCollectionTypesForList.Contains(type.GetGenericTypeDefinition())
            )
            {
                listMethod = ListMethodCall(typeOfList);
            }
            else if (type.GetGenericTypeDefinition() == typeof(ObservableCollection<>))
            {
                listMethod = InvokeCollectionMethod("ObservableCollection", typeOfList);
            }
            else if (type.GetGenericTypeDefinition() == typeof(IList<>))
            {
                listMethod = IListMethodCall(typeOfList);
            }
            else if (type.GetTypeInfo().IsGenericType && type.GetGenericTypeDefinition() == typeof(Collection<>))
            {
                listMethod = CollectionMethodCall(typeOfList);
            }
            else if (type.GetGenericTypeDefinition() == typeof(ICollection<>))
            {
                listMethod = ICollectionMethodCall(typeOfList);
            }
            else if (type.GetTypeInfo().IsGenericType && type.GetGenericTypeDefinition() == typeof(Dictionary<,>))
            {
                listMethod = DictionaryMethodCall(type.GenericTypeArguments);
            }
            else if (type.GetGenericTypeDefinition() == typeof(IDictionary<,>))
            {
                listMethod = IDictionaryMethodCall(type.GenericTypeArguments);
            }
            else if (type.GetGenericTypeDefinition() == typeof(IEnumerable<>))
            {
                listMethod = IEnumerableMethodCall(typeOfList);
            }

            return listMethod;
        }

        /// <summary>
        ///     Gets the method.
        /// </summary>
        /// <param name="nameOfMethod">The name of method.</param>
        /// <returns></returns>
        static MethodInfo GetMethod(string nameOfMethod)
        {
            return typeof(RandomValue).GetRuntimeMethods().First(x => x.Name == nameOfMethod);
        }

        /// <summary>
        ///     Gets the type of the method call associated with.
        /// </summary>
        /// <param name="propertyType">Type of the property.</param>
        /// <returns></returns>
        static object GetMethodCallAssociatedWithType(Type propertyType)
        {
            var supportType = GetSupportType(propertyType);

            switch (supportType)
            {
                case SupportType.Basic:
                    return SupportedTypes[propertyType].Invoke(propertyType);
                case SupportType.Enum:
                    return EnumMethodCall(propertyType);
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
                    return ObjectMethodCall(propertyType);
                default:
                    return null;
            }
        }

        /// <summary>
        ///     Gets the type of the support.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        static SupportType GetSupportType(Type type)
        {
            if (SupportedTypes.ContainsKey(type))
            {
                return SupportType.Basic;
            }
            if (type.GetTypeInfo().IsEnum)
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
            if (type.GetTypeInfo().IsClass)
            {
                return SupportType.UserDefined;
            }

            return SupportType.NotSupported;
        }

        /// <summary>
        ///     is the collection method call.
        /// </summary>
        /// <param name="typeOfList">The type of list.</param>
        /// <returns></returns>
        static object ICollectionMethodCall(Type typeOfList)
        {
            return InvokeCollectionMethod("ICollection", typeOfList);
        }

        /// <summary>
        ///     is the dictionary method call.
        /// </summary>
        /// <param name="genericTypeArguments">The generic type arguments.</param>
        /// <returns></returns>
        static object IDictionaryMethodCall(Type[] genericTypeArguments)
        {
            var method = GetMethod("IDictionary");

            return method
                .MakeGenericMethod(genericTypeArguments[0], genericTypeArguments[1])
                .Invoke(null, new object[] {null});
        }

        /// <summary>
        ///     is the enumerable method call.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        static object IEnumerableMethodCall(Type type)
        {
            return GetMethod("IEnumerable")
                .MakeGenericMethod(type)
                .Invoke(null, new object[] { });
        }

        /// <summary>
        ///     is the list method call.
        /// </summary>
        /// <param name="typeOfList">The type of list.</param>
        /// <returns></returns>
        static object IListMethodCall(Type typeOfList)
        {
            return InvokeCollectionMethod("IList", typeOfList);
        }

        /// <summary>
        ///     Invokes the collection method.
        /// </summary>
        /// <param name="nameOfMethod">The name of method.</param>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        static object InvokeCollectionMethod(string nameOfMethod, Type type)
        {
            var method = GetMethod(nameOfMethod);

            return method
                .MakeGenericMethod(type)
                .Invoke(null, new object[] {null});
        }

        /// <summary>
        ///     Determines whether [is nullable type] [the specified property type].
        /// </summary>
        /// <param name="propertyType">Type of the property.</param>
        /// <returns>
        ///     <c>true</c> if [is nullable type] [the specified property type]; otherwise, <c>false</c>.
        /// </returns>
        static bool IsNullableType(Type propertyType)
        {
            return propertyType.GetTypeInfo().IsGenericType && propertyType.GetGenericTypeDefinition() == typeof(Nullable<>);
        }

        /// <summary>
        ///     Determines whether [is supported collection] [the specified property type].
        /// </summary>
        /// <param name="propertyType">Type of the property.</param>
        /// <returns>
        ///     <c>true</c> if [is supported collection] [the specified property type]; otherwise, <c>false</c>.
        /// </returns>
        static bool IsSupportedCollection(Type propertyType)
        {
            var hasImplementedICollection = propertyType.GetTypeInfo().ImplementedInterfaces.Any(x => x.Name == "ICollection");

            return hasImplementedICollection
                   || propertyType.GetTypeInfo().IsGenericType &&
                   (propertyType.IsArray
                    || propertyType.GetGenericTypeDefinition() == typeof(ICollection<>)
                    || propertyType.GetGenericTypeDefinition() == typeof(IList<>)
                    || propertyType.GetGenericTypeDefinition() == typeof(IEnumerable<>)
                    || propertyType.GetGenericTypeDefinition() == typeof(IDictionary<,>)
                    || SupportedCollectionTypesForList.Contains(propertyType.GetGenericTypeDefinition())
                   );
        }

        /// <summary>
        ///     Lists the method call.
        /// </summary>
        /// <param name="typeOfList">The type of list.</param>
        /// <returns></returns>
        static object ListMethodCall(Type typeOfList)
        {
            return InvokeCollectionMethod("List", typeOfList);
        }

        /// <summary>
        ///     Nullables the method call.
        /// </summary>
        /// <param name="propertyType">Type of the property.</param>
        /// <returns></returns>
        static object NullableMethodCall(Type propertyType)
        {
            var baseType = propertyType.GetGenericArguments()[0];
            return GetMethodCallAssociatedWithType(baseType);
        }

        /// <summary>
        ///     Objects the method call.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        static object ObjectMethodCall(Type type)
        {
            var mi = typeof(RandomValue).GetRuntimeMethods()
                                        .First(x => x.Name == "Object" && x.GetParameters()
                                                                           .FirstOrDefault() == null);

            mi = mi.MakeGenericMethod(type);
            return mi.Invoke(null, new object[] { });
        }

        /// <summary>
        ///     Properties the has no setter.
        /// </summary>
        /// <param name="prop">The property.</param>
        /// <returns></returns>
        static bool PropertyHasNoSetter(PropertyInfo prop)
        {
            return prop.SetMethod == null;
        }
        #endregion
    }
}