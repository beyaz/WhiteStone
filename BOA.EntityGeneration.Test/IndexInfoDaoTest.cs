using System.Collections.Generic;
using BOA.Common.Helpers;
using BOA.DatabaseAccess;
using BOA.EntityGeneration.Dao;
using BOA.EntityGeneration.DbModel;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BOA.CodeGeneration.Contracts.Dao
{
    public class BOACardDatabase : SqlDatabase
    {
        public BOACardDatabase():base($"Server={@"srvxdev\zumrut"};Database={@"BOACard"};Trusted_Connection=True;")
        {
            
        }
    }

    [TestClass]
    public class IndexInfoDaoTest
    {
        #region Public Methods
        [TestMethod]
        public void Should_evaluates_IndexInformation_by_table_name()
        {
           

            var dao = new IndexInfoDao {Database = new BOACardDatabase()};

            var information = dao.GetIndexInformation("CRD", "CARD_MASTER");

            information.Should().BeEquivalentTo(JsonHelper.Deserialize<IReadOnlyList<IndexInfo>>(@"[
  {
    'Name': 'CARD_MASTER_LOYALTY_NO',
    'IsClustered': false,
    'IsNonClustered': true,
    'IsUnique': false,
    'IsPrimaryKey': false,
    'ColumnNames': [
      'LOYALTY_NO'
    ]
  },
  {
    'Name': 'IX_CARD_MASTER_1',
    'IsClustered': false,
    'IsNonClustered': true,
    'IsUnique': false,
    'IsPrimaryKey': false,
    'ColumnNames': [
      'CUSTOMER_NUMBER'
    ]
  },
  {
    'Name': 'IX_CARD_MASTER_2',
    'IsClustered': false,
    'IsNonClustered': true,
    'IsUnique': true,
    'IsPrimaryKey': false,
    'ColumnNames': [
      'SHADOW_CARD_NUMBER'
    ]
  },
  {
    'Name': 'IX_CARD_MASTER_CARD_REF_NUMBER',
    'IsClustered': false,
    'IsNonClustered': true,
    'IsUnique': true,
    'IsPrimaryKey': false,
    'ColumnNames': [
      'CARD_REF_NUMBER'
    ]
  },
  {
    'Name': 'IX_CARD_MASTER_MAIN_CARD_REF_NUMBER',
    'IsClustered': false,
    'IsNonClustered': true,
    'IsUnique': false,
    'IsPrimaryKey': false,
    'ColumnNames': [
      'MAIN_CARD_REF_NUMBER'
    ]
  },
  {
    'Name': 'IX_CARD_MASTER_MAIN_CUSTOMER_NUMBER',
    'IsClustered': false,
    'IsNonClustered': true,
    'IsUnique': false,
    'IsPrimaryKey': false,
    'ColumnNames': [
      'MAIN_CUSTOMER_NUMBER'
    ]
  },
  {
    'Name': 'PK_CARD_MASTER',
    'IsClustered': true,
    'IsNonClustered': false,
    'IsUnique': true,
    'IsPrimaryKey': true,
    'ColumnNames': [
      'RECORD_ID'
    ]
  }
]"));
        }
        #endregion
    }
}