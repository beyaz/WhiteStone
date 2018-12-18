using System;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Markup;
using BOA.Common.Helpers;
using BOA.Jaml.Markup;

namespace BOA.Jaml
{
    #region Builder    
    /// <summary>
    ///     Json based xaml loder
    /// </summary>
    public class Builder
    {
        #region Fields
        Node _node;
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
            Build(_node);

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
        public Builder SetJson(string jsonString)
        {
            try
            {
                _node = TransformHelper.Transform(jsonString);
            }
            catch (Exception e)
            {
                throw new ArgumentException("json parse error.", e);
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

        static bool TryToInvokeCustomProperty(Builder builder, Node node)
        {
            return builder.Config.TryToInvokeCustomProperty(builder, node);
        }

        void Build(Node node)
        {
            foreach (var property in node.Properties)
            {
                Assign(property);
            }
        }

        Builder Clone(Node node)
        {
            var builder = (Builder) Activator.CreateInstance(GetType());
            builder._node       = node;
            builder.DataContext = DataContext;
            builder.Config      = Config;

            return builder;
        }

        void ProcessGridRowsAndColumns(Grid grid)
        {
            if (_node.Properties.Any(p => p.Name.ToUpperEN() == "ROWS"))
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

            if (_node.Properties.Any(p => p.Name.ToUpperEN() == "COLUMNS" || p.Name.ToUpperEN() == "COLS"))
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
        #endregion

        #region Assignment
        bool TryToSetView(Builder builder, Node node)
        {
            if (node.NameToUpperInEnglish == "VIEW")
            {
                ViewName = node.ValueAsString.ToUpperEN();
                Config.TryToCreateElement(this);
                if (View == null)
                {
                    View = CreateDefaultWpfElement(node.ValueAsString);
                }

                return true;
            }

            return false;
        }

        bool TryToSetName(Builder builder, Node node)
        {
            var fe = DataContext as FrameworkElement;
            if (node.NameToUpperInEnglish == "NAME" && fe != null)
            {
                var fieldInfo = fe.GetType().GetPublicNonStaticField(node.ValueAsString);
                if (fieldInfo != null)
                {
                    fieldInfo.SetValue(fe, View);
                    return true;
                }

                fe.RegisterName(node.ValueAsString, View);
                return true;
            }

            return false;
        }

        bool TryToHandleStackPanelTitle(Builder builder, Node node)
        {
            if (node.NameToUpperInEnglish == "TITLE" && View is StackPanel)
            {
                View.SetValue(HeaderedContentControl.HeaderProperty, node.ValueAsString);
                return true;
            }

            return false;
        }

        static readonly DependencyProperty GravityProperty = DependencyProperty.Register("Gravity", typeof(double?), typeof(Builder));

        bool TryToHandleGravity(Builder builder, Node node)
        {
            if (node.NameToUpperInEnglish == "GRAVITY")
            {
                View.SetValue(GravityProperty, node.ValueAsNumber.ToDouble());
                return true;
            }

            return false;
        }

        bool TryToHandleArrayValues(Builder builder, Node node)
        {
            if (!node.ValueIsArray)
            {
                return false;
            }

            if (ViewAsIAddChild == null)
            {
                throw new ArgumentException(nameof(ViewAsIAddChild));
            }

            foreach (var item in node.ValueAsArray)
            {
                ViewAsIAddChild.AddChild(Clone(item).Build().View);
            }

            return true;
        }

        bool TryToBindingExpression(Builder builder, Node node)
        {
            if (!node.ValueIsBindingExpression)
            {
                return false;
            }

            var bindingInfoContract = node.ValueAsBindingInfo;

            if (bindingInfoContract != null)
            {
                var dp = SearchDependencyPropertyInView(node.Name);
                if (dp == null)
                {
                    throw Errors.DependencyPropertyNotFound(node.Name);
                }

                var binding = bindingInfoContract.ConvertToBinding(TypeFinder);
                binding.Source = DataContext;

                BindingOperations.SetBinding(View, dp, binding);
                return true;
            }

            return false;
        }

        bool TryToHandleDProperty(Builder builder, Node node)
        {
            var attributeName = node.Name;

            // BOA.UI.EditorBase.LabelWidthProperty
            if (attributeName.Contains('.')) // dependency property
            {
                var dp = BuilderUtility.SearchDependencyProperty(attributeName, TypeFinder);
                if (dp == null)
                {
                    throw Errors.DependencyPropertyNotFound(attributeName);
                }

                object attributeValue = null;
                if (node.ValueIsString)
                {
                    attributeValue = BuilderUtility.NormalizeValueForType(node.ValueAsString, dp.PropertyType);
                }

                if (node.ValueIsBoolean)
                {
                    attributeValue = BuilderUtility.NormalizeValueForType(node.ValueAsBoolean, dp.PropertyType);
                }

                if (node.ValueIsNumber)
                {
                    attributeValue = BuilderUtility.NormalizeValueForType(node.ValueAsNumber, dp.PropertyType);
                }

                View.SetValue(dp, attributeValue);

                return true;
            }

            return false;
        }

        bool TryToInvokeDefaultProperty(Builder builder, Node node)
        {
            var    attributeName  = node.Name;
            object attributeValue = null;

            if (node.ValueIsString)
            {
                attributeValue = node.ValueAsString;
            }

            if (node.ValueIsBoolean)
            {
                attributeValue = node.ValueAsBoolean;
            }

            if (node.ValueIsNumber)
            {
                attributeValue = node.ValueAsNumber;
            }

            if (attributeValue == null)
            {
                throw new ArgumentException(node.ToString());
            }

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

        bool TryToHandleHeightPropertyAutoValue(Builder builder, Node node)
        {
            if (node.NameToUpperInEnglish == "HEIGHT" && node.ValueAsStringToUpperInEnglish == "AUTO")
            {
                View.SetValue(FrameworkElement.HeightProperty, double.NaN);
                View.SetValue(HeightIsAutoProperty, true);
                return true;
            }

            return false;
        }

        bool TryToHandleWidthPropertyAutoValue(Builder builder, Node node)
        {
            if (node.NameToUpperInEnglish == "WIDTH" && node.ValueAsStringToUpperInEnglish == "AUTO")
            {
                View.SetValue(FrameworkElement.WidthProperty, double.NaN);
                View.SetValue(WidthIsAutoProperty, true);
                return true;
            }

            return false;
        }

        bool TryToHandleMarginValues(Builder builder, Node node)
        {
            var name = node.NameToUpperInEnglish;

            if (name == "MARGINLEFT" || name == "LEFTMARGIN")
            {
                var fe = (FrameworkElement) View;

                fe.Margin = new Thickness(node.ValueAsNumberAsDouble, fe.Margin.Top, fe.Margin.Right, fe.Margin.Bottom);
                return true;
            }

            if (name == "MARGINTOP" || name == "TOPMARGIN")
            {
                var fe = (FrameworkElement) View;

                fe.Margin = new Thickness(fe.Margin.Left, node.ValueAsNumberAsDouble, fe.Margin.Right, fe.Margin.Bottom);
                return true;
            }

            if (name == "MARGINRIGHT" || name == "RIGHTMARGIN")
            {
                var fe = (FrameworkElement) View;

                fe.Margin = new Thickness(fe.Margin.Left, fe.Margin.Top, node.ValueAsNumberAsDouble, fe.Margin.Bottom);
                return true;
            }

            if (name == "MARGINBOTTOM" || name == "BOTTOMMARGIN")
            {
                var fe = (FrameworkElement) View;

                fe.Margin = new Thickness(fe.Margin.Left, fe.Margin.Top, fe.Margin.Right, node.ValueAsNumberAsDouble);
                return true;
            }

            return false;
        }

        void Assign(Node node)
        {
            var tuple = new Func<Builder, Node, bool>[]
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
                if (func(this, node))
                {
                    return;
                }
            }
        }
        #endregion
    }
    #endregion
}