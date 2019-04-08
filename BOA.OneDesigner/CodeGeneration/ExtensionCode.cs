using BOA.Common.Helpers;

namespace BOA.OneDesigner.CodeGeneration
{
    static class ExtensionCode
    {
        public static void afterConstructor(PaddedStringBuilder sb)
        {
            
            sb.AppendLine();
            sb.AppendLine("Extension.afterConstructor(this);");
        }

       

        public static void afterProxyDidRespond(PaddedStringBuilder sb)
        {
            
            sb.AppendLine();
            sb.AppendLine("Extension.afterProxyDidRespond(this, proxyResponse);");
        }
        
    }
}