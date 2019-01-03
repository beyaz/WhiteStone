using System;
using System.IO;
using BOA.Common.Helpers;

namespace BOA.Services
{
    /// <summary>
    ///     The json file
    /// </summary>
    public class JsonFile<T> where T : new()
    {
        #region Constructors
        /// <summary>
        ///     Initializes a new instance of the <see cref="JsonFile{T}" /> class.
        /// </summary>
        public JsonFile(string filePath)
        {
            if (filePath == null)
            {
                throw new ArgumentNullException(nameof(filePath));
            }

            FilePath = filePath;
        }
        #endregion

        #region Public Properties
        /// <summary>
        ///     Gets the file path.
        /// </summary>
        public string FilePath { get; }
        #endregion

        #region Public Methods
        /// <summary>
        ///     Deletes this instance.
        /// </summary>
        public void Delete()
        {
            if (File.Exists(FilePath))
            {
                File.Delete(FilePath);
            }
        }

        /// <summary>
        ///     Loads this instance.
        /// </summary>
        public T Load()
        {
            var fileNotExists = File.Exists(FilePath) == false;
            if (fileNotExists)
            {
                Save(new T());
            }

            var configurationAsString = File.ReadAllText(FilePath);

            return JsonHelper.Deserialize<T>(configurationAsString);
        }

        /// <summary>
        ///     Saves the specified configuration.
        /// </summary>
        public void Save(T configuration)
        {
            FileHelper.WriteAllText(FilePath, JsonHelper.Serialize(configuration));
        }
        #endregion
    }
}