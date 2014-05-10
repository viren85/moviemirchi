function LoadTweets() {
    var tweetPath = "../api/Twitter?start=0&page=20";
    CallHandler(tweetPath, ShowTweets);
}

var ShowTweets = function (data) {
    var jdata = JSON.parse(data);

    var tweets = [];
    for (var v in jdata) {
        var t = jdata[v];
        tweets.push({
            twitterid: "@" + (t.ReplyScreenName || ""),
            text: (t.TextMessage || ""),
        });
    }

    var twtr = new TwitterControl(".tweets", tweets);
    twtr.startTimer(12000);
}

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

var TwitterControl = function (selector, data) {

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
                cell.find(".tweet-user").text(tweet.twitterid);
                cell.find(".tweet-content").text(txt);
                children.fadeIn(2000);
            });
        });
    };

    TwitterControl.prototype.startTimer = function (timeout) {
        timeout = (timeout) ? timeout : 6000;
        var threadRender = this.render;
        _timer = setInterval(function () {
            threadRender();
        }, timeout);
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
