using System;
using System.Collections.Generic;
using BOA.Common.Helpers;
using BOA.DatabaseAccess;
using Ninject;

namespace BOA.EntityGeneration.BOACardCustomSqlIntoProjectInjection
{
    public class DataAccess
    {
        #region Public Properties
        [Inject]
        public IDatabase Database { get; set; }

        [Inject]
        public SqlDbTypeMap SqlDbTypeMap { get; set; }
        #endregion

        #region Public Methods
        public ProjectCustomSqlInfo GetByProfileId(string profileId)
        {
            var list = new List<CustomSqlInfo>();

            Database.CommandText = $"SELECT objectid, text  FROM dbo.objects WITH (NOLOCK) WHERE profileid = '{profileId}' AND objecttype = 'CUSTOMSQL'";
            var reader = Database.ExecuteReader();
            while (reader.Read())
            {
                list.Add(new CustomSqlInfo
                {
                    Name = reader["objectid"].ToString(),
                    Sql = reader["text"]+string.Empty
                });
            }

            reader.Close();

            foreach (var customSqlInfo in list)
            {
                var items = new List<CustomSqlInfoParameter>();

                Database.CommandText = $"select parameterid,datatype,nullableflag from dbo.objectparameters WITH (NOLOCK) WHERE profileid = '{profileId}' AND objectid = '{customSqlInfo.Name}'";
                reader               = Database.ExecuteReader();
                while (reader.Read())
                {
                    var item = new CustomSqlInfoParameter
                    {
                        Name     = reader["parameterid"].ToString(),
                        DataType = reader["datatype"].ToString(),
                        IsNullable = reader["nullableflag"] + string.Empty == "1"
                    };
                    item.NameInDotnet        = item.Name.ToContractName();
                    item.DataTypeInDotnet    = GetDataTypeInDotnet(item.DataType,item.IsNullable);
                    item.SqlDatabaseTypeName = GetSqlDbTypeName(item.DataType);

                    items.Add(item);
                }

                reader.Close();

                customSqlInfo.Parameters = items;
            }

            foreach (var customSqlInfo in list)
            {
                var items = new List<CustomSqlInfoResult>();

                Database.CommandText = $"select resultid,datatype, nullableflag from dbo.objectresults WITH (NOLOCK) WHERE profileid = '{profileId}' AND objectid = '{customSqlInfo.Name}'";
                reader               = Database.ExecuteReader();
                while (reader.Read())
                {
                    var item = new CustomSqlInfoResult
                    {
                        Name       = reader["resultid"].ToString(),
                        DataType   = reader["datatype"].ToString(),
                        IsNullable = reader["nullableflag"] + string.Empty == "1"
                    };

                    item.NameInDotnet = item.Name.ToContractName();

                    if (item.DataType.Equals("object", StringComparison.OrdinalIgnoreCase))
                    {
                        continue;
                    }

                    item.DataTypeInDotnet = GetDataTypeInDotnet(item.DataType, item.IsNullable);
                    if (item.Name.EndsWith("_flag", StringComparison.OrdinalIgnoreCase))
                    {
                        item.DataTypeInDotnet = item.IsNullable ? "bool?" : "bool";
                    }

                    if (item.DataType.Equals("char", StringComparison.OrdinalIgnoreCase))
                    {
                        item.SqlReaderMethod = item.IsNullable ? SqlReaderMethods.GetBooleanNullableValue : SqlReaderMethods.GetBooleanValue;
                    }
                    else
                    {
                        item.SqlReaderMethod = SqlDbTypeMap.GetSqlReaderMethod(item.DataType.ToUpperEN(), item.IsNullable);
                    }

                    items.Add(item);
                }

                reader.Close();

                customSqlInfo.ResultColumns = items;
            }

            var returnValue = new ProjectCustomSqlInfo
            {
                CustomSqlInfoList = list
            };
            Database.CommandText = $@"SELECT typesnamespace,typesprojectpath, businessnamespace,businessprojectpath FROM dbo.profileskt WITH(NOLOCK) WHERE profileid = '{profileId}'";

            reader = Database.ExecuteReader();
            while (reader.Read())
            {
                returnValue.TypesProjectPath        = reader["typesprojectpath"].ToString();
                returnValue.BusinessProjectPath     = reader["businessprojectpath"].ToString();
                returnValue.NamespaceNameOfType     = reader["typesnamespace"].ToString();
                returnValue.NamespaceNameOfBusiness = reader["businessnamespace"].ToString();
                break;
            }

            reader.Close();

            return returnValue;
        }
        #endregion

        #region Methods
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

            var suffix = (isNullable ? "?" : string.Empty);

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
                return "char" + suffix;
            }

            if (dataType.Equals("object", StringComparison.OrdinalIgnoreCase))
            {
                return "object";
            }

            if (dataType.Equals("decimal", StringComparison.OrdinalIgnoreCase))
            {
                return "decimal" + suffix;
            }

            throw new NotImplementedException(dataType);
        }

        static string GetSqlDbTypeName(string dataType)
        {
            if (dataType.Equals("string", StringComparison.OrdinalIgnoreCase))
            {
                return "Varchar";
            }

            if (dataType.Equals("varchar", StringComparison.OrdinalIgnoreCase))
            {
                return "Varchar";
            }

            if (dataType.Equals("bigint", StringComparison.OrdinalIgnoreCase))
            {
                return "BigInt";
            }

            if (dataType.Equals("numeric", StringComparison.OrdinalIgnoreCase))
            {
                return "Decimal";
            }

            if (dataType.Equals("datetime", StringComparison.OrdinalIgnoreCase))
            {
                return "Date";
            }

            if (dataType.Equals("Int16", StringComparison.OrdinalIgnoreCase))
            {
                return "SmallInt";
            }

            if (dataType.Equals("smallint", StringComparison.OrdinalIgnoreCase))
            {
                return "SmallInt";
            }

            if (dataType.Equals("date", StringComparison.OrdinalIgnoreCase))
            {
                return "Date";
            }

            if (dataType.Equals("bit", StringComparison.OrdinalIgnoreCase))
            {
                return "Bit";
            }

            if (dataType.Equals("int", StringComparison.OrdinalIgnoreCase))
            {
                return "Int";
            }

            if (dataType.Equals("long", StringComparison.OrdinalIgnoreCase))
            {
                return "BigInt";
            }

            if (dataType.Equals("char", StringComparison.OrdinalIgnoreCase))
            {
                return "Char";
            }

            throw new NotImplementedException(dataType);
        }
        #endregion
    }
}