using BOA.DataFlow;
using BOA.EntityGeneration.BoaRepositoryFileExporting;
using BOA.EntityGeneration.DataFlow;
using BOA.Tasks;

namespace BOA.EntityGeneration.BOACardDatabaseSchemaToDllExporting.ProjectExporters
{
    /// <summary>
    ///     The business project exporter
    /// </summary>
    public class BusinessProjectExporter
    {
        #region Public Methods
        public static void Export(IDataContext context)
        {
            var schemaName            = context.Get(Data.SchemaName);
            var allInOneSourceCode    = context.Get(BoaRepositoryFileExporter.File).ToString();
            var namingPattern = context.Get(Data.NamingPattern);
            
            var ProjectExportLocation = namingPattern.RepositoryProjectDirectory;

            FileSystem.WriteAllText(context, ProjectExportLocation + "Boa.cs", allInOneSourceCode);

            var progress = context.Get(Data.SchemaGenerationProcess);

            var ns = namingPattern.RepositoryNamespace;

            var projectDirectory = $"{ProjectExportLocation}{ns}\\";

            var csprojFilePath       = $"{projectDirectory}{ns}.csproj";
            var assemblyInfoFilePath = $"{projectDirectory}\\Properties\\AssemblyInfo.cs";


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
    <WarningLevel>3</WarningLevel>
    <DocumentationFile>bin\Debug\{ns}.xml</DocumentationFile>
  </PropertyGroup>
  <PropertyGroup Condition="" '$(Configuration)|$(Platform)' == 'Release|AnyCPU' "">
    <DebugType>pdbonly</DebugType>
    <Optimize>true</Optimize>
    <OutputPath>bin\Release\</OutputPath>
    <DefineConstants>TRACE</DefineConstants>
    <ErrorReport>prompt</ErrorReport>
    <WarningLevel>3</WarningLevel>
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
    <Compile Include=""Boa.cs"" />
    <Compile Include=""Shared.cs"" />
  </ItemGroup>
  <ItemGroup />
  <Import Project=""$(MSBuildToolsPath)\Microsoft.CSharp.targets"" />
  <PropertyGroup>
    <PostBuildEvent>copy /y ""$(TargetDir)$(TargetName).*"" ""d:\boa\server\bin\""</PostBuildEvent>
  </PropertyGroup>
</Project>

";

            FileSystem.WriteAllText(context, csprojFilePath, content.Trim());

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

            FileSystem.WriteAllText(context, assemblyInfoFilePath, assemblyInfoContent.Trim());

            progress.Text = "Compile started.";
            context.Get(MsBuildQueue.MsBuildQueueId).Push(new MSBuildData
            {
                ProjectFilePath = csprojFilePath
            });
            progress.Text = "Compile finished.";
        }
        #endregion
    }
}