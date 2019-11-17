using BOA.DataFlow;
using BOA.EntityGeneration.BOACardDatabaseSchemaToDllExporting.MethodWriters.BoaSystem;
using BOA.EntityGeneration.DataFlow;
using BOA.EntityGeneration.ScriptModel.Creators;
using static BOA.EntityGeneration.DataFlow.Data;

namespace BOA.EntityGeneration.BOACardDatabaseSchemaToDllExporting.ClassWriters
{

    

    /// <summary>
    ///     The generator of business class
    /// </summary>
    public class GeneratorOfBusinessClass
    {

        public static void EndNamespace(IDataContext context)
        {
            var sb = context.Get(BoaRepositoryFile);
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
            var sb = context.Get(BoaRepositoryFile);
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
                MethodWriters.BoaSystem.DeleteByKeyMethodWriter.Write(context);
            }
            #endregion

            InsertMethodWriter.Write(context);
           

            #region Update
            if (tableInfo.IsSupportSelectByKey)
            {
                sb.AppendLine();
                MethodWriters.BoaSystem.UpdateByKeyMethodWriter.Write(context);

            }
            #endregion

            #region SelectByKey
            if (tableInfo.IsSupportSelectByKey)
            {

                sb.AppendLine();
                MethodWriters.BoaSystem.SelectByKeyMethodWriter.Write(context);
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

        /// <summary>
        ///     Writes the using list.
        /// </summary>
        public static void WriteUsingList(IDataContext context)
        {
            var sb = context.Get(BoaRepositoryFile);

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