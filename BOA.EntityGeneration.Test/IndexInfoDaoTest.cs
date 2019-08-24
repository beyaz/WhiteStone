using System.Collections.Generic;
using BOA.Common.Helpers;
using BOA.EntityGeneration.BOACardDatabaseSchemaToDllExporting.Exporters;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BOA.EntityGeneration.DbModel.SqlServerDataAccess
{
    [TestClass]
    public class IndexInfoDaoTest
    {
        #region Public Methods
        [TestMethod]
        public void Should_evaluates_IndexInformation_by_table_name()
        {
            var dao = new IndexInfoAccess {Database = Kernel.CreateConnection()};

            var information = dao.GetIndexInformation("CRD", "CARD_MASTER");

            information.Should().BeEquivalentTo(JsonHelper.Deserialize<IReadOnlyList<IndexInfo>>(@"
  [
  {
    'ColumnNames': [
      'LOYALTY_NO'
    ],
    'IsClustered': false,
    'IsNonClustered': true,
    'IsPrimaryKey': false,
    'IsUnique': false,
    'Name': 'CARD_MASTER_LOYALTY_NO'
  },
  {
    'ColumnNames': [
      'CUSTOMER_NUMBER'
    ],
    'IsClustered': false,
    'IsNonClustered': true,
    'IsPrimaryKey': false,
    'IsUnique': false,
    'Name': 'IX_CARD_MASTER_1'
  },
  {
    'ColumnNames': [
      'SHADOW_CARD_NUMBER'
    ],
    'IsClustered': false,
    'IsNonClustered': true,
    'IsPrimaryKey': false,
    'IsUnique': true,
    'Name': 'IX_CARD_MASTER_2'
  },
  {
    'ColumnNames': [
      'CARD_REF_NUMBER'
    ],
    'IsClustered': false,
    'IsNonClustered': true,
    'IsPrimaryKey': false,
    'IsUnique': false,
    'Name': 'IX_CARD_MASTER_CARD_REF_NUMBER'
  },
  {
    'ColumnNames': [
      'ROW_GUID'
    ],
    'IsClustered': false,
    'IsNonClustered': true,
    'IsPrimaryKey': false,
    'IsUnique': true,
    'Name': 'IX_CARD_MASTER_CARD_REF_NUMBER_MASKED_CARD_NUMBER'
  },
  {
    'ColumnNames': [
      'MAIN_CARD_REF_NUMBER'
    ],
    'IsClustered': false,
    'IsNonClustered': true,
    'IsPrimaryKey': false,
    'IsUnique': false,
    'Name': 'IX_CARD_MASTER_MAIN_CARD_REF_NUMBER'
  },
  {
    'ColumnNames': [
      'MAIN_CUSTOMER_NUMBER'
    ],
    'IsClustered': false,
    'IsNonClustered': true,
    'IsPrimaryKey': false,
    'IsUnique': false,
    'Name': 'IX_CARD_MASTER_MAIN_CUSTOMER_NUMBER'
  },
  {
    'ColumnNames': [
      'RECORD_ID'
    ],
    'IsClustered': true,
    'IsNonClustered': false,
    'IsPrimaryKey': true,
    'IsUnique': true,
    'Name': 'PK_CARD_MASTER'
  }
]

"));
        }
        #endregion
    }
}