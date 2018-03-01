using System;
using System.Data;
using System.Linq;
using System.Text;

namespace WhiteStone.Services
{
    /// <summary>
    ///     Responsible for generate string representation of any DataTable.
    /// </summary>
    public interface IDataTableStringifier
    {
        /// <summary>
        ///     Generates string representation of given <paramref name="table" />.
        /// </summary>
        string StringifyDataTable(DataTable table);
    }

    class DataTableStringifier: IDataTableStringifier
    {
        public bool IsOuterBordersPresent { get; set; } //Whether outer borders of table needed
        public bool IsHeaderHorizontalSeparatorPresent { get; set; } // Whether horizontal line separator between table title and data is needed. Useful to set 'false' if you expect only 1 or 2 rows of data - no need for additional lines then
        public char ValueSeparator { get; set; } //Vertical line character
        public char HorizontalLinePadChar { get; set; } // Horizontal line character
        public char HorizontalLineSeparator { get; set; } // Horizontal border (between header and data) column separator (crossing of horizontal and vertical borders)
        public int ValueMargin { get; set; } // Horizontal margin from table borders (inner and outer) to cell values
        public int MaxColumnWidth { get; set; } // To avoid too wide columns with thousands of characters. Longer values will be cropped in the center
        public string LongValuesEllipses { get; set; } // Cropped values wil be inserted this string in the middle to mark the point of cropping

        public DataTableStringifier()
        {
            MaxColumnWidth = int.MaxValue;
            IsHeaderHorizontalSeparatorPresent = true;
            ValueSeparator = '|';
            ValueMargin = 1;
            HorizontalLinePadChar = '-';
            HorizontalLineSeparator = '+';
            LongValuesEllipses = "...";
            IsOuterBordersPresent = false;
        }

        public string StringifyDataTable(DataTable table)
        {
            if (table == null) throw new ArgumentNullException("table");

            var colCount = table.Columns.Count;
            var rowCount = table.Rows.Count;
            var colHeaders = new string[colCount];
            var cells = new string[rowCount, colCount];
            var colWidth = new int[colCount];

            for (var i = 0; i < colCount; i++)
            {
                var column = table.Columns[i];
                var colName = ValueToLimitedLengthString(column.ColumnName);
                colHeaders[i] = colName;
                if (colWidth[i] < colName.Length)
                {
                    colWidth[i] = colName.Length;
                }
            }

            for (var i = 0; i < rowCount; i++)
            {
                var row = table.Rows[i];
                for (var j = 0; j < colCount; j++)
                {
                    var valStr = ValueToLimitedLengthString(row[j]);
                    cells[i, j] = valStr;
                    if (colWidth[j] < valStr.Length)
                    {
                        colWidth[j] = valStr.Length;
                    }
                }
            }

            var valueSeparatorWithMargin = string.Concat(new string(' ', ValueMargin), ValueSeparator, new string(' ', ValueMargin));
            var leftBorder = IsOuterBordersPresent ? string.Concat(ValueSeparator, new string(' ', ValueMargin)) : "";
            var rightBorder = IsOuterBordersPresent ? string.Concat(new string(' ', ValueMargin), ValueSeparator) : "";
            var horizLine = new string(HorizontalLinePadChar, colWidth.Sum() + (colCount - 1)*(ValueMargin*2 + 1) + (IsOuterBordersPresent ? (ValueMargin + 1)*2 : 0));

            var tableBuilder = new StringBuilder();

            if (IsOuterBordersPresent)
            {
                tableBuilder.AppendLine(horizLine);
            }

            tableBuilder.Append(leftBorder);
            for (var i = 0; i < colCount; i++)
            {
                tableBuilder.Append(colHeaders[i].PadRight(colWidth[i]));
                if (i < colCount - 1)
                {
                    tableBuilder.Append(valueSeparatorWithMargin);
                }
            }
            tableBuilder.AppendLine(rightBorder);

            if (IsHeaderHorizontalSeparatorPresent)
            {
                if (IsOuterBordersPresent)
                {
                    tableBuilder.Append(ValueSeparator);
                    tableBuilder.Append(HorizontalLinePadChar, ValueMargin);
                }
                for (var i = 0; i < colCount; i++)
                {
                    tableBuilder.Append(new string(HorizontalLinePadChar, colWidth[i]));
                    if (i < colCount - 1)
                    {
                        tableBuilder.Append(HorizontalLinePadChar, ValueMargin);
                        tableBuilder.Append(HorizontalLineSeparator);
                        tableBuilder.Append(HorizontalLinePadChar, ValueMargin);
                    }
                }
                if (IsOuterBordersPresent)
                {
                    tableBuilder.Append(HorizontalLinePadChar, ValueMargin);
                    tableBuilder.Append(ValueSeparator);
                }
                tableBuilder.AppendLine();
            }

            for (var i = 0; i < rowCount; i++)
            {
                tableBuilder.Append(leftBorder);
                for (var j = 0; j < colCount; j++)
                {
                    tableBuilder.Append(cells[i, j].PadRight(colWidth[j]));
                    if (j < colCount - 1)
                    {
                        tableBuilder.Append(valueSeparatorWithMargin);
                    }
                }
                tableBuilder.AppendLine(rightBorder);
            }

            if (IsOuterBordersPresent)
            {
                tableBuilder.AppendLine(horizLine);
            }

            return tableBuilder.ToString(0, tableBuilder.Length - 1); //Trim last enter char
        }

        string ValueToLimitedLengthString(object value)
        {
            var strValue = value.ToString();
            if (strValue.Length > MaxColumnWidth)
            {
                var beginningLength = MaxColumnWidth/2;
                var endingLength = (MaxColumnWidth + 1)/2 - LongValuesEllipses.Length;
                return string.Concat(strValue.Substring(0, beginningLength), LongValuesEllipses, strValue.Substring(strValue.Length - endingLength, endingLength));
            }
            return strValue;
        }
    }
}