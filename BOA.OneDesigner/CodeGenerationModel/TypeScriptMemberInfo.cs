namespace BOA.OneDesigner.CodeGenerationModel
{
    public class TypeScriptMemberInfo
    {
        #region Public Properties
        public string Code          { get; set; }
        public bool   IsConstructor { get; set; }
        public bool   IsField       { get; set; }
        public bool   IsMethod      { get; set; }
        public bool   IsRender      { get; set; }
        
        public bool IsComponentMountMethod { get; set; }
        #endregion

        #region Public Methods
        public static int Compare(TypeScriptMemberInfo left, TypeScriptMemberInfo right)
        {
            if (left.IsRender && right.IsRender)
            {
                return 0;
            }

            if (left.IsConstructor && right.IsConstructor)
            {
                return 0;
            }

            if (left.IsField && right.IsField)
            {
                return 0;
            }

            if (left.IsMethod && right.IsMethod)
            {
                return 0;
            }

            if (left.IsComponentMountMethod && right.IsComponentMountMethod)
            {
                return 0;
            }
            

            if (left.IsField)
            {
                return -1;
            }

            if (right.IsField)
            {
                return 1;
            }

            if (left.IsConstructor)
            {
                return -1;
            }

            if (right.IsConstructor)
            {
                return 1;
            }

            if (left.IsComponentMountMethod )
            {
                return -1;
            }

            if (right.IsComponentMountMethod )
            {
                return 1;
            }

            if (left.IsMethod)
            {
                return -1;
            }

            return 1;
        }
        #endregion
    }
}