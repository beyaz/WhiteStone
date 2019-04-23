using BOA.Common.Helpers;
using BOA.EntityGeneration.Common;
using BOA.EntityGeneration.DbModel;

namespace BOA.EntityGeneration.Generators
{
    class ReadContractMethod : GeneratorBase
    {
        #region Public Methods
        public override string ToString()
        {
            var sb = new PaddedStringBuilder();

            sb.AppendLine($"void {Names.ISupportDmlOperation}.LoadFrom(IDataReader reader, DataRecordReader recordReader)");
            sb.AppendLine("{");
            sb.PaddingCount++;

            foreach (var columnInfo in TableInfo.Columns)
            {
                sb.AppendLine(GetLine(columnInfo));
            }

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
                return $"{c.ColumnName.ToContractName()}= recordReader.{c.SqlReaderMethod}(DataRecordReader.ReadValue(reader, \"{c.ColumnName}\")) == \"1\";";
            }

            return $"{c.ColumnName.ToContractName()} = recordReader.{c.SqlReaderMethod}(DataRecordReader.ReadValue(reader, \"{c.ColumnName}\"));";
        }
        #endregion
    }
}