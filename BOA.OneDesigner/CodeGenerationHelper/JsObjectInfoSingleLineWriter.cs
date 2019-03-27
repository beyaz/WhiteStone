using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BOA.OneDesigner.CodeGeneration
{
    static class JsObjectInfoSingleLineWriter
    {
        #region Public Methods
        public static string FieldToString(KeyValuePair<string, string> field)
        {
            return $"{field.Key}: {field.Value}";
        }

        public static string ToString(JsObject jsObject)
        {
            var sb = new StringBuilder();
            Write(sb, jsObject);
            return sb.ToString();
        }

        public static void Write(StringBuilder sb, JsObject jsObject)
        {
            sb.Append("{ ");
            sb.Append(string.Join(", ", jsObject.ToList().ConvertAll(FieldToString)));
            sb.Append(" }");
        }
        #endregion
    }
}