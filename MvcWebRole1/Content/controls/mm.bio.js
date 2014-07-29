var width = $(document).width();
var ShowPersonBio = function (imgPath, name, bioText, affiliation) {
    var bio = "<div class=\"bio\">" +
        //class=\"bio-pic-img\"
        "<div class=\"bio-pic\"><img src=\"" + imgPath + "\" style=\"width: 35px; height: 35px; top: 17%; left: 20%; position: absolute\" onerror=\"new Util().LoadDefaultImage(this,'critic');\" /></div>" +
        "<div class=\"intro\"><b>" + affiliation + "</b>" +
        //"<div>" + bioText + "</div>" +
        "<div class=\"intro-text\">Loading...</div>" +
    "</div>";

    return bio;
}

var InitBio = function () {
    $(".bio-pic").find("img").each(function () {
        var ratio = this.width / this.height;
        var newHeight = 240 * ratio;
        $(this).width("240px").height(newHeight + "px");
        
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