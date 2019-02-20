using System.Collections.Generic;
using System.Linq;
using System.Text;
using BOA.Common.Helpers;

namespace BOA.OneDesigner.CodeGeneration
{
    public class JsObject : Dictionary<string, string>
    {
        #region Public Properties
        public bool HasValue => Count > 0;
        #endregion
    }

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

    static class JsObjectInfoMultiLineWriter
    {
        #region Public Methods
        public static string ToString(JsObject jsObject)
        {
            var sb = new PaddedStringBuilder();
            Write(sb, jsObject);
            return sb.ToString();
        }

        public static void Write(PaddedStringBuilder sb, JsObject jsObject)
        {
            var items = jsObject.ToList();
            var len   = items.Count - 1;

            if (len == 0)
            {
                sb.Append("{ " + JsObjectInfoSingleLineWriter.FieldToString(items[0]) + " }");
                return;
            }

            sb.Append("{");
            sb.AppendLine();
            sb.PaddingCount++;

            var i = 0;
            while (i < len)
            {
                sb.AppendLine(JsObjectInfoSingleLineWriter.FieldToString(items[i]) + ",");
                i++;
            }

            sb.AppendLine(JsObjectInfoSingleLineWriter.FieldToString(items[i]));

            sb.PaddingCount--;
            sb.AppendWithPadding("}");
        }
        #endregion
    }
}