/*
    Expected Json Structure
    [
        {   "title" : "Upcoming", "section" : "upcoming" },
        {   "title" : "Now Playing", "section" : "now-playing" }
    ]
*/
var GetNavBar = function (json) {
    if (json == null || json == "undefined")
        return;

    var list = $("<ul/>").attr("class", "top-nav-bar");

    for (i = 0; i < json.length; i++) {
        var item = $("<li/>").attr("link-id", json[i].section).html(json[i].title).click(function () {
            $('html, body').animate({
                scrollTop: $("#" + $(this).attr('link-id')).offset().top - 120
            }, 500);
            return false;
        });

        $(list).append(item);
    }

    $('html, body').bind('DOMMouseScroll', function (e) {

        if ($(window).width() > 300) {
            if (e.originalEvent.detail > 0) {
                //$(".nav-bar-container").slideUp();
            } else {

                /*$("ul.top-nav-bar").find("li").each(function () {
                     $(this).show();
                });*/
                ArrangeTopNavLinks();

                //$(".nav-bar-container").slideDown();
            }
        }
        else {
            $(".nav-bar-container").slideUp();
        }

        ClearSearchReults();
    });

    //IE, Opera, Safari
    $('html, body').bind('mousewheel', function (e) {
        if ($(window).width() > 300) {
            if (e.originalEvent.wheelDelta < 0) {
                //$(".nav-bar-container").slideUp();
            } else {
                /*$("ul.top-nav-bar").find("li").each(function () {
                    $(this).show();
                });*/

                ArrangeTopNavLinks();
                //$(".nav-bar-container").slideDown();
            }
        }
        else {
            $(".nav-bar-container").slideUp();
        }

        ClearSearchReults();
    });

    $(window).scroll(function () {
        if ($(window).width() > 320) {
            if ($(this).scrollTop() > 70) {
                $(".small-logo img").css("width", "40px").css("height", "60px");
                $('.scrollToTop').fadeIn();
            } else {
                $(".small-logo img").css("width", "70px").css("height", "120px");
            }
        }
        else {
            if ($(this).scrollTop() > 70) {
                $(".small-logo img").css("width", "40px").css("height", "60px");
                $('.scrollToTop').fadeIn();
            } else {
                $(".small-logo img").css("width", "50px").css("height", "95px");
            }
        }
    });

    return list;
}