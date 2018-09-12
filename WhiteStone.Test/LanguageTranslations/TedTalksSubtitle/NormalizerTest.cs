using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BOA.LanguageTranslations.TedTalksSubtitle
{
    [TestClass]
    public class NormalizerTest
    {
        #region Public Methods
        [TestMethod]
        public void NormalizeFile()
        {
            var path = @"D:\English Translate\ElonMusk\tr.en.srt";
            Normalizer.NormalizeFile(path);
        }

        [TestMethod]
        public void Test()
        {
            const string data = @"

1
00:00:12,174 --> 00:00:15,306
Chris Anderson: Elon, nasıl çılgın bir hayal
Chris Anderson: Elon, what kind of crazy dream

2
00:00:15,306 --> 00:00:17,462
seni otomobil sanayisine girmeye ve
would persuade you to think of trying

3
00:00:17,462 --> 00:00:20,820
elektrikli araba üretmeyi düşünmeye ikna etti?
to take on the auto industry and build an all-electric car?

4
00:00:20,820 --> 00:00:24,222
Elon Musk: Aslında hikaye üniversite dönemime kadar uzanıyor.
Elon Musk: Well, it goes back to when I was in university.


";
            var result = Normalizer.Normalize(data);

            const string expectedResult = @"
Chris Anderson: Elon, nasıl çılgın bir hayal seni otomobil sanayisine girmeye ve elektrikli araba üretmeyi düşünmeye ikna etti?
Chris Anderson: Elon, what kind of crazy dream would persuade you to think of trying to take on the auto industry and build an all-electric car?

Elon Musk: Aslında hikaye üniversite dönemime kadar uzanıyor.
Elon Musk: Well, it goes back to when I was in university.

";

            Assert.AreEqual(expectedResult.Trim(), result.Trim());
        }
        #endregion
    }
}