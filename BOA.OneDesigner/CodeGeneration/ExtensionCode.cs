using BOA.Common.Helpers;

namespace BOA.OneDesigner.CodeGeneration
{
    static class ExtensionCode
    {
        public static void CallFunction(PaddedStringBuilder sb, string functionName)
        {
            
            sb.AppendLine();
            sb.AppendLine($"if (Extension.{functionName})");
            sb.AppendLine("{");
            sb.PaddingCount++;

            sb.AppendLine($"Extension.{functionName}(this);");

            sb.PaddingCount--;
            sb.AppendLine("}");
            
        }
    }
}