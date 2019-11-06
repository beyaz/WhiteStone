using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using BOA.CodeGeneration.Common;
using BOA.CodeGeneration.Model;
using BOA.EntityGeneration;
using BOA.EntityGeneration.DbModel;
using BOA.EntityGeneration.DbModel.Interfaces;
using ColumnInfo = BOA.EntityGeneration.DbModel.Types.ColumnInfo;

namespace BOA.CodeGeneration.Generators
{
    class ReadContractGenerator : WriterBase
    {
        #region Constructors
        #region Constructor
        public ReadContractGenerator(WriterContext context)
            : base(context)
        {
        }
        #endregion
        #endregion

        #region Properties
        IReadOnlyList<IColumnInfo> Columns => Context.Table.Columns;

        string ContractName => Context.Naming.ContractName;

        string DatabaseTableFullPath => Context.Naming.DatabaseTableFullPath;

        bool DoCompressionForVarBinaryColumns => Context.Config.DoCompressionForVarBinaryColumns;
        #endregion

        #region Public Methods
        public string Generate()
        {
            Padding = PaddingForMethodDeclaration;
            WriteLine("/// <summary>");
            WriteLine("///" + PaddingForComment + "Reads '{0}' record from SqlDataReader", DatabaseTableFullPath);
            WriteLine("/// </summary>");
            WriteLine("public static {0} ReadContract({0} contract, IDataReader reader)", ContractName);
            WriteLine("{");
            Padding++;
            foreach (var r in Columns)
            {
                var propertyName = r.ColumnName;
                var readerMethod = GetReaderMethod(r);

                if (r.DataType.IsEqual(SqlDbType.VarBinary )&& DoCompressionForVarBinaryColumns)
                {
                    WriteLine("contract." + propertyName + " = CompressionHelper.DecompressBuffer(SQLDBHelper." + readerMethod + "(reader[" + "\"" + propertyName + "\"]));");
                }
                else if (IsDoNotTrimColumns(r))
                {
                    WriteLine("contract." + propertyName + " = SQLDBHelper." + readerMethod + "(reader[" + "\"" + propertyName + "\"],false);");
                }
                else
                {
                    var readValueFromIDataReader = "SQLDBHelper." + readerMethod + "(reader[" + "\"" + propertyName + "\"])";

                    if (Context.Config.ReadValueFromDataReader != null)
                    {
                        readValueFromIDataReader = Context.Config.ReadValueFromDataReader(new CustomReadValueFromIDataReaderInput
                        {
                            PropertyName       = propertyName,
                            ReaderArgumentName = "reader",
                            ReaderMethodName   = readerMethod
                        });
                    }

                    WriteLine("contract." + propertyName + " = " + readValueFromIDataReader + ";");
                }
            }

            WriteLine();
            WriteLine("return contract;");
            Padding--;
            WriteLine("}");

            return GeneratedString;
        }
        #endregion

        #region Methods
        SqlReaderMethods GetReaderMethod(IColumnInfo c)
        {
            var propertyName  = c.ColumnName;
            var readerMethod  = c.SqlReaderMethod;
            var specificReads = Context.Config.ReadContractSpecificReads;

            if (specificReads != null && specificReads.ContainsKey(propertyName))
            {
                readerMethod = specificReads[propertyName];
            }

            if (IsSecureColumn(c))
            {
                readerMethod = GetSecureColumnReadMethod(c);
            }

            return readerMethod;
        }

        SqlReaderMethods GetSecureColumnReadMethod(IColumnInfo columnInfo)
        {
            if (columnInfo.SqlReaderMethod == SqlReaderMethods.GetStringValue)
            {
                return SqlReaderMethods.GetSecureStringValue;
            }

            throw new InvalidOperationException("SqlReaderMethod not found." + columnInfo.SqlReaderMethod);
        }

        bool IsDoNotTrimColumns(IColumnInfo columnInfo)
        {
            var doNotTrimColumns = Context.Config.DoNotTrimColumns;
            if (doNotTrimColumns == null)
            {
                return false;
            }

            return doNotTrimColumns.Contains(columnInfo.ColumnName);
        }

        bool IsSecureColumn(IColumnInfo columnInfo)
        {
            return Context.Config.IsSecureColumn(columnInfo.ColumnName);
        }
        #endregion
    }
}