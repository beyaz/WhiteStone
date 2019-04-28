using System;
using System.Collections.Generic;
using BOA.Common.Helpers;
using BOA.DatabaseAccess;
using Ninject;

namespace BOA.EntityGeneration.BOACardCustomSqlIntoProjectInjection
{
    public class DataAccess
    {
        [Inject]
        public SqlDbTypeMap SqlDbTypeMap { get; set; }

        [Inject]
        public IDatabase Database { get; set; }

        static string GetDataTypeInDotnet(string dataType)
        {
            if (dataType.Equals("string",StringComparison.OrdinalIgnoreCase))
            {
                return  "string";
            }
            if (dataType.Equals("varchar",StringComparison.OrdinalIgnoreCase))
            {
                return  "string";
            }

            if (dataType.Equals("bigint",StringComparison.OrdinalIgnoreCase))
            {
                return "long?";
            }

            if (dataType.Equals("numeric",StringComparison.OrdinalIgnoreCase))
            {
                return  "decimal?";
            }

            if (dataType.Equals("datetime",StringComparison.OrdinalIgnoreCase))
            {
                return  "DateTime?";
            }

            if (dataType.Equals("Int16",StringComparison.OrdinalIgnoreCase))
            {
                return  "short?";
            }

            if (dataType.Equals("smallint",StringComparison.OrdinalIgnoreCase))
            {
                return  "short?";
            }

            if (dataType.Equals("int",StringComparison.OrdinalIgnoreCase))
            {
                return  "int?";
            }

            if (dataType.Equals("date",StringComparison.OrdinalIgnoreCase))
            {
                return  "DateTime?";
            }

            if (dataType.Equals("bit",StringComparison.OrdinalIgnoreCase))
            {
                return  "bool?";
            }
            
            if (dataType.Equals("long",StringComparison.OrdinalIgnoreCase))
            {
                return  "long?";
            }
            
            if (dataType.Equals("char",StringComparison.OrdinalIgnoreCase))
            {
                return  "char?";
            }

            if (dataType.Equals("object",StringComparison.OrdinalIgnoreCase))
            {
                return  "object";
            }
            
            
            if (dataType.Equals("decimal",StringComparison.OrdinalIgnoreCase))
            {
                return  "decimal?";
            }

            throw  new NotImplementedException(dataType);
        }

        static string GetSqlDbTypeName(string dataType)
        {

            if (dataType.Equals("string",StringComparison.OrdinalIgnoreCase))
            {
                return  "Varchar";
            }
            if (dataType.Equals("varchar",StringComparison.OrdinalIgnoreCase))
            {
                return  "Varchar";
            }

            if (dataType.Equals("bigint",StringComparison.OrdinalIgnoreCase))
            {
                return "BigInt";
            }

            if (dataType.Equals("numeric",StringComparison.OrdinalIgnoreCase))
            {
                return  "Decimal";
            }

            if (dataType.Equals("datetime",StringComparison.OrdinalIgnoreCase))
            {
                return  "Date";
            }

            if (dataType.Equals("Int16",StringComparison.OrdinalIgnoreCase))
            {
                return  "SmallInt";
            }

            if (dataType.Equals("smallint",StringComparison.OrdinalIgnoreCase))
            {
                return  "SmallInt";
            }

            if (dataType.Equals("date",StringComparison.OrdinalIgnoreCase))
            {
                return  "Date";
            }

            if (dataType.Equals("bit",StringComparison.OrdinalIgnoreCase))
            {
                return  "Bit";
            }

            if (dataType.Equals("int",StringComparison.OrdinalIgnoreCase))
            {
                return  "Int";
            }

            if (dataType.Equals("long",StringComparison.OrdinalIgnoreCase))
            {
                return  "BigInt";
            }

            if (dataType.Equals("char",StringComparison.OrdinalIgnoreCase))
            {
                return  "Char";
            }

            throw new NotImplementedException(dataType);
        }

        public ProjectCustomSqlInfo GetByProfileId(string profileId)
        {

            var list = new List<CustomSqlInfo>();


            Database.CommandText = $"SELECT objectid  FROM dbo.objects WITH (NOLOCK) WHERE profileid = '{profileId}' AND objecttype = 'CUSTOMSQL'";
            var reader = Database.ExecuteReader();
            while (reader.Read())
            {
                list.Add(new CustomSqlInfo
                {
                    Name = reader["objectid"].ToString()
                });
            }
            reader.Close();





            foreach (var customSqlInfo in list)
            {
                var items = new List<CustomSqlInfoParameter>();

                Database.CommandText = $"select parameterid,datatype from dbo.objectparameters WITH (NOLOCK) WHERE profileid = '{profileId}' AND objectid = '{customSqlInfo.Name}'";
                reader               = Database.ExecuteReader();
                while (reader.Read())
                {
                    var item = new CustomSqlInfoParameter
                    {
                        Name     = reader["parameterid"].ToString(),
                        DataType = reader["datatype"].ToString()
                    };
                    item.NameInDotnet        = item.Name.ToContractName();
                    item.DataTypeInDotnet    = GetDataTypeInDotnet(item.DataType);
                    item.SqlDatabaseTypeName = GetSqlDbTypeName(item.DataType);
                    
                    items.Add(item);
                }
                reader.Close();

                customSqlInfo.Parameters = items;
            }


            foreach (var customSqlInfo in list)
            {
                var items = new List<CustomSqlInfoResult>();

                Database.CommandText = $"select resultid,datatype from dbo.objectresults WITH (NOLOCK) WHERE profileid = '{profileId}' AND objectid = '{customSqlInfo.Name}'";
                reader               = Database.ExecuteReader();
                while (reader.Read())
                {
                    var item = new CustomSqlInfoResult
                    {
                        Name     = reader["resultid"].ToString(),
                        DataType = reader["datatype"].ToString()
                    };

                    item.NameInDotnet = item.Name.ToContractName();

                    if (item.DataType.Equals("object",StringComparison.OrdinalIgnoreCase))
                    {
                        continue;
                    }

                    item.DataTypeInDotnet = GetDataTypeInDotnet(item.DataType);

                    if (item.DataType.Equals("char",StringComparison.OrdinalIgnoreCase))
                    {
                        item.SqlReaderMethod = SqlReaderMethods.GetBooleanValue;
                    }
                    else
                    {
                        item.SqlReaderMethod = SqlDbTypeMap.GetSqlReaderMethod(item.DataType.ToUpperEN(),true);      
                    }
                    
                    

                    items.Add(item);
                }
                reader.Close();

                customSqlInfo.ResultColumns = items;
            }

            var returnValue =  new ProjectCustomSqlInfo
            {
                CustomSqlInfoList = list,
                
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
    }
}