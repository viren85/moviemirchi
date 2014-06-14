function GetRateControl(rate) {
    //var mirchi = (rate > 5) ? GetRedMirchi() : GetGreenMirchi();
    var mirchi = GetMovieRate(Math.round(rate));
    return mirchi;
}

function GetMovieRateControl(rate) {

    var result = "";
    var r = Math.round(rate);
    /*var mirchi = (r > 5) ? GetRedMirchi() : GetGreenMirchi();
    var grayMirchi= (r > 5) ? GetRedGrayMirchi() : GetGreenGrayMirchi();

    for (i = 1; i <= 10; i++) {
        result += i <= r ? mirchi : grayMirchi;
    }*/

    return GetMovieRate(r);
}

function GetMovieRate(rating)
{
    return "<span class='rate rate-" + rating + "'></span>";
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
        if (rate == "" || rate == null || rate == "undefined" || rate == "0") {
            rate = { "criticrating": "60", "teekharating": "7.5", "feekharating": "2.5" };
        }

        var html =
                    "<div class=\"movie-data-row rate-data-row\"><div class=\"rating-container\"><div class=\"liner\">Teekha hai ki feeka hai ?</div>" +
                    "<div class=\"content\">" +
                        "<div class=\"mirchi mirchimeter\">" +
                            GetMovieRateControl(rate.criticrating / 10) +
                        "</div>" +
                        "<div>" +
                            "<div class=\"rate-row\">" +
                                "<span class=\"text\">Critic reviews rating:</span>" +
                                "<span class=\"percent " + (rate.criticrating <= 50 ? 'feeka' : 'teekha') + "\">" + rate.criticrating + "</span></div>" +
                            "<div class=\"rate-row\">" +
                                "<span class=\"text\">Teekha critic reviews:</span>" +
                                "<span class=\"teekha\">" + rate.teekharating + "</span></div>" +
                            "<div class=\"rate-row\">" +
                                "<span class=\"text\">Feeka critic reviews:</span>" +
                                "<span class=\"feeka\">" + rate.feekharating + "</span></div>" +
                        "</div>" +
                    "</div></div></div>";

        return html;
    };
}
