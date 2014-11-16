var LoadMovieTube = function (movies, tubeTitle) {
    var tube = "<div class=\"tube-tilte\">" + tubeTitle + "</div><div class=\"movie-tube-container\">" +
                    "<div class=\"tube-left-nav\"><img src=\"/images/gtk_media_forward_rtl.png\" /></div>" +
                    "<div class=\"tube-right-nav\"><img src=\"/images/gtk_media_forward_ltr.png\" /></div>" +
                    "<div class=\"tube-tile-container\">";

    $.each(movies, function () {
        try {
            var poster = JSON.parse(this.Posters);
            var src = (poster != null && poster.length > 0) ? PUBLIC_BLOB_URL + poster[poster.length - 1] : PUBLIC_BLOB_URL + "default-movie.jpg";
            tube += "<a href=\"/movie/" + this.UniqueName + "\"><img id=\"" + this.UniqueName + "\" title=\"" + this.Name + "\" src=\"" + src + "\" alt=\"" + this.Name + "\" style=\"width: 200px; height: 250px;float: left; margin-left: 10px;\" /></a>";
        }
        catch (e) {
            //alert(e.message);
        }
    });

    tube += "</div>";
    return tube;
}

var LoadPhotoTube = function (photos, tubeTitle, pics) {
    var tube = "<div class=\"tube-tilte\">" + tubeTitle + "</div><div class=\"photo-tube-container gallery\">" +
                    "<div class=\"tube-left-nav\"><img src=\"/images/gtk_media_forward_rtl.png\" /></div>" +
                    "<div class=\"tube-right-nav\"><img src=\"/images/gtk_media_forward_ltr.png\" /></div>" +
                    "<div class=\"tube-tile-container\">";

    var pictures = [];
    if (pics && pics != "") {
        pictures = JSON.parse(pics);
    }

    for (i = 0; i < photos.length; i++) {
        try {
            var source = "";

            if (pictures.length == 0 || pictures[i] == null || pictures[i].source == null || pictures[i].source == "undefined" || pictures[i].source == "") {
                source = "<span class=\"source\">Source: IMDB</span>";
            } else {
                source = "<span class=\"source\">Source: Santa Banta</span>";
            }

            tube += "<a href=\"" + PUBLIC_BLOB_URL + photos[i] + "\" rel=\"prettyPhoto[gallery]\"><img src=\"" + PUBLIC_BLOB_URL + photos[i] + "\" style=\"width: 200px; height: 250px;float: left; margin-left: 10px;\" />" + source + "</a>";

        }
        catch (e) {
            alert(e);
        }
    }

    if (photos.length == 1)
        $(".movie-photos").remove();

    tube += "</div>";

    return tube;
}

var LoadTrailerTube = function (videos, tubeTitle) {
    var tube = "<div class=\"trailer-tube-container\">" +
                    "<div class=\"tube-left-nav\"><img src=\"/images/gtk_media_forward_rtl.png\" /></div>" +
                    "<div class=\"tube-right-nav\"><img src=\"/images/gtk_media_forward_ltr.png\" /></div>" +
                    "<div class=\"tube-tile-container\"><ul>";

    for (i = 0; i < videos.length; i++) {
        var t = "<li data-toggle=\"modal\" data-target=\"#modal-video\" onclick=\"DisplayModal('" + videos[i].YoutubeURL.trim() + "?autoplay=1');\" class=\"song\" video-link=\"" + videos[i].YoutubeURL.trim() + "?autoplay=1\" title=\"Play YouTube Trailer - " + videos[i].Title + "\"><img class=\"song-thumb\" src=\"" + videos[i].Thumb + "\" /><img src=\"../images/play-video.png\" title=\"Play YouTube Trailer\" class=\"song-play\" video-link=\"" + videos[i].YoutubeURL.trim() + "?autoplay=1\" /><span title=\"" + videos[i].Title + "\">" + new Util().GetEllipsisText(videos[i].Title, 16) + "</span></li>";
        tube += t;
    }

    tube += "</ul></div>";
    return tube;
}

