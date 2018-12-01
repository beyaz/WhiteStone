using System;

namespace BOAPlugins.FormApplicationGenerator.Types
{
    [Serializable]
    public class BField
    {
        #region Constructors
        public BField(DotNetType dotNetType, Enum name)
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
        public string         Name          { get; }
        public string         ParamType     { get; set; }
        public string         Label         { get; set; }
        #endregion
    }
}