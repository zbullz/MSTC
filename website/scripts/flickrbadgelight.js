/* Flickrbadge Light v1 by Christian Heilmann http://wait-till-i.com */
function YAHOO_config_run(o){if(o===undefined){var feedUrl='http://yui.yahooapis.com/2.3.0/build/yahoo-dom-event/yahoo-dom-event.js';var s=document.createElement('script');s.src=feedUrl;s.type='text/javascript';document.getElementsByTagName('head')[0].appendChild(s);};if(o&&o.name==='yahoo-dom-event'){YAHOO.example.flickrbadge=function(){var YD=YAHOO.util.Dom,YE=YAHOO.util.Event,d=document,head=d.getElementsByTagName('head')[0];

// Variables to change 
var containerClass='flickrbadge';
var originClass='flickrlink';
var listClass='flickritems';
var CSSURL='flickrbadgelight.css';
var seeAllLabel='see all photos';
var noLinksClass='nolinks';

// Minified, do not edit 
var bs=[];var c=d.createElement('link');var reg=/\/|:|@|\./g;c.rel='stylesheet';c.type='text/css';c.href=CSSURL;head.appendChild(c);function seed(o){var chunks=o.link.split('/');chunks[4]=o.items[0].author_id;var id=chunks.join('/').replace(reg,'');var ori=d.getElementById(id);var out='';var list=d.createElement('ul');YD.addClass(list,listClass);var all=bs[id].amount||o.items.length;for(var i=0;i<all;i++){out+='<li><a href="'+o.items[i].link+'"><img src="'+o.items[i].media.m.replace('_m','_s')+'" alt="'+o.items[i].title+'"></a></li>';};list.innerHTML=out;ori.appendChild(list);bs[id].sets=list.getElementsByTagName('ul');if(bs[id].nolinks===false){var p=d.createElement('p');var a=d.createElement('a');a.href=o.link;a.innerHTML=seeAllLabel;p.appendChild(a);ori.appendChild(p);}};function init(o){var src=o.getElementsByTagName('a')[0];YD.addClass(src,originClass);var data=sanitizeUrl(src.href.replace('%40','@'));o.id=data[1];bs[o.id]={};var c=o.className.match(/thumbs(\d+)/);bs[o.id].amount=c?c[1]:null;bs[o.id].nolinks=YD.hasClass(o,noLinksClass);var s=d.createElement('script');s.src=data[0];s.type='text/javascript';head.appendChild(s);if(bs[o.id].nolinks){src.parentNode.removeChild(src);}};function sanitizeUrl(url){var url=url.replace(/\/$/,'');var chunks=url.split('/');var all=chunks.length;var apiurl='http://api.flickr.com/services/feeds/photos_public.gne?id=';apiurl+=(url.indexOf('tags')!==-1)?chunks[all-3]+'&tags='+chunks[all-1]:chunks[all-1];apiurl+='&format=json';var id=chunks.join('/').replace(reg,'');return[apiurl,id];};var bs=YD.getElementsByClassName(containerClass,'div');YD.batch(bs,init);return{seed:seed};}();};};function jsonFlickrFeed(o){YAHOO.example.flickrbadge.seed(o);};YAHOO_config={listener:YAHOO_config_run};YAHOO_config_run();
