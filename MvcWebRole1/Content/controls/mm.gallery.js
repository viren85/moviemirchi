var ArrangeImages = function (container) {
    //var width = $(document).width();
    $(container).find("img").each(function () {
        var ratio = this.width / this.height;
        var newWidth = 200 * ratio;
        $(this).width(newWidth + "px").height("200px");
    });
}