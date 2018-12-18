using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Markup;
using CustomUIMarkupLanguage.Markup;

namespace CustomUIMarkupLanguage.UIBuilding
{
    #region Builder    
    /// <summary>
    ///     The builder
    /// </summary>
    public class Builder
    {
        #region Static Fields
        /// <summary>
        ///     The custom property handlers
        /// </summary>
        static readonly List<Func<Builder, UIElement, Node, bool>> _customPropertyHandlers = new List<Func<Builder, UIElement, Node, bool>>
        {
            WpfExtra.TextBlock_IsBold,
            WpfExtra.RichTextBox_Text
        };

        /// <summary>
        ///     The try to create element
        /// </summary>
        static readonly List<Func<Builder, Node, UIElement>> _tryToCreateElement = new List<Func<Builder, Node, UIElement>>
        {
            WpfExtra.RichTextBox_Create
        };
        #endregion

        #region Fields
        /// <summary>
        ///     The creation completed handlers
        /// </summary>
        readonly List<Action<Builder, UIElement, Node>> _creationCompletedHandlers = new List<Action<Builder, UIElement, Node>>
        {
            ProcessGridRowsAndColumns
        };
        #endregion

        #region Constructors
        /// <summary>
        ///     Initializes a new instance of the <see cref="Builder" /> class.
        /// </summary>
        public Builder()
        {
            TypeFinder = new TypeFinder();
        }
        #endregion

        #region Public Properties
        /// <summary>
        ///     Gets or sets the caller.
        /// </summary>
        public UIElement Caller { get; set; }

        /// <summary>
        ///     Gets or sets the data context.
        /// </summary>
        public object DataContext { get; set; }

        /// <summary>
        ///     Gets or sets the type finder.
        /// </summary>
        public TypeFinder TypeFinder { get; set; }
        #endregion

        #region Public Methods
        /// <summary>
        ///     Registers the custom property.
        /// </summary>
        public static void RegisterCustomProperty(Func<Builder, UIElement, Node, bool> execute)
        {
            _customPropertyHandlers.Insert(0, execute);
        }

        /// <summary>
        ///     Registers the element creation.
        /// </summary>
        public static void RegisterElementCreation(Func<Builder, Node, UIElement> func)
        {
            _tryToCreateElement.Add(func);
        }

        /// <summary>
        ///     Loads the specified json string.
        /// </summary>
        public void Load(string jsonString)
        {
            Node node = null;
            try
            {
                node = TransformHelper.Transform(jsonString);
            }
            catch (Exception e)
            {
                throw Errors.JsonParsingError(e);
            }

            Build(Caller, node);
        }

        /// <summary>
        ///     Registers the on creation completed.
        /// </summary>
        public void RegisterOnCreationCompleted(Action<Builder, UIElement, Node> action)
        {
            _creationCompletedHandlers.Add(action);
        }
        #endregion

        #region Methods
        /// <summary>
        ///     Creates the default WPF element.
        /// </summary>
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

