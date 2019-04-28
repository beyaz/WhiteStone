using System;
using System.Collections.Generic;
using System.IO;
using BOA.Common.Helpers;
using BOA.DatabaseAccess;
using Ninject;

namespace BOA.EntityGeneration.BOACardCustomSqlIntoProjectInjection
{

    public class DataAccess
    {
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
                reader = Database.ExecuteReader();
                while (reader.Read())
                {
                    var item = new CustomSqlInfoParameter
                    {
                        Name     = reader["parameterid"].ToString(),
                        DataType = reader["datatype"].ToString()
                    };
                    item.NameInDotnet = item.Name.ToContractName();
                    item.DataTypeInDotnet = GetDataTypeInDotnet(item.DataType);
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

                    item.NameInDotnet        = item.Name.ToContractName();

                    if (item.DataType.Equals("object",StringComparison.OrdinalIgnoreCase))
                    {
                        continue;
                    }

                    item.DataTypeInDotnet   = GetDataTypeInDotnet(item.DataType);

                    if (item.DataType.Equals("char",StringComparison.OrdinalIgnoreCase))
                    {
                        item.SqlReaderMethod = "GetBooleanValue";
                    }
                    else
                    {
                        item.SqlReaderMethod = SqlDataType.GetSqlReaderMethod(item.DataType.ToUpperEN(),true);      
                    }
                    
                    

                    items.Add(item);
                }
                reader.Close();

                customSqlInfo.ResultColumns = items;
            }

            var returnValue =  new ProjectCustomSqlInfo
            {
                CustomSqlInfoList   = list,
                
            };
            Database.CommandText = $@"SELECT typesnamespace,typesprojectpath, businessnamespace,businessprojectpath FROM dbo.profileskt WITH(NOLOCK) WHERE profileid = '{profileId}'";

            reader = Database.ExecuteReader();
            while (reader.Read())
            {
                returnValue.TypesProjectPath = reader["typesprojectpath"].ToString();
                returnValue.BusinessProjectPath = reader["businessprojectpath"].ToString();
                returnValue.NamespaceNameOfType = reader["typesnamespace"].ToString();
                returnValue.NamespaceNameOfBusiness = reader["businessnamespace"].ToString();
                break;
            }
            reader.Close();

