using System.Collections.Generic;
using System.Linq;
using BOA.Common.Helpers;
using BOAPlugins.Utility;

namespace BOA.OneDesigner.CodeGenerationHelper
{
    /// <summary>
    ///     The binding path helper
    /// </summary>
    static class BindingPathHelper
    {
        #region Public Methods
        /// <summary>
        ///     Evaluates the window request default creation in render function.
        /// </summary>
        public static string EvaluateWindowRequestDefaultCreationInRenderFunction(IEnumerable<string> bindingPathsInDesigner)
        {
            var jsObject = new JsObject();

            foreach (var path in bindingPathsInDesigner)
            {
                var arr = path.SplitAndClear(".");

                var jsObj = jsObject;

                foreach (var propertyName in arr.Take(arr.Count - 1))
                {
                    var jsProperty           = jsObj.GetOrCreateProperty(TypescriptNaming.NormalizeBindingPath(propertyName));
                    jsProperty.Value = jsObj = (JsObject) jsProperty.Value ?? new JsObject();
                }
            }

            return jsObject.ToString();
        }
        #endregion

        /// <summary>
        ///     The js object
        /// </summary>
        class JsObject
        {
            #region Fields
            /// <summary>
            ///     The properties
            /// </summary>
            public readonly List<JsProperty> Properties = new List<JsProperty>();
            #endregion

            #region Public Methods
            /// <summary>
            ///     Creates the property.
            /// </summary>
            public JsProperty CreateProperty(string name)
            {
                var property = new JsProperty {Name = name};

                Properties.Add(property);

                return property;
            }

            /// <summary>
            ///     Gets the or create property.
            /// </summary>
            public JsProperty GetOrCreateProperty(string name)
            {
                var property = Properties.FirstOrDefault(x => x.Name == name);

                if (property == null)
                {
                    property = new JsProperty {Name = name};

                    Properties.Add(property);
                }

                return property;
            }

            /// <summary>
            ///     Determines whether the specified name has property.
            /// </summary>
            public bool HasProperty(string name)
            {
                return Properties.Any(x => x.Name == name);
            }

            /// <summary>
            ///     Returns a <see cref="System.String" /> that represents this instance.
            /// </summary>
            public override string ToString()
            {
                if (Properties.Count == 0)
                {
                    return "{}";
                }

                return "{ " + string.Join(", ", Properties) + " }";
            }
            #endregion
        }

        /// <summary>
        ///     The js property
        /// </summary>
        class JsProperty
        {
            #region Public Properties
            /// <summary>
            ///     Gets or sets the name.
            /// </summary>
            public string Name { get; set; }

            /// <summary>
            ///     Gets or sets the value.
            /// </summary>
            public object Value { get; set; }
            #endregion

            #region Public Methods
            /// <summary>
            ///     Returns a <see cref="System.String" /> that represents this instance.
            /// </summary>
            public override string ToString()
            {
                if (Value == null)
                {
                    return Name + ": {}";
                }

                return Name + ": " + Value;
            }
            #endregion
        }
    }
}