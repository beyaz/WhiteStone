using System;
using System.Globalization;

namespace CustomUIMarkupLanguage.Jaml
{
        static class Extensions
        {
            public static Type GetType(this ITypeFinder finder, string fullTypeName)
            {
                var type = finder.Find(fullTypeName);
                if (type == null)
                {
                    throw Errors.TypeNotFound(fullTypeName);
                }
                return type;
            }

           
           



        }
    }
