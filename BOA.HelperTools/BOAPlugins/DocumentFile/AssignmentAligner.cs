﻿using System;
using System.Collections.Generic;

namespace BOAPlugins.DocumentFile
{
    class AssignmentAlignerData
    {
        #region Public Properties
        public List<string> Lines       { get; set; }
        public List<string> OutputLines { get; set; } = new List<string>();
        #endregion
    }

    static class AssignmentAligner
    {
        #region Methods
        internal static void Align(AssignmentAlignerData data)
        {
            var lines = data.Lines;

            var start = 0;
            var i     = 0;
            while (i < lines.Count)
            {
                var hasAssignment = HasAssignment(lines[i]);
                if (!hasAssignment)
                {
                    AlignRange(lines, start, i);
                }

                if (i == lines.Count - 1)
                {
                    AlignRange(lines, start, i);
                }

                i++;
            }
        }

        static void AlignRange(List<string> lines, int start, int end)
        {
            var max = -1;
            for (var i = start; i < end; i++)
            {
                if (string.IsNullOrWhiteSpace(lines[i]))
                {
                    continue;
                }

                max = Math.Max(max, lines[i].IndexOf(" = ", StringComparison.Ordinal));
            }

            for (var i = start; i < end; i++)
            {
                if (string.IsNullOrWhiteSpace(lines[i]))
                {
                    continue;
                }

                var index = lines[i].IndexOf(" = ", StringComparison.Ordinal);

                var firstPart  = lines[i].Substring(0, index).Trim();
                var secondPart = lines[i].Substring(index + 2, lines[i].Length - index - 2).Trim();

                lines[i] = firstPart.PadRight(max, ' ') + " = " + secondPart;
            }
        }

        static bool HasAssignment(string line)
        {
            if (string.IsNullOrWhiteSpace(line))
            {
                return false;
            }

            var index = line.IndexOf(" = ", StringComparison.Ordinal);

            if (index <= 0)
            {
                return false;
            }

            var firstPart = line.Substring(0, index).Trim();

            if (firstPart.Contains("\""))
            {
                return false;
            }

            return true;
        }
        #endregion
    }
}