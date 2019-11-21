using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using BOA.CodeGeneration.Common;
using BOA.CodeGeneration.Model;
using BOA.CodeGeneration.Util;
using BOA.EntityGeneration.DbModel;
using BOA.EntityGeneration.DbModel.Interfaces;

namespace BOA.CodeGeneration.Generators
{
    class SelectByColumnsCs : WriterBase
    {
        #region Constructors
        #region Constructor
        public SelectByColumnsCs(WriterContext context, CustomSelectMethod customSelect)
            : base(context)
        {
            CustomSelect = customSelect;
        }
        #endregion
        #endregion

        #region Properties
        internal CustomSelectMethod CustomSelect { get; }

        internal virtual string ForcedComment => new SelectByColumnsSql(Context, CustomSelect).GetDefaultComment();

        internal virtual string GenericResponseMethodReturnType
        {
            get
            {
                if (SelectOnlySpecificColumn != null)
                {
                    return "IReadOnlyList<" + SelectOnlySpecificColumn.DotNetType + ">";
                }

                var contractName = ContractName;

                if (CustomSelect.MustBeReturnReadonlyContract)
                {
                    contractName = IReadonly + contractName;
                }

                if (CustomSelect.MustBeReturnFirstContract)
                {
                    return contractName;
                }

                return "List<{0}>".FormatCode(contractName);
            }
        }

        protected virtual string ExecutionMethod => "ExecuteReader";

        string Comment => CustomSelect.Comment;

        string ListHolderType
        {
            get
            {
                var contractName = ContractName;

                if (CustomSelect.MustBeReturnReadonlyContract)
                {
                    contractName = IReadonly + contractName;
                }

                if (SelectOnlySpecificColumn != null)
                {
                    return "List<" + SelectOnlySpecificColumn.DotNetType + ">";
                }

                return "List<{0}>".FormatCode(contractName);
            }
        }

        IColumnInfo SelectOnlySpecificColumn
        {
            get
            {
                if (CustomSelect.SelectOnlySpecificColumn == null)
                {
                    return null;
                }

                return Columns.First(c => c.ColumnName == CustomSelect.SelectOnlySpecificColumn);
            }
        }
        #endregion

