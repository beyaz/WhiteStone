using System;
using System.Linq;
using BOA.Common.Helpers;
using BOA.EntityGeneration.BOACardDatabaseSchemaToDllExporting.ClassWriters;
using BOA.EntityGeneration.BOACardDatabaseSchemaToDllExporting.DataAccess;
using BOA.EntityGeneration.BOACardDatabaseSchemaToDllExporting.Util;
using Ninject;

namespace BOA.EntityGeneration.BOACardDatabaseSchemaToDllExporting.AllInOne
{
    public class AllBusinessClassesInOne
    {
        #region Public Properties
        [Inject]
        public SchemaExporterDataPreparer DataPreparer { get; set; }

        [Inject]
        public GeneratorOfBusinessClass GeneratorOfBusinessClass { get; set; }

        [Inject]
        public NamingHelper NamingHelper { get; set; }
        #endregion

        #region Public Methods
        public string GetCode(string schemaName)
        {
            var sb = new PaddedStringBuilder();

            Write(sb, schemaName);

            return sb.ToString();
        }
        #endregion

        #region Methods
        void Write(PaddedStringBuilder sb, string schemaName)
        {

            var items = DataPreparer.Prepare(schemaName);

            if (items.Count == 0)
            {
                throw new NotImplementedException(schemaName);
            }

            GeneratorOfBusinessClass.WriteUsingList(sb, items.First());
            sb.AppendLine();
            sb.AppendLine($"namespace {NamingHelper.GetBusinessClassNamespace(schemaName)}");
            sb.AppendLine("{");
            sb.PaddingCount++;

            UtilClass(sb);

            foreach (var tableInfo in items)
            {
                Tracer.Trace($"Generating Business class for {tableInfo.TableName}");
                sb.AppendLine();
                GeneratorOfBusinessClass.WriteClass(sb, tableInfo);
            }

            sb.PaddingCount--;
            sb.AppendLine("}"); // end of namespace    
            
        }


        
        

        
        

        
        

