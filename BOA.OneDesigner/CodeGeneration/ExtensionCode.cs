using BOA.Common.Helpers;

namespace BOA.OneDesigner.CodeGeneration
{
    static class ExtensionCode
    {
        public static void CallFunction(PaddedStringBuilder sb, string functionName)
        {
            
            sb.AppendLine();
            if (functionName == "onActionClick")
            {
                sb.AppendLine($"Extension.{functionName}(this, command.commandName);");
            }
            else
            {
                sb.AppendLine($"Extension.{functionName}(this);");
            }
        }

        public static void onActionClick(PaddedStringBuilder sb)
        {
            
            sb.AppendLine();
            sb.AppendLine("Extension.onActionClick(this, command.commandName);");
        }
    }
}