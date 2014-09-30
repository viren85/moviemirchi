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

    var body = $('html, body');
    body.bind('DOMMouseScroll', getFunctionOnScroll('DOMMouseScroll'));
    //IE, Opera, Safari
    body.bind('mousewheel', getFunctionOnScroll('mousewheel'));
};

$(window).scroll(function () {
    if ($(window).width() > 320) {
        if ($(this).scrollTop() > 40) {
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