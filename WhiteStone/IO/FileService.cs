using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security;
using System.Security.Permissions;
using System.Text;

namespace WhiteStone.IO
{
    /// <summary>
    ///     Utility file methods.
    /// </summary>
    public class FileService
    {
        #region Public Properties
        /// <summary>
        ///     Gets directory seperator
        /// </summary>
        public virtual char DirectorySeparatorChar
        {
            get { return Path.DirectorySeparatorChar; }
        }
        #endregion

        #region Public Methods
        /// <summary>
        ///     Appends specific text to file.
        /// </summary>
        /// <param name="path"></param>
        /// <param name="contents"></param>
        public void Append(string path, string contents)
        {
            EnsureDirectory(path);
            File.AppendAllText(path, contents, Encoding.UTF8);
        }

        /// <summary>
        ///     Creates all directories and subdirectories in the specified path.
        /// </summary>
        /// <param name="path"></param>
        public virtual void CreateDirectory(string path)
        {
            Directory.CreateDirectory(path);
        }

        /// <summary>
        ///     if path is not exists then creates a file.
        /// </summary>
        /// <param name="path"></param>
        public void EnsureDirectory(string path)
        {
            if (!path.EndsWith(DirectorySeparatorChar.ToString(), StringComparison.CurrentCulture))
            {
                path = path.Substring(0, path.LastIndexOf(DirectorySeparatorChar) + 1);
            }
            if (!Directory.Exists(path))
            {
                CreateDirectory(path);
            }
        }

        /// <summary>
        ///     Determines whether the specified file exists.
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public virtual bool Exists(string path)
        {
            return File.Exists(path);
        }

        /// <summary>
        ///     Reads all text of given path.
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public virtual string Read(string path)
        {
            return File.ReadAllText(path, Encoding.UTF8);
        }

        /// <summary>
        ///     Gets byte[] value of file
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public virtual byte[] ReadAsByte(string path)
        {
            byte[] array;
            using (var fileStream = new FileStream(path, FileMode.Open, FileAccess.Read))
            {
                array = new byte[fileStream.Length];
                fileStream.Read(array, 0, (int) fileStream.Length);
            }
            return array;
        }

        /// <summary>
        ///     Gets filenames by filter.
        /// </summary>
        /// <param name="dir"></param>
        /// <param name="predicateForFileName"></param>
        /// <returns></returns>
        public IList<string> SearchFileNames(string dir, Predicate<string> predicateForFileName)
        {
            var list = new List<string>();
            SearchInOneDirectory(dir, list, predicateForFileName);
            return list;
        }

        /// <summary>
        ///     Tries to delete specific file.
        /// </summary>
        public bool TryDelete(string path)
        {
            if (!Exists(path))
            {
                return false;
            }
            File.Delete(path);
            return true;
        }

        /// <summary>
        ///     Writes contents to given path
        /// </summary>
        /// <param name="path"></param>
        /// <param name="contents"></param>
        public void Write(string path, string contents)
        {
            EnsureDirectory(path);

            if (!HasWritePermission(path))
            {
                throw new IOException("HasNoPermissionToWrite: " + path);
            }
            if (File.Exists(path) && IsReadOnly(path))
            {
                throw new IOException("FileIsReadonly: " + path);
            }

            File.WriteAllText(path, contents, Encoding.UTF8);
        }

        /// <summary>
        ///     Writes all bytes to given <paramref name="path" />
        /// </summary>
        /// <param name="path"></param>
        /// <param name="byteArray"></param>
        public void Write(string path, byte[] byteArray)
        {
            EnsureDirectory(path);
            using (var fileStream = File.Create(path))
            {
                fileStream.Write(byteArray, 0, byteArray.Length);
            }
        }
        #endregion

        #region Methods
        static bool HasWritePermission(string path)
        {
            try
            {
                new FileIOPermission(FileIOPermissionAccess.Write, path).Demand();
            }
            catch (SecurityException)
            {
                return false;
            }
            return true;
        }

        static bool IsReadOnly(string path)
        {
            return new FileInfo(path).IsReadOnly;
        }

        void SearchInOneDirectory(string dir, List<string> list, Predicate<string> predicateForFileName)
        {
            var array = Directory.GetFiles(dir);
            list.AddRange(array.Where(text => predicateForFileName(text)));
            array = Directory.GetDirectories(dir);
            foreach (var dir2 in array)
            {
                SearchInOneDirectory(dir2, list, predicateForFileName);
            }
        }
        #endregion
    }
}