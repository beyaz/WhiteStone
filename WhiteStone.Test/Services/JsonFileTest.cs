using System;
using BOA.Common.Helpers;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BOA.Services
{
    [TestClass]
    public class JsonFileTest
    {
        #region Public Properties
        public string Prop1 { get; set; }
        #endregion

        [TestMethod]
        public void Should_throw_exception_when_file_path_is_null()
        {
            Func<JsonFile<JsonFileTest>> action = () => new JsonFile<JsonFileTest>(null);

            action.Should().Throw<ArgumentNullException>();

        }
        #region Public Methods
        [TestMethod]
        public void Should_save_to_file_if_file_not_exists()
        {
            EmbeddedAssembly.AttachToCurrentDomain();

            var jsonFile = new JsonFile<JsonFileTest>("A\\t.txt");

            jsonFile.Delete();

            var data  = jsonFile.Load();

            data.Prop1 = "A";

            // ACT
            jsonFile.Save(data);

            data = jsonFile.Load();

            // ASSERT
            data.Prop1.Should().Be("A");
        }

        

        #endregion
    }
}