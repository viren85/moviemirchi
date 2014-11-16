//var width = $(document).width();
var ShowPersonBio = function (affiliation) {
    var bio = "<div class=\"bio\">" +
        "<div class=\"bio-pic\"></div>" +
        "<div class=\"intro\"><b>" + affiliation + "</b>" +
        "<div class=\"intro-text\"></div>" +
    "</div>";

    return bio;
}

var InitBio = function () {
    $(".bio-pic").find("img").each(function () {
        var ratio = this.width / this.height;
        var newWidth = 250 * ratio;
        $(this).height("250px").width(newWidth + "px");
        
        /*$(this).hover(
            function () {
                if ($(this).attr("src").indexOf("user.png") < 0) {
                    $(this).attr("org-w", $(this).css("width")).css("width", "350px").css("top", "-50px").css("left", "-50px");
                    $(this).attr("org-h", $(this).css("height")).css("height", "500px");
                    $(this).css("z-index", "100").css("position", "absolute");
                    $(this).parent().css("height", "351px").css("z-index", "100");
                }
            },
            function () {
                if ($(this).attr("src").indexOf("user.png") < 0) {
                    $(this).css("width", $(this).attr("org-w")).css("height", $(this).attr("org-h")).css("z-index", "1").css("top", "0px").css("left", "0px");
                    $(this).parent().css("height", $(this).attr("org-h")).css("z-index", "1");
                }
            }
       );*/
    });
}