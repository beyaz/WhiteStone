using System;
using System.Collections.Generic;
using BOA.DatabaseAccess;
using BOA.EntityGeneration.DbModel;
using WhiteStone.Helpers;

namespace BOA.EntityGeneration.BOACardDatabaseSchemaToDllExporting.Models
{
    /// <summary>
    ///     The generator data
    /// </summary>
    [Serializable]
    public class TableInfo : DbModel.TableInfo
    {
        #region Public Properties
        /// <summary>
        ///     Gets or sets the name of the database enum.
        /// </summary>
        public string DatabaseEnumName { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether this instance is support get all.
        /// </summary>
        public bool IsSupportGetAll { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether this instance is support select by index.
        /// </summary>
        public bool IsSupportSelectByIndex { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether this instance is support select by key.
        /// </summary>
        public bool IsSupportSelectByKey { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether this instance is support select by unique index.
        /// </summary>
        public bool IsSupportSelectByUniqueIndex { get; set; }

        /// <summary>
        ///     Gets or sets the non unique index information list.
        /// </summary>
        public IReadOnlyList<IndexInfo> NonUniqueIndexInfoList { get; set; }
        

        public IReadOnlyList<SequenceInfo> SequenceList { get; set; }

        /// <summary>
        ///     Gets or sets the unique index information list.
        /// </summary>
        public IReadOnlyList<IndexInfo> UniqueIndexInfoList { get; set; }

        public bool ShouldGenerateSelectAllByValidFlagMethodInBusinessClass { get; set; }
        #endregion
    }

    [Serializable]
    public class SequenceInfo
    {
        public string Name { get; set; }
        public string TargetColumnName { get; set; }
    }

    static class SequenceInfoHelper
    {
        public static IReadOnlyList<SequenceInfo> GetSequenceListOfTable(this IDatabase Database, string schema, string tableName)
        {
            Database.CommandText = $"select  DISTINCT(columnname) AS TargetColumnName , (schemaname +'.' + sequencename) AS Name from BOACard.dbo.tablesequences WHERE schemaname = '{schema}' AND tablename = '{tableName}'";

            return Database.ExecuteReader().ToList<SequenceInfo>();
        }
    }

}