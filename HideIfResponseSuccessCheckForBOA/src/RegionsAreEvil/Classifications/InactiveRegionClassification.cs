// -----------------------------------------------------------------------
// <copyright file="InactiveRegionClassification.cs" company="Equilogic (Pty) Ltd">
//     Copyright © Equilogic (Pty) Ltd. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Documents;


namespace RegionsAreEvil.Classifications
{
    using System.ComponentModel.Composition;
    using System.Windows.Media;

    using Microsoft.VisualStudio.Text.Classification;
    using Microsoft.VisualStudio.Utilities;

    [Export(typeof(EditorFormatDefinition))]
    [ClassificationType(ClassificationTypeNames = Constants.InactiveRegionClassificationTypeNames)]
    [Name(Constants.InactiveRegionName)]
    [DisplayName(Constants.InactiveRegionName)]
    [UserVisible(true)]
    [Order(After = Constants.OrderAfterPriority, Before = Constants.OrderBeforePriority)]
    internal sealed class InactiveRegionClassification : ClassificationFormatDefinition
    {
        #region Initialization

        // Methods
        public InactiveRegionClassification()
        {
            ForegroundColor = Colors.Gray;
            FontRenderingSize = Options.InActiveRegionSize;
            ForegroundOpacity = Options.InActiveRegionOpacity;
        }

        #endregion
    }

    public static class Options
    {
        static string OptionFilePath=> Path.GetDirectoryName(typeof(Options).Assembly.Location) + Path.DirectorySeparatorChar + "HideIfResponseSuccessCheckForBOA.Options.txt";
        public static bool IsReadFromFile;

        public static int InActiveRegionSize = 10;
        public static double InActiveRegionOpacity= 0.5;

        public static int    ActiveRegionSize    = 10;
        public static double ActiveRegionOpacity = 0.5;

        static Options()
        {
            if (File.Exists(OptionFilePath))
            {
                var lines = File.ReadAllText(OptionFilePath).Split(Environment.NewLine.ToCharArray()).Where(x => !string.IsNullOrWhiteSpace(x)).Select(x=>x.Trim()).ToList();

                InActiveRegionSize = int.Parse(GetValue(lines, "InActiveRegionSize:"));
                InActiveRegionOpacity = double.Parse(GetValue(lines, "InActiveRegionOpacity:"));

                ActiveRegionSize = int.Parse(GetValue(lines, "ActiveRegionSize:"));
                ActiveRegionOpacity = double.Parse(GetValue(lines, "ActiveRegionOpacity:"));

                IsReadFromFile = true;
            }
        }

        static string GetValue(IEnumerable<string> lines, string key)
        {
            return lines.FirstOrDefault(x => x.StartsWith(key))?.Substring(key.Length);
        }
    }
}