var LoadSongTube = function (songs, tubeTitle) {
    var tube = "<div class=\"song-tube-container\">" +
                    "<div class=\"tube-left-nav\"><img src=\"/images/gtk_media_forward_rtl.png\" /></div>" +
                    "<div class=\"tube-right-nav\"><img src=\"/images/gtk_media_forward_ltr.png\" /></div>" +
                    "<div class=\"tube-tile-container\"><ul>";

    for (i = 0; i < songs.length; i++) {
        var t = "<li data-toggle=\"modal\" data-target=\"#modal-video\" onclick=\"DisplayModal('" + songs[i].YoutubeURL.trim() + "?autoplay=1');\" class=\"song\" video-link=\"" + songs[i].YoutubeURL.trim() + "?autoplay=1\" title=\"Play Song - " + songs[i].SongTitle + "\"><img class=\"song-thumb\" src=\"" + songs[i].Thumb + "\" /><img src=\"../images/play-video.png\" title=\"Play YouTube Trailer\" class=\"song-play\" video-link=\"" + songs[i].YoutubeURL.trim() + "?autoplay=1\" /><span title=\"" + songs[i].SongTitle + "\">" + new Util().GetEllipsisText(songs[i].SongTitle, 16) + "</span></li>";
        tube += t;
    }

    tube += "</ul></div>";
    return tube;
}

var LoadReviewsTube = function (reviews, tubeTitle) {
    var tube = "<div class=\"review-tube-container\">" +
                    "<div class=\"tube-left-nav\"><img src=\"/images/gtk_media_forward_rtl.png\" /></div>" +
                    "<div class=\"tube-right-nav\"><img src=\"/images/gtk_media_forward_ltr.png\" /></div>" +
                    "<div class=\"tube-tile-container\"><ul>";

    reviews.forEach(function (review) {
        try {
            if (review.OutLink) {

                var reviewText = review.Tags ? review.Tags : new Util().GetEllipsisText(review.Review, 200);

                tube +=
                    "<li class=\"arrow_container\">" +
                        "<div class=\"left\">" +
                            "<div class=\"info\">" +
                                "<div class=\"reviewer\">" +
                                    "<a href=\"/reviewer/" + FormPathFromName(review.ReviewerName) + "\">" +
                                        "<img src=\"" + GetReviewerPic(review.ReviewerName) + "\" style=\"height:100px;width:100px\" onerror=\"this.src='" + PUBLIC_BLOB_URL + "default-movie.jpg'\" />" +
                                    "</a>" +
                                    "<div class=\"reviewer-name\"><a href=\"/reviewer/" + FormPathFromName(review.ReviewerName) + "\" title=\"" + review.ReviewerName + "\">" + new Util().GetEllipsisText(review.ReviewerName, 20) + "</a></div>" +
                                    "<div class=\"affiliation\">" + review.Affiliation + "</div>" +
                                    "<div class=\"mirchimeter\">" + (review.CriticsRating ? GetRateControl(review.CriticsRating / 10) : "") + "</div>" +
                                "</div>" +
                            "</div>" +
                        "</div>" +
                        "<div class=\"right\">" +
                            "<div class=\"review\">" +
                                "<div class=\"arrow_box\">" +
                                    "<div class=\"review-content\">" +
                                        "<blockquote class=\"quote\">" + reviewText + "</blockquote>" +
                                            "<div class=\"more-link\"><a target=\"_new\" href=\"" + review.OutLink.toLowerCase() + "\"  onclick=\"trackReviewLink('" + review.OutLink.toLowerCase() + "');\">More...</a></div>" +
                                    "</div>" +
                                "</div>" +
                            "</div>" +
                        "</div>" +
                    "</li>";
            }
        } catch (e) {

        }
    });

    tube += "</ul></div>";
    return tube;
}

var LoadCriticReviewsTube = function (reviews, tubeTitle) {
    var tube = "<div class=\"tube-tilte\">" + tubeTitle + "</div><div class=\"review-tube-container\">" +
                    "<div class=\"tube-left-nav\"><img src=\"/images/gtk_media_forward_rtl.png\" /></div>" +
                    "<div class=\"tube-right-nav\"><img src=\"/images/gtk_media_forward_ltr.png\" /></div>" +
                    "<div class=\"tube-tile-container\"><ul>";

    reviews.forEach(function (review) {
        try {
            if (review.OutLink) {

                var reviewText = review.Tags ? review.Tags : new Util().GetEllipsisText(review.Review, 200);
                var posters = JSON.parse(review.MoviePoster);
                tube +=
                    "<li class=\"arrow_container\">" +
                        "<div class=\"left\">" +
                            "<div class=\"info\">" +
                                "<div class=\"reviewer\">" +
                                    "<a href=\"/movie/" + ShowPath(review.MovieName) + "\">" +
                                        "<img src=\"" + PUBLIC_BLOB_URL + posters[posters.length - 1] + "\" style=\"height:150px;width:100px\" onerror=\"this.src='" + PUBLIC_BLOB_URL + "default-movie.jpg'\" />" +
                                    "</a>" +
                                    "<div class=\"reviewer-name\"><a href=\"/movie/" + ShowPath(review.MovieName) + "\" title=\"" + review.MovieName + "\">" + new Util().GetEllipsisText(review.MovieName, 20) + "</a></div>" +
                                    "<div class=\"mirchimeter\">" + (review.CriticsRating ? GetRateControl(review.CriticsRating / 10) : "") + "</div>" +
                                "</div>" +
                            "</div>" +
                        "</div>" +
                        "<div class=\"right\">" +
                            "<div class=\"review\">" +
                                "<div class=\"arrow_box\">" +
                                    "<div class=\"review-content\">" +
                                        "<blockquote class=\"quote\">" + reviewText + "</blockquote>" +
                                            "<div class=\"more-link\"><a target=\"_new\" href=\"" + review.OutLink.toLowerCase() + "\"  onclick=\"trackReviewLink('" + review.OutLink.toLowerCase() + "');\">More...</a></div>" +
                                    "</div>" +
                                "</div>" +
                            "</div>" +
                        "</div>" +
                    "</li>";
            }
        } catch (e) {

        }
    });

    tube += "</ul></div>";
    return tube;
}


