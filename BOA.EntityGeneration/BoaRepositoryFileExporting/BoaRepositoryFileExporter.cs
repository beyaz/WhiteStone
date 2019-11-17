using System.IO;
using BOA.Common.Helpers;
using BOA.DataFlow;
using BOA.EntityGeneration.BOACardDatabaseSchemaToDllExporting.ProjectExporters;
using BOA.EntityGeneration.BOACardDatabaseSchemaToDllExporting.SharedClasses;
using BOA.EntityGeneration.BoaRepositoryFileExporting.MethodWriters;
using BOA.EntityGeneration.DataFlow;
using BOA.EntityGeneration.ScriptModel.Creators;
using static BOA.EntityGeneration.DataFlow.Data;

namespace BOA.EntityGeneration.BoaRepositoryFileExporting
{

    

    class BoaRepositoryFileExporter
    {
        internal static readonly IDataConstant<PaddedStringBuilder> File = DataConstant.Create<PaddedStringBuilder>(nameof(File));



        public static void AttachEvents(IDataContext context)
        {
            context.AttachEvent(SchemaExportingEvent.SchemaExportStarted, InitializeOutput);
            context.AttachEvent(SchemaExportingEvent.SchemaExportStarted, WriteUsingList);
            context.AttachEvent(SchemaExportingEvent.SchemaExportStarted, EmptyLine);
            context.AttachEvent(SchemaExportingEvent.SchemaExportStarted, BeginNamespace);
            context.AttachEvent(SchemaExportingEvent.SchemaExportStarted, WriteEmbeddedClasses);
            

            context.AttachEvent(TableExportingEvent.TableExportStarted, WriteClass);

            context.AttachEvent(SchemaExportingEvent.SchemaExportFinished, EndNamespace);
            context.AttachEvent(SchemaExportingEvent.SchemaExportFinished, ExportFileToDirectory);
            context.AttachEvent(SchemaExportingEvent.SchemaExportFinished, ClearOutput);
        }





        #region Public Methods
        public static void BeginNamespace(IDataContext context)
        {
            var sb            = context.Get(BoaRepositoryFileExporter.File);
            var namingPattern = context.Get(Data.NamingPattern);
            
            var config = context.Get(Data.Config);

            sb.AppendLine();
            sb.BeginNamespace(namingPattern.RepositoryNamespace);
            ObjectHelperSqlUtilClassWriter.Write(sb,config);

        }
        #endregion

        #region Methods
    

        static void WriteEmbeddedClasses(IDataContext context)
        {
            var sb = context.Get(File);

            var path = Path.GetDirectoryName(typeof(BoaRepositoryFileExporter).Assembly.Location) + Path.DirectorySeparatorChar + "BoaRepositoryFileEmbeddedCodes.txt";

            sb.AppendAll(System.IO.File.ReadAllText(path));
            sb.AppendLine();
        }
        #endregion


        static void ExportFileToDirectory(IDataContext context)
        {
            var allInOneSourceCode = context.Get(File).ToString();
            var namingPattern      = context.Get(Data.NamingPattern);

            FileSystem.WriteAllText(context, namingPattern.RepositoryProjectDirectory + "Boa.cs", allInOneSourceCode);
        }

        static void InitializeOutput(IDataContext context)
        {
            context.Add(File, new PaddedStringBuilder());
        }

        static void ClearOutput(IDataContext context)
        {
            context.Remove(File);
        }

        static void EmptyLine(IDataContext context)
        {
            context.Get(File).AppendLine();
        }

        public static void EndNamespace(IDataContext context)
        {
            var sb = context.Get(File);
            sb.EndNamespace();
        }

        #region Constants
        /// <summary>
        ///     The contract parameter name
        /// </summary>
        const string contractParameterName = "contract";

        /// <summary>
        ///     The parameter identifier
        /// </summary>
        const string ParameterIdentifier = "@";

        /// <summary>
        ///     The parameter name of column names
        /// </summary>
        const string ParameterNameOf_columnNames = "columnNames";

        /// <summary>
        ///     The top count parameter name
        /// </summary>
        const string TopCountParameterName = "$resultCount";
        #endregion

       

        #region Public Methods

        

        
        /// <summary>
        ///     Writes the class.
        /// </summary>
        public static void WriteClass(IDataContext context)
        {
            var sb = context.Get(File);
            var tableInfo = context.Get(TableInfo);
          





         
            var className = tableInfo.TableName.ToContractName();
           

            
           
            
            ContractCommentInfoCreator.Write(sb, tableInfo);
            sb.AppendLine($"public sealed class {className} : ObjectHelper");
            sb.AppendLine("{");
            sb.PaddingCount++;

            ContractCommentInfoCreator.Write(sb, tableInfo);
            sb.AppendLine($"public {className}(ExecutionDataContext context) : base(context) {{ }}");

            #region Delete
            if (tableInfo.IsSupportSelectByKey)
            {
                sb.AppendLine();
                DeleteByKeyMethodWriter.Write(context);
            }
            #endregion

            InsertMethodWriter.Write(context);
           

            #region Update
            if (tableInfo.IsSupportSelectByKey)
            {
                sb.AppendLine();
                UpdateByKeyMethodWriter.Write(context);

            }
            #endregion

            #region SelectByKey
            if (tableInfo.IsSupportSelectByKey)
            {

                sb.AppendLine();
                SelectByKeyMethodWriter.Write(context);
            }
            #endregion

            SelectByUniqueIndexMethodWriter.Write(context);

           

            var selectAllInfo = SelectAllInfoCreator.Create(tableInfo);

            SelectAllMethodWriter.Write(context);

            if (tableInfo.ShouldGenerateSelectAllByValidFlagMethodInBusinessClass)
            {
                SelectAllByValidFlagMethodWriter.Write(context);
            }

            sb.CloseBracket();

            
            
        }

        static void WriteUsingList(IDataContext context)
        {
            var sb = context.Get(File);

            var namingPattern = context.Get(Data.NamingPattern);

            foreach (var line in namingPattern.BoaRepositoryUsingLines)
            {
                sb.AppendLine(line);
            }
            
        }
        #endregion

        #region Methods
       

        

        

      

        


       
        
        #endregion
    }
}