        /// <summary>
        ///     Processes the grid rows and columns.
        /// </summary>
        static void ProcessGridRowsAndColumns(Builder builder, UIElement element, Node node)
        {
            var grid = element as Grid;
            if (grid == null)
            {
                return;
            }

            if (node.Properties.Any(p => p.NameToUpperInEnglish == "ROWS"))
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
                                rowDefinition = new RowDefinition {Height = new GridLength(1, GridUnitType.Star)};
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

            if (node.Properties.Any(p => p.NameToUpperInEnglish == "COLUMNS" || p.NameToUpperInEnglish == "COLS"))
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

        /// <summary>
        ///     Searches the dependency property in view.
        /// </summary>
        static DependencyProperty SearchDependencyPropertyInView(string name, UIElement element)
        {
            var descriptor = DependencyPropertyDescriptor.FromName(name, element.GetType(), element.GetType());
            if (descriptor == null)
            {
                return null;
            }

            return descriptor.DependencyProperty;
        }

        /// <summary>
        ///     Tries to invoke custom property.
        /// </summary>
        static bool TryToInvokeCustomProperty(Builder builder, UIElement element, Node node)
        {
            foreach (var fn in _customPropertyHandlers)
            {
                var isHandled = fn(builder, element, node);
                if (isHandled)
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        ///     Builds the specified element.
        /// </summary>
        void Build(UIElement element, Node node)
        {
            foreach (var property in node.Properties)
            {
                if (property.NameToUpperInEnglish == "VIEW" ||
                    property.NameToUpperInEnglish == "ui")
                {
                    continue;
                }

                Assign(element, property);
            }

            TryToFireCreationCompletedHandler(element, node);
        }

        /// <summary>
        ///     Tries to fire creation completed handler.
        /// </summary>
        void TryToFireCreationCompletedHandler(UIElement element, Node node)
        {
            foreach (var fn in _creationCompletedHandlers)
            {
                fn(this, element, node);
            }
        }
        #endregion

        #region Assignment
        /// <summary>
        ///     Creates the instance.
        /// </summary>
        UIElement CreateInstance(Node node)
        {
            UIElement element = null;
            foreach (var fn in _tryToCreateElement)
            {
                element = fn(this, node);
                if (element != null)
                {
                    return element;
                }
            }

            element = CreateDefaultWpfElement(node.Properties["view"].ValueAsString);

            if (element == null)
            {
                throw Errors.ElementCreationFailedException(node);
            }

            return element;
        }

        /// <summary>
        ///     Tries the name of to set.
        /// </summary>
        bool TryToSetName(Builder builder, UIElement element, Node node)
        {
            var fe = DataContext as FrameworkElement;
            if (node.NameToUpperInEnglish == "NAME" && fe != null)
            {
                var fieldInfo = fe.GetType().GetPublicNonStaticField(node.ValueAsString);
                if (fieldInfo != null)
                {
                    fieldInfo.SetValue(fe, element);
                    return true;
                }

                fe.RegisterName(node.ValueAsString, element);
                return true;
            }

            return false;
        }

        /// <summary>
        ///     Tries to handle stack panel title.
        /// </summary>
        bool TryToHandleStackPanelTitle(Builder builder, UIElement element, Node node)
        {
            if (node.NameToUpperInEnglish == "TITLE" && element is StackPanel)
            {
                element.SetValue(HeaderedContentControl.HeaderProperty, node.ValueAsString);
                return true;
            }

            return false;
        }

        /// <summary>
        ///     The gravity property
        /// </summary>
        static readonly DependencyProperty GravityProperty = DependencyProperty.Register("Gravity", typeof(double?), typeof(Builder));

        /// <summary>
        ///     Tries to handle gravity.
        /// </summary>
        bool TryToHandleGravity(Builder builder, UIElement element, Node node)
        {
            if (node.NameToUpperInEnglish == "GRAVITY")
            {
                element.SetValue(GravityProperty, node.ValueAsNumberAsDouble);
                return true;
            }

            return false;
        }

        /// <summary>
        ///     Tries to handle array values.
        /// </summary>
        bool TryToHandleArrayValues(Builder builder, UIElement element, Node node)
        {
            if (!node.ValueIsArray)
            {
                return false;
            }

            var addChild = element as IAddChild;
            if (addChild == null)
            {
                throw Errors.ElementMustBeInheritFromIAddChild(element);
            }

            foreach (var item in node.ValueAsArray)
            {
                var uiElement = CreateInstance(item);

                Build(uiElement, item);

                addChild.AddChild(uiElement);
            }

            return true;
        }

        /// <summary>
        ///     Tries to binding expression.
        /// </summary>
        bool TryToBindingExpression(Builder builder, UIElement element, Node node)
        {
            if (!node.ValueIsBindingExpression)
            {
                return false;
            }

            var bindingInfoContract = node.ValueAsBindingInfo;

            if (bindingInfoContract != null)
            {
                var dp = SearchDependencyPropertyInView(node.Name, element);
                if (dp == null)
                {
                    throw Errors.DependencyPropertyNotFound(node.Name);
                }

                var binding = bindingInfoContract.ConvertToBinding(TypeFinder);
                binding.Source = DataContext;

                BindingOperations.SetBinding(element, dp, binding);
                return true;
            }

            return false;
        }

        /// <summary>
        ///     Tries to handle d property.
        /// </summary>
        bool TryToHandleDProperty(Builder builder, UIElement element, Node node)
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

                element.SetValue(dp, attributeValue);

                return true;
            }

            return false;
        }

        /// <summary>
        ///     Tries to invoke default property.
        /// </summary>
        bool TryToInvokeDefaultProperty(Builder builder, UIElement element, Node node)
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
                throw Errors.AttributeValueIsInvalid(node);
            }

