using System;
using System.Collections.Generic;
using System.IO;
using BOA.Common.Helpers;
using BOA.DatabaseAccess;
using BOA.EntityGeneration.BOACardDatabaseSchemaToDllExporting.DataAccess;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BOA.EntityGeneration.DbModel.SqlServerDataAccess
{

  


    [TestClass]
    public class SampleDatabaseTest
    {
        #region Public Methods
        [TestMethod]
        public void AllInOne()
        {
            var mdfFilePath = $"{Directory.GetParent(Environment.CurrentDirectory).Parent?.FullName}{Path.DirectorySeparatorChar}SampleDatabase.mdf";

            var connectionString = $@"Data Source = (LocalDB)\MSSQLLocalDB; AttachDbFilename = {mdfFilePath}; Integrated Security = True";

            var database = new SqlDatabase(connectionString)
            {
                CommandTimeout = 1000 * 60 * 60
            };


            var dao = new IndexInfoAccess {Database = database};

            dao.GetIndexInformation("A", "y");

           
        }
        #endregion
    }
}