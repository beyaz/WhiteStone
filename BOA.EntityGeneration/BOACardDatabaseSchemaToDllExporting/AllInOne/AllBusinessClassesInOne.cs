using System;
using System.Linq;
using BOA.Common.Helpers;
using BOA.EntityGeneration.BOACardDatabaseSchemaToDllExporting.ClassWriters;
using BOA.EntityGeneration.BOACardDatabaseSchemaToDllExporting.DataAccess;
using BOA.EntityGeneration.BOACardDatabaseSchemaToDllExporting.Models;
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

            Tracer.CurrentSchemaProcess.Total   = items.Count;
            Tracer.CurrentSchemaProcess.Current = 0;

            foreach (var tableInfo in items)
            {
                Tracer.CurrentSchemaProcess.Text = $"Generating business class: {tableInfo.TableName}";

                Tracer.CurrentSchemaProcess.Current++;


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
    #region Constants
    /// <summary>
    ///     AND
    /// </summary>
    public const string AndSymbol = ""            AND "";

    /// <summary>
    ///     WHERE
    /// </summary>
    public const string WhereSymbol = ""            WHERE "";

    /// <summary>
    ///     The comma
    /// </summary>
    const string Comma = ""."";
    #endregion

    #region Public Methods
    /// <summary>
    ///     Contract parameter cannot be null.
    /// </summary>
    public static GenericResponse<int> ContractCannotBeNull(this ObjectHelper objectHelper, [CallerMemberName] string memberName = null)
    {
        var returnObject = InitializeResponse<int>(objectHelper, memberName);

        const string errorMessage = ""'contract' parameter cannot be null."";

        returnObject.Results.Add(new Result {ErrorMessage = errorMessage});

        return returnObject;
    }

    /// <summary>
    ///     'columnNames' parameter cannot be null.
    /// </summary>
    public static GenericResponse<int> ColumnNamesCannotBeNullOrEmpty(this ObjectHelper objectHelper, [CallerMemberName] string memberName = null)
    {
        var returnObject = InitializeResponse<int>(objectHelper, memberName);

        const string errorMessage = ""'columnNames' parameter cannot be null or empty."";

        returnObject.Results.Add(new Result {ErrorMessage = errorMessage});

        return returnObject;
    }


    /// <summary>
    ///     Executes the command and returns effected row count.
    /// </summary>
    public static GenericResponse<int> ExecuteNonQuery(this ObjectHelper objectHelper, SqlCommand command, [CallerMemberName] string memberName = null)
    {
        var returnObject = InitializeResponse<int>(objectHelper, memberName);

        var response = objectHelper.DBLayer.ExecuteNonQuery(command);
        if (!response.Success)
        {
            returnObject.Results.AddRange(response.Results);
            return returnObject;
        }

        returnObject.Value = response.Value;

        return returnObject;
    }

    /// <summary>
    ///     Executes the reader.
    /// </summary>
    public static GenericResponse<List<TContract>> ExecuteReader<TContract>(this ObjectHelper objectHelper, SqlCommand command, Action<IDataReader, TContract> ReadContract, [CallerMemberName] string memberName = null) where TContract : new()
    {
        var returnObject = InitializeResponse<List<TContract>>(objectHelper, memberName);

        var response = objectHelper.DBLayer.ExecuteReader(command);
        if (!response.Success)
        {
            returnObject.Results.AddRange(response.Results);
            return returnObject;
        }

        var reader = response.Value;

        var listOfDataContract = new List<TContract>();

        while (reader.Read())
        {
            var dataContract = new TContract();

            ReadContract(reader, dataContract);

            listOfDataContract.Add(dataContract);
        }

        reader.Close();

        returnObject.Value = listOfDataContract;

        return returnObject;
    }

    /// <summary>
    ///     Executes the reader for only one record.
    /// </summary>
    public static GenericResponse<TContract> ExecuteReaderForOnlyOneRecord<TContract>(this ObjectHelper objectHelper, SqlCommand command, Action<IDataReader, TContract> ReadContract, [CallerMemberName] string memberName = null) where TContract : new()
    {
        var returnObject = InitializeResponse<TContract>(objectHelper, memberName);

        var response = objectHelper.DBLayer.ExecuteReader(command);
        if (!response.Success)
        {
            returnObject.Results.AddRange(response.Results);
            return returnObject;
        }

        var reader = response.Value;

        while (reader.Read())
        {
            var dataContract = new TContract();

            ReadContract(reader, dataContract);

            returnObject.Value = dataContract;

            break;
        }

        reader.Close();

        return returnObject;
    }

    /// <summary>
    ///     Executes the command and returns the scalar value.
    /// </summary>
    public static GenericResponse<int> ExecuteScalar(this ObjectHelper objectHelper, SqlCommand command, [CallerMemberName] string memberName = null)
    {
        var returnObject = InitializeResponse<int>(objectHelper, memberName);

        var response = objectHelper.DBLayer.ExecuteScalar<int>(command);
        if (!response.Success)
        {
            returnObject.Results.AddRange(response.Results);
            return returnObject;
        }

        returnObject.Value = response.Value;

        return returnObject;
    }

    /// <summary>
    ///     Initializes the response.
    /// </summary>
    public static ResponseBase InitializeResponse(this ObjectHelper objectHelper, [CallerMemberName] string memberName = null)
    {
        var key = objectHelper.GetType().FullName + Comma + memberName;

        return objectHelper.InitializeResponseBase(key);
    }

    /// <summary>
    ///     Initializes the response.
    /// </summary>
    public static GenericResponse<T> InitializeResponse<T>(this ObjectHelper objectHelper, [CallerMemberName] string memberName = null)
    {
        var key = objectHelper.GetType().FullName + Comma + memberName;

        return objectHelper.InitializeGenericResponse<T>(key);
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

    /// <summary>
    ///     Indicates sequence fetch error occured.
    /// </summary>
    public static GenericResponse<int> SequenceFetchError(this ObjectHelper objectHelper, ResponseBase response, [CallerMemberName] string memberName = null)
    {
        var returnObject = InitializeResponse<int>(objectHelper, memberName);

        returnObject.Results.AddRange(response.Results);

        return returnObject;
    }

    /// <summary>
    ///     Wheres the conditions cannot be null or empty.
    /// </summary>
    public static GenericResponse<List<TContract>> WhereConditionsCannotBeNullOrEmpty<TContract>(this ObjectHelper objectHelper)
    {
        var returnObject = objectHelper.InitializeResponse<List<TContract>>();

        const string errorMessage = ""'where conditions' parameter cannot be null or empty."";

        returnObject.Results.Add(new Result {ErrorMessage = errorMessage});

        return returnObject;
    }
    #endregion
}



".Trim());
           sb.AppendLine();
        }


        [Inject]
        public Tracer Tracer { get; set; }
        #endregion
    }
}