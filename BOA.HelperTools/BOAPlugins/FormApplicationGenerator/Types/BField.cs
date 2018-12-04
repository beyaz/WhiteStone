using System;

namespace BOAPlugins.FormApplicationGenerator.Types
{
    [Serializable]
    public abstract class BField
    {
        #region Public Properties
        public string BindingPath { get;  set; }
        public string Label { get; set; }
        public string SnapName { get; set; }
        #endregion
    }

   

    [Serializable]
    public class BInputMask : BField
    {
        #region Constructors
        public BInputMask(string bindingPath)
        {
            BindingPath = bindingPath;
        }
        #endregion

        #region Public Properties
        public bool   IsCreditCard { get; set; }
        #endregion
    }

    [Serializable]
    public class BField_Kaldırılacak : BField
    {
        #region Constructors
        public BField_Kaldırılacak(DotNetType dotNetType, Enum name)
        {
            Name       = name.ToString();
            DotNetType = dotNetType;

            if (dotNetType == DotNetType.Int32 ||
                dotNetType == DotNetType.Decimal)
            {
                ComponentType = Types.ComponentType.BInputNumeric;
            }

            else if (dotNetType == DotNetType.DateTime)
            {
                ComponentType = Types.ComponentType.BDateTimePicker;
            }

            else if (dotNetType == DotNetType.Boolean)
            {
                ComponentType = Types.ComponentType.BCheckBox;
            }
            else
            {
                ComponentType = Types.ComponentType.BInput;
            }
        }
        #endregion

        #region Public Properties
        public ComponentType? ComponentType { get; set; }
        public DotNetType     DotNetType    { get; }
        public string         Label         { get; set; }
        public string         Name          { get; }
        public string         ParamType     { get; set; }
        #endregion
    }
}