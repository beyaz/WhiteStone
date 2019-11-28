using System.Collections.Generic;
using System.Linq;
using System.Threading;
using BOA.Common.Helpers;
using BOA.EntityGeneration.BOACardDatabaseSchemaToDllExporting.ProjectExporters;
using BOA.EntityGeneration.BOACardDatabaseSchemaToDllExporting.Util;
using BOA.EntityGeneration.CustomSQLExporting.Exporters;
using WhiteStone.Helpers;

namespace BOA.EntityGeneration.ConstantsProjectGeneration
{
    class Generator
    {
        #region Constants
        const string FileName = "AllEnums.cs";
        #endregion

        #region Public Properties
        public Context Context { get; } = new Context();
        #endregion

        #region Properties
        ConfigurationContract Config      => Context.Config;
        PaddedStringBuilder   File        => Context.File;
        FileSystem            FileSystem  => Context.FileSystem;
        ProcessContract       ProcessInfo => Context.ProcessInfo;
        #endregion

        #region Public Methods
        public void Generate()
        {
            InitEnumInformationList();

            WriteContent();

            ExportFile();

            ExportCsProjFiles();

            Context.MsBuildQueue.Build();
        }
        #endregion

        #region Methods
        void AppendEnumPropertyToClass(IReadOnlyList<EnumInfo> propertyList)
        {
            var className = propertyList.First().ClassName.ToContractName();

            File.AppendLine("[Serializable]");
            File.AppendLine($"public class {className} : EnumBase<{className}, int>");
            File.OpenBracket();

            foreach (var item in propertyList)
            {
                File.AppendLine($"public static readonly {className} {item.PropertyName.ToContractName()} = new {className}(\"{item.StringValue}\", {item.NumberValue});");
            }

            File.AppendLine();
            File.AppendLine($"public {className}(string name, int value) : base(name, value)");
            File.OpenBracket();
            File.CloseBracket();
            File.AppendLine();
            File.AppendLine($"public static explicit operator {className}(string value)");
            File.OpenBracket();
            File.AppendLine($"return Parse<{className}>(value);");
            File.CloseBracket();

            File.CloseBracket();
        }

        void ExportCsProjFiles()
        {
            var csprojFileGenerator = new CsprojFileGenerator
            {
                FileSystem       = FileSystem,
                FileNames        = new List<string> {FileName},
                NamespaceName    = Config.NamespaceName,
                IsClientDll      = true,
                ProjectDirectory = Config.ProjectDirectory,
                References       = new[] {"<Reference Include=\"BOA.Card.Definitions\"><HintPath>D:\\BOA\\Server\\bin\\BOA.Card.Definitions.dll</HintPath></Reference>"}
            };

            var csprojFilePath = csprojFileGenerator.Generate();

            Context.MsBuildQueue.Push(csprojFilePath);
        }

        void ExportFile()
        {
            ProcessInfo.Text = "Writing files.";

            var filePath = Config.ProjectDirectory + FileName;

            FileSystem.WriteAllText(filePath, File.ToString());
        }

        void InitEnumInformationList()
        {
            var database = Context.Database;

            database.CommandText = @"

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
            Context.EnumInfoList      = database.ExecuteReader().ToList<EnumInfo>();
            Context.EnumClassNameList = Context.EnumInfoList.OrderBy(x => x.ClassName).GroupBy(x => x.ClassName).Select(x => x.Key).ToList();
        }

        void WriteContent()
        {
            File.AppendLine("using BOA.Card.Definitions;");
            File.AppendLine();
            File.AppendLine($"namespace {Config.NamespaceName}");
            File.OpenBracket();

            ProcessInfo.Total   = Context.EnumClassNameList.Count;
            ProcessInfo.Current = 0;

            foreach (var className in Context.EnumClassNameList)
            {
                ProcessInfo.Text = $"Exporting class: {className}";
                Thread.Sleep(10);

                File.AppendLine();
                AppendEnumPropertyToClass(Context.EnumInfoList.Where(x => x.ClassName == className).ToList());

                ProcessInfo.Current++;
            }

            File.CloseBracket();
        }
        #endregion
    }
}