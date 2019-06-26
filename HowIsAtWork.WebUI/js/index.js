
/// <reference path="vue.js" />

"use strict";
var Config = {
    suggestionUrl:'../testData/suggestions.json',
    searchInputId: '#tags'
}

var app = new Vue({
    el: "#page",

    data:
    {
        searchResults:
        [
            { commentCount: 141 },
            { commentCount: 142 },
            { commentCount: 143 }
        ]
    },
    computed:
    {
    },
    methods:
    {
    }
});


var InitializeSearchInput = function()
{
    // http://www.runningcoder.org/jquerytypeahead/demo/
    
    $.typeahead({
        input:'.js-typeahead',
        minLength: 1,
        order: "asc",
        
        dynamic:true,
        
        source: {
            url:Config.suggestionUrl
        },
        callback: {
            onClickAfter: function (node, a, item, event) {
				window.location.href = item.url; // Set window location to site url
			}
        }
    });
}

$(window.document).ready(function ()
{
  InitializeSearchInput();      
});
