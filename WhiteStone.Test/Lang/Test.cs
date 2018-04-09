using System.Linq;
using Lang.Lexers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Lang
{
    [TestClass]
    public class Test
    {
        #region Public Methods
        [TestMethod]
        public void ParseBindingExpression()
        {
            var expression = "{Binding     XXX.Y.Z,Mode=OneWay,Converter= uuu.oo.o7 , ConverterParameter= 'j4 A5: p o ş'}";
            var lexer      = new Lexer(expression);
            var tokens     = lexer.Lex().ToList();
            Assert.AreEqual(24, tokens.Count);
        }
        #endregion
    }
}