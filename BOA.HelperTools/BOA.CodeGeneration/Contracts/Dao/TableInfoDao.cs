﻿using System;
using System.Data;
using System.Linq;
using BOA.CodeGeneration.Contracts.DaoHelper;
using BOA.CodeGeneration.Services;
using BOA.DatabaseAccess;

namespace BOA.CodeGeneration.Contracts.Dao
{
    public class TableInfoDao
    {
        #region Public Properties
        public IDatabase Database { get; set; }
        #endregion

        #region Public Methods
        public TableInfo GetInfo(string tableCatalog, string schema, string tableName)
        {
            using (var dal = new DataAccess(Database))
            {
                if (!dal.TableExists(schema, tableName))
                {
                    throw new ArgumentException("TableNotFoundInDatabase:" + tableName);
                }

                var table = dal.GetTableInformation(tableCatalog, schema, tableName);

                if (table.Rows.Count <= 0)
                {
                    throw new ArgumentException("TableNotFoundInDatabase:" + tableName);
                }

                var columns = TableInfoGeneratorFromMsSql.CreateColumns(table);

                var primaryKeyColumns = dal.GetPrimaryColumns(schema, tableName);

                foreach (DataRow r in primaryKeyColumns.Rows)
                {
                    var indexName = r[0].ToString().Trim();

                    (from c in columns where c.ColumnName == indexName select c).First().IsPrimaryKey = true;
                }

                foreach (var c in columns)
                {
                    c.Comment = dal.GetColumnComment(schema, tableName, c.ColumnName);
                }

                return new TableInfo
                {
                    Columns           = columns,
                    IdentityColumn    = columns.FirstOrDefault(c => c.IsIdentity),
                    HasIdentityColumn = columns.Any(c => c.IsIdentity),
                    PrimaryKeyColumns = columns.Where(c => c.IsPrimaryKey).ToList()
                };
            }
        }
        #endregion
    }
}