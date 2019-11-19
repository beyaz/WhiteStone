using System;
using System.Collections.Generic;
using System.Linq;
using BOA.DataFlow;
using BOA.EntityGeneration.BOACardDatabaseSchemaToDllExporting.ProjectExporters;
using BOA.Tasks;
using static BOA.EntityGeneration.CustomSQLExporting.Data;
using static BOA.EntityGeneration.CustomSQLExporting.Wrapper.CustomSqlExporter;

namespace BOA.EntityGeneration.CustomSQLExporting.Exporters
{
    class EntityCsprojFileExporter
    {
        #region Public Methods
        public static void AttachEvents(IDataContext context)
        {
            context.AttachEvent(OnProfileInfoInitialized, InitializeAssemblyReferences);
            context.AttachEvent(OnProfileInfoRemove, Export);
        }
        #endregion

        #region Methods
        static void Export(IDataContext context)
        {
            var profileNamingPattern = context.Get(ProfileNamingPattern);

            var assemblyReferences = string.Join(Environment.NewLine, EntityAssemblyReferences[context].Distinct());

            var ns               = profileNamingPattern.EntityNamespace;
            var projectDirectory = profileNamingPattern.EntityProjectDirectory;

            var csprojFilePath = $"{projectDirectory}{ns}.csproj";

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
    " + assemblyReferences + @"
    <Reference Include=""System"" />
    <Reference Include=""System.Core"" />
  </ItemGroup>
  <ItemGroup>
    <Compile Include=""Properties\AssemblyInfo.cs"" />
    <Compile Include=""All.cs"" />
  </ItemGroup>
  <ItemGroup />
  <Import Project=""$(MSBuildToolsPath)\Microsoft.CSharp.targets"" />
  <PropertyGroup>
    <PostBuildEvent>
        copy /y ""$(TargetDir)$(TargetName).*"" ""d:\boa\server\bin\""
        copy /y ""$(TargetDir)$(TargetName).*"" ""d:\boa\client\bin\""
        copy /y ""$(TargetDir)$(TargetName).*"" ""d:\boa\one\""
    </PostBuildEvent>
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

            var assemblyInfoFilePath = $"{projectDirectory}\\Properties\\AssemblyInfo.cs";
            FileSystem.WriteAllText(context, assemblyInfoFilePath, assemblyInfoContent.Trim());

            context.Get(MsBuildQueue.MsBuildQueueId).Push(new MSBuildData {ProjectFilePath = csprojFilePath});
        }

        static void InitializeAssemblyReferences(IDataContext context)
        {
            EntityAssemblyReferences[context] = new List<string>();
            EntityAssemblyReferences[context].AddRange(ProfileNamingPattern[context].EntityAssemblyReferences);
        }
        #endregion
    }
}