using System;
using System.Collections.Generic;
using System.Linq;
using BOA.CodeGeneration.Common;
using BOA.CodeGeneration.Model;
using BOA.EntityGeneration.DbModel;
using ColumnInfo = BOA.EntityGeneration.DbModel.ColumnInfo;

namespace BOA.CodeGeneration.Generators
{
    class UpdateSqlCustom : UpdateSql
    {
        #region Fields
        readonly IEnumerable<IColumnInfo> _updateColumns;
        #endregion

        #region Constructors
        public UpdateSqlCustom(WriterContext context, CustomUpdateMethod customUpdateMethod)
            : base(context)
        {
            CustomUpdate = customUpdateMethod;

            var updateColumns = customUpdateMethod.UpdateColumnNames.Split(',');
            var whereColumns  = customUpdateMethod.WhereColumnNames.Split(',');

            foreach (var columnName in updateColumns)
            {
                if (Context.Table.Columns.All(x => x.ColumnName != columnName))
                {
                    throw new ArgumentException("InvalidColumnName:" + columnName);
                }
            }

            foreach (var columnName in whereColumns)
            {
                if (Context.Table.Columns.All(x => x.ColumnName != columnName))
                {
                    throw new ArgumentException("InvalidColumnName:" + columnName);
                }
            }

            WhereColumns = from c in Context.Table.Columns
                           where whereColumns.Contains(c.ColumnName)
                           select c;

            ProcedureParameters = from c in Context.Table.Columns
                                  where updateColumns.Contains(c.ColumnName) ||
                                        whereColumns.Contains(c.ColumnName) ||
                                        Names2.GenericUpdateInformationColumns.Contains(c.ColumnName)
                                  select c;

            _updateColumns = from c in Context.Table.Columns
                             where updateColumns.Contains(c.ColumnName) ||
                                   Names2.GenericUpdateInformationColumns.Contains(c.ColumnName)
                             select c;
        }
        #endregion

        #region Public Properties
        public CustomUpdateMethod CustomUpdate { get; set; }
        #endregion

        #region Properties
        protected override string Comment => CustomUpdate.Comment;

        protected override string NameOfSqlProcedureUpdate => CustomUpdate.SqlProcedureName;

        protected override IEnumerable<IColumnInfo> ProcedureParameters { get; }

        protected override IEnumerable<IColumnInfo> WhereColumns { get; }
        #endregion

        #region Methods
        protected override IEnumerable<IColumnInfo> GetUpdateColumns()
        {
            return _updateColumns;
        }
        #endregion
    }
}