function InitMovieTube(container) {
    // calculating width of parent for the placement of navigation div
    var rightParentWid = parseInt($('.tube-right-nav').parent().outerWidth());
    var rightWid = parseInt($('.tube-right-nav').outerWidth()) / 1.2;
    // .tube-right-navositioning right navigation div

    $('.tube-right-nav').css({ 'left': rightParentWid + rightWid + 50 + 'px' });
    // Code to run on mouse hover on parent div
    $(container).hover(function () {
        $(container + ' .tube-left-nav, ' + container + ' .tube-right-nav').animate({ 'opacity': '0.5' }, { duration: 500, queue: true });
    }, function () {
        $(container + ' .tube-left-nav, ' + container + ' .tube-right-nav').animate({ 'opacity': '0' }, { duration: 500, queue: true });
    });
    // variables to hold id of set Interval so we can call clear Interval for those IDs
    var rightVar, leftVar;
    // setting interval for starting animation again on while mouse is over right div
    $(container + ' .tube-right-nav').stop(true, true).hover(function () {
        rightVar = setInterval(function () { rightHover(container); }, 1000);
    }, function () {
        clearInterval(rightVar);
    });
    // setting interval for starting animation again on while mouse is over left div
    $(container + ' .tube-left-nav').stop(true, true).hover(function () {
        leftVar = setInterval(function () { leftHover(container); }, 1000);
    }, function () {
        clearInterval(leftVar);
    });
    // function for right div mouse hover
    function rightHover(container) {
        // here we are checking the left of child so we can run our animation until it reaches to last image
        // if two gets equal then this will reverse the image back to start position so animation can run from beginning
        if (parseInt($(container + ' .tube-tile-container').css('left')) >= ($(container + ' .tube-tile-container').parent().width() - $(container + ' .tube-tile-container').width() + 70)) {
            // to move images to left we will set left property of child div to negative direction
            $(container + ' .tube-tile-container').stop(true).animate({ 'left': '-=' + $(container + ' .tube-tile-container img').width() }, 500);
        } else {
            $(container + ' .tube-tile-container').stop(true).animate({ 'left': 0 }, 500);
        }
    }

    function leftHover(container) {
        if ((parseInt($(container + ' .tube-tile-container').css('left')) <= 0) && ($(container + ' .tube-tile-container').css('left') != 'auto')) {
            // to move images to right we will set left property of child div to positive direction
            $(container + ' .tube-tile-container').stop(true).animate({ 'left': '+=' + $(container + ' .tube-tile-container img').width() }, 500);
        }
        else {
            $(container + ' .tube-tile-container').stop(true).animate({ 'left': $(container + ' .tube-tile-container').parent().width() - $(container + ' .tube-tile-container').width() }, 500);
        }
    }
}

