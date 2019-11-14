using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using BOA.Common.Helpers;
using BOA.DatabaseAccess;
using BOA.EntityGeneration.BOACardCustomSqlIntoProjectInjection.Models.Impl;
using BOA.EntityGeneration.BOACardCustomSqlIntoProjectInjection.Models.Interfaces;
using BOA.EntityGeneration.BOACardDatabaseSchemaToDllExporting.DataAccess;
using BOA.EntityGeneration.DbModel;
using BOA.EntityGeneration.DbModel.SqlServerDataAccess;

namespace BOA.EntityGeneration.BOACardCustomSqlIntoProjectInjection.DataAccess
{
    /// <summary>
    ///     The project custom SQL information data access
    /// </summary>
    public class ProjectCustomSqlInfoDataAccess
    {
        #region Public Methods
        /// <summary>
        ///     Gets the by profile identifier.
        /// </summary>
        public static ProjectCustomSqlInfo GetByProfileId(IDatabase database, ConfigContract config, string profileId)
        {
            var project = GetByProfileIdFromDatabase(database, profileId);

            foreach (var customSqlInfo in project.CustomSqlInfoList)
            {
                Fill(customSqlInfo, config, database);
            }

            for (var i = 0; i < project.CustomSqlInfoList.Count; i++)
            {
                project.CustomSqlInfoList[i].SwitchCaseIndex = i;
            }

            return project;
        }
        #endregion

        #region Methods
        /// <summary>
        ///     Fills the specified custom SQL information.
        /// </summary>
        static void Fill(CustomSqlInfo customSqlInfo, ConfigContract config, IDatabase database)
        {
            var tableInfoDao = new TableInfoDao {Database = database, IndexInfoAccess = new IndexInfoAccess {Database = database}};

            if (customSqlInfo.ResultColumns.Any(x => x.DataType.Equals("object", StringComparison.OrdinalIgnoreCase)))
            {
                var customSqlInfoResults = new List<CustomSqlInfoResult>();

                foreach (var item in customSqlInfo.ResultColumns)
                {
                    if (item.DataType.Equals("object", StringComparison.OrdinalIgnoreCase))
                    {
                        var tableInfo = GeneratorDataCreator.Create(config, database, tableInfoDao.GetInfo(TableCatalogName.BOACard, customSqlInfo.SchemaName, item.Name));

                        customSqlInfoResults.AddRange(from columnInfo in tableInfo.Columns
                                                      select new CustomSqlInfoResult
                                                      {
                                                          Name             = columnInfo.ColumnName,
                                                          DataType         = columnInfo.DataType,
                                                          IsNullable       = columnInfo.IsNullable,
                                                          DataTypeInDotnet = columnInfo.DotNetType,
                                                          NameInDotnet     = columnInfo.ColumnName.ToContractName(),
                                                          SqlReaderMethod  = columnInfo.SqlReaderMethod
                                                      });

                        continue;
                    }

                    Fill(item);
                    customSqlInfoResults.Add(item);
                }

                customSqlInfo.ResultColumns = customSqlInfoResults;

                return;
            }

            foreach (var item in customSqlInfo.ResultColumns)
            {
                Fill(item);
            }
        }

        /// <summary>
        ///     Fills the specified item.
        /// </summary>
        static void Fill(CustomSqlInfoResult item)
        {
            item.NameInDotnet = item.Name.ToContractName();

            item.DataTypeInDotnet = GetDataTypeInDotnet(item.DataType, item.IsNullable);

            if (item.Name.EndsWith("_FLAG", StringComparison.OrdinalIgnoreCase))
            {
                var sqlDataTypeIsChar = item.DataType.EndsWith("char", StringComparison.OrdinalIgnoreCase);
                if (!sqlDataTypeIsChar)
                {
                    throw new InvalidOperationException($"{item.Name} column should be char.");
                }

                item.DataTypeInDotnet = DotNetTypeName.DotNetBool;
                item.SqlReaderMethod  = SqlReaderMethods.GetBooleanValue;
                if (item.IsNullable)
                {
                    item.DataTypeInDotnet = DotNetTypeName.GetDotNetNullableType(DotNetTypeName.DotNetBool);
                    item.SqlReaderMethod  = SqlReaderMethods.GetBooleanNullableValue2;
                }
            }
            else
            {
                item.SqlReaderMethod = SqlDbTypeMap.GetSqlReaderMethod(item.DataType.ToUpperEN(), item.IsNullable);
            }
        }

