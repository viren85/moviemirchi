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
}
