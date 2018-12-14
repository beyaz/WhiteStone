using System;
using System.Collections.Generic;

namespace BOA.Jaml
{
    /// <summary>
    ///     Configuration of builder.
    /// </summary>
    public class BuilderConfig
    {
        readonly List<Func<Assignment, bool>> _customPropertyHandlers    = new List<Func<Assignment, bool>>();
        readonly List<Action<Builder>>        _creationCompletedHandlers = new List<Action<Builder>>();
        readonly List<Action<Builder>>        _tryToCreateElement        = new List<Action<Builder>>();

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
}