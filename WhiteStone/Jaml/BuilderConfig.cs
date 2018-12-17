using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using BOA.Common.Helpers;

namespace BOA.Jaml
{
    /// <summary>
    ///     Configuration of builder.
    /// </summary>
    public class BuilderConfig
    {
        #region Fields
        /// <summary>
        ///     The creation completed handlers
        /// </summary>
        readonly List<Action<Builder>> _creationCompletedHandlers = new List<Action<Builder>>();

        /// <summary>
        ///     The custom property handlers
        /// </summary>
        readonly List<Func<Assignment, bool>> _customPropertyHandlers = new List<Func<Assignment, bool>>
        {
            RichTextBox_Text,
            TextBlock_IsBold
        };

        /// <summary>
        ///     The try to create element
        /// </summary>
        readonly List<Action<Builder>> _tryToCreateElement = new List<Action<Builder>>
        {
            RichTextBox_Create
        };
        #endregion

        #region Public Methods
        /// <summary>
        ///     Riches the text box create.
        /// </summary>
        public static void RichTextBox_Create(Builder builder)
        {
            if (builder.ViewName == "TEXTAREA")
            {
                builder.View = new RichTextBox
                {
                    AcceptsTab = true
                };
            }
        }

        /// <summary>
        ///     Riches the text box text.
        /// </summary>
        public static bool RichTextBox_Text(Assignment assignment)
        {
            var richTextBox = assignment.Builder.View as RichTextBox;
            if (richTextBox == null)
            {
                return false;
            }

            if (assignment.Name == "Text")
            {
                richTextBox.TextChanged += (s, e) =>
                {
                    var text = richTextBox.GetText();

                    var propertyInfo = assignment.Builder.DataContext.GetType().GetPublicNonStaticProperty(assignment.ValueAsString, true);

                    propertyInfo.SetValue(assignment.Builder.DataContext, text);
                };

                return true;
            }

            return false;
        }

        /// <summary>
        ///     Texts the block is bold.
        /// </summary>
        public static bool TextBlock_IsBold(Assignment assignment)
        {
            var textBlock = assignment.Builder.View as TextBlock;
            if (textBlock == null)
            {
                return false;
            }

            if (assignment.NameToUpperInEnglish == "ISBOLD")
            {
                if (assignment.ValueAsBoolean == true)
                {
                    textBlock.FontWeight = FontWeights.Bold;
                    return true;
                }

                throw new ArgumentException(assignment.ValueAsString);
            }

            return false;
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
        ///     Called when [custom property].
        /// </summary>
        public BuilderConfig OnCustomProperty(Func<Assignment, bool> execute)
        {
            _customPropertyHandlers.Add(execute);
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
        #endregion

        #region Methods
        /// <summary>
        ///     Tries to create element.
        /// </summary>
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

        /// <summary>
        ///     Tries to fire creation completed handlers.
        /// </summary>
        internal void TryToFireCreationCompletedHandlers(Builder builder)
        {
            foreach (var fn in _creationCompletedHandlers)
            {
                fn(builder);
            }
        }

        /// <summary>
        ///     Tries to invoke custom property.
        /// </summary>
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
        #endregion
    }
}