function InitTrailerTube(container) {
    // calculating width of parent for the placement of navigation div
    var rightParentWid = parseInt($('.tube-right-nav').parent().outerWidth());
    var rightWid = parseInt($('.tube-right-nav').outerWidth()) / 1.2;
    // .tube-right-navositioning right navigation div

    $('.tube-right-nav').css({ 'left': rightParentWid + rightWid + 50 + 'px' });
    // Code to run on mouse hover on parent div
    $(container).hover(function () {
        $(container + ' .tube-left-nav, ' + container + ' .tube-right-nav').animate({ 'opacity': '0.5' }, { duration: 500, queue: true });
    }, function () {
        $(container + ' .tube-left-nav, ' + container + ' .tube-right-nav').animate({ 'opacity': '0' }, { duration: 500, queue: true });
    });
    // variables to hold id of set Interval so we can call clear Interval for those IDs
    var rightVar, leftVar;
    // setting interval for starting animation again on while mouse is over right div
    $(container + ' .tube-right-nav').stop(true, true).hover(function () {
        rightVar = setInterval(function () { rightHover(container); }, 1000);
    }, function () {
        clearInterval(rightVar);
    });
    // setting interval for starting animation again on while mouse is over left div
    $(container + ' .tube-left-nav').stop(true, true).hover(function () {
        leftVar = setInterval(function () { leftHover(container); }, 1000);
    }, function () {
        clearInterval(leftVar);
    });
    // function for right div mouse hover
    function rightHover(container) {
        // here we are checking the left of child so we can run our animation until it reaches to last image
        // if two gets equal then this will reverse the image back to start position so animation can run from beginning
        if (parseInt($(container + ' .tube-tile-container').css('left')) >= ($(container + ' .tube-tile-container').parent().width() - $(container + ' .tube-tile-container').width() + 70)) {
            // to move images to left we will set left property of child div to negative direction
            $(container + ' .tube-tile-container').stop(true).animate({ 'left': '-=' + $(container + ' .tube-tile-container img').width() }, 500);
        } else {
            $(container + ' .tube-tile-container').stop(true).animate({ 'left': 0 }, 500);
        }
    }

    function leftHover(container) {
        if ((parseInt($(container + ' .tube-tile-container').css('left')) <= 0) && ($(container + ' .tube-tile-container').css('left') != 'auto')) {
            // to move images to right we will set left property of child div to positive direction
            $(container + ' .tube-tile-container').stop(true).animate({ 'left': '+=' + $(container + ' .tube-tile-container img').width() }, 500);
        }
        else {
            $(container + ' .tube-tile-container').stop(true).animate({ 'left': $(container + ' .tube-tile-container').parent().width() - $(container + ' .tube-tile-container').width() }, 500);
        }
    }
}

