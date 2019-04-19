using System;
using System.Linq;
using BOA.Common.Helpers;

namespace BOA.CodeGeneration.Contracts.Transforms
{
    class ReadContractMethod
    {
        #region Public Properties
        public TableInfo TableInfo { get; set; }
        #endregion

        #region Public Methods
        public override string ToString()
        {
            var sb = new PaddedStringBuilder();

            sb.AppendLine($"object {Names.ISupportDmlOperationInfo}.ReadContract(IDataReader reader)");
            sb.AppendLine("{");
            sb.PaddingCount++;

            sb.AppendLine($"return new {TableInfo.TableName.ToContractName()}Contract");
            sb.AppendLine("{");
            sb.PaddingCount++;

            sb.AppendAll(string.Join("," + Environment.NewLine, TableInfo.Columns.Select(GetLine)));
            sb.AppendLine();

            sb.PaddingCount--;
            sb.AppendLine("};");

            sb.PaddingCount--;
            sb.AppendLine("}");

            return sb.ToString();
        }
        #endregion

        #region Methods
        static string GetLine(ColumnInfo c)
        {
            if (c.ColumnName == Names.VALID_FLAG)
            {
                return $"{c.ColumnName.ToContractName()}= DataReaderUtil.{c.SqlReaderMethod}(DataReaderUtil.ReadValue(reader, \"{c.ColumnName}\")) == \"1\"";
            }

            return $"{c.ColumnName.ToContractName()}= DataReaderUtil.{c.SqlReaderMethod}(DataReaderUtil.ReadValue(reader, \"{c.ColumnName}\"))";
        }
        #endregion
    }
}