using System.Collections.Generic;
using System.Linq;
using BOA.Common.Helpers;
using BOAPlugins.Utility;

namespace BOA.OneDesigner.CodeGenerationHelper
{
    static class BindingPathHelper
    {
        #region Public Methods
        public static string EvaluateWindowRequestDefaultCreationInRenderFunction(IReadOnlyList<string> bindingPathsInDesigner)
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

        class JsObject
        {
            #region Fields
            public readonly List<JsProperty> Properties = new List<JsProperty>();
            #endregion

            #region Public Methods
            public JsProperty CreateProperty(string name)
            {
                var property = new JsProperty {Name = name};

                Properties.Add(property);

                return property;
            }

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

            public bool HasProperty(string name)
            {
                return Properties.Any(x => x.Name == name);
            }

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

        class JsProperty
        {
            #region Public Properties
            public string Name  { get; set; }
            public object Value { get; set; }
            #endregion

            #region Public Methods
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