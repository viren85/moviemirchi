﻿function LoadNews() {
    var newsPath = "/api/News?start=0&page=20";
    CallHandler(newsPath, ShowNews);
}

var ShowNews = function (data) {
    try {
        
        var jdata = JSON.parse(data);
        
        new Util().RemoveLoadImage($("#news-container-tube"));
        if (jdata.Status != undefined || jdata.Status == "Error") {
            $(".news-container").html(jdata.UserMessage);
        }
        else {
            if (data.length < 10) {
                // TODO fix this and few lines below
                ////$(".tweets").parent().hide();
            } else {
                ////$(".tweets").parent().show();
                var news = [];
                for (var v in jdata) {
                    var t = jdata[v];
                    news.push({
                        Link: t.Link,
                        Title: t.Title,
                        Description: t.Desc,
                        Source: t.Source,
                        Image: t.Image,
                    });
                }

                var control = new NewsControl(".news-container", news);
                control.startTimer(10000);
            }
        }
    } catch (e) {
        console.log(e);
        $(".news-container").html("Unable to find news.");
    }

    $(".footer").show();
}

var Timer = function (callback, delay) {
    var timerId;

    this.pause = function () {
        window.clearInterval(timerId);
    };

    this.resume = function () {
        callback();
        timerId = window.setInterval(callback, delay);
    };

    this.resume();
};

var Iterator = function (a, n) {
    var current = 0,
        l = a.length;
    return function () {
        end = current + n;
        var part = a.slice(current, end);
        if (end > l) {
            end = end % l;
            part = part.concat(a.slice(0, end));
        }
        current = end;
        return part;
    };
};

var NewsControl = function (selector, data) {

    var _selector = selector;
    var _data = data;
    var _n = 8; //// TODO: this is workaround for news dupe issue - set _n back to 4
    var _iterator = new Iterator(_data, _n);
    var _cells, _timer;

    // Setup
    (function () {
        $(selector).append(
            "<div class='news-content'>" +
                "<div class='news-left'>" +
                    "<div class=\"tweet-cell\"></div>" +
                    "<div class=\"tweet-cell\"></div>" +
                "</div>" +
                "<div class='news-right'>" +
                    "<div class=\"tweet-cell\"></div>" +
                    "<div class=\"tweet-cell\"></div>" +
                "</div>" +
            "</div>");

        _cells = $(_selector + " .news-content").find(".tweet-cell");

        $(selector).mouseenter(function () {
            if (_timer) {
                _timer.pause();
            }
        });
        $(selector).mouseleave(function () {
            if (_timer) {
                _timer.resume();
            }
        });
    })();

    NewsControl.prototype.render = function () {
        var newsItems = _iterator();

        $.each(newsItems, function (index, entry) {
            //// TODO: this is workaround for news dupe issue - get rid of the index condition
            if (((index % 4) % 2) && entry.Link) {

                var isImageUrl = entry.Image ? true : false;
                var newsTitleText = new Util().GetEllipsisText(entry.Title, 95);

                var newsTextLength = $(window).width() < 320 ? 80 : $(window).width() < 768 ? 110 : 180;

                var html =
                    "<div>" +
                        "<div class='news-title'>" + newsTitleText + "</div>" +
                        "<div class='news-content-container'>" +
                            "<div class='news-content-text'>" +
                                (isImageUrl ? "<div class='left'><img class=\"img\" src=\"" + entry.Image + "\" alt=\"Image\" /></div>" : "") +
                                "<div class='" + (isImageUrl ? "news-right-content" : "both") + "'>" + new Util().GetEllipsisText(entry.Description, newsTextLength) + "</div>" +
                            "</div>" +
                        "</div>" +
                        "<div class='news-author news-link'>" +
                            "<a target=\"_new\" href=\"" + entry.Link.toLowerCase() + "\" onclick=\"trackNewsLink('" + entry.Link + "');\">" +
                                (entry.Source ? entry.Source : "Link") +
                            "</a>" +
                        "</div>" +
                    "</div>";

                //// TODO: this is workaround for news dupe issue - set the index back to index
                var cell = $(_cells[Math.round(index / 2) - 1]);
                var cc = cell.find("div")[0];
                var ael = cc ? $(cc) : cell;

                ael.fadeOut(1000, function () {
                    ael.empty();
                    ael.append(html);
                    ael.fadeIn(2000);
                });
            }
        });
    };

    NewsControl.prototype.startTimer = function (timeout) {
        timeout = (timeout) ? timeout : 6000;
        var threadRender = this.render;
        if (_timer) {
            _timer = null;
        }
        _timer = new Timer(threadRender, timeout);
    };

    NewsControl.prototype.stopTimer = function () {
        if (_timer) {
            clearInterval(_timer);
            _timer = null;
        }
    };

    // First render
    NewsControl.prototype.render();
};