﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media.Effects;
using CustomUIMarkupLanguage.Markup;

namespace CustomUIMarkupLanguage.UIBuilding
{
    public delegate void CreationCompletedCallback(Builder builder, UIElement element, Node node);

    public delegate UIElement ElementCreationDelegate(Builder builder, Node node);

    public delegate bool CustomPropertyHandlerDelegate(Builder builder, UIElement element, Node node);

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
        static readonly List<CustomPropertyHandlerDelegate> _customPropertyHandlers = new List<CustomPropertyHandlerDelegate>
        {
            WpfExtra.TextBlock_IsBold,
            WpfExtra.RichTextBox_Text,
            WpfExtra.Button_Text,
            WpfExtra.RadioButton_Label,
            WpfExtra.IsVisible,
            WpfExtra.ControlStyles,
            WpfExtra.HorizontalAlignmentIsCenter,
            WpfExtra.VerticalAlignmentIsCenter,
            WpfExtra.MarginInChildren,
            WpfExtra.HasSimpleDropShadowEffect,
            WpfExtra.BorderStyle
        };

        /// <summary>
        ///     The try to create element
        /// </summary>
        static readonly List<ElementCreationDelegate> _tryToCreateElement = new List<ElementCreationDelegate>
        {
            WpfExtra.StackPanelCreations,
            WpfExtra.RichTextBox_Create,
            WpfExtra.ListBox_Create,
            Card.CreateCard,
            ActionButton.HandleCreation,
            LabeledTextBox.On
        };

        /// <summary>
        ///     The transforms
        /// </summary>
        static readonly List<Action<Node>> Transforms = new List<Action<Node>>
        {
            WpfExtra.TransformViewName
        };
        #endregion

        #region Fields
        /// <summary>
        ///     The creation completed handlers
        /// </summary>
        readonly List<CreationCompletedCallback> _creationCompletedHandlers = new List<CreationCompletedCallback>
        {
            ProcessGridRowsAndColumns,
            WpfExtra.MarginInChildrenEnd,
            Card.CardCreationEnd
        };
        #endregion

        #region Public Properties
        public static Func<string, Type> FindType { get; set; } = TypeFinder.GetType;

        /// <summary>
        ///     Gets or sets the caller.
        /// </summary>
        public UIElement Caller { get; set; }
        #endregion

        #region Public Methods
        /// <summary>
        ///     Adds the transform.
        /// </summary>
        public static void AddTransform(Action<Node> action)
        {
            Transforms.Add(action);
        }

        /// <summary>
        ///     Registers the custom property.
        /// </summary>
        public static void RegisterCustomProperty(CustomPropertyHandlerDelegate execute)
        {
            _customPropertyHandlers.Insert(0, execute);
        }

        /// <summary>
        ///     Registers the element creation.
        /// </summary>
        public static void RegisterElementCreation(ElementCreationDelegate func)
        {
            _tryToCreateElement.Add(func);
        }

        /// <summary>
        ///     Registers the element creation.
        /// </summary>
        public static void RegisterElementCreation(string uiName, Type uiType)
        {
            UIElement fun(Builder builder, Node node)
            {
                if (node.UI == uiName.ToUpperEN())
                {
                    return (UIElement) Activator.CreateInstance(uiType);
                }

                return null;
            }

            _tryToCreateElement.Add(fun);
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

            if (node.UI == null)
            {
                node.Properties.Items.Insert(0, new Node {Name = "view", ValueAsString = Caller.GetType().Name});
            }

            foreach (var transform in Transforms)
            {
                transform(node);
            }

            BuildProperties(Caller, node);
        }

