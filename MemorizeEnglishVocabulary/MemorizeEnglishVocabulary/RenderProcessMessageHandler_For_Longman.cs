using System.Collections.Generic;
using System.Text;
using System.Windows;
using CefSharp;

namespace WpfApp2
{
    public class RenderProcessMessageHandler_For_Longman : IRenderProcessMessageHandler
    {
        public IDictionary<string, string> EnToTrMap;

        string GetMapAsJsObject()
        {
            var sb = new StringBuilder();
            if (EnToTrMap == null)
            {
                return "{}";
            }

            sb.AppendLine("var EnToTrMap ={};");
            foreach (var pair in EnToTrMap)
            {
                var key = pair.Key;
                if (key.Contains("'"))
                {
                    MessageBox.Show("Problem into embedd js " + key);
                }
                var value = pair.Value.Replace("'", "''").Replace("\"", "''");

                sb.AppendLine("EnToTrMap['" + key + "'] = '" + value + "';");

            }

            return sb.ToString();

        }



        // Wait for the underlying `Javascript Context` to be created, this is only called for the main frame.
        // If the page has no javascript, no context will be created.
        void IRenderProcessMessageHandler.OnContextCreated(IWebBrowser browserControl, IBrowser browser, IFrame frame)
        {
            var enToTrMap = GetMapAsJsObject();


            string script = @"document.addEventListener('DOMContentLoaded', function()
{
    var word = $('.pagetitle').html(),
        i,
        item;

    $('.header').remove();
    $('.footer').remove();
    $('.responsive_cell2').remove(); 

    $('body > div.content > div.responsive_cell6 > div.entry_content > div > span > span.dictionary_intro.span').remove();

    $('.Tail').remove();
    $('.assetlink').remove();

    $( function()
    { 
        var dictentryArr = $('.dictentry');
        if( dictentryArr.length > 1 )
        {
            dictentryArr.last().remove(); 
        }
        
    });


    $('.etym').remove();
    $('#ad_btmslot').remove();
    

    

   
    for(i=1;i < 150;i++)
    {
        if(i<2)
        {
            continue;
        }

        item = $('#'+word.replace(' ','-')+'__'+i);
        if(item.length === 0)
        {
            break;
        }

        item.remove();
    }


    $( function(){ $('.asset > div').remove(); } );

    $( function(){ 
        $('.HYPHENATION').remove();
        $('.HOMNUM').remove();
        $('.PronCodes').remove();
        $('body > div.content > div.responsive_cell6 > div.entry_content > div > span > span.dictlink > span > span.frequent.Head > span.tooltip.LEVEL').remove();
        $('body > div.content > div.responsive_cell6 > div.entry_content > div > span > span.dictlink > span > span.frequent.Head > span.FREQ').remove();
    });




" + enToTrMap + @"




var exampleElements = $('.EXAMPLE');
var currentExampleElement = null;
var tryPlay = function(index)
{	
	var exampleElement = exampleElements.get(index);
	if(exampleElement == null || index > 2)
	{
		return;		
	}
	
	exampleElement = $(exampleElement);


    var enKey   = exampleElement.text().trim();
    var trValue = EnToTrMap[enKey];

    // alert(enKey);
    // alert(EnToTrMap[enKey]);

    if(trValue)
    {
        var html = exampleElement.html();

        alert(html);
        
        var newValue = enKey + '<br/><span>' + trValue + '</span>';
        // html = html.replace(enKey,newValue);
        alert(newValue);
        exampleElement.html( newValue );
    }


	if (currentExampleElement)
    {
        currentExampleElement.css({background:''});
    }

	currentExampleElement = exampleElement;
	
	currentExampleElement.css({background:'yellow'});
	
	
	
    var element = exampleElement.find(' > span');
	

    var src_mp3 = element.attr('data-src-mp3');
    var audio   = new Audio(src_mp3);
    
    audio.playbackRate = 0.7;
    audio.play();
    audio.addEventListener('ended', function()
	{
        setTimeout( function(){ tryPlay(++index); },2000);
    });	
}

// play american pronunciation

var americanPronunciation = $('.amefile');
var americanPronunciationAudio   = new Audio(americanPronunciation.attr('data-src-mp3'));

americanPronunciationAudio.playbackRate = 0.9;
americanPronunciationAudio.play();
americanPronunciationAudio.addEventListener('ended', function()
{
    //
	setTimeout( StartToPlaySamples,1000);
});	
function StartToPlaySamples()
{
    tryPlay(0);
}




        





});";

            frame.ExecuteJavaScriptAsync(script);
        }

        public void OnContextReleased(IWebBrowser browserControl, IBrowser browser, IFrame frame)
        {

        }

        public void OnFocusedNodeChanged(IWebBrowser browserControl, IBrowser browser, IFrame frame, IDomNode node)
        {

        }

        public void OnUncaughtException(IWebBrowser browserControl, IBrowser browser, IFrame frame, JavascriptException exception)
        {

        }
    }
}