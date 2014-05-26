function GetRateControl(rate) {
    var mirchi = (rate > 5) ? GetRedMirchi() : GetGreenMirchi();
    return mirchi;
}

function GetMovieRateControl(rate) {

    var result = "";
    var r = Math.round(rate);
    var mirchi = (r > 5) ? GetRedMirchi() : GetGreenMirchi();
    var grayMirchi= (r > 5) ? GetRedGrayMirchi() : GetGreenGrayMirchi();

    for (i = 1; i <= 10; i++) {
        result += i <= r ? mirchi : grayMirchi;
    }

    return result;
}

function GetRedMirchi() {
    return "<span class='red-mirchi'></span>";
}

function GetGreenMirchi() {
    return "<span class='green-mirchi'></span>";
}

function GetRedGrayMirchi() {
    return "<span class='red-gray-mirchi'></span>";
}

function GetGreenGrayMirchi() {
    return "<span class='green-gray-mirchi'></span>";
}

var RatingControl = function () {
    RatingControl.prototype.GetRatingControl = function (rate) {

        // TODO - Remove this if condition, once ewverything is implemented end-end
        if (rate == "" || rate == null || rate == "undefined") {
            rate = { "percent": "60", "theeka": "7.5", "feeka": "2.5" };
        }

        var html =
                    "<div class=\"movie-data-row rate-data-row\"><div class=\"rating-container\"><div class=\"liner\">Theeka hai ki feeka hai ?</div>" +
                    "<div class=\"content\">" +
                        "<div class=\"mirchi mirchimeter\">" +
                            GetMovieRateControl(rate.percent / 10) +
                        "</div>" +
                        "<div>" +
                            "<div class=\"rate-row\">" +
                                "<span class=\"text\">Critic reviews rating:</span>" +
                                "<span class=\"percent " + (rate.percent <= 50 ? 'feeka' : 'theeka') + "\">" + rate.percent + "</span></div>" +
                            "<div class=\"rate-row\">" +
                                "<span class=\"text\">Theeka critic reviews:</span>" +
                                "<span class=\"theeka\">" + rate.theeka + "</span></div>" +
                            "<div class=\"rate-row\">" +
                                "<span class=\"text\">Feeka critic reviews:</span>" +
                                "<span class=\"feeka\">" + rate.feeka + "</span></div>" +
                        "</div>" +
                    "</div></div></div>";

        return html;
    };
}
