using System.Collections.Generic;
using System.Linq;
using System.Threading;
using BOA.Common.Helpers;
using WhiteStone.Helpers;

namespace BOA.EntityGeneration.ConstantsProjectGeneration
{
    class Generator
    {
        #region Public Properties
        public Context Context { get; } = new Context();
        #endregion

        #region Properties
        ConstantsProjectGenerationConfig Config      => Context.Config;
        PaddedStringBuilder              File        => Context.File;
        FileSystem                       FileSystem  => Context.FileSystem;
        ProcessContract                  ProcessInfo => Context.ProcessInfo;
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
                FileNames        = new List<string> {Config.SourceCodeFileName},
                NamespaceName    = Config.NamespaceName,
                IsClientDll      = true,
                ProjectDirectory = Config.ProjectDirectory,
                References       = Config.AsseblyReferences
            };

            var csprojFilePath = csprojFileGenerator.Generate();

            Context.MsBuildQueue.Push(csprojFilePath);
        }

        void ExportFile()
        {
            ProcessInfo.Text = "Writing files.";

            var filePath = Config.ProjectDirectory + Config.SourceCodeFileName;

            FileSystem.WriteAllText(filePath, File.ToString());
        }

        void InitEnumInformationList()
        {
            var database = Context.Database;

            database.CommandText              = Config.DataSourceProcedureFullName;
            database.CommandIsStoredProcedure = true;
            Context.EnumInfoList              = database.ExecuteReader().ToList<EnumInfo>();
            Context.EnumClassNameList         = Context.EnumInfoList.OrderBy(x => x.ClassName).GroupBy(x => x.ClassName).Select(x => x.Key).ToList();
        }

        void WriteContent()
        {
            foreach (var line in Config.UsingLines)
            {
                File.AppendLine(line);
            }

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