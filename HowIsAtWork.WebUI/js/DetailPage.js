
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
    visibleRecords:[],
    name:"ttt"
};

var Config = 
{
    initialDataUrl:'../testData/detail.json'
};

var app = new Vue(
{
    el: "#page",

    data:Model,
    computed:{    },
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
   
}


var OnSearchSuccess = function(data)
{
    Object.assign(Model,data);
    
    EvaluatePage();
}



var LoadInitialData = function(query)
{
    $.getJSON(Config.initialDataUrl,OnSearchSuccess);    
}

$(window.document).ready(function ()
{ 
    LoadInitialData();
});
