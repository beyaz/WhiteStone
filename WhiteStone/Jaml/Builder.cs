using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Markup;
using BOA.Common.Helpers;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace BOA.Jaml
{
    #region Builder    
    /// <summary>
    ///     Json based xaml loder
    /// </summary>
    public class Builder
    {
        #region Fields
        Dictionary<string, object> _data;
        JObject                    _jObject;
        #endregion

        #region Constructors
        /// <summary>
        ///     Initializes a new instance of the <see cref="Builder" /> class.
        /// </summary>
        public Builder()
        {
            TypeFinder = new TypeFinder();
            Config     = new BuilderConfig();
        }
        #endregion

        #region Public Properties
        /// <summary>
        ///     Gets or sets the configuration.
        /// </summary>
        public BuilderConfig Config { get; set; }

        /// <summary>
        ///     Data context value of Element
        /// </summary>
        public object DataContext { get; set; }

        /// <summary>
        ///     Gets or sets the type finder.
        /// </summary>
        public ITypeFinder TypeFinder { get; set; }

        /// <summary>
        ///     Created element after build
        /// </summary>
        public UIElement View { get; set; }

        /// <summary>
        ///     Gets or sets the name of the view.
        /// </summary>
        public string ViewName { get; private set; }
        #endregion

        #region Properties
        IAddChild ViewAsIAddChild
        {
            get { return View as IAddChild; }
        }
        #endregion

        #region Public Methods
        /// <summary>
        ///     Builds this instance.
        /// </summary>
        public Builder Build()
        {
            Build(_jObject);

            TryToFireCreationCompletedHandler();

            return this;
        }

        /// <summary>
        ///     Clones this instance.
        /// </summary>
        public Builder Clone()
        {
            return new Builder
            {
                DataContext = DataContext,
                Config      = Config
            };
        }

        /// <summary>
        ///     Sets the json.
        /// </summary>
        public Builder SetJson(string value)
        {
            try
            {
                _jObject = (JObject) JsonConvert.DeserializeObject(value);
            }
            catch (Exception)
            {
                throw new ArgumentException("json parse error.");
            }

            return this;
        }
        #endregion

        #region Methods
        static UIElement CreateDefaultWpfElement(string wpfElementName)
        {
            var className   = "System.Windows.Controls." + wpfElementName;
            var controlType = typeof(FrameworkElement).Assembly.GetType(className, false, true);
            if (controlType != null)
            {
                return (UIElement) Activator.CreateInstance(controlType);
            }

            throw Errors.TypeNotFound(wpfElementName);
        }

        void Build(JObject jObject)
        {
            foreach (var property 
                in jObject.Properties())
            {
                object value  = property.Value;
                var    jValue = property.Value as JValue;

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

        Builder Clone(JObject jObject)
        {
            var builder = (Builder) Activator.CreateInstance(GetType());
            builder._jObject    = jObject;
            builder.DataContext = DataContext;
            builder.Config      = Config;

            return builder;
        }

        void ProcessGridRowsAndColumns(Grid grid)
        {
            if (_jObject.Properties().Any(p => p.Name.ToUpperEN() == "ROWS"))
            {
                var rowIndex = 0;
                foreach (var gridChild in grid.Children)
                {
                    var gridChildAsDpObject = (DependencyObject) gridChild;
                    var splitter            = gridChild as GridSplitter;
                    var isGridSplitter      = splitter != null;

                    RowDefinition rowDefinition = null;

                    var height = (double) gridChildAsDpObject.GetValue(FrameworkElement.HeightProperty);

                    if (!double.IsNaN(height))
                    {
                        rowDefinition = new RowDefinition {Height = new GridLength(height, GridUnitType.Pixel)};
                    }
                    else
                    {
                        var gravity = gridChildAsDpObject.GetValue(GravityProperty) as double?;
                        if (gravity != null)
                        {
                            rowDefinition = new RowDefinition {Height = new GridLength(gravity.Value, GridUnitType.Star)};
                        }
                        else
                        {
                            var heightIsAuto = gridChildAsDpObject.GetValue(HeightIsAutoProperty) as bool?;
                            if (heightIsAuto == true || isGridSplitter)
                            {
                                rowDefinition = new RowDefinition {Height = new GridLength(0, GridUnitType.Auto)};
                            }
                            else
                            {
                                rowDefinition = new RowDefinition {Height = new GridLength(1, GridUnitType.Pixel)};
                            }
                        }
                    }

                    grid.RowDefinitions.Add(rowDefinition);

                    gridChildAsDpObject.SetValue(Grid.RowProperty, rowIndex);

                    if (isGridSplitter)
                    {
                        if (double.IsNaN(splitter.Height))
                        {
                            splitter.Height = 5;
                        }

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
                    var gridChildAsDpObject = (DependencyObject) gridChild;
                    var splitter            = gridChild as GridSplitter;
                    var isGridSplitter      = splitter != null;

                    var width = (double) gridChildAsDpObject.GetValue(FrameworkElement.WidthProperty);

                    ColumnDefinition columnDefinition = null;

                    if (!double.IsNaN(width))
                    {
                        columnDefinition = new ColumnDefinition {Width = new GridLength(width, GridUnitType.Pixel)};
                    }
                    else
                    {
                        var gravity = gridChildAsDpObject.GetValue(GravityProperty) as double?;
                        if (gravity != null)
                        {
                            columnDefinition = new ColumnDefinition {Width = new GridLength(gravity.Value, GridUnitType.Star)};
                        }
                        else
                        {
                            var widthIsAuto = gridChildAsDpObject.GetValue(WidthIsAutoProperty) as bool?;
                            if (widthIsAuto == true || isGridSplitter)
                            {
                                columnDefinition = new ColumnDefinition {Width = new GridLength(0, GridUnitType.Auto)};
                            }
                            else
                            {
                                columnDefinition = new ColumnDefinition {Width = new GridLength(1, GridUnitType.Star)};
                            }
                        }
                    }

                    grid.ColumnDefinitions.Add(columnDefinition);

                    gridChildAsDpObject.SetValue(Grid.ColumnProperty, columnIndex);

                    if (isGridSplitter)
                    {
                        if (double.IsNaN(splitter.Width))
                        {
                            splitter.Width = 5;
                        }

                        splitter.SetValue(FrameworkElement.HorizontalAlignmentProperty, HorizontalAlignment.Stretch);
                    }

                    columnIndex++;
                }
            }
        }

        DependencyProperty SearchDependencyPropertyInView(string name)
        {
            var descriptor = DependencyPropertyDescriptor.FromName(name, View.GetType(), View.GetType());
            if (descriptor == null)
            {
                return null;
            }

            return descriptor.DependencyProperty;
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
        #endregion

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
                var fieldInfo = fe.GetType().GetPublicNonStaticField(context.ValueAsString);
                if (fieldInfo != null)
                {
                    fieldInfo.SetValue(fe,View);
                    return true;
                }

                fe.RegisterName(context.ValueAsString, View);
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
                    ViewAsIAddChild.AddChild(Clone((JObject) item).Build().View);
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
            var attributeName  = context.Name;
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
            var attributeName  = context.Name;
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

                var handlerMethodParameters = handlerMethod.GetParameters();
                if (handlerMethodParameters.Length == 0)
                {
                    var eventInfoAddMethod = eventInfo.GetAddMethod(true);
                    var eventParameters    = eventInfoAddMethod.GetParameters();
                    if (eventParameters.Length > 0)
                    {
                        if (eventInfo.EventHandlerType == typeof(KeyEventHandler))
                        {
                            var             dataContext = DataContext;
                            KeyEventHandler handler     = (s, e) => { handlerMethod.Invoke(dataContext, null); };
                            eventInfo.AddEventHandler(View, handler);
                            return true;
                        }

                        throw new NotImplementedException(attributeName);
                    }
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
        static readonly DependencyProperty WidthIsAutoProperty  = DependencyProperty.Register("WidthIsAuto", typeof(bool?), typeof(Builder));

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
                var fe = (FrameworkElement) View;

                fe.Margin = new Thickness(input.ValueToDouble, fe.Margin.Top, fe.Margin.Right, fe.Margin.Bottom);
                return true;
            }

            if (name == "MARGINTOP" || name == "TOPMARGIN")
            {
                var fe = (FrameworkElement) View;

                fe.Margin = new Thickness(fe.Margin.Left, input.ValueToDouble, fe.Margin.Right, fe.Margin.Bottom);
                return true;
            }

            if (name == "MARGINRIGHT" || name == "RIGHTMARGIN")
            {
                var fe = (FrameworkElement) View;

                fe.Margin = new Thickness(fe.Margin.Left, fe.Margin.Top, input.ValueToDouble, fe.Margin.Bottom);
                return true;
            }

            if (name == "MARGINBOTTOM" || name == "BOTTOMMARGIN")
            {
                var fe = (FrameworkElement) View;

                fe.Margin = new Thickness(fe.Margin.Left, fe.Margin.Top, fe.Margin.Right, input.ValueToDouble);
                return true;
            }

            return false;
        }

        void Assign(string attributeName, object attributeValue)
        {
            var context = new Assignment {Builder = this, Name = attributeName, Value = attributeValue};
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

            return (TValueType) value;
        }
        #endregion
    }
    #endregion

    #region BuilderUtility
    #endregion
}