        /// <summary>
        ///     Gets the by profile identifier from database.
        /// </summary>
        static ProjectCustomSqlInfo GetByProfileIdFromDatabase(IDatabase Database, string profileId)
        {
            var list = new List<CustomSqlInfo>();

            Database.CommandText = $"SELECT objectid, text, schemaname, resultcollectionflag FROM dbo.objects WITH (NOLOCK) WHERE profileid = '{profileId}' AND objecttype = 'CUSTOMSQL'";

            var reader = Database.ExecuteReader();
            while (reader.Read())
            {
                list.Add(new CustomSqlInfo
                {
                    Name                  = reader["objectid"].ToString(),
                    Sql                   = reader["text"] + string.Empty,
                    SchemaName            = reader["schemaname"] + string.Empty,
                    SqlResultIsCollection = reader["resultcollectionflag"] + string.Empty == "1",
                    ProfileId             = profileId
                });
            }

            reader.Close();

            foreach (var customSqlInfo in list)
            {
                customSqlInfo.Parameters = ReadInputParameters(customSqlInfo, Database);
            }

            foreach (var customSqlInfo in list)
            {
                customSqlInfo.ResultColumns = ReadResultColumns(customSqlInfo, Database);
            }

            ProjectCustomSqlInfo returnValue = null;

            Database.CommandText = $@"SELECT typesnamespace,typesprojectpath, businessnamespace,businessprojectpath FROM dbo.profileskt WITH(NOLOCK) WHERE profileid = '{profileId}'";

            reader = Database.ExecuteReader();
            while (reader.Read())
            {
                returnValue = new ProjectCustomSqlInfo
                {
                    ProfileId               = profileId,
                    CustomSqlInfoList       = list,
                    TypesProjectPath        = reader["typesprojectpath"].ToString(),
                    BusinessProjectPath     = reader["businessprojectpath"].ToString(),
                    NamespaceNameOfType     = reader["typesnamespace"].ToString(),
                    NamespaceNameOfBusiness = reader["businessnamespace"].ToString()
                };
                break;
            }

            reader.Close();

            return returnValue;
        }

        /// <summary>
        ///     Gets the data type in dotnet.
        /// </summary>
        static string GetDataTypeInDotnet(string dataType, bool isNullable)
        {
            if (dataType.Equals("string", StringComparison.OrdinalIgnoreCase))
            {
                return "string";
            }

            if (dataType.Equals("varchar", StringComparison.OrdinalIgnoreCase))
            {
                return "string";
            }

            var suffix = isNullable ? "?" : string.Empty;

            if (dataType.Equals("bigint", StringComparison.OrdinalIgnoreCase))
            {
                return "long" + suffix;
            }

            if (dataType.Equals("numeric", StringComparison.OrdinalIgnoreCase))
            {
                return "decimal" + suffix;
            }

            if (dataType.Equals("datetime", StringComparison.OrdinalIgnoreCase))
            {
                return "DateTime" + suffix;
            }

            if (dataType.Equals("Int16", StringComparison.OrdinalIgnoreCase))
            {
                return "short" + suffix;
            }

            if (dataType.Equals("smallint", StringComparison.OrdinalIgnoreCase))
            {
                return "short" + suffix;
            }

            if (dataType.Equals("int", StringComparison.OrdinalIgnoreCase))
            {
                return "int" + suffix;
            }

            if (dataType.Equals("date", StringComparison.OrdinalIgnoreCase))
            {
                return "DateTime" + suffix;
            }

            if (dataType.Equals("bit", StringComparison.OrdinalIgnoreCase))
            {
                return "bool" + suffix;
            }

            if (dataType.Equals("long", StringComparison.OrdinalIgnoreCase))
            {
                return "long" + suffix;
            }

            if (dataType.Equals("char", StringComparison.OrdinalIgnoreCase))
            {
                return "string";
            }

            if (dataType.Equals("object", StringComparison.OrdinalIgnoreCase))
            {
                return "object";
            }

            if (dataType.Equals("decimal", StringComparison.OrdinalIgnoreCase))
            {
                return "decimal" + suffix;
            }

            if (SqlDbType.TinyInt.ToString().Equals(dataType, StringComparison.OrdinalIgnoreCase))
            {
                return DotNetTypeName.DotNetByte + suffix;
            }

            throw new NotImplementedException(dataType);
        }

