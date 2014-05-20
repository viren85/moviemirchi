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

    // This function returns the total number of elements possible to be displayed in possible width area
    // The input argument is container width in %
    Util.prototype.GetElementCount = function (containerWidth, singleElementWidth) {
        var width = $(document).width();
        var singleWidth = Math.round(width * (containerWidth / 100));
        return Math.floor(singleWidth / singleElementWidth) - 1;
    }
}
