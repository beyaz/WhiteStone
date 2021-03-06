﻿using System.Linq;
using System.Windows.Data;
using CustomUIMarkupLanguage.Markup;
using CustomUIMarkupLanguage.TextTokenizer;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CustomUIMarkupLanguage.Test.TextTokenizer
{
    [TestClass]
    public class TokenizerTest
    {
        #region Public Methods
        [TestMethod]
        public void ParseBindingExpressions()
        {
            var str = @"{Binding A.Br5.C_76,Mode=TwoWay}";

            var tokens = Tokenizer.Tokenize(BindingExpressionTokenDefinitions.Value, str).ToList();

            Assert.AreEqual(13, tokens.Count);



            var info = BindingExpressionParser.TryParse("{Binding     XXX.Y.Z,Mode=OneWay,Converter= " + typeof(TokenizerTest).FullName + " , ConverterParameter= 'j4 A5: p o ş'}");
            Assert.AreEqual("XXX.Y.Z",info.SourcePath);
            Assert.AreEqual(BindingMode.OneWay,info.BindingMode);

            Assert.AreEqual(typeof(TokenizerTest).FullName,info.ConverterTypeFullName);

            Assert.AreEqual("j4 A5: p o ş",info.ConverterParameter as string);

            

        }

        [TestMethod]
        public void ParseBindingExpressionsWithEnglish()
        {
            

            var info = BindingExpressionParser.TryParse("{Binding Inner.A}");
            Assert.IsTrue(info.SourcePath == "Inner.A");

        }
        #endregion
    }
}