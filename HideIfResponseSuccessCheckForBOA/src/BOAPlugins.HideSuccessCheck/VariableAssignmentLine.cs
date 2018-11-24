using System.Collections.Generic;
using System.Linq;

namespace BOAPlugins.HideSuccessCheck
{
    public class VariableAssignmentLine
    {
        #region Public Properties
        public string AssignedValue    { get; set; }
        public string VariableName     { get; set; }
        public string VariableTypeName { get; set; }
        #endregion

        #region Public Methods
        public static bool IsResponseValueMatch(VariableAssignmentLine currentLineAsAssignmentLine, VariableAssignmentLine responseValueAssignmentToAnotherVariable)
        {
            var variableName = currentLineAsAssignmentLine.VariableName + ".Value";
            if (variableName == responseValueAssignmentToAnotherVariable.AssignedValue)
            {
                return true;
            }

            if (responseValueAssignmentToAnotherVariable.AssignedValue.StartsWith(variableName + "."))
            {
                var extraExtension = responseValueAssignmentToAnotherVariable.AssignedValue.RemoveFromStart(variableName);

                if (!currentLineAsAssignmentLine.AssignedValue.EndsWith(extraExtension))
                {
                    currentLineAsAssignmentLine.AssignedValue += extraExtension;
                }

                return true;
            }

            return false;
        }

        public static VariableAssignmentLine Parse(string line)
        {
            if (line == null)
            {
                return null;
            }

            var arr = line.Split('=');

            if (arr.Length < 2)
            {
                return null;
            }

            var namePart = arr[0]?.Trim().Split(' ').Where(x => !string.IsNullOrWhiteSpace(x)).ToArray();

            VariableAssignmentLine variableAssignmentLine = null;

            if (namePart?.Length == 1)
            {
                variableAssignmentLine = new VariableAssignmentLine
                {
                    VariableName = namePart[0]
                };
            }
            else if (namePart?.Length == 2)
            {
                variableAssignmentLine = new VariableAssignmentLine
                {
                    VariableName     = namePart[1],
                    VariableTypeName = namePart[0]
                };
            }
            else
            {
                return null;
            }

            var items = new List<string>();
            for (var i = 1; i < arr.Length; i++)
            {
                items.Add(arr[i].Trim());
            }

            var valuePart = string.Join(" = ", items).Trim();
            if (string.IsNullOrWhiteSpace(valuePart))
            {
                return null;
            }

            if (valuePart.EndsWith(";") == false)
            {
                return null;
            }

            variableAssignmentLine.AssignedValue = valuePart.Substring(0, valuePart.Length - 1);

            return variableAssignmentLine;
        }
        #endregion
    }
}