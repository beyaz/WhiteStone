
/// <reference path="vue.js" />

"use strict";

var Model = 
{
    labels:{
        siteShortName:'İşinde Nasıl ?',
        siteShortNameToUpper:'İŞİNDE NASIL ?'
    },
    records:[],
    paginationRecordCountPerPage: 3,
    paginationIndex:1,
    paginationCount:3,
    searchResults:[],
    visibleRecords:[]
};


var Config = {
    suggestionUrl:'../testData/suggestions.json?{{query}}',
    searchInputId: '#tags',
    searchUrl:'../testData/search.json',
    initialResultsUrl:'../testData/initialResults.json'
    
}



var app = new Vue({
    el: "#page",

    data:Model,
    computed:
    {
    },
    methods:
    {
        gotoPaginationIndex: function(clickedPaginationIndex)
        {
            Model.paginationIndex = clickedPaginationIndex;
            EvaluatePage();
        }
    }
});


var EvaluatePage = function()
{
    Model.paginationCount = Math.ceil(Model.records.length / Model.paginationRecordCountPerPage);
    
    var from = (Model.paginationIndex - 1) * Model.paginationRecordCountPerPage;
    
    var end = from + Model.paginationRecordCountPerPage;
    
    Model.visibleRecords = Model.records.slice(from,end);
}



var OnSearchSuccess = function(data)
{
    Model.records = data;
    
    EvaluatePage();
}

var Search = function(query)
{
    $.getJSON(Config.searchUrl,OnSearchSuccess);    
}

var InitializeSearchInput = function()
{
    $.typeahead({
        input:'.js-typeahead',
        minLength: 1,
        order: "asc",
        
        dynamic:true,
        
        source: {
            url:Config.suggestionUrl,
            url:Config.suggestionUrl,
            data:{q:'{{query}}'}
          
        },
        minLength : 1,
        callback: {
            onClickAfter: function (node, a, item, event) 
            {
                var selectedText = item.display;
				Search(selectedText);
			}
        }
    });
}

var LoadInitialData = function(query)
{
    $.getJSON(Config.initialResultsUrl,OnSearchSuccess);    
}

$(window.document).ready(function ()
{
    InitializeSearchInput();
    LoadInitialData();
});
