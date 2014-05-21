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

    Util.prototype.IsEmailValid = function (emailText) {
        var pattern = /^[a-zA-Z0-9\-_]+(\.[a-zA-Z0-9\-_]+)*@[a-z0-9]+(\-[a-z0-9]+)*(\.[a-z0-9]+(\-[a-z0-9]+)*)*\.[a-z]{2,4}$/;
        if (pattern.test(emailText)) {
            return true;
        } else {
            return false;
        }
    }

    Util.prototype.SetCookie = function (cname, cvalue, exdays) {
        var d = new Date();
        d.setTime(d.getTime() + (exdays * 24 * 60 * 60 * 1000));
        var expires = "expires=" + d.toGMTString();
        document.cookie = cname + "=" + cvalue + "; " + expires;
    }

    Util.prototype.GetCookie = function (cname) {
        var name = cname + "=";
        var ca = document.cookie.split(';');
        for (var i = 0; i < ca.length; i++) {
            var c = ca[i].trim();
            if (c.indexOf(name) == 0) return c.substring(name.length, c.length);
        }
        return "";
    }
}