        #region Public Methods
        public string Generate()
        {
            Padding = PaddingForMethodDeclaration;
            WriteLine("/// <summary>");

            WriteLine("///" + PaddingForComment + "<para> {0} </para>", ForcedComment);

            if (Comment != null)
            {
                foreach (var commentLine in from line in Comment.Split(Environment.NewLine.ToCharArray())
                                            where !string.IsNullOrWhiteSpace(line)
                                            select line)
                {
                    WriteLine("///" + PaddingForComment + "<para> " + commentLine + " </para>");
                }
            }

            if (Parameters.Any())
            {
                foreach (var p in Parameters)
                {
                    var column       = p.FindColumn(Columns);
                    var propertyName = p.GetPropertyName(Columns);

                    if (propertyName == null)
                    {
                        WriteLine("///" + PaddingForComment + "<param name=\"{0}\">WHERE {1} = {0}</param>",
                                  column.ColumnName.AsMethodParameter2(), column.ColumnName);
                    }
                    else if (propertyName == Names2.NotEqual)
                    {
                        WriteLine("///" + PaddingForComment + "<param name=\"{0}\">WHERE {1} != {0} </param>",
                                  column.ColumnName.AsMethodParameter2() + propertyName, column.ColumnName);
                    }
                    else if (propertyName == Names2.BiggerThan)
                    {
                        WriteLine("///" + PaddingForComment + "<param name=\"{0}\">WHERE {1} &gt; {0}</param>",
                                  column.ColumnName.AsMethodParameter2() + propertyName, column.ColumnName);
                    }
                    else if (propertyName == Names2.BiggerThanOrEquals)
                    {
                        WriteLine("///" + PaddingForComment + "<param name=\"{0}\">WHERE {1} &gt;= {0}</param>",
                                  column.ColumnName.AsMethodParameter2() + propertyName, column.ColumnName);
                    }
                    else if (propertyName == Names2.LessThan)
                    {
                        WriteLine("///" + PaddingForComment + "<param name=\"{0}\">WHERE {1} &lt; {0}</param>",
                                  column.ColumnName.AsMethodParameter2() + propertyName, column.ColumnName);
                    }
                    else if (propertyName == Names2.LessThanOrEquals)
                    {
                        WriteLine("///" + PaddingForComment + "<param name=\"{0}\">WHERE {1} &lt;= {0}</param>",
                                  column.ColumnName.AsMethodParameter2() + propertyName, column.ColumnName);
                    }
                    else if (propertyName == Names2.StartsWith)
                    {
                        WriteLine("///" + PaddingForComment + "<param name=\"{0}\">WHERE {1} LIKE {0} + '%'</param>",
                                  column.ColumnName.AsMethodParameter2() + propertyName, column.ColumnName);
                    }
                    else if (propertyName == Names2.EndsWith)
                    {
                        WriteLine("///" + PaddingForComment + "<param name=\"{0}\">WHERE {1} LIKE '%' + {0}</param>",
                                  column.ColumnName.AsMethodParameter2() + propertyName, column.ColumnName);
                    }
                    else if (propertyName == Names2.Contains)
                    {
                        WriteLine("///" + PaddingForComment + "<param name=\"{0}\">WHERE {1} LIKE '%' + {0} + '%'</param>",
                                  column.ColumnName.AsMethodParameter2() + propertyName, column.ColumnName);
                    }
                    else if (propertyName == Names2.IsNull)
                    {
                        WriteLine("///" + PaddingForComment + "<param name=\"{0}\">WHERE(( {0} = true AND {1} IS NULL ) OR ( {0} = false AND {1} IS NOT NULL )) </param>",
                                  column.ColumnName.AsMethodParameter2() + propertyName, column.ColumnName);
                    }
                    else
                    {
                        throw new Exception(propertyName);
                    }
                }
            }

            WriteLine("/// </summary>");
            WritePadding();

            var methodAccess = "public ";
            if (CustomSelect.MemberAccessibility == MemberAccessibility.Private)
            {
                methodAccess = "";
            }

            if (CustomSelect.MemberAccessibility == MemberAccessibility.Internal)
            {
                methodAccess = "internal ";
            }

            Write(methodAccess);
            Write("GenericResponse<{1}> {0}(", NameOfCsMethod, GenericResponseMethodReturnType);

            if (Parameters.Any())
            {
                var last = Parameters.Last();
                foreach (var p in Parameters)
                {
                    var column             = p.FindColumn(Columns);
                    var propertyName       = p.GetPropertyName(Columns);
                    var propertyNameIsNull = propertyName == Names2.IsNull;

                    var parameterName = column.ColumnName.AsMethodParameter2() + propertyName;
                    var parameterType = column.DotNetType;

                    if (p.CanBeNull)
                    {
                        parameterType = DotNetTypeName.GetDotNetNullableType(parameterType);
                    }

                    if (propertyNameIsNull)
                    {
                        parameterType = "bool";
                    }

                    Write("{0} {1}", parameterType, parameterName);

                    if (p != last)
                    {
                        Write(",");
                    }
                }
            }

            Write(")");
            WriteLine();

            WriteLine("{");

            Padding++;

            WriteLine("var returnObject = CreateResponse<{0}>();", GenericResponseMethodReturnType);

            WriteLine();

            WriteLine("var command = DBLayer.GetDBCommand(Databases.{0}, \"{1}\");",
                      Context.Naming.DatabaseEnumName, Context.Naming.SchemaName + "." + NameOfSqlProcedure);

            WriteLine();

            if (Parameters.Any())
            {
                WriteLine("#region Parameters");

                foreach (var p in Parameters)
                {
                    var column       = p.FindColumn(Columns);
                    var propertyName = p.GetPropertyName(Columns);

                    var parameterName = column.ColumnName.AsMethodParameter2() + propertyName;

                    var sqlDatabaseTypeName = column.SqlDbType;
                    if (p.GetPropertyName(Columns) == Names2.IsNull)
                    {
                        sqlDatabaseTypeName = SqlDbType.TinyInt;
                        parameterName       = "(" + parameterName + " ? 1 : 0)";
                    }

                    WriteLine("DBLayer.AddInParameter(command, \"{0}\", SqlDbType.{1}, {2});",
                              column.ColumnName + propertyName, sqlDatabaseTypeName.ToString(), parameterName);
                }

                WriteLine("#endregion");
                WriteLine();
            }

            WriteLine("var sp = DBLayer.{0}(command);", ExecutionMethod);
            WriteLine("if (!sp.Success)");
            WriteLine("{");
            Padding++;
            WriteLine("return returnObject.Add(sp);");
            Padding--;
            WriteLine("}");

            ProcessReturnValues();

            WriteLine("return returnObject;");
            Padding--;
            WriteLine("}");

            return GeneratedString;
        }
        #endregion

        #region Methods
        protected virtual void ProcessReturnValues()
        {
            WriteLine("var reader = sp.Value;");
            WriteLine();

            WriteLine("#region Fill from SqlDataReader to " + ListHolderType);
            WriteLine("var listOfDataContract = new {0}();", ListHolderType);
            WriteLine();
            WriteLine("while (reader.Read())");
            WriteLine("{");
            Padding++;

            if (SelectOnlySpecificColumn != null)
            {
                WriteLine("var value" + " = SQLDBHelper." + SelectOnlySpecificColumn.SqlReaderMethod + "(reader[" + "\"" + SelectOnlySpecificColumn.ColumnName + "\"]);");
                WriteLine("listOfDataContract.Add(value);");
            }
            else
            {
                WriteLine("var dataContract = new {0}();", ContractName);

                WriteLine();

                WriteLine("ReadContract(dataContract,reader);");

                WriteLine();
                WriteLine("listOfDataContract.Add(dataContract);");
            }

            Padding--;
            WriteLine("}");
            WriteLine("reader.Close();");
            WriteLine("#endregion");

            WriteLine();

            if (CustomSelect.MustBeReturnFirstContract)
            {
                WriteLine("returnObject.Value = listOfDataContract.FirstOrDefault();");
            }
            else
            {
                WriteLine("returnObject.Value = listOfDataContract;");
            }

            WriteLine();
        }
        #endregion

        #region Parameters
        string NameOfSqlProcedure => CustomSelect.SqlProcedureName;

        string NameOfCsMethod => CustomSelect.DotNetMethodName;

        string ContractName => Context.Naming.ContractName;

        IReadOnlyList<IColumnInfo> Columns => Context.Table.Columns;

        IReadOnlyList<Where> Parameters => CustomSelect.Parameters;

        string IReadonly
        {
            get
            {
                if (Context.Config.TableName == "CardAccount" ||
                    Context.Config.TableName == "DebitTransaction")
                {
                    return "IReadOnly";
                }

                // fixme: bunu da O  büyük olmalı.
                return "IReadonly";
            }
        }
        #endregion
    }
}