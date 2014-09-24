function LoadTweets(type, name) {
    $(".tweets").parent().hide();
    if (type == null || type == "undefined" || name == null || name == "undefined") {
        var tweetPath = "/api/Twitter?start=0&page=20";
        CallHandler(tweetPath, ShowTweets);
    }
    else {
        var tweetPath = "/api/Twitter?start=0&page=20&type=" + type + "&name=" + name;
        CallHandler(tweetPath, ShowTweets);
    }
}

var ShowTweets = function (data) {    
    try {
        var jdata = JSON.parse(data);
        new Util().RemoveLoadImage($("#tweets-tube"));

        if (jdata.Status != undefined || jdata.Status == "Error") {
            $(".tweets").html(jdata.UserMessage);
        }
        else {

            if (data.length < 10) {
                $(".tweets").parent().hide();
            }
            else {
                $(".tweets").parent().show();
                var tweets = [];
                for (var v in jdata) {
                    var t = jdata[v];
                    tweets.push({
                        twitterid: "@" + (t.ReplyScreenName || ""),
                        text: (t.TextMessage || ""),
                    });
                }

                var twtr = new TwitterControl(".tweets", tweets);
                twtr.startTimer(8000);
            }
        }

    } catch (e) {
        $(".tweets").html("Unable to get tweets.");
    }
}

var Timer = function (callback, delay) {
    var timerId;

    this.pause = function () {
        window.clearInterval(timerId);
    };

    this.resume = function () {
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

var highlightText = function (txt) {
    var tokens = [];
    var splitter = ['!', ',', ':'];
    var incount = 1;

    // Split
    var c = ' ';
    $.each(txt.split(c), function (i, t) {
        tokens.push(t);
        tokens.push(c);
    });
    ++incount;

    $.each(splitter, function (i, c) {
        var ttokens = [];
        $.each(tokens, function (i, to) {
            if (i % incount === 0) {
                if (c === ':' && to.indexOf("http") !== -1) {
                    ttokens.push(to);
                    ttokens.push(c);
                } else {
                    $.each(to.split(c), function (i, t) {
                        ttokens.push(t);
                        ttokens.push(c);
                    });
                }
                ttokens.splice(ttokens.length - 1, 1);
            } else {
                ttokens.push(to);
            }
        });
        ++incount;
        tokens = ttokens;
    });

    // Add span
    var res = [];
    $.each(tokens, function (i, t) {
        if (t.indexOf('@') !== -1) {
            res.push("<span class=\"handle\">" + t + "</span>");
        } else if (t.indexOf('#') !== -1) {
            res.push("<span class=\"hashtag\">" + t + "</span>");
        } else if (t.indexOf('http') !== -1) {
            res.push("<span class=\"link\">" + t + "</span>");
        } else {
            res.push(t);
        }
    });

    // Merge
    return res.join('');
};

var TwitterControl = function (selector, data) {

    var _selector = selector;
    var _data = data;
    var _n = 4;
    var _iterator = new Iterator(_data, _n);
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

    TwitterControl.prototype.render = function () {
        var tweets = _iterator();
        $.each(tweets, function (index, tweet) {
            var cell = $(_cells[index]);
            var children = cell.children();
            children.fadeOut(1000, function () {
                var txt = tweet.text;
                txt = txt.length > 100 ?
                    txt.substring(0, 100) + "..." :
                    txt;
                var htxt = highlightText(txt);
                cell.find(".tweet-user").text(tweet.twitterid);
                cell.find(".tweet-content").html(htxt);
                children.fadeIn(2000);
            });
        });
    };

    TwitterControl.prototype.startTimer = function (timeout) {
        timeout = (timeout) ? timeout : 6000;
        var threadRender = this.render;
        if (_timer) {
            _timer = null;
        }
        _timer = new Timer(threadRender, timeout);
    };

    TwitterControl.prototype.stopTimer = function () {
        if (_timer) {
            clearInterval(_timer);
            _timer = null;
        }
    };

    // First render
    TwitterControl.prototype.render();
};
