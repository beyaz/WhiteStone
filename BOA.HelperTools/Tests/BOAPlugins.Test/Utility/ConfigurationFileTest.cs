using System.IO;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BOAPlugins.Utility
{
    [TestClass]
    public class ConfigurationFileTest
    {
        #region Public Methods
        [TestMethod]
        public void Should_save_to_user_documents_if_file_not_exists()
        {
            var configurationFile = new ConfigurationFile
            {
                DirectoryPath = "A" + Path.DirectorySeparatorChar
            };
            configurationFile.Delete();

            var configuration = new Configuration
            {
                CheckInSolutionIsEnabled = true
            };

            // ACT
            configurationFile.Save(configuration);

            configuration = configurationFile.Load();

            // ASSERT
            configuration.CheckInSolutionIsEnabled.Should().BeTrue();
        }
        #endregion
    }
}