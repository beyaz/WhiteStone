using System.Linq;
using System.Text;

namespace BOAPlugins.HideSuccessCheck
{
    public static class RegionParser
    {

        class LineInfo
        {
            public string Text { get; set; }
            public int Index { get; set; }
        }

        static LineInfo CleanRead(RegionParserData data, int index)
        {
            var lineCount = data.LineCount;
            
            while (index<lineCount)
            {
                var line = data[index]?.Trim();
                if (string.IsNullOrWhiteSpace(line))
                {
                    index++;
                    continue;
                }

                if (line.StartsWith("//"))
                {
                    index++;
                    continue;
                }

                return new LineInfo
                {
                    Text = line,
                    Index = index
                };
            }

            return null;
        }
            

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
                    //var response = bo.call();   i                 
                    //if (!response.Success)      i+1
                    //{                           i+2
                    //    return returnObject.Add(response);       -> var x = bo.call();
                    //}
                    // var x = response.Value;

                    const int leftBracketOffset = 2;
                    if (data[i + leftBracketOffset]?.Trim() != "{")
                    {
                        continue;
                    }

                    var rightBracketOffset = 4;

                    LineInfo lineInfo1 = null;
                    LineInfo lineInfo2 = null;
                    LineInfo lineInfo3 = null;
                    


                    lineInfo1 = CleanRead(data,i + leftBracketOffset + 1);
                    if (lineInfo1 != null)
                    {
                        lineInfo2 = CleanRead(data,lineInfo1.Index+1);

                        if (lineInfo2 != null)
                        {
                            lineInfo3 = CleanRead(data,lineInfo2.Index+1);
                        }
                    }
                    

                    // returnObject.Results.AddRange(responseAcc.Results);
                    if (IsAddingResultToReturnObject(lineInfo1?.Text) &&
                        lineInfo2?.Text == "return returnObject;" &&
                        lineInfo3?.Text == "}")
                    {
                        rightBracketOffset = lineInfo3.Index - i;
                    }
                    else if (lineInfo1?.Text?.StartsWith("return returnObject") == true &&
                             lineInfo2?.Text?.Trim() == "}")
                    {
                        rightBracketOffset = lineInfo2.Index - i;
                    }
                    else if (lineInfo1?.Text?.StartsWith("Context.AddResult(") == true &&
                             lineInfo2?.Text == "return;" &&
                             lineInfo3?.Text  == "}")
                    {
                        rightBracketOffset = lineInfo3.Index - i;
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


                    LineInfo lineInfo = null;

                    // walk empty lines
                    if (rightBracketOffset < lineCount - 1)
                    {

                        lineInfo = CleanRead(data,i + rightBracketOffset + 1);

                        

                        

                        responseValueAssignmentToAnotherVariable = VariableAssignmentLine.Parse(data[lineInfo.Index]);
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
                            StartLine   = LineNumber,
                            StartOffset = firstCharIndex,
                            EndLine     = data.GetLineNumber(lineInfo.Index),
                            Text        = sb.ToString()
                        });

                        i = lineInfo.Index;
                        continue;
                    }

                    data.Regions.Add(new Region
                    {
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