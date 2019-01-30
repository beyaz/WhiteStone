using System;
using System.IO;
using BOA.Common.Helpers;

namespace BOA.OneDesigner.Helpers
{
    public static class Error
    {
        #region Public Methods
        public static InvalidOperationException BindingPathShouldHaveValue(string location, string pathName)
        {
            return new InvalidOperationException(string.Join(Environment.NewLine,
                                                             nameof(BindingPathShouldHaveValue),
                                                             nameof(location) + Path.AltDirectorySeparatorChar + location,
                                                             nameof(pathName) + Path.AltDirectorySeparatorChar + pathName));
        }

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

        public static InvalidOperationException RequestNotFound(string requestName,string assemblyPath=null)
        {
            throw new InvalidOperationException(GetMessageRequestNotFound(requestName,assemblyPath));    
        }

        public static string GetMessageRequestNotFound(string requestName, string assemblyPath =null)
        {
            if (assemblyPath.HasValue())
            {
                return ($@"{requestName} not found in {assemblyPath}");    
            }

            return ($@"{requestName} not found in d:\boa\server\bin\");
        }
        #endregion
    }
}