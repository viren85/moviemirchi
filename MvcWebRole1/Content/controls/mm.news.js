function LoadNews() {
    var newsPath = "../api/News?start=0&page=20";
    CallHandler(newsPath, ShowNews);
}

var ShowNews = function (data) {
    var jdata = JSON.parse(data);

    var news = [];
    for (var v in jdata) {
        var t = jdata[v];
        news.push({
            title: (t.Title || ""),
            description: (t.Description || ""),
        });
    }
    
    var ne = new NewsControl(".news-container", news);
    ne.startTimer(12000);
}

var newsData;

var iterator = function (a, n) {
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
    var _n = 4;
    var _iterator = new iterator(_data, _n);
    var _cells, _timer;

    // Setup
    (function () {
        $(selector).html(
            "<div class='tweet-container'>" +
                "<div class='tweet-left'></div>" +
                "<div class='tweet-right'></div>" +
            "</div>");

        var left = $(_selector + " .tweet-container .tweet-left");
        var right = $(_selector + " .tweet-container .tweet-right");

        var html =
            "<div class=\"tweet-cell\">" +
                "<div class=\"tweet-user\"></div>" +
                "<div class=\"tweet-content\"></div>" +
            "</div>";
        left.append(html).append(html);
        right.append(html).append(html);

        _cells = $(_selector + " .tweet-container").find(".tweet-cell");
    })();

    NewsControl.prototype.render = function () {
        var newsItems = _iterator();
        $.each(newsItems, function (index, n) {
            var cell = $(_cells[index]);
            var children = cell.children();
            children.fadeOut(1000, function () {
                var txt = n.description.length > 100 ? n.description.substring(0, 100) + "..." : n.description;
                cell.find(".tweet-user").text(n.title);
                cell.find(".tweet-content").text(txt);
                children.fadeIn(2000);
            });
        });
    };

    NewsControl.prototype.startTimer = function (timeout) {
        timeout = (timeout) ? timeout : 6000;
        var threadRender = this.render;
        _timer = setInterval(function () {
            threadRender();
        }, timeout);
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
/*((function ShowNewsControl(feedUrls, selector) {

    var render = function (entries) {

        entries.forEach(function (entry) {

            var getValue = function (k) {
                var el = entry[k];
                return el ? el : "";
            };

            var isOK = function (v) {
                return (v && v !== "");
            };

            var link = getValue("link");
            if (isOK(link)) {

                var getSpan = function (k, f) {
                    var v = getValue(k);
                    var vv = isOK(v) ? f ? f(v) : v : null;
                    return isOK(vv) ? "<span>" + vv + "</span>" : "";
                };

                var getUrl = function () {
                    var m = entry["mediaGroups"];
                    if (m && m.length > 0) {
                        var cg = m[0]["contents"];
                        if (cg && cg.length > 0) {
                            var c = cg[0];
                            var u = isOK(c["url"]) ? c["url"] : null;
                            u = ((isOK(c["type"]) ? c["type"] : "").indexOf("image") != -1) ? u : null;
                            return u;
                        }
                    }
                    return null;
                };

                var url = getUrl();
                var isUrl = isOK(url);
                var author = getSpan("author");
                var newsTitleSpan = getSpan("title");
                var newsTitleText = $(newsTitleSpan).text();
                var publishDateClass = 'news-publish-date';
                // When news title is greater than specific length, it shall span over multiple lines.
                // The overlap results in to publish date beneath title bar.  
                if (newsTitleText.length > 48) {
                    // Set the margin for
                    publishDateClass = 'news-publish-date news-publish-date-2';
                } else if (newsTitleText.length > 95) {
                    $(newsTitleSpan).html(newsTitleText.substr(0, 95) + "...");
                }

                var html =
                "<li class='news-item'>" +
                    "<div class='news-title'>" + newsTitleSpan + "</div>" +
                    "<div class='news-content-container'>" +
                        "<div class='" + publishDateClass + "'>" +
                            getSpan("publishedDate", function (v) { return "Published on: " + new Date(v).toLocaleString(); }) +
                        "</div>" +
                        "<div class='news-content'>" +
                            (isUrl ? "<div class='left'><img class=\"img\" src=\"" + url + "\" alt=\"Image\" /></div>" : "") +
                            "<div class='" + (isUrl ? "news-right" : "both") + "'>" + getSpan("contentSnippet") + "</div>" +
                        "</div>" +
                        "<div class='news-author news-link'><a target=\"_new\" href=\"" + link + "\">" +
                            (isOK(author) ? author : "Link") +
                        "</a></div>" +
                    "</div>" +
                "</li>";

                $(selector).append(html);
            }
        });

        // Adding the pager
        // TODO: here .parent() is hardcoded - that should be removed
        PreparePaginationControl($(selector).parent(), {
            "pagerContainer": "news-pager",
            "tilesInPage": 3,
            "totalTileCount": $(selector).find("li.news-item").length,
            "pagerContainerId": "news-pager",
        });

        entries = null;
    };

    var displayError = function (v) {
        var html =
            "<div class='entryContainer'><span class='error'>" + v + "</span></div>";
        $(selector).append(html);
    };

    var entries = [];
    var accumulate = function (v) {
        v.forEach(function (f) {
            entries.push(f);
        });

        if (startingpoint.deferred.state() === "resolved") {
            complete();
        }
    };
    var sort = function () {
        entries.sort(function (a, b) {
            return new Date(b.publishedDate) - new Date(a.publishedDate);
        });
    };
    var complete = function () {
        if (entries && entries.length > 0) {
            sort();
            render(entries);
        } else {
            displayError("We are unable to show news at this time");
        }
    };

    function DeferredAjax(opts) {
        this.deferred = $.Deferred();
        this.feedUrl = opts.feedUrl;
    }

    DeferredAjax.prototype.invoke = function () {
        var self = this;
        return $.ajax({
            type: "GET",
            url: 'http://ajax.googleapis.com/ajax/services/feed/load?v=1.0&num=10&callback=?&q=' + encodeURIComponent(self.feedUrl),
            dataType: "JSON",
            success: function (data) {
                self.deferred.resolve();
                if (data && data.responseData && data.responseData.feed && data.responseData.feed.entries) {
                    accumulate(data.responseData.feed.entries);
                }
            }
        });
    };

    DeferredAjax.prototype.promise = function () {
        return this.deferred.promise();
    };

    var startingpoint = $.Deferred();
    startingpoint.resolve();

    $.each(feedUrls, function (ix, feedUrl) {
        var da = new DeferredAjax({
            feedUrl: feedUrl
        });
        $.when(startingpoint)
            .then(function () {
                da.invoke();
            });
        startingpoint = da;
    });

})([
            "http://www.bollywoodnewsworld.com/category/bollywood-news/feed",
            "http://www.glamsham.com/rss/glamrss_scoops.xml",
            "http://www.bollywoodhungama.com/rss/news.xml",
            "http://feeds.hindustantimes.com/HT-Bollywood"
], ".news-container ul"));
*/