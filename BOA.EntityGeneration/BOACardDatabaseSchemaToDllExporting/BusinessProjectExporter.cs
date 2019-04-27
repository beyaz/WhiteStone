using BOA.Common.Helpers;
using Ninject;

namespace BOA.EntityGeneration.BOACardDatabaseSchemaToDllExporting
{
    public class BusinessProjectExporter
    {
        #region Constants
        const string ExportDirectory = @"D:\temp\";
        #endregion

        #region Public Properties
        [Inject]
        public NamingHelper NamingHelper { get; set; }
        #endregion

        #region Public Methods
        public void Export(string schemaName, string allInOneSourceCode)
        {
            var businessClassNamespace = NamingHelper.GetBusinessClassNamespace(schemaName);

            var projectDirectory = $"{ExportDirectory}{businessClassNamespace}\\";

            var csprojFilePath       = $"{projectDirectory}{businessClassNamespace}.csproj";
            var assemblyInfoFilePath = $"{projectDirectory}\\Properties\\AssemblyInfo.cs";

            const string allInOneFileName = "All";

            FileHelper.WriteAllText($"{projectDirectory}{allInOneFileName}.cs", allInOneSourceCode);

            var content = $@"

<?xml version=""1.0"" encoding=""utf-8""?>
<Project ToolsVersion=""15.0"" xmlns=""http://schemas.microsoft.com/developer/msbuild/2003"">
  <Import Project=""$(MSBuildExtensionsPath)\$(MSBuildToolsVersion)\Microsoft.Common.props"" Condition=""Exists('$(MSBuildExtensionsPath)\$(MSBuildToolsVersion)\Microsoft.Common.props')"" />
  <PropertyGroup>
    <Configuration Condition="" '$(Configuration)' == '' "">Debug</Configuration>
    <Platform Condition="" '$(Platform)' == '' "">AnyCPU</Platform>
    <OutputType>Library</OutputType>
    <AppDesignerFolder>Properties</AppDesignerFolder>
    <RootNamespace>{businessClassNamespace}</RootNamespace>
    <AssemblyName>{businessClassNamespace}</AssemblyName>
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
    <Reference Include=""BOA.Messaging"">
      <HintPath>D:\BOA\Server\bin\BOA.Messaging.dll</HintPath>
    </Reference>
    <Reference Include=""System"" />
    <Reference Include=""System.Core"" />
    <Reference Include=""System.Xml.Linq"" />
    <Reference Include=""System.Data.DataSetExtensions"" />
    <Reference Include=""Microsoft.CSharp"" />
    <Reference Include=""System.Data"" />
    <Reference Include=""System.Net.Http"" />
    <Reference Include=""System.Xml"" />
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

            FileHelper.WriteAllText(csprojFilePath, content.Trim());

            var assemblyInfoContent = $@"
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

// General Information about an assembly is controlled through the following
// set of attributes. Change these attribute values to modify the information
// associated with an assembly.
[assembly: AssemblyTitle(""{businessClassNamespace}"")]
[assembly: AssemblyDescription("""")]
[assembly: AssemblyConfiguration("""")]
[assembly: AssemblyCompany("""")]
[assembly: AssemblyProduct(""{businessClassNamespace}"")]
[assembly: AssemblyCopyright(""Copyright ©  2019"")]
[assembly: AssemblyTrademark("""")]
[assembly: AssemblyCulture("""")]

// Setting ComVisible to false makes the types in this assembly not visible
// to COM components.  If you need to access a type in this assembly from
// COM, set the ComVisible attribute to true on that type.
[assembly: ComVisible(false)]

// Version information for an assembly consists of the following four values:
//
//      Major Version
//      Minor Version
//      Build Number
//      Revision
//
// You can specify all the values or you can default the Build and Revision Numbers
// by using the '*' as shown below:
// [assembly: AssemblyVersion(""1.0.*"")]
[assembly: AssemblyVersion(""1.0.0.0"")]
[assembly: AssemblyFileVersion(""1.0.0.0"")]
";

            FileHelper.WriteAllText(assemblyInfoFilePath, assemblyInfoContent);
        }
        #endregion
    }
}