jQuery.extend(jQuery.easing,
{
    easeInQuad: function (x, t, b, c, d) {
        return c * (t /= d) * t + b;
    },
    easeOutQuad: function (x, t, b, c, d) {
        return -c * (t /= d) * (t - 2) + b;
    },
    easeInOutQuad: function (x, t, b, c, d) {
        if ((t /= d / 2) < 1) return c / 2 * t * t + b;
        return -c / 2 * ((--t) * (t - 2) - 1) + b;
    },
    easeInCubic: function (x, t, b, c, d) {
        return c * (t /= d) * t * t + b;
    },
    easeOutCubic: function (x, t, b, c, d) {
        return c * ((t = t / d - 1) * t * t + 1) + b;
    },
    easeInOutCubic: function (x, t, b, c, d) {
        if ((t /= d / 2) < 1) return c / 2 * t * t * t + b;
        return c / 2 * ((t -= 2) * t * t + 2) + b;
    },
    easeInQuart: function (x, t, b, c, d) {
        return c * (t /= d) * t * t * t + b;
    },
    easeOutQuart: function (x, t, b, c, d) {
        return -c * ((t = t / d - 1) * t * t * t - 1) + b;
    },
    easeInOutQuart: function (x, t, b, c, d) {
        if ((t /= d / 2) < 1) return c / 2 * t * t * t * t + b;
        return -c / 2 * ((t -= 2) * t * t * t - 2) + b;
    },
    easeInQuint: function (x, t, b, c, d) {
        return c * (t /= d) * t * t * t * t + b;
    },
    easeOutQuint: function (x, t, b, c, d) {
        return c * ((t = t / d - 1) * t * t * t * t + 1) + b;
    },
    easeInOutQuint: function (x, t, b, c, d) {
        if ((t /= d / 2) < 1) return c / 2 * t * t * t * t * t + b;
        return c / 2 * ((t -= 2) * t * t * t * t + 2) + b;
    },
    easeInSine: function (x, t, b, c, d) {
        return -c * Math.cos(t / d * (Math.PI / 2)) + c + b;
    },
    easeOutSine: function (x, t, b, c, d) {
        return c * Math.sin(t / d * (Math.PI / 2)) + b;
    },
    easeInOutSine: function (x, t, b, c, d) {
        return -c / 2 * (Math.cos(Math.PI * t / d) - 1) + b;
    },
    easeInExpo: function (x, t, b, c, d) {
        return (t == 0) ? b : c * Math.pow(2, 10 * (t / d - 1)) + b;
    },
    easeOutExpo: function (x, t, b, c, d) {
        return (t == d) ? b + c : c * (-Math.pow(2, -10 * t / d) + 1) + b;
    },
    easeInOutExpo: function (x, t, b, c, d) {
        if (t == 0) return b;
        if (t == d) return b + c;
        if ((t /= d / 2) < 1) return c / 2 * Math.pow(2, 10 * (t - 1)) + b;
        return c / 2 * (-Math.pow(2, -10 * --t) + 2) + b;
    },
    easeInCirc: function (x, t, b, c, d) {
        return -c * (Math.sqrt(1 - (t /= d) * t) - 1) + b;
    },
    easeOutCirc: function (x, t, b, c, d) {
        return c * Math.sqrt(1 - (t = t / d - 1) * t) + b;
    },
    easeInOutCirc: function (x, t, b, c, d) {
        if ((t /= d / 2) < 1) return -c / 2 * (Math.sqrt(1 - t * t) - 1) + b;
        return c / 2 * (Math.sqrt(1 - (t -= 2) * t) + 1) + b;
    },
    easeInElastic: function (x, t, b, c, d) {
        var s = 1.70158; var p = 0; var a = c;
        if (t == 0) return b; if ((t /= d) == 1) return b + c; if (!p) p = d * .3;
        if (a < Math.abs(c)) { a = c; var s = p / 4; }
        else var s = p / (2 * Math.PI) * Math.asin(c / a);
        return -(a * Math.pow(2, 10 * (t -= 1)) * Math.sin((t * d - s) * (2 * Math.PI) / p)) + b;
    },
    easeOutElastic: function (x, t, b, c, d) {
        var s = 1.70158; var p = 0; var a = c;
        if (t == 0) return b; if ((t /= d) == 1) return b + c; if (!p) p = d * .3;
        if (a < Math.abs(c)) { a = c; var s = p / 4; }
        else var s = p / (2 * Math.PI) * Math.asin(c / a);
        return a * Math.pow(2, -10 * t) * Math.sin((t * d - s) * (2 * Math.PI) / p) + c + b;
    },
    easeInOutElastic: function (x, t, b, c, d) {
        var s = 1.70158; var p = 0; var a = c;
        if (t == 0) return b; if ((t /= d / 2) == 2) return b + c; if (!p) p = d * (.3 * 1.5);
        if (a < Math.abs(c)) { a = c; var s = p / 4; }
        else var s = p / (2 * Math.PI) * Math.asin(c / a);
        if (t < 1) return -.5 * (a * Math.pow(2, 10 * (t -= 1)) * Math.sin((t * d - s) * (2 * Math.PI) / p)) + b;
        return a * Math.pow(2, -10 * (t -= 1)) * Math.sin((t * d - s) * (2 * Math.PI) / p) * .5 + c + b;
    },
    easeInBack: function (x, t, b, c, d, s) {
        if (s == undefined) s = 1.70158;
        return c * (t /= d) * t * ((s + 1) * t - s) + b;
    },
    easeOutBack: function (x, t, b, c, d, s) {
        if (s == undefined) s = 1.70158;
        return c * ((t = t / d - 1) * t * ((s + 1) * t + s) + 1) + b;
    },
    easeInOutBack: function (x, t, b, c, d, s) {
        if (s == undefined) s = 1.70158;
        if ((t /= d / 2) < 1) return c / 2 * (t * t * (((s *= (1.525)) + 1) * t - s)) + b;
        return c / 2 * ((t -= 2) * t * (((s *= (1.525)) + 1) * t + s) + 2) + b;
    },
    easeInBounce: function (x, t, b, c, d) {
        return c - jQuery.easing.easeOutBounce(x, d - t, 0, c, d) + b;
    },
    easeOutBounce: function (x, t, b, c, d) {
        if ((t /= d) < (1 / 2.75)) {
            return c * (7.5625 * t * t) + b;
        } else if (t < (2 / 2.75)) {
            return c * (7.5625 * (t -= (1.5 / 2.75)) * t + .75) + b;
        } else if (t < (2.5 / 2.75)) {
            return c * (7.5625 * (t -= (2.25 / 2.75)) * t + .9375) + b;
        } else {
            return c * (7.5625 * (t -= (2.625 / 2.75)) * t + .984375) + b;
        }
    },
    easeInOutBounce: function (x, t, b, c, d) {
        if (t < d / 2) return jQuery.easing.easeInBounce(x, t * 2, 0, c, d) * .5 + b;
        return jQuery.easing.easeOutBounce(x, t * 2 - d, 0, c, d) * .5 + c * .5 + b;
    }
});