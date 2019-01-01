using System;

namespace BOA.OneDesigner.AppModel
{
    public static class Error
    {
        public static InvalidOperationException InvalidOperation(string message = null)
        {
            return new InvalidOperationException(message);
        }
    }
}