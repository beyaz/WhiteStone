﻿using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BOA.Tasks
{
    [TestClass]
    public class MsBuildTest
    {
        #region Public Methods
        [TestMethod]
        public void Build()
        {
            MSBuild.Build(new MSBuildData {ProjectFilePath = @"D:\work\BOA.CardModules\Dev\AutoGeneratedCodes\BNS\BOA.Types.Kernel.Card.BNS\BOA.Types.Kernel.Card.BNS.csproj"});
            MSBuild.Build(new MSBuildData {ProjectFilePath = @"D:\work\BOA.CardModules\Dev\AutoGeneratedCodes\BNS\BOA.Types.Kernel.Card.BNS\BOA.Types.Kernel.Card.BNS.csproj"});
            MSBuild.Build(new MSBuildData {ProjectFilePath = @"D:\work\BOA.CardModules\Dev\AutoGeneratedCodes\BNS\BOA.Types.Kernel.Card.BNS\BOA.Types.Kernel.Card.BNS.csproj"});

            MSBuild.Build(new MSBuildData {ProjectFilePath = @"D:\work\BOA.CardModules\Dev\AutoGeneratedCodes\BOA.Card.AutoGeneratedCodes.sln"});
        }
        #endregion
    }
}