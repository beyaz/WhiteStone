using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BOA.OneDesigner.CodeGeneration
{
    public class JsObject : Dictionary<string, string>
    {
    }

    static class JsObjectInfoSingleLineWriter
    {
        #region Public Methods
        public static void Write( StringBuilder sb, JsObject jsObject)
        {
            sb.Append("{ ");
            sb.Append(string.Join(" , ", jsObject.ToList().ConvertAll(FieldToString)));
            sb.Append(" }");
        }

        public static string ToString(  JsObject jsObject)
        {
            var sb = new StringBuilder();
            Write(sb,jsObject);
            return sb.ToString();
        }
        #endregion

        #region Methods
        static string FieldToString(KeyValuePair<string, string> field)
        {
            return $"{field.Key}: {field.Value}";
        }
        #endregion
    }
}