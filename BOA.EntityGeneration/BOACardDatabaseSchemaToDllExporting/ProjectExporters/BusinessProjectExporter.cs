﻿
using BOA.EntityGeneration.BOACardDatabaseSchemaToDllExporting.Util;
using BOA.Tasks;
using Ninject;
using static ___Company___.EntityGeneration.DataFlow.DataContext;

namespace BOA.EntityGeneration.BOACardDatabaseSchemaToDllExporting.ProjectExporters
{
    public class BusinessProjectExporter
    {
        #region Public Properties
        [Inject]
        public Config Config { get; set; }

        [Inject]
        public FileSystem FileSystem { get; set; }

        [Inject]
        public MsBuildQueue MsBuildQueue { get; set; }

        [Inject]
        public NamingHelper NamingHelper { get; set; }

        [Inject]
        public ProjectExportLocation ProjectExportLocation { get; set; }

        [Inject]
        public Tracer Tracer { get; set; }
        #endregion

        #region Public Methods
        public void Export(string schemaName, string allInOneSourceCode)
        {
            Context.FireEvent(Context.BeforeBusinessClassExport);

            var ns = NamingHelper.GetBusinessClassNamespace(schemaName);

            var projectDirectory = $"{ProjectExportLocation.GetExportLocationOfBusinessProject(schemaName)}{ns}\\";

            var csprojFilePath       = $"{projectDirectory}{ns}.csproj";
            var assemblyInfoFilePath = $"{projectDirectory}\\Properties\\AssemblyInfo.cs";

            const string allInOneFileName = "All";

            var content = $@"

<?xml version=""1.0"" encoding=""utf-8""?>
<Project ToolsVersion=""15.0"" xmlns=""http://schemas.microsoft.com/developer/msbuild/2003"">
  <Import Project=""$(MSBuildExtensionsPath)\$(MSBuildToolsVersion)\Microsoft.Common.props"" Condition=""Exists('$(MSBuildExtensionsPath)\$(MSBuildToolsVersion)\Microsoft.Common.props')"" />
  <PropertyGroup>
    <Configuration Condition="" '$(Configuration)' == '' "">Debug</Configuration>
    <Platform Condition="" '$(Platform)' == '' "">AnyCPU</Platform>
    <OutputType>Library</OutputType>
    <AppDesignerFolder>Properties</AppDesignerFolder>
    <RootNamespace>{ns}</RootNamespace>
    <AssemblyName>{ns}</AssemblyName>
    <TargetFrameworkVersion>v4.6.1</TargetFrameworkVersion>
    <FileAlignment>512</FileAlignment>
    <Deterministic>true</Deterministic>
    <SccProjectName>SAK</SccProjectName>
    <SccLocalPath>SAK</SccLocalPath>
    <SccAuxPath>SAK</SccAuxPath>
    <SccProvider>SAK</SccProvider>
  </PropertyGroup>
  <PropertyGroup Condition="" '$(Configuration)|$(Platform)' == 'Debug|AnyCPU' "">
    <DebugSymbols>true</DebugSymbols>
    <DebugType>full</DebugType>
    <Optimize>false</Optimize>
    <OutputPath>bin\Debug\</OutputPath>
    <DefineConstants>DEBUG;TRACE</DefineConstants>
    <ErrorReport>prompt</ErrorReport>
    <WarningLevel>4</WarningLevel>
    <DocumentationFile>bin\Debug\{ns}.xml</DocumentationFile>
  </PropertyGroup>
  <PropertyGroup Condition="" '$(Configuration)|$(Platform)' == 'Release|AnyCPU' "">
    <DebugType>pdbonly</DebugType>
    <Optimize>true</Optimize>
    <OutputPath>bin\Release\</OutputPath>
    <DefineConstants>TRACE</DefineConstants>
    <ErrorReport>prompt</ErrorReport>
    <WarningLevel>4</WarningLevel>
  </PropertyGroup>
  <ItemGroup>
    <Reference Include=""BOA.Base"">
      <HintPath>D:\BOA\Server\bin\BOA.Base.dll</HintPath>
    </Reference>
    <Reference Include=""BOA.Common"">
      <HintPath>D:\BOA\Server\bin\BOA.Common.dll</HintPath>
    </Reference>
    <Reference Include=""BOA.Types.Kernel.Card.{schemaName}"">
      <HintPath>D:\BOA\Server\bin\BOA.Types.Kernel.Card.{schemaName}.dll</HintPath>
    </Reference>
    <Reference Include=""BOA.Types.Kernel.Card"">
      <HintPath>d:\boa\server\bin\BOA.Types.Kernel.Card.dll</HintPath>
    </Reference>    
    <Reference Include=""System"" />
    <Reference Include=""System.Core"" />
    <Reference Include=""System.Data"" />
  </ItemGroup>
  <ItemGroup>
    <Compile Include=""Properties\AssemblyInfo.cs"" />
    <Compile Include=""{allInOneFileName}.cs"" />
  </ItemGroup>
  <ItemGroup />
  <Import Project=""$(MSBuildToolsPath)\Microsoft.CSharp.targets"" />
  <PropertyGroup>
    <PostBuildEvent>copy /y ""$(TargetDir)$(TargetName).*"" ""d:\boa\server\bin\""</PostBuildEvent>
  </PropertyGroup>
</Project>

";

            FileSystem.WriteAllText(csprojFilePath, content.Trim());

            var assemblyInfoContent = $@"
using System.Reflection;
using System.Runtime.InteropServices;

[assembly: AssemblyTitle(""{ns}"")]
[assembly: AssemblyDescription("""")]
[assembly: AssemblyConfiguration("""")]
[assembly: AssemblyCompany("""")]
[assembly: AssemblyProduct(""{ns}"")]
[assembly: AssemblyCopyright(""Copyright ©  2019"")]
[assembly: AssemblyTrademark("""")]
[assembly: AssemblyCulture("""")]
[assembly: ComVisible(false)]
[assembly: AssemblyVersion(""1.0.0.0"")]
[assembly: AssemblyFileVersion(""1.0.0.0"")]
";

            FileSystem.WriteAllText(assemblyInfoFilePath, assemblyInfoContent.Trim());

            Tracer.SchemaGenerationProcess.Text = "Compile started.";
            MsBuildQueue.Push(new MSBuildData
            {
                ProjectFilePath = csprojFilePath
            });
            Tracer.SchemaGenerationProcess.Text = "Compile finished.";
        }

        public void ExportAllInOneFile(string schemaName, string allInOneSourceCode)
        {
            var allInOneFilePath = Config.FilePathForAllDaoInOneFile.Replace("{SchemaName}", schemaName);

            FileSystem.WriteAllText(allInOneFilePath, allInOneSourceCode);
        }
        #endregion
    }
}