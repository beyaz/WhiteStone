using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BOA.Common.Helpers;

namespace BOA.OneDesigner.CodeGeneration
{
    public class JsObject : Dictionary<string, string>
    {
        public bool HasValue => Count > 0;
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
        public static string FieldToString(KeyValuePair<string, string> field)
        {
            return $"{field.Key}: {field.Value}";
        }
        #endregion
    }
    static class JsObjectInfoMultiLineWriter
    {
        #region Public Methods
        public static void Write( PaddedStringBuilder sb, JsObject jsObject)
        {
            sb.AppendLine("{");
            sb.PaddingCount++;

            var items = jsObject.ToList();
            var len = items.Count - 1;
            var i = 0;
            while (i<len)
            {
                sb.AppendLine(JsObjectInfoSingleLineWriter.FieldToString(items[i])+",");
                i++;
            }

            sb.AppendLine(JsObjectInfoSingleLineWriter.FieldToString(items[i])); 
            
            // sb.AppendLine(string.Join(","+Environment.NewLine, jsObject.ToList().ConvertAll(JsObjectInfoSingleLineWriter.FieldToString)));
            sb.PaddingCount--;
            sb.AppendWithPadding("}");
        }

        public static string ToString(  JsObject jsObject)
        {
            var sb = new PaddedStringBuilder();
            Write(sb,jsObject);
            return sb.ToString();
        }
        #endregion

       
    }

}