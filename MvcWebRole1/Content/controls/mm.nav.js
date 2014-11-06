/*
    Expected Json Structure
    [
        {   "title" : "Upcoming", "section" : "upcoming" },
        {   "title" : "Now Playing", "section" : "now-playing" }
    ]
*/
var GetNavBar = function (json) {
    if (!json) {
        return "";
    }

    var body = $('html, body');
    var ul = $("<ul/>").addClass("top-nav-bar");
    var navbar = $(".nav-bar-container");

    $.each(json, function (k, v) {

        var li = $("<li/>").text(v.title).attr("link-id", v.section);
        li.click(function () {

            body.animate({
                scrollTop: $("#" + v.section).offset().top - 120
            }, 500);

            navbar.hide();
            return false;
        });

        ul.append(li);
    });

    // Ideally this call should be some place other than here 
    handlePageScrolling();

    return ul;
}

var handlePageScrolling = function () {

    var getFunctionOnScroll = function (eventName, e) {

        var test = (function (eventName) {
            if (eventName === 'mousewheel') {
                return function (e) { return e.originalEvent.detail > 0; };
            } else if (eventName === 'DOMMouseScroll') {
                return function (e) { return e.originalEvent.wheelDelta < 0; };
            }
            return function (e) { return true; };
        })(eventName);

        return function (e) {

            if ($(window).width() > 300) {
                if (test(e)) {
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
                //$(".nav-bar-container").slideUp();
            }

            ClearSearchReults();
        };
    };
};

function ArrangeTopNavLinks() {
    if ($(window).width() > 700) {
        var width = $(document).width() - 100;
        var listCounter = 0;
        var possibleLinks = Math.floor(width / 200);

        $("ul.top-nav-bar").find("li").each(function () {
            if (listCounter > possibleLinks) {
                $(this).hide();
            }

            listCounter++;
        });
    }
}

$(document).ready(function () {
    $(".nav-options").click(function (e) {
        if ($(".nav-bar-container").css("display") == "block") {
            $(".nav-bar-container").css("display", "none");
        }
        else {
            $(".nav-bar-container").css("display", "block");
        }

        e.stopPropagation();
    });

    $(".nav-options").bind('touchstart', function (event) {
        if ($(".nav-bar-container").css("display") == "block") {
            $(".nav-bar-container").css("display", "none");
        }
        else {
            $(".nav-bar-container").css("display", "block");
        }

        event.preventDefault();
    });

    $(".nav-options").hover(function (e) {
        $(".nav-bar-container").css("display", "block");
        e.stopPropagation();
    });

    $(window).resize(function () {
        ArrangeTopNavLinks();
        $(".movie-list ul").each(function () {
            ScaleElement($(this));
        });
    });
});