
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
    name:"ttt",
    imageUrl:"http://via.placeholder.com/565x565.jpg",
    occupation:"Kılıç Ustası_0",
    startCount:3,
    startCountRemain:2,
    commentCount:56,
    monitoringCount:67,
    score:3.7,
    infoAttributes:[
      {name:"Meslek",value:"Kılıç Ustası"},
      {name:"telefon",value:"0545 566 76 87"},
      {name:"Lokasyon",value:"İstanbul / Ümraniye"}
    ]
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
  
    $("#ex8").bootstrapSlider({
      min:0,
      max:100,
      precision:20,
      value:66,
      tooltip: 'always',
      ticks_tooltip:true
    });
  
});
