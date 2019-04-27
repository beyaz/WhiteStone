﻿using System.Collections.Generic;
using System.Linq;
using BOA.CodeGeneration.Common;
using BOA.CodeGeneration.Model;
using BOA.EntityGeneration.Common;
using ColumnInfo = BOA.EntityGeneration.DbModel.Types.ColumnInfo;
using Names2 = BOA.CodeGeneration.Common.Names2;

namespace BOA.CodeGeneration.Generators
{
    class InsertCs : WriterBase
    {
        #region Constructors
        #region Constructor
        public InsertCs(WriterContext context)
            : base(context)
        {
        }
        #endregion
        #endregion

        #region Properties
        bool DoCompressionForVarBinaryColumns => Context.Config.DoCompressionForVarBinaryColumns;
        #endregion

        #region Public Methods
        public string Generate()
        {
            Padding = PaddingForMethodDeclaration;
            WriteLine("/// <summary>");
            WriteLine("///" + PaddingForComment + "Insert new record into '{0}'", DatabaseTableFullPath);
            WriteLine("/// </summary>");

            var methodAccess = "public ";
            if (Context.Config.InsertMethodMustBePrivate)
            {
                methodAccess = "";
            }

            WriteLine($"{methodAccess}GenericResponse<int> {NameOfCsMethodInsert}({ContractName} contract)");

            WriteLine("{");

            Padding++;

            WriteLine("var returnObject = CreateResponse<int>();");
            WriteLine();
            WriteLine("if (contract == null)");
            WriteLine("{");
            Padding++;
            WriteLine($"return returnObject.AddError({MethodParameterCannotBeNullMessage});");
            Padding--;
            WriteLine("}");

            WriteLine();

            WriteLine($"var command = DBLayer.GetDBCommand(Databases.{DatabaseEnumName}, \"{DatabaseTargetSchemaForProcedureNames}.{NameOfSqlProcedureInsert}\");");

            WriteLine("");

            WriteLine("#region Parameters");

            var contractUpdated = false;

            if (Columns.Any(c => c.ColumnName == Names2.UserName))
            {
                contractUpdated = true;
                WriteLine("contract.UserName = {0};", Names2.UserNameValue);
            }

            if (Columns.Any(c => c.ColumnName == Names2.HostName))
            {
                contractUpdated = true;
                WriteLine("contract.HostName = {0};", Names2.HostNameValue);
            }

            if (Columns.Any(c => c.ColumnName == Names2.HostIP))
            {
                contractUpdated = true;
                WriteLine("contract.HostIP = {0};", Names2.HostIPValue);
            }

            if (Columns.Any(c => c.ColumnName == Names2.SystemDate))
            {
                contractUpdated = true;
                WriteLine("contract.SystemDate = {0};", Names2.SystemDateValue);
            }

            if (contractUpdated)
            {
                WriteLine();
            }

            var columns = GetProcedureParameterColumns();

            foreach (var c in columns)
            {
                var dbLayerMethod = GetDbLayerMethod(c);

                var inputValue = $"contract.{c.ColumnName}";

                if (c.DataType == SqlDataType.VarBinary &&
                    DoCompressionForVarBinaryColumns)
                {
                    inputValue = $"CompressionHelper.CompressBuffer({inputValue})";
                }

                WriteLine($"DBLayer.{dbLayerMethod}(command, \"{c.ColumnName}\", SqlDbType.{c.SqlDatabaseTypeName}, {inputValue});");
            }

            WriteLine("#endregion");
            WriteLine();

            if (Context.Table.HasIdentityColumn)
            {
                WriteLine("var sp = DBLayer.ExecuteScalar<int>(command);");
                WriteLine("if (!sp.Success)");
                WriteLine("{");
                Padding++;
                WriteLine("return returnObject.Add(sp);");
                Padding--;
                WriteLine("}");
            }
            else
            {
                WriteLine("var sp = DBLayer.ExecuteNonQuery(command);");
                WriteLine("if (!sp.Success)");
                WriteLine("{");
                Padding++;
                WriteLine("return returnObject.Add(sp);");
                Padding--;
                WriteLine("}");
            }

            WriteLine("returnObject.Value = sp.Value;");
            WriteLine();

            if (Context.Table.HasIdentityColumn)
            {
                WriteLine($"contract.{Context.Table.IdentityColumn.ColumnName} = returnObject.Value;");
                WriteLine();
            }

            WriteLine("return returnObject;");
            Padding--;
            WriteLine("}");

            return GeneratedString;
        }
        #endregion

        #region Methods
        string GetDbLayerMethod(ColumnInfo columnInfo)
        {
            if (Context.Config.IsSecureColumn(columnInfo.ColumnName))
            {
                return "AddInSecureParameter";
            }

            return "AddInParameter";
        }

        List<ColumnInfo> GetProcedureParameterColumns()
        {
            return InsertSql.GetProcedureParameterColumns(Columns);
        }
        #endregion

        #region Parameters
        string MethodParameterCannotBeNullMessage => Context.Config.MethodParameterCannotBeNullMessage;

        string NameOfSqlProcedureInsert => Context.Naming.NameOfSqlProcedureInsert;

        string DatabaseTargetSchemaForProcedureNames => Context.Naming.SchemaName;

        string DatabaseEnumName => Context.Naming.DatabaseEnumName;

        string DatabaseTableFullPath => Context.Naming.DatabaseTableFullPath;

        string NameOfCsMethodInsert => Context.Naming.NameOfDotNetMethodInsert;

        string ContractName => Context.Naming.ContractName;

        IReadOnlyList<ColumnInfo> Columns => Context.Table.Columns;
        #endregion
    }
}