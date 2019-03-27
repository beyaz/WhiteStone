using System;
using System.IO;
using BOA.Common.Helpers;

namespace BOA.OneDesigner.Helpers
{

    public class BusinessException:Exception
    {
        public BusinessException(string message):base(message)
        {
            
        }

    }

    public static class Error
    {
        #region Public Methods
        public static BusinessException BindingPathShouldHaveValue(string location, string pathName)
        {
            return new BusinessException(string.Join(Environment.NewLine,
                                                             nameof(BindingPathShouldHaveValue),
                                                             nameof(location) + Path.AltDirectorySeparatorChar + location,
                                                             nameof(pathName) + Path.AltDirectorySeparatorChar + pathName));
        }

        public static BusinessException InvalidBindingPath(string container, string bindingPath)
        {
            return new BusinessException(string.Join(Environment.NewLine,
                                                             nameof(InvalidBindingPath),
                                                             nameof(container) + Path.VolumeSeparatorChar + container,
                                                             nameof(bindingPath) + Path.VolumeSeparatorChar + bindingPath));
        }

        public static BusinessException InvalidOperation(string message = null)
        {
            return new BusinessException(message);
        }

        public static BusinessException RequestNotFound(string requestName,string assemblyPath=null)
        {
            throw new BusinessException(GetMessageRequestNotFound(requestName,assemblyPath));    
        }

        public static string GetMessageRequestNotFound(string requestName, string assemblyPath =null)
        {
            if (assemblyPath.HasValue())
            {
                return ($@"'{requestName}' not found in '{assemblyPath}'");    
            }

            return ($@"'{requestName}' not found in d:\boa\server\bin\");
        }
        #endregion
    }
}