function GetRateControl(rate) {
    var mirchi = GetMovieRate(Math.round(rate));
    return mirchi;
}

function GetMovieRateControl(rate, rateValue) {
    var result = "";
    var r = Math.round(rate);

    return GetMovieRate(r, rateValue);
}

function GetMovieRate(rating, rateValue) {
    // TODO: Get rid of return once we want to show mirchi
    return "<span />";

    if (rating == undefined || rating < 0) {
        return ""
    } else if (rateValue == undefined || rateValue == null) {
        return "<span class='rate rate-" + rating + "'></span>";
    } else {
        return
        "<span itemprop=\"aggregateRating\" itemscope itemtype=\"http://schema.org/AggregateRating\" class='rate rate-" + rating + "'></span>" +
        "<meta itemprop=\"ratingValue\" content=\"" + rateValue + "\">" +
        "<span class='rate-value'>" + rateValue + "</span>";
    }
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
    RatingControl.prototype.GetRatingControl = function (rate, movie) {

        // TODO - Remove this if condition, once ewverything is implemented end-end
        if (typeof rate === "undefined" || rate === "" || rate === null || rate === "undefined" || rate === "0" || rate === 0) {
            rate = { "criticrating": "", "teekharating": "", "feekharating": "" };
        }

        var hideContentControl = "";
        if (typeof rate.criticrating === "undefined" || rate.criticrating === ""
            || typeof rate.feekharating === "undefined" || rate.feekharating === ""
            || typeof rate.teekharating === "undefined" || rate.teekharating === "") {
            hideContentControl = "style=\"display:none\"";
        }

        var html =
                    "<div class=\"movie-data-row rate-data-row\"><div class=\"rating-container\"><div class=\"liner\">Teekha hai ki feeka hai ?</div>" +
                    "<div class=\"content\" " + hideContentControl + " style=\"display: none\">" +
                        "<div class=\"mirchi mirchimeter\">" +
                            GetMovieRateControl(rate.criticrating / 10, movie.Rating) +
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