            var property = element.GetType().GetProperty(attributeName);
            if (property == null)
            {
                var eventInfo = element.GetType().GetEvent(attributeName);
                if (eventInfo == null)
                {
                    throw Errors.PropertyNotFound(attributeName, element.GetType());
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
                            eventInfo.AddEventHandler(element, handler);
                            return true;
                        }

                        throw new NotImplementedException(attributeName);
                    }
                }

                eventInfo.AddEventHandler(element, handlerMethod.CreateDelegate(eventInfo.EventHandlerType, DataContext));
                return true;
            }

            var converter = TypeDescriptor.GetConverter(property.PropertyType);

            if (converter.CanConvertFrom(attributeValue.GetType()))
            {
                attributeValue = converter.ConvertFrom(attributeValue);
            }

            property.SetValue(element, attributeValue);

            return true;
        }

        /// <summary>
        ///     The height is automatic property
        /// </summary>
        static readonly DependencyProperty HeightIsAutoProperty = DependencyProperty.Register("HeightIsAuto", typeof(bool?), typeof(Builder));

        /// <summary>
        ///     The width is automatic property
        /// </summary>
        static readonly DependencyProperty WidthIsAutoProperty = DependencyProperty.Register("WidthIsAuto", typeof(bool?), typeof(Builder));

        /// <summary>
        ///     Tries to handle height property automatic value.
        /// </summary>
        bool TryToHandleHeightPropertyAutoValue(Builder builder, UIElement element, Node node)
        {
            if (node.NameToUpperInEnglish == "HEIGHT" && node.ValueAsStringToUpperInEnglish == "AUTO")
            {
                element.SetValue(FrameworkElement.HeightProperty, double.NaN);
                element.SetValue(HeightIsAutoProperty, true);
                return true;
            }

            return false;
        }

        /// <summary>
        ///     Tries to handle width property automatic value.
        /// </summary>
        bool TryToHandleWidthPropertyAutoValue(Builder builder, UIElement element, Node node)
        {
            if (node.NameToUpperInEnglish == "WIDTH" && node.ValueAsStringToUpperInEnglish == "AUTO")
            {
                element.SetValue(FrameworkElement.WidthProperty, double.NaN);
                element.SetValue(WidthIsAutoProperty, true);
                return true;
            }

            return false;
        }

        /// <summary>
        ///     Tries to handle margin values.
        /// </summary>
        bool TryToHandleMarginValues(Builder builder, UIElement element, Node node)
        {
            var name = node.NameToUpperInEnglish;

            if (name == "MARGINLEFT" || name == "LEFTMARGIN")
            {
                var fe = (FrameworkElement) element;

                fe.Margin = new Thickness(node.ValueAsNumberAsDouble, fe.Margin.Top, fe.Margin.Right, fe.Margin.Bottom);
                return true;
            }

            if (name == "MARGINTOP" || name == "TOPMARGIN")
            {
                var fe = (FrameworkElement) element;

                fe.Margin = new Thickness(fe.Margin.Left, node.ValueAsNumberAsDouble, fe.Margin.Right, fe.Margin.Bottom);
                return true;
            }

            if (name == "MARGINRIGHT" || name == "RIGHTMARGIN")
            {
                var fe = (FrameworkElement) element;

                fe.Margin = new Thickness(fe.Margin.Left, fe.Margin.Top, node.ValueAsNumberAsDouble, fe.Margin.Bottom);
                return true;
            }

            if (name == "MARGINBOTTOM" || name == "BOTTOMMARGIN")
            {
                var fe = (FrameworkElement) element;

                fe.Margin = new Thickness(fe.Margin.Left, fe.Margin.Top, fe.Margin.Right, node.ValueAsNumberAsDouble);
                return true;
            }

            return false;
        }

        /// <summary>
        ///     Assigns the specified element.
        /// </summary>
        void Assign(UIElement element, Node node)
        {
            var tuple = new Func<Builder, UIElement, Node, bool>[]
            {
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
                if (func(this, element, node))
                {
                    return;
                }
            }
        }
        #endregion
    }
    #endregion
}