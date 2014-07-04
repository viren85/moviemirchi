function GetRateControl(rate) {

    var r = Math.round(rate);
    var rateContainer = $("<div/>").attr("class", "rate-container").html("<span class='rate rate-" + r + "'></span><span class='myscore'>" + r + "</span>");
    /*for (i = 1; i <= r; i++) {
        var mirchi = (r > 5) ? GetRedMirchi() : GetGreenMirchi();
        $(rateContainer).append(mirchi);
    }*/

    return $(rateContainer).html();
}

function GetRedMirchi() {
    return "<span class='red-mirchi'></span>";
}

function GetGreenMirchi() {
    return "<span class='green-mirchi'></span>";
}