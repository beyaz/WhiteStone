using System.Collections.Generic;
using System.Data;

namespace BOA.Data
{
    /// <summary>
    ///     The DML SQL generator output
    /// </summary>
    class DmlSqlGeneratorOutput
    {
        #region Public Properties
        /// <summary>
        ///     Gets or sets the generated parameters.
        /// </summary>
        public List<IDbDataParameter> GeneratedParameters { get; set; }

        /// <summary>
        ///     Gets or sets the genrated SQL.
        /// </summary>
        public string GenratedSql { get; set; }
        #endregion
    }
}