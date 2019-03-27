using System.Linq;
using BOA.Common.Helpers;

namespace BOA.OneDesigner.CodeGeneration
{
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