        static void UtilClass(PaddedStringBuilder sb)
        {
            sb.AppendAll(@"

/// <summary>
///     Utility methods
/// </summary>
static class Util
{
    /// <summary>
    ///     AND
    /// </summary>
    public const string AndSymbol = ""            AND "";

    /// <summary>
    ///     WHERE
    /// </summary>
    public const string WhereSymbol = ""            WHERE "";

    /// <summary>
    ///     Cannots the be empty.
    /// </summary>
    public static string CannotBeEmpty(string valueName)
    {
        const string format = ""'{0}' boş olmamalıdır."";

        return string.Format(format, valueName);
    }

    /// <summary>
    ///     Cannots the be null.
    /// </summary>
    public static string CannotBeNull(string valueName)
    {
        const string format = ""'{0}' is null."";

        return string.Format(format, valueName);
    }

    /// <summary>
    ///     Processes the condition.
    /// </summary>
    public static void ProcessCondition(WhereCondition whereCondition, List<string> whereLines, DbColumnInfo dbColumn, List<DbParameterInfo> parameters)
    {
        const string prefix = ""@"";

        switch (whereCondition.ConditionType)
        {
            case ConditionType.Equal:
            {
                const string operand = "" = "";

                whereLines.Add(dbColumn.Name + operand + prefix + dbColumn.Name);

                parameters.Add(new DbParameterInfo
                {
                    Name      = dbColumn.Name,
                    SqlDbType = dbColumn.SqlDbType,
                    Value     = whereCondition.Value
                });

                return;
            }

            case ConditionType.NotEqual:
            {
                const string operand = "" != "";

                whereLines.Add(dbColumn.Name + operand + prefix + dbColumn.Name);

                parameters.Add(new DbParameterInfo
                {
                    Name      = dbColumn.Name,
                    SqlDbType = dbColumn.SqlDbType,
                    Value     = whereCondition.Value
                });

                return;
            }

            case ConditionType.GreaterThan:
            {
                const string operand = "" > "";

                whereLines.Add(dbColumn.Name + operand + prefix + dbColumn.Name);

                parameters.Add(new DbParameterInfo
                {
                    Name      = dbColumn.Name,
                    SqlDbType = dbColumn.SqlDbType,
                    Value     = whereCondition.Value
                });

                return;
            }

            case ConditionType.GreaterOrEqual:
            {
                const string operand = "" >= "";

                whereLines.Add(dbColumn.Name + operand + prefix + dbColumn.Name);

                parameters.Add(new DbParameterInfo
                {
                    Name      = dbColumn.Name,
                    SqlDbType = dbColumn.SqlDbType,
                    Value     = whereCondition.Value
                });

                return;
            }

            case ConditionType.LessThan:
            {
                const string operand = "" < "";

                whereLines.Add(dbColumn.Name + operand + prefix + dbColumn.Name);

                parameters.Add(new DbParameterInfo
                {
                    Name      = dbColumn.Name,
                    SqlDbType = dbColumn.SqlDbType,
                    Value     = whereCondition.Value
                });

                return;
            }

            case ConditionType.LessOrEqual:
            {
                const string operand = "" <= "";

                whereLines.Add(dbColumn.Name + operand + prefix + dbColumn.Name);

                parameters.Add(new DbParameterInfo
                {
                    Name      = dbColumn.Name,
                    SqlDbType = dbColumn.SqlDbType,
                    Value     = whereCondition.Value
                });

                return;
            }

            case ConditionType.IsNull:
            {
                const string operand = "" IS NULL "";

                whereLines.Add(dbColumn.Name + operand);

                parameters.Add(new DbParameterInfo
                {
                    Name      = dbColumn.Name,
                    SqlDbType = dbColumn.SqlDbType,
                    Value     = whereCondition.Value
                });

                return;
            }

            case ConditionType.IsNotNull:
            {
                const string operand = "" IS NOT NULL "";

                whereLines.Add(dbColumn.Name + operand);

                parameters.Add(new DbParameterInfo
                {
                    Name      = dbColumn.Name,
                    SqlDbType = dbColumn.SqlDbType,
                    Value     = whereCondition.Value
                });

                return;
            }

            case ConditionType.StartsWith:
            {
                const string operand = "" LIKE "";

                whereLines.Add(dbColumn.Name + operand + prefix + dbColumn.Name + "" + '%'"");

                parameters.Add(new DbParameterInfo
                {
                    Name      = dbColumn.Name,
                    SqlDbType = dbColumn.SqlDbType,
                    Value     = whereCondition.Value
                });

                return;
            }

            case ConditionType.EndsWith:
            {
                const string operand = "" LIKE '%' + "";

                whereLines.Add(dbColumn.Name + operand + prefix + dbColumn.Name);

                parameters.Add(new DbParameterInfo
                {
                    Name      = dbColumn.Name,
                    SqlDbType = dbColumn.SqlDbType,
                    Value     = whereCondition.Value
                });

                return;
            }

            case ConditionType.Contains:
            {
                const string operand = "" LIKE '%' + "";

                whereLines.Add(dbColumn.Name + operand + prefix + dbColumn.Name + "" + '%'"");

                parameters.Add(new DbParameterInfo
                {
                    Name      = dbColumn.Name,
                    SqlDbType = dbColumn.SqlDbType,
                    Value     = whereCondition.Value
                });

                return;
            }

            default:
            {
                throw new ArgumentOutOfRangeException();
            }
        }
    }

    /// <summary>
    ///     Read nullable flag
    /// </summary>
    public static bool? ReadNullableFlag(string flag)
    {
        if (flag == null)
        {
            return null;
        }

        return flag == ""1"";
    }
}


".Trim());
           sb.AppendLine();
        }


        [Inject]
        public Tracer Tracer { get; set; }
        #endregion
    }
}