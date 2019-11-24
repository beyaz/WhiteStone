using BOA.Common.Helpers;
using BOA.DataFlow;
using BOA.EntityGeneration.CustomSQLExporting.Wrapper;

namespace BOA.EntityGeneration.CustomSQLExporting.Exporters
{
    class CustomSqlClassGenerator : ContextContainer
    {
        #region Static Fields
        public static readonly Property<PaddedStringBuilder> Text = Property.Create<PaddedStringBuilder>(nameof(CustomSqlClassGenerator) + "->" + nameof(Text));
        #endregion

        #region Properties
        PaddedStringBuilder sb => Text[Context];
        #endregion

        #region Public Methods
        public void AttachEvents()
        {
            Context.ProfileInfoInitialized += Begin;

            Context.CustomSqlInfoInitialized += WriteSwitchCaseCondition;

            Context.ProfileInfoRemove += End;
        }

        public void InitializeText()
        {
            Context.Add(Text, new PaddedStringBuilder());
        }
        #endregion

        #region Methods
        void Begin()
        {
            sb.AppendLine("public static class CustomSql");
            sb.OpenBracket();

            sb.AppendLine("public static TOutput Execute<TOutput, T>(ObjectHelper objectHelper, ICustomSqlProxy<TOutput, T> input) where TOutput : GenericResponse<T>");
            sb.OpenBracket();

            sb.AppendLine("switch (input.Index)");
            sb.OpenBracket();
        }

        void End()
        {
            sb.CloseBracket(); // end of switch

            sb.AppendLine();
            sb.AppendLine("throw new InvalidOperationException(input.GetType().FullName);");

            sb.CloseBracket(); // end of method

            sb.CloseBracket(); // end of class
        }

        void WriteSwitchCaseCondition()
        {
            sb.AppendLine($"case {customSqlInfo.SwitchCaseIndex}:");
            sb.OpenBracket();
            sb.AppendLine($"return (TOutput) (object) new {customSqlNamingPattern.RepositoryClassName}(objectHelper.Context).Execute(({customSqlNamingPattern.InputClassName})(object) input);");
            sb.CloseBracket();
        }
        #endregion
    }
}