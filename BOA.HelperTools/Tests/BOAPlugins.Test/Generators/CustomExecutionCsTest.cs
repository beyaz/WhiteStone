﻿using System.IO;
using BOA.CodeGeneration.Common;
using BOA.CodeGeneration.Generators;
using BOA.CodeGeneration.Model;
using BOA.CodeGeneration.Services;
using BOA.CodeGeneration.SQLParser;
using BOA.CodeGeneration.Util;
using BOA.Common.Helpers;
using BOA.DatabaseAccess;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WhiteStone.Helpers;

namespace BOACodeGeneratorTest.Generators
{
    [TestClass]
    public class CustomExecutionCsTest
    {
        #region Properties
        static string ExpectedOutputForExecuteReader => File.ReadAllText("Generators\\GetKMHLimitByDebitCard.txt");
        static string ExpectedOutputForExecuteScalar => File.ReadAllText("Generators\\SelectTotalUsedAmountOfAccountCs.txt");

        static string sel_KMHLimitByDebitCard => File.ReadAllText("Generators\\DBT.sel_KMHLimitByDebitCard.txt");

        IDatabase DbLayerPrep
        {
            get
            {
                var connection = "Server={0};Database={1};Trusted_Connection=True;".FormatCode(ServerNames.PrepAtlas, "BOA");
                return new SqlDatabase(connection);
            }
        }
        #endregion

        #region Public Methods
        [TestMethod]
        public void ExecuteReaderTest()
        {
            var testData = new CustomExecution
            {
                DotNetMethodName = "GetKMHLimitByDebitCard",
                ReturnValueType  = "BOA.Types.Kernel.DebitCard.DebitCardKMHInfoContract".LoadBOAType(),
                SqlProcedureName = "sel_KMHLimitByDebitCard",
                ExecutionType    = ExecutionType.ExecuteReader,

                Comment                   = "Aloha",
                ProcedureDefinitionScript = sel_KMHLimitByDebitCard,
                DatabaseEnumName          = "Boa",
                ProcedureFullName         = "DBT.sel_KMHLimitByDebitCard",
                Database                  = DbLayerPrep
            };


            var instance = new CustomExecutionCs(null, testData);

            var cs = instance.Generate();

            StringHelper.IsEqualAsData(ExpectedOutputForExecuteReader, cs, GlobalizationUtility.EnglishCulture)
                        .Should().BeTrue();
        }

        [TestMethod]
        public void ExecuteScalarTest()
        {
            var testData = new CustomExecution
            {
                DotNetMethodName = "SelectTotalUsedAmountOfAccount",
                ReturnValueType = "decimal?".LoadBOAType(),
                SqlProcedureName = "sel_DebitTransactionTotalUsedAmountOfAccount",
                ExecutionType = ExecutionType.ExecuteScalar,

                Comment = "Aloha"
            };
            testData.ProcedureDefinitionScript = File.ReadAllText("Generators\\DBT.sel_DebitTransactionTotalUsedAmountOfAccount.txt");
            testData.DatabaseEnumName = "Boa";
            testData.ProcedureFullName = "DBT.sel_DebitTransactionTotalUsedAmountOfAccount";
            testData.Database = DbLayerPrep;

            var instance = new CustomExecutionCs(null, testData);


            var cs = instance.Generate();

            Assert.IsTrue(StringHelper.IsEqualAsData(ExpectedOutputForExecuteScalar, cs, GlobalizationUtility.EnglishCulture));
        }

        IBOAProcedureCommentParser BOAProcedureCommentParser => new BOAProcedureCommentParser();

        [TestMethod]
        public void ParseSqlCommentForCsMethod()
        {
            var comment = "Purpose       : Selects used amount of account number for given @transactionDate.";

            comment = BOAProcedureCommentParser.GetCommentForDotNet(comment);

            Assert.AreEqual("Selects used amount of account number for given @transactionDate.", comment);

            // test no purpose info
            comment = @"
/* KUWAIT TURKISH PARTICIPATION BANK INC.

    All rights are reserved. Reproduction or transmission in whole or in part, in
    any form or by any means, electronic, mechanical or otherwise, is prohibited
    without the prior written consent of the copyright owner.
*/
";

            comment = BOAProcedureCommentParser.GetCommentForDotNet(comment);

            Assert.IsNull(comment);

            // test with purpose info with multiline
            comment = @"
/* KUWAIT TURKISH PARTICIPATION BANK INC.

    All rights are reserved. Reproduction or transmission in whole or in part, in
    any form or by any means, electronic, mechanical or otherwise, is prohibited
    without the prior written consent of the copyright owner.

    Generator Information
        Generated By  : Abdullah BEYAZTAŞ
        Purpose       : Selects used amount of account number for given @transactionDate.
*/
";

            comment = BOAProcedureCommentParser.GetCommentForDotNet(comment);

            Assert.AreEqual("Selects used amount of account number for given @transactionDate.", comment);

            // test with purpose info with multiline with star
            comment =
                @"/*  KUWAIT TURKISH PARTICIPATION BANK INC.
 *   
 *  All rights are reserved. Reproduction or transmission in whole or in part, in          
 *  any form or by any means, electronic, mechanical or otherwise, is prohibited
 *  without the prior written consent of the copyright owner. 
 * 
 * 
 *  Generator Information
 *      Generated By: Abdullah Beyaztaş
 *		Purpose: Selects used amount of account number for given @transactionDate.
 *
 */";

            comment = BOAProcedureCommentParser.GetCommentForDotNet(comment);

            Assert.AreEqual("Selects used amount of account number for given @transactionDate.", comment);
        }

        [TestMethod]
        public void TryToGetReturnColumnNames()
        {
            var procedure = Parser.ParseProcedure(sel_KMHLimitByDebitCard);

            using (var db = DbLayerPrep)
            {
                var arr = procedure.TryToGetReturnColumnNames(db);
                Assert.AreEqual(8, arr.Count);
                Assert.AreEqual("CardNumber", arr[5].ColumnName);
            }
        }
        #endregion
    }
}