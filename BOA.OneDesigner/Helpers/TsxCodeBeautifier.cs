using System;
using System.Linq;
using BOA.Common.Helpers;

namespace BOA.OneDesigner.Helpers
{
    public class TsxCodeBeautifier
    {
        #region Public Methods
        public static string Beautify(string code)
        {
            if (string.IsNullOrWhiteSpace(code))
            {
                return code;
            }

            var lines = code.SplitToLines().ToList();

            var i = 0;
            while (true)
            {
                if (i + 1 >= lines.Count)
                {
                    break;
                }

                if (string.IsNullOrWhiteSpace(lines[i]) && (string.IsNullOrWhiteSpace(lines[i + 1]) || 
                                                            lines[i + 1].Trim()=="}"||
                                                            lines[i + 1].Trim()=="{"))
                {
                    lines.RemoveAt(i);
                    continue;
                }

                i++;
            }


            return string.Join(Environment.NewLine, lines);
        }
        #endregion
    }
}