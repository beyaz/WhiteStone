using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Markup;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace WhiteStone
{
    namespace Jaml
    {
        #region ITypeFinder
        /// <summary>
        ///     Defines the i type finder.
        /// </summary>
        public interface ITypeFinder
        {
            /// <summary>
            ///     Finds the specified type full name.
            /// </summary>
            Type Find(string typeFullName);
        }

        /// <summary>
        ///     Defines the type finder.
        /// </summary>
        public class TypeFinder : ITypeFinder
        {
            const char Dot = '.';

            static readonly Dictionary<string, Type> QualifiedTypeNames = new Dictionary<string, Type>
            {
                {"OBJECT", typeof(object)},
                {"STRING", typeof(string)},
                {"BOOL", typeof(bool)},
                {"BYTE", typeof(byte)},
                {"CHAR", typeof(char)},
                {"DECIMAL", typeof(decimal)},
                {"DOUBLE", typeof(double)},
                {"SHORT", typeof(short)},
                {"INT", typeof(int)},
                {"LONG", typeof(long)},
                {"SBYTE", typeof(sbyte)},
                {"FLOAT", typeof(float)},
                {"USHORT", typeof(ushort)},
                {"UINT", typeof(uint)},
                {"ULONG", typeof(ulong)},
                {"VOID", typeof(void)}
            };

            /// <summary>
            ///     Finds the specified type full name.
            /// </summary>
            public Type Find(string typeFullName)
            {
                var isQualifiedName = typeFullName.IndexOf(Dot) == -1;
                if (isQualifiedName)
                {
                    typeFullName = typeFullName.ToUpperEN();

                    Type returnValue = null;
                    QualifiedTypeNames.TryGetValue(typeFullName, out returnValue);
                    return returnValue;
                }

                var type = Type.GetType(typeFullName);
                if (type != null) return type;
                foreach (var a in AppDomain.CurrentDomain.GetAssemblies())
                {
                    type = a.GetType(typeFullName);
                    if (type != null)
                    {
                        return type;
                    }
                }

                var assemblyLocation = GetType().Assembly.Location;
                if (assemblyLocation != null)
                {
                    var searchDirectory = Directory.GetParent(assemblyLocation).FullName + Path.DirectorySeparatorChar;
                    foreach (var fileInfo in GetDotNetFiles(searchDirectory))
                    {
                        try
                        {
                            var a = Assembly.LoadFile(fileInfo.FullName); 
                            type = a.GetType(typeFullName);
                            if (type != null)
                            {
                                return type;
                            }
                        }
                        catch (Exception)
                        {
                            // ignored
                        }
                    }
                }

                return null;
            }

            static IEnumerable<FileInfo> GetDotNetFiles(string directory)
            {
                return new DirectoryInfo(directory).GetFiles().Where(f => f.Extension == ".dll" || f.Extension == ".exe");
            }
        }
        #endregion

        #region Config
        /// <summary>
        ///     Configuration of builder.
        /// </summary>
        public class BuilderConfig
        {
            readonly List<Func<Assignment, bool>> _customPropertyHandlers = new List<Func<Assignment, bool>>();
            readonly List<Action<Builder>> _creationCompletedHandlers = new List<Action<Builder>>();
            readonly List<Action<Builder>> _tryToCreateElement = new List<Action<Builder>>();

            /// <summary>
            ///     Called when [custom property].
            /// </summary>
            public BuilderConfig OnCustomProperty(Func<Assignment, bool> execute)
            {
                _customPropertyHandlers.Add(execute);
                return this;
            }

            /// <summary>
            ///     Called when [creation completed].
            /// </summary>
            public BuilderConfig OnCreationCompleted(Action<Builder> action)
            {
                _creationCompletedHandlers.Add(action);
                return this;
            }

            /// <summary>
            ///     Tries to create element.
            /// </summary>
            public BuilderConfig TryToCreateElement(Action<Builder> action)
            {
                _tryToCreateElement.Add(action);
                return this;
            }

            internal bool TryToInvokeCustomProperty(Assignment input)
            {
                foreach (var fn in _customPropertyHandlers)
                {
                    var isHandled = fn(input);
                    if (isHandled)
                    {
                        return true;
                    }
                }

                return false;
            }

            internal void TryToFireCreationCompletedHandlers(Builder builder)
            {
                foreach (var fn in _creationCompletedHandlers)
                {
                    fn(builder);
                }
            }

            internal void TryToCreateElement(Builder builder)
            {
                foreach (var fn in _tryToCreateElement)
                {
                    fn(builder);
                    if (builder.View != null)
                    {
                        return;
                    }
                }
            }
        }
        #endregion

        #region Errors
        /// <summary>
        ///     Error codes in this type
        /// </summary>
        public static class Errors
        {
            /// <summary>
            ///     Types the not found.
            /// </summary>
            public static Exception TypeNotFound(string typeName)
            {
                return new ArgumentException(typeName + " not found ");
            }

            /// <summary>
            ///     Properties the not found.
            /// </summary>
            public static Exception PropertyNotFound(string propertyName, Type type)
            {
                return new ArgumentException(propertyName + " not found in " + type.FullName);
            }

            /// <summary>
            ///     Dependencies the property not found.
            /// </summary>
            public static Exception DependencyPropertyNotFound(string propertyName)
            {
                return new ArgumentException("DependencyPropertyNotFound:" + propertyName);
            }

            /// <summary>
            ///     Parsings the error.
            /// </summary>
            public static Exception ParsingError(string message)
            {
                return new ArgumentException("ParsingError:" + message);
            }

            /// <summary>
            ///     Methods the not found.
            /// </summary>
            public static Exception MethodNotFound(string methodName, Type type)
            {
                return new ArgumentException(methodName + " not found in " + type.FullName);
            }
        }
        #endregion

        #region TypeFinderExtensions
        /// <summary>
        ///     Defines the type finder extensions.
        /// </summary>
        public static class TypeFinderExtensions
        {
            /// <summary>
            ///     Gets the type.
            ///     <para>if given typeName not found then throws Exception</para>
            /// </summary>
            public static Type GetType(this ITypeFinder finder, string fullTypeName)
            {
                var type = finder.Find(fullTypeName);
                if (type == null)
                {
                    throw Errors.TypeNotFound(fullTypeName);
                }
                return type;
            }
        }
        #endregion

        #region Assignment
        /// <summary>
        ///     Defines the assignment.
        /// </summary>
        public class Assignment
        {
            /// <summary>
            ///     Gets the builder.
            /// </summary>
            public Builder Builder { get; internal set; }

            /// <summary>
            ///     Gets or sets the name.
            /// </summary>
            public string Name { get; set; }

            /// <summary>
            ///     Gets or sets the value.
            /// </summary>
            public object Value { get; set; }

            /// <summary>
            ///     Gets the value as string.
            /// </summary>
            public string ValueAsString
            {
                get { return Value as string; }
            }

            /// <summary>
            ///     Gets the value to double.
            /// </summary>
            public double ValueToDouble
            {
                get { return Value.ToDouble(); }
            }

            /// <summary>
            ///     Gets the property name to upper in english.
            /// </summary>
            public string NameToUpperInEnglish
            {
                get { return Name.ToUpperEN(); }
            }
        }
        #endregion

        #region Builder    
        /// <summary>
        ///     Json based xaml loder
        /// </summary>
        public class Builder
        {
            #region Fields
            JObject _jObject;
            Dictionary<string, object> _data;
            #endregion

            /// <summary>
            ///     Gets or sets the type finder.
            /// </summary>
            public ITypeFinder TypeFinder { get; set; }

            /// <summary>
            ///     Gets or sets the configuration.
            /// </summary>
            public BuilderConfig Config { get; set; }

            /// <summary>
            ///     Sets the json.
            /// </summary>
            public Builder SetJson(string value)
            {
                _jObject = (JObject)JsonConvert.DeserializeObject(value);
                return this;
            }

            /// <summary>
            ///     Data context value of Element
            /// </summary>
            public object DataContext { get; set; }

            /// <summary>
            ///     Created element after build
            /// </summary>
            public UIElement View { get; set; }

            /// <summary>
            ///     Gets or sets the name of the view.
            /// </summary>
            public string ViewName { get; private set; }

            IAddChild ViewAsIAddChild
            {
                get { return View as IAddChild; }
            }

            #region Constructor
            /// <summary>
            ///     Initializes a new instance of the <see cref="Builder" /> class.
            /// </summary>
            public Builder()
            {
                TypeFinder = new TypeFinder();
                Config = new BuilderConfig();
            }
            #endregion

            Builder Clone(JObject jObject)
            {
                var builder = (Builder)Activator.CreateInstance(GetType());
                builder._jObject = jObject;
                builder.DataContext = DataContext;
                builder.Config = Config;

                return builder;
            }

            /// <summary>
            ///     Clones this instance.
            /// </summary>
            public Builder Clone()
            {
                return new Builder
                {
                    DataContext = DataContext,
                    Config = Config
                };
            }

            /// <summary>
            ///     Builds this instance.
            /// </summary>
            public Builder Build()
            {
                Build(_jObject);

                TryToFireCreationCompletedHandler();

                return this;
            }

            void Build(JObject jObject)
            {
                foreach (var property in jObject.Properties())
                {
                    object value = property.Value;
                    var jValue = property.Value as JValue;

                    var propertyValueAsJObject = property.Value as JObject;
                    if (jValue != null)
                    {
                        value = jValue.Value;
                    }
                    else if (propertyValueAsJObject != null)
                    {
                        value = Clone(propertyValueAsJObject).Build().View;
                    }
                    else
                    {
                        var jArray = property.Value as JArray;
                        if (jArray != null)
                        {
                            value = jArray.ToArray();
                        }
                    }

                    Assign(property.Name, value);
                }
            }

            void ProcessGridRowsAndColumns(Grid grid)
            {
                if (_jObject.Properties().Any(p => p.Name.ToUpperEN() == "ROWS"))
                {
                    var rowIndex = 0;
                    foreach (var gridChild in grid.Children)
                    {
                        var gridChildAsDpObject = (DependencyObject)gridChild;

                        RowDefinition rowDefinition = null;

                        var height = (double)gridChildAsDpObject.GetValue(FrameworkElement.HeightProperty);

                        if (!double.IsNaN(height))
                        {
                            rowDefinition = new RowDefinition { Height = new GridLength(height, GridUnitType.Pixel) };
                        }
                        else
                        {
                            var gravity = gridChildAsDpObject.GetValue(GravityProperty) as double?;
                            if (gravity != null)
                            {
                                rowDefinition = new RowDefinition { Height = new GridLength(gravity.Value, GridUnitType.Star) };
                            }
                            else
                            {
                                var heightIsAuto = gridChildAsDpObject.GetValue(HeightIsAutoProperty) as bool?;
                                if (heightIsAuto == true)
                                {
                                    rowDefinition = new RowDefinition { Height = new GridLength(0, GridUnitType.Auto) };
                                }
                                else
                                {
                                    rowDefinition = new RowDefinition { Height = new GridLength(1, GridUnitType.Pixel) };
                                }
                            }
                        }

                        grid.RowDefinitions.Add(rowDefinition);

                        gridChildAsDpObject.SetValue(Grid.RowProperty, rowIndex);

                        var splitter = gridChild as GridSplitter;
                        if (splitter != null)
                        {
                            splitter.SetValue(FrameworkElement.HorizontalAlignmentProperty, HorizontalAlignment.Stretch);
                        }

                        rowIndex++;
                    }
                }

                if (_jObject.Properties().Any(p => p.Name.ToUpperEN() == "COLUMNS" || p.Name.ToUpperEN() == "COLS"))
                {
                    var columnIndex = 0;
                    foreach (var gridChild in grid.Children)
                    {
                        var gridChildAsDpObject = (DependencyObject)gridChild;

                        var width = (double)gridChildAsDpObject.GetValue(FrameworkElement.WidthProperty);

                        ColumnDefinition columnDefinition = null;

                        if (!double.IsNaN(width))
                        {
                            columnDefinition = new ColumnDefinition { Width = new GridLength(width, GridUnitType.Pixel) };
                        }
                        else
                        {
                            var gravity = gridChildAsDpObject.GetValue(GravityProperty) as double?;
                            if (gravity != null)
                            {
                                columnDefinition = new ColumnDefinition { Width = new GridLength(gravity.Value, GridUnitType.Star) };
                            }
                            else
                            {
                                var widthIsAuto = gridChildAsDpObject.GetValue(WidthIsAutoProperty) as bool?;
                                if (widthIsAuto == true)
                                {
                                    columnDefinition = new ColumnDefinition { Width = new GridLength(0, GridUnitType.Auto) };
                                }
                                else
                                {
                                    columnDefinition = new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) };
                                }
                            }
                        }

                        grid.ColumnDefinitions.Add(columnDefinition);

                        gridChildAsDpObject.SetValue(Grid.ColumnProperty, columnIndex);

                        var splitter = gridChild as GridSplitter;
                        if (splitter != null)
                        {
                            splitter.SetValue(FrameworkElement.HorizontalAlignmentProperty, HorizontalAlignment.Stretch);
                        }

                        columnIndex++;
                    }
                }
            }

            void TryToFireCreationCompletedHandler()
            {
                var grid = View as Grid;
                if (grid != null)
                {
                    ProcessGridRowsAndColumns(grid);
                }

                Config.TryToFireCreationCompletedHandlers(this);
            }

            bool TryToInvokeCustomProperty(Assignment context)
            {
                return Config.TryToInvokeCustomProperty(context);
            }

            static UIElement CreateDefaultWpfElement(string wpfElementName)
            {
                var className = "System.Windows.Controls." + wpfElementName;
                var controlType = typeof(FrameworkElement).Assembly.GetType(className, false, true);
                if (controlType != null)
                {
                    return (UIElement)Activator.CreateInstance(controlType);
                }

                throw Errors.TypeNotFound(wpfElementName);
            }

            #region Assignment
            bool TryToSetView(Assignment context)
            {
                if (context.Name.ToUpper(new CultureInfo("EN-us")) == "VIEW")
                {
                    ViewName = context.ValueAsString.ToUpperEN();
                    Config.TryToCreateElement(this);
                    if (View == null)
                    {
                        View = CreateDefaultWpfElement(context.ValueAsString);
                    }

                    return true;
                }

                return false;
            }

            bool TryToSetName(Assignment context)
            {
                var fe = DataContext as FrameworkElement;
                if (context.Name.ToUpperEN() == "NAME" && fe != null)
                {
                    fe.RegisterName(context.Value.ToString(), View);
                    return true;
                }

                return false;
            }

            bool TryToHandleStackPanelTitle(Assignment context)
            {
                if (context.Name.ToUpperEN() == "TITLE" && View is StackPanel)
                {
                    View.SetValue(HeaderedContentControl.HeaderProperty, context.Value);
                    return true;
                }

                return false;
            }

            static readonly DependencyProperty GravityProperty = DependencyProperty.Register("Gravity", typeof(double?), typeof(Builder));

            bool TryToHandleGravity(Assignment context)
            {
                if (context.Name.ToUpperEN() == "GRAVITY")
                {
                    View.SetValue(GravityProperty, context.Value.ToDouble());
                    return true;
                }

                return false;
            }

            bool TryToHandleArrayValues(Assignment context)
            {
                var attributeValueAsConfigArray = context.Value as Array;

                if (ViewAsIAddChild != null && attributeValueAsConfigArray != null)
                {
                    foreach (var item in attributeValueAsConfigArray)
                    {
                        ViewAsIAddChild.AddChild(Clone((JObject)item).Build().View);
                    }

                    return true;
                }

                return false;
            }

            bool TryToBindingExpression(Assignment context)
            {
                var attributeValueAsString = context.Value as string;
                if (attributeValueAsString.IsBindingExpression())
                {
                    var dp = SearchDependencyPropertyInView(context.Name);
                    if (dp == null)
                    {
                        throw Errors.DependencyPropertyNotFound(context.Name);
                    }

                    var binding = attributeValueAsString.ConvertToBinding(TypeFinder);
                    binding.Source = DataContext;

                    BindingOperations.SetBinding(View, dp, binding);
                    return true;
                }

                return false;
            }

            bool TryToHandleDProperty(Assignment context)
            {
                var attributeName = context.Name;
                var attributeValue = context.Value;

                // BOA.UI.EditorBase.LabelWidthProperty
                if (attributeName.Contains('.')) // dependency property
                {
                    var dp = BuilderUtility.SearchDependencyProperty(attributeName, TypeFinder);
                    if (dp == null)
                    {
                        throw Errors.DependencyPropertyNotFound(attributeName);
                    }

                    attributeValue = BuilderUtility.NormalizeValueForType(attributeValue, dp.PropertyType);

                    View.SetValue(dp, attributeValue);

                    return true;
                }

                return false;
            }

            bool TryToInvokeDefaultProperty(Assignment context)
            {
                var attributeName = context.Name;
                var attributeValue = context.Value;

                var property = View.GetType().GetProperty(attributeName);
                if (property == null)
                {
                    var eventInfo = View.GetType().GetEvent(attributeName);
                    if (eventInfo == null)
                    {
                        throw Errors.PropertyNotFound(attributeName, View.GetType());
                    }

                    var handlerMethod = DataContext.GetType().GetMethod(attributeValue.ToString());
                    if (handlerMethod == null)
                    {
                        throw Errors.MethodNotFound(attributeValue.ToString(), DataContext.GetType());
                    }
                    eventInfo.AddEventHandler(View, handlerMethod.CreateDelegate(eventInfo.EventHandlerType, DataContext));
                    return true;
                }

                var converter = TypeDescriptor.GetConverter(property.PropertyType);

                if (converter.CanConvertFrom(attributeValue.GetType()))
                {
                    attributeValue = converter.ConvertFrom(attributeValue);
                }

                property.SetValue(View, attributeValue);

                return true;
            }

            static readonly DependencyProperty HeightIsAutoProperty = DependencyProperty.Register("HeightIsAuto", typeof(bool?), typeof(Builder));
            static readonly DependencyProperty WidthIsAutoProperty = DependencyProperty.Register("WidthIsAuto", typeof(bool?), typeof(Builder));

            bool TryToHandleHeightPropertyAutoValue(Assignment context)
            {
                if (context.Name.ToUpperEN() == "HEIGHT" && context.Value.ToString().ToUpperEN() == "AUTO")
                {
                    View.SetValue(FrameworkElement.HeightProperty, double.NaN);
                    View.SetValue(HeightIsAutoProperty, true);
                    return true;
                }

                return false;
            }

            bool TryToHandleWidthPropertyAutoValue(Assignment context)
            {
                if (context.Name.ToUpperEN() == "WIDTH" && context.Value.ToString().ToUpperEN() == "AUTO")
                {
                    View.SetValue(FrameworkElement.WidthProperty, double.NaN);
                    View.SetValue(WidthIsAutoProperty, true);
                    return true;
                }

                return false;
            }

            bool TryToHandleMarginValues(Assignment input)
            {
                var name = input.Name.ToUpperEN();

                if (name == "MARGINLEFT" || name == "LEFTMARGIN")
                {
                    var fe = (FrameworkElement)View;

                    fe.Margin = new Thickness(input.ValueToDouble, fe.Margin.Top, fe.Margin.Right, fe.Margin.Bottom);
                    return true;
                }
                if (name == "MARGINTOP" || name == "TOPMARGIN")
                {
                    var fe = (FrameworkElement)View;

                    fe.Margin = new Thickness(fe.Margin.Left, input.ValueToDouble, fe.Margin.Right, fe.Margin.Bottom);
                    return true;
                }

                if (name == "MARGINRIGHT" || name == "RIGHTMARGIN")
                {
                    var fe = (FrameworkElement)View;

                    fe.Margin = new Thickness(fe.Margin.Left, fe.Margin.Top, input.ValueToDouble, fe.Margin.Bottom);
                    return true;
                }

                if (name == "MARGINBOTTOM" || name == "BOTTOMMARGIN")
                {
                    var fe = (FrameworkElement)View;

                    fe.Margin = new Thickness(fe.Margin.Left, fe.Margin.Top, fe.Margin.Right, input.ValueToDouble);
                    return true;
                }

                return false;
            }

            void Assign(string attributeName, object attributeValue)
            {
                var context = new Assignment { Builder = this, Name = attributeName, Value = attributeValue };
                var tuple = new Func<Assignment, bool>[]
                {
                    TryToSetView,
                    TryToSetName,
                    TryToHandleGravity,
                    TryToHandleStackPanelTitle,
                    TryToHandleArrayValues,
                    TryToInvokeCustomProperty,
                    TryToBindingExpression,
                    TryToHandleDProperty,
                    TryToHandleMarginValues,
                    TryToHandleHeightPropertyAutoValue,
                    TryToHandleWidthPropertyAutoValue,
                    TryToInvokeDefaultProperty
                };

                foreach (var func in tuple)
                {
                    if (func(context))
                    {
                        return;
                    }
                }
            }
            #endregion

            DependencyProperty SearchDependencyPropertyInView(string name)
            {
                var descripter = DependencyPropertyDescriptor.FromName(name, View.GetType(), View.GetType());
                if (descripter == null)
                {
                    return null;
                }
                return descripter.DependencyProperty;
            }

            #region Data
            /// <summary>
            ///     Sets the data.
            /// </summary>
            public TValue SetData<TValue>(string key, TValue value)
            {
                if (_data == null)
                {
                    _data = new Dictionary<string, object>();
                }

                _data[key] = value;

                return value;
            }

            /// <summary>
            ///     Gets the data.
            /// </summary>
            public TValueType GetData<TValueType>(string key)
            {
                if (_data == null)
                {
                    return default(TValueType);
                }

                object value = null;

                _data.TryGetValue(key, out value);

                return (TValueType)value;
            }
            #endregion
        }
        #endregion

        #region BuilderUtility
        /// <summary>
        ///     Defines the builder utility.
        /// </summary>
        public static class BuilderUtility
        {
            /// <summary>
            ///     Determines whether value is BindingExpression.
            /// </summary>
            public static bool IsBindingExpression(this string value)
            {
                if (value == null)
                {
                    return false;
                }
                if (value.Replace(" ", "").StartsWith("{Binding", StringComparison.Ordinal))
                {
                    return true;
                }
                value = value.Trim();

                if (value.StartsWith("{", StringComparison.OrdinalIgnoreCase) && value.EndsWith("}", StringComparison.OrdinalIgnoreCase))
                {
                    return true;
                }

                return false;
            }

            /// <summary>
            ///     Gets the binding.
            /// </summary>
            public static Binding ConvertToBinding(this string bindingExpressionAsText, ITypeFinder typeFinder)
            {
                var binding = new Binding
                {
                    Mode = BindingMode.TwoWay
                };
                var list = bindingExpressionAsText.Split('{', '}', '=', ',', ' ').Where(x => !string.IsNullOrWhiteSpace(x)).Select(x => x.Trim()).ToList();

                var pathIndex = list.FindIndex(x => x.Contains("Binding")) + 1;
                if (list[pathIndex] == "Path")
                {
                    pathIndex += 1;
                }

                binding.Path = new PropertyPath(list[pathIndex]);

                var modeIndex = list.FindIndex(x => x == "Mode");
                if (modeIndex > 0)
                {
                    var mode = BindingMode.Default;

                    var isParsed = Enum.TryParse(list[modeIndex + 1], true, out mode);
                    if (!isParsed)
                    {
                        throw Errors.ParsingError(list[modeIndex + 1]);
                    }
                    binding.Mode = mode;
                }

                var converterIndex = list.FindIndex(x => x == "Converter");
                if (converterIndex > 0)
                {
                    var converterTypeName = list[converterIndex + 1];
                    var converterType = typeFinder.GetType(converterTypeName);
                    binding.Converter = (IValueConverter)Activator.CreateInstance(converterType);
                }

                var updateSourceTriggerIndex = list.FindIndex(x => x == "UpdateSourceTrigger");
                if (updateSourceTriggerIndex > 0)
                {
                    var updateSourceTriggerValue = list[updateSourceTriggerIndex + 1];

                    binding.UpdateSourceTrigger = (UpdateSourceTrigger)Enum.Parse(typeof(UpdateSourceTrigger), updateSourceTriggerValue, true);
                }

                return binding;
            }

            /// <summary>
            ///     Searches the dependency property in view.
            /// </summary>
            public static DependencyProperty SearchDependencyProperty(string dpFullName, ITypeFinder typeFinder)
            {
                var lastDotIndex = dpFullName.LastIndexOf('.');
                var propertyName = dpFullName.Substring(lastDotIndex + 1);

                var fieldInfo = typeFinder.GetType(dpFullName.Substring(0, lastDotIndex))
                                          .GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.FlattenHierarchy)
                                          .FirstOrDefault(p => p.FieldType == typeof(DependencyProperty) &&
                                                               p.Name == propertyName);

                if (fieldInfo == null)
                {
                    return null;
                }

                return (DependencyProperty)fieldInfo.GetValue(null);
            }

            /// <summary>
            ///     Normalizes the given <paramref name="value" /> for given <paramref name="targetType" />.
            ///     <para>Example</para>
            ///     <para>5(long) , typeof(int) returns 5(int)</para>
            /// </summary>
            public static object NormalizeValueForType(object value, Type targetType)
            {
                var valueConverter = TypeDescriptor.GetConverter(targetType);

                // special case
                if (valueConverter is DoubleConverter)
                {
                    if (value is long || value is int)
                    {
                        value = value.ToString();
                    }
                }

                if (valueConverter is NullableConverter)
                {
                    if (value is long)
                    {
                        value = value.ToString();
                    }
                }

                if (valueConverter.CanConvertFrom(value.GetType()))
                {
                    value = valueConverter.ConvertFrom(value);
                }

                return value;
            }
        }
        #endregion
    }
}