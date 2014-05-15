var Util = function () {
    Util.prototype.GetEllipsisText = function (text, len) {
        if (text == null || len == null) {
            return text;
        }
        else if (text == "undefined" || len == "undefined") {
            return text;
        }
        else {
            return (text.length > len) ? (text.substr(0, len) + " [...]") : text;
        }
    }

    Util.prototype.LoadDefaultImage = function (imgElement, type) {
        var path = "";

        switch (type) {
            case "movie":
                path = "/Poster/Images/default-movie.jpg";
                break;
            case "artist":
            case "critic":
                path = "/Images/user.png";
                break;
        }

        $(imgElement).attr("src", path).css("height", "250px");
    }
}
