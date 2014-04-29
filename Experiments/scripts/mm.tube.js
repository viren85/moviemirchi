var pagerControlCounter = 1;
function GetTubeControl(sectionTitle, tileContainer, pagerContainerId) {
    var pagerId;
    if (pagerContainerId == null || pagerContainerId == "undefined") {
        pagerId = "pager" + pagerControlCounter;
        pagerControlCounter++;
    }
    else {
        pagerId = pagerContainerId;
    }

    var tubeControl = $("<div class=\"tube-container\"><div class=\"section-title large-fonts\">" + sectionTitle + "</div><div class=\"" + tileContainer + "\"><ul></ul><div id=\"" + pagerId + "\"></div></div></div>");
    return tubeControl;
}