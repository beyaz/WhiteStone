using System.Collections.Generic;
using System.Linq;
using BOA.CodeGeneration.Common;
using BOA.CodeGeneration.Model;
using BOA.CodeGeneration.Util;
using BOA.EntityGeneration.Common;
using ColumnInfo = BOA.CodeGeneration.Contracts.ColumnInfo;

namespace BOA.CodeGeneration.Generators
{
    public class InsertSql : WriterBase
    {
        #region Constructors
        public InsertSql(WriterContext context)
            : base(context)
        {
        }
        #endregion

        #region Properties
        bool CanGenerateSetNoCountOn
        {
            get
            {
                if (Context.Config.CanGenerateSetNoCountOnInsert)
                {
                    return true;
                }

                return !Context.Table.HasIdentityColumn;
            }
        }

        IReadOnlyList<Contracts.ColumnInfo> Columns => Context.Table.Columns;

        string DatabaseTableFullPath => Context.Config.DatabaseTableFullPath;

        string DatabaseTargetSchemaForProcedureNames => Context.Naming.SchemaName;

        string NameOfSqlProcedureInsert => Context.Naming.NameOfSqlProcedureInsert;

        string NameOfSqlProceduresWillBeRunCatalogName => Context.Naming.NameOfSqlProceduresWillBeRunCatalogName;
        #endregion

        #region Public Methods
        public static List<Contracts.ColumnInfo> GetProcedureParameterColumns(IReadOnlyList<Contracts.ColumnInfo> columns)
        {
            var returnList = new List<ColumnInfo>();

            foreach (var columnInfo in columns)
            {
                if (Names.GenericUpdateInformationColumns.Contains(columnInfo.ColumnName))
                {
                    continue;
                }

                if (columnInfo.IsIdentity)
                {
                    continue;
                }

                if (columnInfo.DataType == SqlDataType.Timestamp)
                {
                    continue;
                }

                returnList.Add(columnInfo);
            }

            return returnList;
        }

        public string Generate()
        {
            WriteLine("USE {0}", NameOfSqlProceduresWillBeRunCatalogName);
            WriteLine("GO");
            WriteLine(string.Empty);

            WriteLine("IF EXISTS (SELECT TOP 1 1 FROM sys.objects WHERE object_id = OBJECT_ID(N'[{0}].[{1}]') AND type in (N'P', N'PC'))",
                      DatabaseTargetSchemaForProcedureNames, NameOfSqlProcedureInsert);

            Padding++;

            WriteLine("DROP PROCEDURE {0}", DatabaseTargetSchemaForProcedureNames + "." + NameOfSqlProcedureInsert);

            Padding--;

            WriteLine("GO");
            WriteLine("");

            var comments = new List<string>
            {
                "Insert new record into '{0}'".FormatCode(DatabaseTableFullPath)
            };
            WriteSqlComment(comments);

            WriteLine("CREATE PROCEDURE " + DatabaseTargetSchemaForProcedureNames + "." + NameOfSqlProcedureInsert);
            WriteLine("(");

            Padding++;
            WriteInsertParameters();
            Padding--;

            WriteLine(")");
            WriteLine("AS");
            if (CanGenerateSetNoCountOn)
            {
                WriteLine("SET NOCOUNT ON");
            }

            WriteLine("BEGIN");
            WriteLine();

            Padding++;
            WriteLine("INSERT INTO {0}", Context.Config.TablePathForSqlScript);
            WriteLine("(");

            Padding++;
            WriteInputParameters();
            Padding--;

            WriteLine(")");
            WriteLine("VALUES");

            WriteLine("(");
            Padding++;
            WriteValues();
            Padding--;
            WriteLine(")");
            WriteLine();

            if (Context.Table.HasIdentityColumn)
            {
                WriteLine("SELECT CAST(SCOPE_IDENTITY() AS INT)");
            }

            WriteLine();
            Padding--;
            WriteLine("END");

            return GeneratedString;
        }
        #endregion

        #region Methods
        void WriteInputParameters()
        {
            var lines = GetProcedureParameterColumns(Columns).Select(c => c.ColumnName.NormalizeColumnNameForReversedKeywords()).ToList();

            WriteLinesWithComma(lines);
        }

        void WriteInsertParameters()
        {
            var lines = new List<string>();

            foreach (var column in GetProcedureParameterColumns(Columns))
            {
                var dataType = column.DataType;

                if (dataType == SqlDataType.VarBinary)
                {
                    dataType = dataType + "(MAX)";
                }

                if (column.IsNullable)
                {
                    lines.Add($"@{column.ColumnName} {dataType} = NULL");
                    continue;
                }

                lines.Add($"@{column.ColumnName} {dataType}");
            }

            WriteLinesWithComma(lines);
        }

        void WriteValues()
        {
            WriteLinesWithComma(GetProcedureParameterColumns(Columns).Select(c => "@" + c.ColumnName).ToList());
        }
        #endregion
    }
}