using System;
using System.IO;

namespace BOA.OneDesigner.Helpers
{
    public static class Error
    {
        #region Public Methods
        public static InvalidOperationException InvalidBindingPath(string container, string bindingPath)
        {
            return new InvalidOperationException(string.Join(Environment.NewLine,
                                                             nameof(InvalidBindingPath),
                                                             nameof(container) + Path.VolumeSeparatorChar + container,
                                                             nameof(bindingPath) + Path.VolumeSeparatorChar + bindingPath));
        }

        public static InvalidOperationException InvalidOperation(string message = null)
        {
            return new InvalidOperationException(message);
        }
        #endregion
    }
}