            return returnValue;
        }
    }

    [Serializable]
    public class ProjectCustomSqlInfo
    {
        public IReadOnlyList<CustomSqlInfo> CustomSqlInfoList { get; set; }
        public string NamespaceNameOfType { get; set; }
        public string NamespaceNameOfBusiness { get; set; }
        public string TypesProjectPath { get; set; }
        public string BusinessProjectPath { get; set; }
    }


    public class ProjectInjector
    {
        [Inject]
        public AllInOneForTypeDll AllInOneForTypeDll { get; set; }

        [Inject]
        public AllInOneForBusinessDll AllInOneForBusinessDll { get; set; }
        [Inject]
        public DataAccess DataAccess { get; set; }

        public void Inject(string profileId)
        {
            Inject(DataAccess.GetByProfileId(profileId));
        }
        void Inject(ProjectCustomSqlInfo data)
        {
            var typeCode = AllInOneForTypeDll.GetCode(data);
            var businessCode = AllInOneForBusinessDll.GetCode(data);

            File.WriteAllText(data.TypesProjectPath+"\\CustomSql.cs",typeCode);
            File.WriteAllText(data.BusinessProjectPath+"\\CustomSql.cs",businessCode);

        }
    }
    public class AllInOneForTypeDll
    {
        [Inject]
        public TypeWriter TypeWriter { get; set; }

        public string GetCode( ProjectCustomSqlInfo data)
        {
            var sb = new PaddedStringBuilder();

            Write(sb,data);


            return sb.ToString();

        }

        public void Write(PaddedStringBuilder sb, ProjectCustomSqlInfo data)
        {
            sb.AppendLine("using BOA.Common.Types;");
            sb.AppendLine("using System;");
            sb.AppendLine("using System.Collections.Generic;");

            sb.AppendLine();
            sb.AppendLine($"namespace {data.NamespaceNameOfType}");
            sb.AppendLine("{");
            sb.PaddingCount++;

            foreach (var item in data.CustomSqlInfoList)
            {
                sb.AppendLine();
                TypeWriter.Write(sb,item);
            }


            sb.PaddingCount--;
            sb.AppendLine("}");
            
        }
    }

    public class AllInOneForBusinessDll
    {

        [Inject]
        public BusinessClassWriter BusinessClassWriter { get; set; }

        public string GetCode( ProjectCustomSqlInfo data)
        {
            var sb = new PaddedStringBuilder();

            Write(sb,data);


            return sb.ToString();

        }

        public void Write(PaddedStringBuilder sb, ProjectCustomSqlInfo data)
        {
            sb.AppendLine("using BOA.Base;");
            sb.AppendLine("using BOA.Base.Data;");
            sb.AppendLine("using BOA.Common.Types;");
            sb.AppendLine($"using {data.NamespaceNameOfType};");
            sb.AppendLine("using System;");
            sb.AppendLine("using System.Data;");
            sb.AppendLine("using System.Collections.Generic;");

            sb.AppendLine();
            sb.AppendLine($"namespace {data.NamespaceNameOfBusiness}");
            sb.AppendLine("{");
            sb.PaddingCount++;

            foreach (var item in data.CustomSqlInfoList)
            {
                sb.AppendLine();
                BusinessClassWriter.Write(sb,item);
            }


            sb.PaddingCount--;
            sb.AppendLine("}");
        }
    }



    [Serializable]
    public class CustomSqlInfo
    {
        #region Public Properties
        public string Name { get; set; }

        public string                                ParameterContractName => Name.ToContractName() + "Request";
        public IReadOnlyList<CustomSqlInfoParameter> Parameters            { get; set; }
        public IReadOnlyList<CustomSqlInfoResult>    ResultColumns         { get; set; }

        public string ResultContractName => Name.ToContractName() + "Contract";

        public string BusinessClassName => Name.ToContractName();

        public string BusinessClassNamespace { get; set; }

        public string Sql { get; set; }
        #endregion
    }

    [Serializable]
    public class CustomSqlInfoParameter
    {
        #region Public Properties
        public string DataType         { get; set; }
        public string DataTypeInDotnet { get; set; }
        public string Name             { get; set; }

        public string NameInDotnet        { get; set; }
        public string SqlDatabaseTypeName { get; set; }
        #endregion
    }

    [Serializable]
    public class CustomSqlInfoResult
    {
        #region Public Properties
        public string DataType { get; set; }

        public string DataTypeInDotnet { get; set; }
        public string Name             { get; set; }

        public string NameInDotnet    { get; set; }
        public string SqlReaderMethod { get; set; }
        #endregion
    }

    public class TypeWriter
    {
        #region Public Methods
        public void Write(PaddedStringBuilder sb, CustomSqlInfo data)
        {
            sb.AppendLine("/// <summary>");
            sb.AppendLine($"///     Result class of '{data.Name}' sql.");
            sb.AppendLine("/// </summary>");
            sb.AppendLine("[Serializable]");
            sb.AppendLine($"public sealed class {data.ResultContractName} : CardContractBase");
            sb.AppendLine("{");
            sb.PaddingCount++;

            foreach (var item in data.ResultColumns)
            {
                sb.AppendLine($"public {item.DataTypeInDotnet} {item.NameInDotnet} {{get; set;}}");
            }

            sb.AppendLine("}");
            sb.PaddingCount--;

            sb.AppendLine();
            sb.AppendLine("/// <summary>");
            sb.AppendLine($"///     Parameter class of '{data.Name}' sql.");
            sb.AppendLine("/// </summary>");
            sb.AppendLine("[Serializable]");
            sb.AppendLine($"public partial class {data.ParameterContractName} : CardRequestBase");
            sb.AppendLine("{");
            sb.PaddingCount++;

            foreach (var item in data.Parameters)
            {
                sb.AppendLine($"public {item.DataTypeInDotnet} {item.NameInDotnet} {{get; set;}}");
            }

            sb.AppendLine("}");
            sb.PaddingCount--;
        }
        #endregion
    }
}