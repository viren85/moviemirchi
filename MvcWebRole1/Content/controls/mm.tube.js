﻿//var PUBLIC_BLOB_URL = "http://127.0.0.1:10000/devstoreaccount1/posters/";
//var PUBLIC_BLOB_URL = "https://moviemirchistorage1.blob.core.windows.net/posters/";
var PUBLIC_BLOB_URL = "https://moviemirchistorage.blob.core.windows.net/posters/";

var pagerControlCounter = 1;
function GetTubeControl(sectionTitle, tileContainer, pagerContainerId, classId) {
    var pagerId;
    if (pagerContainerId == null || pagerContainerId == "undefined") {
        pagerId = "pager" + pagerControlCounter;
        pagerControlCounter++;
    }
    else {
        pagerId = pagerContainerId;
    }

    return $("<div id=\"" + tileContainer + "-tube\" class=\"tube-container " + (classId ? classId : "") + "\"><div class=\"" + tileContainer + "\"><ul></ul><div id=\"" + pagerId + "\"></div></div></div>");
}