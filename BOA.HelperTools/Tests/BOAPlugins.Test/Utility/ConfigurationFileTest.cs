using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BOAPlugins.Utility
{
    [TestClass]
    public class ConfigurationFileTest
    {
        [TestMethod]
        public void Should_save_to_user_documents_if_file_not_exists()
        {
            var configurationFile = new ConfigurationFile();

            var configuration = new Configuration
            {
                CheckInSolutionIsEnabled = true
            };


            configurationFile.Save(configuration);

            configuration = configurationFile.Load();

            configuration.CheckInSolutionIsEnabled.Should().BeTrue();

        }
    }
}
