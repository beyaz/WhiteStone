﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using BOA.Common.Helpers;
using BOA.DatabaseAccess;
using BOA.EntityGeneration.BOACardDatabaseSchemaToDllExporting;
using BOA.EntityGeneration.DbModel.SqlServerDataAccess;
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

        [Inject]
        public TableInfoDao TableInfoDao { get; set; }

        [Inject]
        public TableOverride TableOverride { get; set; }

        [Inject]
        public Tracer Tracer { get; set; }
        #endregion

        #region Public Methods
        public ProjectCustomSqlInfo GetByProfileId(string profileId)
        {
            var project = GetByProfileIdFromDatabase(profileId);

            foreach (var customSqlInfo in project.CustomSqlInfoList)
            {
                Fill(customSqlInfo);
            }

            return project;
        }
        #endregion

        #region Methods
        protected virtual ProjectCustomSqlInfo GetByProfileIdFromDatabase(string profileId)
        {
            var list = new List<CustomSqlInfo>();

            Database.CommandText = $"SELECT objectid, text, schemaname, resultcollectionflag FROM dbo.objects WITH (NOLOCK) WHERE profileid = '{profileId}' AND objecttype = 'CUSTOMSQL'";
            var reader = Database.ExecuteReader();
            while (reader.Read())
            {
                list.Add(new CustomSqlInfo
                {
                    Name       = reader["objectid"].ToString(),
                    Sql        = reader["text"] + string.Empty,
                    SchemaName = reader["schemaname"] + string.Empty,
                    SqlResultIsCollection = (reader["resultcollectionflag"] + string.Empty) == "1"
                    
                });
            }

            reader.Close();

            foreach (var customSqlInfo in list)
            {
                var items = new List<CustomSqlInfoParameter>();

                Tracer.Trace($"Fetching data for {customSqlInfo.Name}");

                Database.CommandText = $"select parameterid,datatype,nullableflag from dbo.objectparameters WITH (NOLOCK) WHERE profileid = '{profileId}' AND objectid = '{customSqlInfo.Name}'";

                reader = Database.ExecuteReader();
                while (reader.Read())
                {
                    var item = new CustomSqlInfoParameter
                    {
                        Name       = reader["parameterid"].ToString(),
                        DataType   = reader["datatype"].ToString(),
                        IsNullable = reader["nullableflag"] + string.Empty == "1"
                    };

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

            throw new NotImplementedException(dataType);
        }

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

            if (dataType.Equals("numeric", StringComparison.OrdinalIgnoreCase)||
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

        void Fill(CustomSqlInfo customSqlInfo)
        {
            foreach (var item in customSqlInfo.Parameters)
            {
                item.CSharpPropertyName        = item.Name.ToContractName();
                item.CSharpPropertyTypeName    = GetDataTypeInDotnet(item.DataType, item.IsNullable);
                item.SqlDbTypeName = GetSqlDbTypeName(item.DataType);

                item.CSharpPropertyTypeName = TableOverride.GetColumnDotnetType(item.Name, item.CSharpPropertyTypeName, item.IsNullable);
            }

            if (customSqlInfo.ResultColumns.Any(x => x.DataType.Equals("object", StringComparison.OrdinalIgnoreCase)))
            {
                if (customSqlInfo.ResultColumns.Count == 1)
                {
                    var tableInfo = TableInfoDao.GetInfo(TableCatalogName.BOACard, customSqlInfo.SchemaName, customSqlInfo.ResultColumns[0].Name);

                    TableOverride.Override(tableInfo);

                    customSqlInfo.ResultColumns = (from columnInfo in tableInfo.Columns
                                                   select
                                                       new CustomSqlInfoResult
                                                       {
                                                           Name             = columnInfo.ColumnName,
                                                           DataType         = columnInfo.DataType,
                                                           IsNullable       = columnInfo.IsNullable,
                                                           DataTypeInDotnet = columnInfo.DotNetType,
                                                           NameInDotnet     = columnInfo.ColumnName.ToContractName(),
                                                           SqlReaderMethod  = columnInfo.SqlReaderMethod
                                                       }).ToList();

                    return;
                }

                throw new NotImplementedException();
            }

            foreach (var item in customSqlInfo.ResultColumns)
            {
                item.NameInDotnet = item.Name.ToContractName();

                item.DataTypeInDotnet = GetDataTypeInDotnet(item.DataType, item.IsNullable);

                item.DataTypeInDotnet = TableOverride.GetColumnDotnetType(item.Name, item.DataTypeInDotnet, item.IsNullable);

                if (item.Name.EndsWith("_FLAG", StringComparison.OrdinalIgnoreCase))
                {
                    item.SqlReaderMethod = item.IsNullable ? SqlReaderMethods.GetBooleanNullableValue : SqlReaderMethods.GetBooleanValue;
                }
                else
                {
                    item.SqlReaderMethod = SqlDbTypeMap.GetSqlReaderMethod(item.DataType.ToUpperEN(), item.IsNullable);
                }
            }
        }

        IReadOnlyList<string> GetByProfileIdList()
        {
            var items = new List<string>();

            Database.CommandText = "SELECT profileid  from BOACard.dbo.profileskt WITH (NOLOCK)";
            var reader = Database.ExecuteReader();
            while (reader.Read())

            {
                items.Add(reader["profileid"] + string.Empty);
            }

            reader.Close();

            return items;
        }
        #endregion
    }
}