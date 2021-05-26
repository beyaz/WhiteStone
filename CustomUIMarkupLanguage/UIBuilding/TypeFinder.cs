using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace CustomUIMarkupLanguage.UIBuilding
{
    /// <summary>
    ///     Defines the type finder.
    /// </summary>
    public static class TypeFinder
    {
        #region Static Fields
        /// <summary>
        ///     The qualified type names
        /// </summary>
        static readonly Dictionary<string, Type> QualifiedTypeNames = new Dictionary<string, Type>
        {
            {"OBJECT", typeof(object)},
            {"STRING", typeof(string)},
            {"BOOL", typeof(bool)},
            {"BYTE", typeof(byte)},
            {"CHAR", typeof(char)},
            {"DECIMAL", typeof(decimal)},
            {"DOUBLE", typeof(double)},
            {"SHORT", typeof(short)},
            {"INT", typeof(int)},
            {"LONG", typeof(long)},
            {"SBYTE", typeof(sbyte)},
            {"FLOAT", typeof(float)},
            {"USHORT", typeof(ushort)},
            {"UINT", typeof(uint)},
            {"ULONG", typeof(ulong)},
            {"VOID", typeof(void)}
        };
        #endregion

        #region Public Methods
        /// <summary>
        ///     Gets the type.
        /// </summary>
        public static Type GetType(string fullTypeName)
        {
            var type = Find(fullTypeName);
            if (type == null)
            {
                throw Errors.TypeNotFound(fullTypeName);
            }

            return type;
        }
        #endregion

        #region Methods
        /// <summary>
        ///     Finds the specified type full name.
        /// </summary>
        static Type Find(string typeFullName)
        {
            const char Dot = '.';

            var isQualifiedName = typeFullName.IndexOf(Dot) == -1;
            if (isQualifiedName)
            {
                typeFullName = typeFullName.ToUpperEN();

                Type returnValue = null;
                QualifiedTypeNames.TryGetValue(typeFullName, out returnValue);
                return returnValue;
            }

            var type = Type.GetType(typeFullName);
            if (type != null)
            {
                return type;
            }

            foreach (var a in AppDomain.CurrentDomain.GetAssemblies())
            {
                type = a.GetType(typeFullName);
                if (type != null)
                {
                    return type;
                }
            }

            var assemblyLocation = typeof(TypeFinder).Assembly.Location;
            {
                var searchDirectory = Directory.GetParent(assemblyLocation)?.FullName + Path.DirectorySeparatorChar;
                foreach (var fileInfo in GetDotNetFiles(searchDirectory))
                {
                    try
                    {
                        var a = Assembly.LoadFile(fileInfo.FullName);
                        type = a.GetType(typeFullName);
                        if (type != null)
                        {
                            return type;
                        }
                    }
                    catch (Exception)
                    {
                        // ignored
                    }
                }
            }

            return null;
        }

        /// <summary>
        ///     Gets the dot net files.
        /// </summary>
        static IEnumerable<FileInfo> GetDotNetFiles(string directory)
        {
            return new DirectoryInfo(directory).GetFiles().Where(f => f.Extension == ".dll" || f.Extension == ".exe");
        }
        #endregion
    }
}