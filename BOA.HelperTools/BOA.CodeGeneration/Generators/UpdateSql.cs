using System.Collections.Generic;
using System.Linq;
using BOA.CodeGeneration.Common;
using BOA.CodeGeneration.Model;
using BOA.EntityGeneration;
using ColumnInfo = BOA.EntityGeneration.DbModel.ColumnInfo;
using Names2 = BOA.CodeGeneration.Common.Names2;

namespace BOA.CodeGeneration.Generators
{
    public class UpdateSql : WriterBase
    {
        #region Constructors
        public UpdateSql(WriterContext context)
            : base(context)
        {
        }
        #endregion

        #region Properties
        protected virtual string Comment => $"Updates only one record of '{DatabaseTableFullPath}'";

        protected virtual string NameOfSqlProcedureUpdate => Context.Naming.NameOfSqlProcedureUpdate;

        protected virtual IEnumerable<ColumnInfo> ProcedureParameters
        {
            get
            {
                return Columns.Where(c => !(
                                         c.ColumnName == Names2.UserName ||
                                         c.ColumnName == Names2.HostName ||
                                         c.ColumnName == Names2.SystemDate ||
                                         c.ColumnName == Names2.HostIP ||
                                         c.IsIdentity && !c.IsPrimaryKey ||
                                         c.DataType == SqlDataType.Timestamp))
                              .Select(y => y)
                              .ToList();
            }
        }

        protected virtual IEnumerable<ColumnInfo> WhereColumns => Context.Table.PrimaryKeyColumns;

        bool CanGenerateSetNoCountOn
        {
            get
            {
                if (Context.Config.CanGenerateSetNoCountOnUpdate)
                {
                    return true;
                }

                return !Context.Table.HasIdentityColumn;
            }
        }

        IReadOnlyList<ColumnInfo> Columns => Context.Table.Columns;

        string DatabaseTableFullPath => Context.Config.DatabaseTableFullPath;

        string DatabaseTargetSchemaForProcedureNames => Context.Naming.SchemaName;

        string NameOfSqlProceduresWillBeRunCatalogName => Context.Naming.NameOfSqlProceduresWillBeRunCatalogName;
        #endregion

        #region Public Methods
        public string Generate()
        {
            WriteLine($"USE {NameOfSqlProceduresWillBeRunCatalogName}");
            WriteLine("GO");
            WriteLine("");

            WriteLine($"IF EXISTS (SELECT TOP 1 1 FROM sys.objects WHERE object_id = OBJECT_ID(N'[{DatabaseTargetSchemaForProcedureNames}].[{NameOfSqlProcedureUpdate}]') AND type in (N'P', N'PC'))");

            Padding++;

            WriteLine($"DROP PROCEDURE {DatabaseTargetSchemaForProcedureNames}.{NameOfSqlProcedureUpdate}");

            Padding--;

            WriteLine("GO");
            WriteLine();

            WriteSqlComment(new List<string>
            {
                Comment
            });

            WriteLine($"CREATE PROCEDURE {DatabaseTargetSchemaForProcedureNames}.{NameOfSqlProcedureUpdate}");
            WriteLine("(");

            Padding++;

            var inputParameterLines = new List<string>();

            foreach (var c in ProcedureParameters)
            {
                var dataType = c.DataType;
                if (dataType == SqlDataType.VarBinary)
                {
                    dataType = dataType + "(MAX)";
                }

                if (c.IsNullable)
                {
                    inputParameterLines.Add($"@{c.ColumnName} {dataType} = NULL");
                    continue;
                }

                inputParameterLines.Add($"@{c.ColumnName} {dataType}");
            }

            WriteLinesWithComma(inputParameterLines);

            Padding--;

            WriteLine(")");
            WriteLine("AS");
            if (CanGenerateSetNoCountOn)
            {
                WriteLine("SET NOCOUNT ON");
            }

            WriteLine("BEGIN");
            WriteLine("");

            Padding++;
            WriteLine("UPDATE T SET");

            Padding++;

            var updateColumns = GetUpdateColumns().Select(c => c.ColumnName.NormalizeColumnNameForReversedKeywords()).ToList();

            var updateLines = updateColumns.Select(columnName => $"T.{columnName} = @{columnName}").ToList();

            WriteLinesWithComma(updateLines);

            Padding--;

            WriteLine();
            Padding++;
            var whereParameters = string.Join(" AND ", from c in WhereColumns select $"T.{c.ColumnName} = @{c.ColumnName}");
            WriteLine($"FROM {Context.Config.TablePathForSqlScript} AS T WHERE {whereParameters}");
            Padding--;

            WriteLine("");
            Padding--;
            WriteLine("END");

            return GeneratedString;
        }
        #endregion

        #region Methods
        protected virtual IEnumerable<ColumnInfo> GetUpdateColumns()
        {
            return Columns.Where(c => !(
                                     c.ColumnName == Names2.UserName ||
                                     c.ColumnName == Names2.HostName ||
                                     c.ColumnName == Names2.SystemDate ||
                                     c.ColumnName == Names2.HostIP ||
                                     c.IsIdentity ||
                                     c.DataType == SqlDataType.Timestamp))
                          .Select(y => y);
        }
        #endregion
    }
}