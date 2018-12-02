using System;
using System.Text;

namespace BOAPlugins.FormApplicationGenerator.Types
{
    [Serializable]
    public class BDateTimePicker : BField
    {
        #region Constructors
        public BDateTimePicker(string bindingPath)
        {
            BindingPath = bindingPath;
        }
        #endregion

        #region Public Methods
        public override string ToJsx(string indent)
        {
            var sb = new StringBuilder();

            sb.AppendLine(indent + "<BDateTimePicker format = \"DDMMYYYY\")");
            sb.AppendLine(indent + "                 value  = {" + BindingPath + "}");
            sb.AppendLine(indent + "           dateOnChange = {(e: any, value: Date) => " + BindingPath + " = value}");
            sb.AppendLine(indent + "  floatingLabelTextDate = {" + Label + "}");
            sb.AppendLine(indent + "                context = {context}/>");

            return sb.ToString();
        }
        #endregion
    }

    [Serializable]
    public class BAccountComponent : BField
    {
        #region Constructors
        public BAccountComponent(string bindingPath)
        {
            BindingPath = bindingPath;
        }
        #endregion

        #region Public Methods
        public override string ToJsx(string indent)
        {
            var sb = new StringBuilder();

            sb.AppendLine(indent+"<BAccountComponent accountNumber   = {" + BindingPath + "}");
            sb.AppendLine(indent+"                   onAccountSelect = {(selectedAccount: any) => " + BindingPath + " = selectedAccount ? selectedAccount.accountNumber : null}");
            sb.AppendLine(indent+"                  isVisibleBalance = {false}");
            sb.AppendLine(indent+"            isVisibleAccountSuffix = {false}");
            sb.AppendLine(indent+"enableShowDialogMessagesInCallback = {false}");
            sb.AppendLine(indent+"                     isVisibleIBAN = {false}");
            sb.AppendLine(indent+"                               ref = {(r: any) => this.snaps.SnapName = r}");
            sb.AppendLine(indent+"                           context = {context}/>");

            return sb.ToString();
        }
        #endregion
    }
}