        /// <summary>
        ///     Registers the on creation completed.
        /// </summary>
        public void RegisterOnCreationCompleted(CreationCompletedCallback action)
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
            if (!(element is Grid grid))
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
                        if (gridChildAsDpObject.GetValue(GravityProperty) is double gravity)
                        {
                            rowDefinition = new RowDefinition {Height = new GridLength(gravity, GridUnitType.Star)};
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
                        if (gridChildAsDpObject.GetValue(GravityProperty) is double gravity)
                        {
                            columnDefinition = new ColumnDefinition {Width = new GridLength(gravity, GridUnitType.Star)};
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
            var propertyInfo = element.GetType().GetProperty(name, BindingFlags.IgnoreCase | BindingFlags.Instance | BindingFlags.Public);
            if (propertyInfo != null)
            {
                name = propertyInfo.Name;
            }

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
        ///     Builds the properties.
        /// </summary>
        void BuildProperties(UIElement element, Node node)
        {
            foreach (var property in node.Properties)
            {
                if (property.NameToUpperInEnglish == "VIEW")
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
            if (node.NameToUpperInEnglish == "NAME" && Caller is FrameworkElement fe)
            {
                var fieldInfo = fe.GetType().GetField(node.ValueAsString, null, false, true, false);
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
        static bool TryToHandleStackPanelTitle(Builder builder, UIElement element, Node node)
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
        static bool TryToHandleGravity(Builder builder, UIElement element, Node node)
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

                BuildProperties(uiElement, item);

                addChild.AddChild(uiElement);
            }

            return true;
        }

        /// <summary>
        ///     Tries to handle node values.
        /// </summary>
        bool TryToHandleNodeValues(Builder builder, UIElement element, Node node)
        {
            if (!node.ValueIsNode)
            {
                return false;
            }

            var addChild = element as IAddChild;
            if (addChild == null)
            {
                throw Errors.ElementMustBeInheritFromIAddChild(element);
            }

            var instance = CreateInstance(node.ValueAsNode);

            BuildProperties(instance, node.ValueAsNode);

            addChild.AddChild(instance);

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

                var binding = bindingInfoContract.ConvertToBinding(FindType, "DataContext.");
                binding.Source              = Caller;
                binding.UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged;

                BindingOperations.SetBinding(element, dp, binding);
                return true;
            }

            return false;
        }

        /// <summary>
        ///     Tries to handle d property.
        /// </summary>
        static bool TryToHandleDProperty(Builder builder, UIElement element, Node node)
        {
            var attributeName = node.Name;

            // BOA.UI.EditorBase.LabelWidthProperty
            if (attributeName.Contains('.')) // dependency property
            {
                var dp = BuilderUtility.SearchDependencyProperty(attributeName, FindType);
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
        ///     Gets or sets a value indicating whether this instance is in design mode.
        /// </summary>
        public bool IsInDesignMode { get; set; }

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

            var property = element.GetType().GetProperty(attributeName, true, false, true, false);
            if (property == null)
            {
                var eventInfo = element.GetType().GetEvent(attributeName);
                if (eventInfo == null)
                {
                    throw Errors.PropertyNotFound(attributeName, element.GetType());
                }

                if (IsInDesignMode)
                {
                    return true;
                }

                var methodInvocationExpression = string.Empty + node.ValueAsString;
                // support this format: this.Notify(OnContactClicked)
                if (methodInvocationExpression.StartsWith("this."))
                {
                    var viewInvocationExpressionInfo = ViewInvocationExpressionInfo.Parse(methodInvocationExpression);

                    var methodName = viewInvocationExpressionInfo.MethodName;

                    var mi = Caller.GetType().GetMethod(methodName, null, false, true, false);

                    var parameters = viewInvocationExpressionInfo.Parameters.ToArray();

                    mi.DoCastOperationsOnParametersForInvokeMethod(parameters);

                    if (element is Button button && node.NameToUpperInEnglish == "CLICK")
                    {
                        button.Click += (s, e) =>
                        {
                            if (mi == null)
                            {
                                throw new MissingMemberException(Caller.GetType().FullName + "->" + methodName);
                            }

                            mi.Invoke(Caller, parameters);
                        };

                        return true;
                    }

                    if (eventInfo.EventHandlerType == typeof(Action))
                    {
                        var caller = Caller;

                        void handler()
                        {
                            mi.Invoke(caller, parameters);
                        }

                        eventInfo.AddMethod.Invoke(element, new object[] {(Action) handler});
                        return true;
                    }

                    if (eventInfo.EventHandlerType == typeof(TextChangedEventHandler))
                    {
                        var caller = Caller;

                        void handler(object s, TextChangedEventArgs e)
                        {
                            mi.Invoke(caller, parameters);
                        }

                        eventInfo.AddMethod.Invoke(element, new object[] {(TextChangedEventHandler) handler});
                        return true;
                    }

                    if (eventInfo.EventHandlerType == typeof(SelectionChangedEventHandler))
                    {
                        var caller = Caller;

                        void handler(object s, SelectionChangedEventArgs e)
                        {
                            mi.Invoke(caller, parameters);
                        }

                        eventInfo.AddMethod.Invoke(element, new object[] {(SelectionChangedEventHandler) handler});
                        return true;
                    }

                    throw new NotImplementedException();
                }

                var handlerMethod = Caller.GetType().GetMethod(attributeValue.ToString(), null, false, true, true);

                var handlerMethodParameters = handlerMethod.GetParameters();
                if (handlerMethodParameters.Length == 0)
                {
                    var eventInfoAddMethod = eventInfo.GetAddMethod(true);
                    var eventParameters    = eventInfoAddMethod.GetParameters();
                    if (eventParameters.Length > 0)
                    {
                        var caller = Caller;

                        if (eventInfo.EventHandlerType == typeof(KeyEventHandler))
                        {
                            void handler(object s, KeyEventArgs e)
                            {
                                handlerMethod.Invoke(caller, null);
                            }

                            eventInfo.AddEventHandler(element, (KeyEventHandler) handler);
                            return true;
                        }

                        if (eventInfo.EventHandlerType == typeof(RoutedEventHandler))
                        {
                            void handler(object s, RoutedEventArgs e)
                            {
                                handlerMethod.Invoke(caller, null);
                            }

                            eventInfo.AddEventHandler(element, (RoutedEventHandler) handler);
                            return true;
                        }

                        if (eventInfo.EventHandlerType == typeof(Action))
                        {
                            void handler()
                            {
                                handlerMethod.Invoke(caller, null);
                            }

                            eventInfo.AddEventHandler(element, (Action) handler);
                            return true;
                        }

                        if (eventInfo.EventHandlerType == typeof(TextChangedEventHandler))
                        {
                            void handler(object s, TextChangedEventArgs e)
                            {
                                handlerMethod.Invoke(caller, null);
                            }

                            eventInfo.AddEventHandler(element, (TextChangedEventHandler) handler);
                            return true;
                        }

                        if (eventInfo.EventHandlerType == typeof(SelectionChangedEventHandler))
                        {
                            void handler(object s, SelectionChangedEventArgs e)
                            {
                                handlerMethod.Invoke(caller, null);
                            }

                            eventInfo.AddEventHandler(element, (SelectionChangedEventHandler) handler);
                            return true;
                        }

                        throw new NotImplementedException(attributeName);
                    }
                }

                eventInfo.AddEventHandler(element, handlerMethod.CreateDelegate(eventInfo.EventHandlerType, Caller));
                return true;
            }

            var converter = TypeDescriptor.GetConverter(property.PropertyType);

            if (converter.CanConvertFrom(attributeValue.GetType()))
            {
                attributeValue = converter.ConvertFrom(attributeValue);
            }
            else
            {
                attributeValue = Cast.To(attributeValue, property.PropertyType, CultureInfo.CurrentUICulture);
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
        static bool TryToHandleHeightPropertyAutoValue(Builder builder, UIElement element, Node node)
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
        static bool TryToHandleWidthPropertyAutoValue(Builder builder, UIElement element, Node node)
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
        static bool TryToHandleMarginValues(Builder builder, UIElement element, Node node)
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
            var tuple = new CustomPropertyHandlerDelegate[]
            {
                TryToSetName,
                TryToHandleGravity,
                TryToHandleStackPanelTitle,
                TryToHandleArrayValues,
                TryToHandleNodeValues,
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