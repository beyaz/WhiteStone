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
        readonly List<Action<Builder>> _creationCompletedHandlers = new List<Action<Builder>>();

        readonly List<Func<Assignment, bool>> _customPropertyHandlers = new List<Func<Assignment, bool>>
        {
            RichTextBox_Text,
            PredefinedConfigs.TextBlock_IsBold
        };

        readonly List<Action<Builder>> _tryToCreateElement = new List<Action<Builder>>
        {
            RichTextBox_Create
        };
        #endregion

        #region Public Methods
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

        internal void TryToFireCreationCompletedHandlers(Builder builder)
        {
            foreach (var fn in _creationCompletedHandlers)
            {
                fn(builder);
            }
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

        static void RichTextBox_Create(Builder builder)
        {
            if (builder.ViewName == "TEXTAREA")
            {
                builder.View = new RichTextBox
                {
                    AcceptsTab = true
                };
            }
        }

        static bool RichTextBox_Text(Assignment assignment)
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


                    var propertyInfo = assignment.Builder.DataContext.GetType().GetPublicNonStaticProperty(assignment.ValueAsString,true);
                    
                    propertyInfo.SetValue(assignment.Builder.DataContext,text);

                    
                };

                return true;
            }

            return false;
        }
        #endregion
    }

    static class PredefinedConfigs
    {
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
    }
}