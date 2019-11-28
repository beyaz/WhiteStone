using System.Collections.Generic;
using System.Linq;
using BOA.Common.Helpers;
using BOA.DatabaseAccess;
using BOA.EntityGeneration.BOACardDatabaseSchemaToDllExporting.ProjectExporters;
using BOA.EntityGeneration.BOACardDatabaseSchemaToDllExporting.Util;
using WhiteStone.Helpers;

namespace BOA.EntityGeneration.ConstantsProjectGeneration
{
    class Generator
    {
        #region Public Properties
        public Context Context { get; } = new Context();
        #endregion

        #region Properties
        ConfigurationContract Config   => Context.Config;
        IDatabase             Database => Context.Database;
        PaddedStringBuilder   file     => Context.File;
        #endregion
         ProcessContract processInfo => Context.processInfo;
         protected FileSystem FileSystem => Context.FileSystem;

        #region Public Methods
        public void Generate()
        {
            InitEnumInformationList();

            WriteContent();
            ExportFile();
        }

        void ExportFile()
        {
            
            processInfo.Text = "Exporting BOA repository.";

            var filePath = Config.ProjectDirectory + "Boa.cs";

            FileSystem.WriteAllText(filePath, sb.ToString());
        }

        void WriteContent()
        {
            file.AppendLine("using BOA.Card.Definitions;");
            file.AppendLine();
            file.AppendLine($"namespace {Config.NamespaceName}");
            file.OpenBracket();

            foreach (var className in Context.EnumClassNameList)
            {
                file.AppendLine();
                AppendEnumPropertyToClass(Context.EnumInfoList.Where(x => x.ClassName == className).ToList());
            }

            file.CloseBracket();
        }

        void AppendEnumPropertyToClass(IReadOnlyList<EnumInfo> propertyList)
        {
            var className = propertyList.First().ClassName.ToContractName();

            file.AppendLine("[Serializable]");
            file.AppendLine($"public class {className} : EnumBase<{className}, int>");
            file.OpenBracket();

            foreach (var item in propertyList)
            {
                file.AppendLine($"public static readonly {className} {item.PropertyName.ToContractName()} = new {className}(\"{item.StringValue}\", {item.NumberValue});");
            }

            file.AppendLine();
            file.AppendLine($"public {className}(string name, int value) : base(name, value)");
            file.OpenBracket();
            file.CloseBracket();
            file.AppendLine();
            file.AppendLine($"public static explicit operator {className}(string value)");
            file.OpenBracket();
            file.AppendLine($"return Parse<{className}>(value);");
            file.CloseBracket();

            file.CloseBracket();

            
        }
        #endregion

        #region Methods
        void InitEnumInformationList()
        {
            Database.CommandText = @"

BEGIN
  IF OBJECT_ID('tempdb..#enumInfo') IS NOT NULL DROP TABLE #enumInfo
      
  ;WITH AlreadyDefinedEnums AS
  (
      SELECT enumclassname, enumitemname, enumvalue, enumsortid, ROW_NUMBER() OVER(PARTITION BY enumitemname ORDER BY enumsortid) AS 'RowNum'
        FROM dbo.enums
    GROUP BY enumclassname,enumitemname,enumvalue,enumsortid  
  )
   SELECT enumclassname AS ClassName, enumitemname AS PropertyName, enumvalue AS StringValue, enumsortid AS NumberValue INTO #enumInfo
    FROM AlreadyDefinedEnums
   WHERE RowNum = 1

  create UNIQUE index idx on #enumInfo (ClassName,PropertyName)
  
  INSERT INTO #enumInfo VALUES('DENEME_CLASS_1','PROPERTY_1','A','0')
  INSERT INTO #enumInfo VALUES('DENEME_CLASS_1','PROPERTY_2','B','0')       


    SELECT * FROM #enumInfo      
END




";
            Context.EnumInfoList      = Database.ExecuteReader().ToList<EnumInfo>();
            Context.EnumClassNameList = Context.EnumInfoList.OrderBy(x => x.ClassName).GroupBy(x => x.ClassName).Select(x => x.Key).ToList();
        }
        #endregion
    }
}