        /// <summary>
        ///     Gets the name of the SQL database type.
        /// </summary>
        static SqlDbType GetSqlDbTypeName(string dataType)
        {
            if (dataType.Equals("string", StringComparison.OrdinalIgnoreCase))
            {
                return SqlDbType.VarChar;
            }

            if (dataType.Equals("varchar", StringComparison.OrdinalIgnoreCase))
            {
                return SqlDbType.VarChar;
            }

            if (dataType.Equals("bigint", StringComparison.OrdinalIgnoreCase))
            {
                return SqlDbType.BigInt;
            }

            if (dataType.Equals("numeric", StringComparison.OrdinalIgnoreCase) ||
                dataType.Equals("decimal", StringComparison.OrdinalIgnoreCase))
            {
                return SqlDbType.Decimal;
            }

            if (dataType.Equals("Int16", StringComparison.OrdinalIgnoreCase))
            {
                return SqlDbType.SmallInt;
            }

            if (dataType.Equals("smallint", StringComparison.OrdinalIgnoreCase))
            {
                return SqlDbType.SmallInt;
            }

            if (dataType.Equals("date", StringComparison.OrdinalIgnoreCase) ||
                dataType.Equals("datetime", StringComparison.OrdinalIgnoreCase))
            {
                return SqlDbType.DateTime;
            }

            if (dataType.Equals("bit", StringComparison.OrdinalIgnoreCase))
            {
                return SqlDbType.Bit;
            }

            if (dataType.Equals("int", StringComparison.OrdinalIgnoreCase))
            {
                return SqlDbType.Int;
            }

            if (dataType.Equals("long", StringComparison.OrdinalIgnoreCase))
            {
                return SqlDbType.BigInt;
            }

            if (dataType.Equals("char", StringComparison.OrdinalIgnoreCase))
            {
                return SqlDbType.Char;
            }

            throw new NotImplementedException(dataType);
        }

        /// <summary>
        ///     Reads the input parameters.
        /// </summary>
        static IReadOnlyList<ICustomSqlInfoParameter> ReadInputParameters(CustomSqlInfo customSqlInfo, IDatabase database)
        {
            var items = new List<ICustomSqlInfoParameter>();

            database.CommandText = $"select parameterid,datatype,nullableflag from dbo.objectparameters WITH (NOLOCK) WHERE profileid = '{customSqlInfo.ProfileId}' AND objectid = '{customSqlInfo.Name}'";

            var reader = database.ExecuteReader();
            while (reader.Read())
            {
                var name       = reader["parameterid"].ToString();
                var dataType   = reader["datatype"].ToString();
                var isNullable = reader["nullableflag"] + string.Empty == "1";

                var cSharpPropertyTypeName = GetDataTypeInDotnet(dataType, isNullable);

                var cSharpPropertyName = name.ToContractName();

                var valueAccessPathForAddInParameter = cSharpPropertyName;

                var sqlDbTypeName = GetSqlDbTypeName(dataType);

                var isChar = sqlDbTypeName == SqlDbType.Char;

                var endsWithFlagSuffix = name.EndsWith("_FLAG", StringComparison.OrdinalIgnoreCase);
                if (endsWithFlagSuffix && isChar)
                {
                    if (isNullable)
                    {
                        cSharpPropertyTypeName = DotNetTypeName.GetDotNetNullableType(DotNetTypeName.DotNetBool);
                    }
                    else
                    {
                        cSharpPropertyTypeName = DotNetTypeName.DotNetBool;
                    }

                    valueAccessPathForAddInParameter = valueAccessPathForAddInParameter + " ? \"1\" : \"0\"";
                }

                var item = new CustomSqlInfoParameter
                {
                    Name                             = name,
                    IsNullable                       = isNullable,
                    CSharpPropertyName               = cSharpPropertyName,
                    CSharpPropertyTypeName           = cSharpPropertyTypeName,
                    SqlDbTypeName                    = sqlDbTypeName,
                    ValueAccessPathForAddInParameter = valueAccessPathForAddInParameter
                };

                items.Add(item);
            }

            reader.Close();

            return items;
        }

        /// <summary>
        ///     Reads the result columns.
        /// </summary>
        static IReadOnlyList<CustomSqlInfoResult> ReadResultColumns(CustomSqlInfo customSqlInfo, IDatabase database)
        {
            var items = new List<CustomSqlInfoResult>();

            database.CommandText = $"select resultid,datatype, nullableflag from dbo.objectresults WITH (NOLOCK) WHERE profileid = '{customSqlInfo.ProfileId}' AND objectid = '{customSqlInfo.Name}'";

            var reader = database.ExecuteReader();

            while (reader.Read())
            {
                var item = new CustomSqlInfoResult
                {
                    Name       = reader["resultid"].ToString(),
                    DataType   = reader["datatype"].ToString(),
                    IsNullable = reader["nullableflag"] + string.Empty == "1"
                };

                items.Add(item);
            }

            reader.Close();

            return items;
        }

        /// <summary>
        ///     Gets the by profile identifier list.
        /// </summary>
        IReadOnlyList<string> GetByProfileIdList(IDatabase database)
        {
            var items = new List<string>();

            database.CommandText = "SELECT profileid  from BOACard.dbo.profileskt WITH (NOLOCK)";
            var reader = database.ExecuteReader();
            while (reader.Read())

            {
                items.Add(reader["profileid"] + string.Empty);
            }

            reader.Close();

            return items;
        }
        #endregion

        static class TableCatalogName
        {
            #region Constants
            public const string BOACard = nameof(BOACard);
            #endregion
        }
    }
}