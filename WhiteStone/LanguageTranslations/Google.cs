//using System.Linq;
//using HtmlAgilityPack;

//namespace BOA.LanguageTranslations
//{
//    class Google
//    {
//        public static WordInfo TranslateEnglishToTurkish(string word)
//        {


//            var url = "https://translate.google.com/#en/tr/mean";
//            var web = new HtmlWeb();
//            var doc = web.Load(url, "10.13.50.100",8080,"beyaztas","hattori_hanzo_41");

//            var result = doc.DocumentNode.SelectNodes("//*[@id='result_box']")?.FirstOrDefault();

           

//            return new WordInfo
//            {
//                Definition = result?.InnerHtml
//            };
//        }
//    }
//}