using System.Collections.Generic;
using System.Linq;
using BOA.Common.Helpers;
using BOAPlugins.Utility;

namespace BOA.OneDesigner.CodeGenerationHelper
{
    static class BindingPathHelper
    {
        class JsProperty
        {
            public string Name  { get; set; }
            public object Value { get; set; }
        }

        class JsObject
        {
            public readonly List<JsProperty> Properties = new List<JsProperty>();

            public bool HasProperty(string name)
            {
                return Properties.Any(x => x.Name == name);
            }

            public JsProperty CreateProperty(string name)
            {
                var property = new JsProperty{Name = name};

                Properties.Add(property);

                return property;
            }

            public JsProperty GetOrCreateProperty(string name)
            {
                var property = Properties.FirstOrDefault(x => x.Name == name);

                if (property == null)
                {
                    property = new JsProperty{Name = name};

                    Properties.Add(property);
                }
                

                return property;
            }
        }


        public static string EvaluateWindowRequestDefaultCreationInRenderFunction(IReadOnlyList<string> bindingPathsInDesigner)
        {

            var jsObject = new JsObject();


            foreach (var path in bindingPathsInDesigner)
            {
                var arr = path.SplitAndClear(".");

                JsObject jsObj = jsObject;

                foreach (var propertyName in arr.Take(arr.Count-1))
                {
                    var jsProperty           = jsObj.GetOrCreateProperty(TypescriptNaming.NormalizeBindingPath(propertyName));
                    jsProperty.Value = jsObj = (JsObject)jsProperty.Value ?? new JsObject();
                }
            }

            return jsObject.ToString();

        }
    }
}