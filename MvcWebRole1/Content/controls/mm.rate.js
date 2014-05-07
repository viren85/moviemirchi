function GetRateControl(rate) {
    var mirchi = (rate > 5) ? GetRedMirchi() : GetGreenMirchi();
    return mirchi;
}

function GetMovieRateControl(rate) {

    var result = "";
    var r = Math.round(rate);
    var mirchi = (r > 5) ? GetRedMirchi() : GetGreenMirchi();
    for (i = 1; i <= r; i++) {
        result += mirchi;
    }

    return result;
}

function GetRedMirchi() {
    return "<span class='red-mirchi'></span>";
}

function GetGreenMirchi() {
    return "<span class='green-mirchi'></span>";
}