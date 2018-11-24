using System.Linq;
using System.Text;

namespace JavaScriptRegions
{
    public static class RegionParser
    {
        #region Public Methods
        public static void Parse(RegionParserData data)
        {
            var lineCount = data.LineCount;

            for (var i = 0; i < lineCount; i++)
            {
                if (i + 4 >= lineCount)
                {
                    continue;
                }

                var nextLineText    = data[i + 1]?.Replace(" ", string.Empty);
                var isResponseCheck = nextLineText?.StartsWith("if(!") == true && nextLineText.EndsWith(".Success)");

                var LineNumber = data.GetLineNumber(i);

                if (isResponseCheck)
                {
                    //var response = bo.call();                    
                    //if (!response.Success)
                    //{
                    //    return returnObject.Add(response);       -> var x = bo.call();
                    //}
                    // var x = response.Value;

                    var leftBracketOffset = 2;
                    if (data[i + leftBracketOffset]?.Trim() != "{")
                    {
                        continue;
                    }

                    var rightBracketOffset = 4;

                    // returnObject.Results.AddRange(responseAcc.Results);
                    if (IsAddingResultToReturnObject(data[i + leftBracketOffset + 1]) &&
                        data[i + leftBracketOffset + 2]?.Trim() == "return returnObject;" &&
                        data[i + leftBracketOffset + 3]?.Trim() == "}")
                    {
                        rightBracketOffset = leftBracketOffset + 3;
                    }
                    else if (data[i + leftBracketOffset + 1]?.Trim().StartsWith("return returnObject") == true &&
                             data[i + leftBracketOffset + 2]?.Trim() == "}")
                    {
                        rightBracketOffset = leftBracketOffset + 2;
                    }
                    else if (data[i + leftBracketOffset + 1]?.Trim().StartsWith("Context.AddResult(") == true &&
                             data[i + leftBracketOffset + 2]?.Trim() == "return;" &&
                             data[i + leftBracketOffset + 3]?.Trim() == "}")
                    {
                        rightBracketOffset = leftBracketOffset + 3;
                    }

                    else
                    {
                        continue;
                    }

                    var firstChar = data[i + 1].First(c => c != ' ');

                    var firstCharIndex = data[i + 1].IndexOf(firstChar);

                    var currentLine = data[i];

                    var currentLineAsAssignmentLine = VariableAssignmentLine.Parse(currentLine);
                    if (currentLineAsAssignmentLine == null)
                    {
                        continue;
                    }

                    VariableAssignmentLine responseValueAssignmentToAnotherVariable = null;

                    // walk empty lines
                    var endOfEmptyLinesOffset = rightBracketOffset + 1;
                    if (rightBracketOffset < lineCount - 1)
                    {
                        while (true)
                        {
                            if (i + endOfEmptyLinesOffset >= lineCount)
                            {
                                break;
                            }

                            if (!string.IsNullOrEmpty(data[i + endOfEmptyLinesOffset]))
                            {
                                break;
                            }

                            endOfEmptyLinesOffset++;
                        }

                        responseValueAssignmentToAnotherVariable = VariableAssignmentLine.Parse(data[i + endOfEmptyLinesOffset]);
                    }

                    
                    if (responseValueAssignmentToAnotherVariable != null && VariableAssignmentLine.IsResponseValueMatch(currentLineAsAssignmentLine, responseValueAssignmentToAnotherVariable))
                    {
                        var sb = new StringBuilder();
                        if (responseValueAssignmentToAnotherVariable.VariableTypeName != null)
                        {
                            sb.Append(responseValueAssignmentToAnotherVariable.VariableTypeName);
                            sb.Append(" ");
                        }

                        sb.Append(responseValueAssignmentToAnotherVariable.VariableName);

                        sb.Append(" = ");

                        sb.Append(currentLineAsAssignmentLine.AssignedValue);
                        sb.Append(";");

                        data.Regions.Add(new Region
                        {
                            Level       = 1,
                            StartLine   = LineNumber,
                            StartOffset = firstCharIndex,
                            EndLine     = data.GetLineNumber(i + endOfEmptyLinesOffset),
                            Text        = sb.ToString()
                        });

                        i = i + endOfEmptyLinesOffset;
                        continue;
                    }

                    data.Regions.Add(new Region
                    {
                        Level       = 1,
                        StartLine   = LineNumber,
                        StartOffset = firstCharIndex,
                        EndLine     = data.GetLineNumber(i + rightBracketOffset),
                        Text        = currentLineAsAssignmentLine.AssignedValue + ";"
                    });

                    i = i + rightBracketOffset;
                }
            }
        }
        #endregion

        #region Methods
        static bool IsAddingResultToReturnObject(string line)
        {
            line = line?.Trim();
            if (line?.StartsWith("returnObject.Results.Add") == true)
            {
                return true;
            }

            if (line?.StartsWith("returnObject.Add(") == true)
            {
                return true;
            }

            return false;
        }
        #endregion
    }
}