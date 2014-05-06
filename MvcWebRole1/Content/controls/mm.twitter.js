function LoadTweets() {
    var tweetPath = "../api/Twitter";
    CallHandler(tweetPath, ShowTweets);
}

var ShowTweets = function (data) {
    var result = JSON.parse(data);
    // Prepare UI for Twitter
}

var tweetData;

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
                cell.find(".tweet-user").text(tweet.twitterid);
                cell.find(".tweet-content").text(tweet.text);
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

$(document).ready(function () {

    tweetData = {
        tweets:
            [
                { "twitterid": "@Bernadette", "text": "Occaecat consequat pariatur incididunt deserunt esse magna. Veniam incididunt culpa nisi esse duis ipsum." },
                { "twitterid": "@Duran", "text": "Lorem et Lorem proident elit pariatur consectetur ullamco deserunt. Labore est est aliqua mollit quis adipisicing consectetur." },
                { "twitterid": "@Sloan", "text": "Lorem sunt in adipisicing reprehenderit enim enim occaecat minim fugiat culpa. Pariatur id adipisicing sit nisi dolor ex aliqua officia aliquip irure ipsum." },
                { "twitterid": "@Woods", "text": "Duis anim non cillum excepteur enim sint exercitation nisi. Commodo aute nostrud id irure." },
                { "twitterid": "@Bonnie", "text": "Veniam aliquip minim sunt Lorem. Cupidatat minim amet aliquip ad in ipsum dolor ad consequat aute." },
                { "twitterid": "@Benita", "text": "Duis qui nisi ut velit eiusmod ut velit elit laboris mollit amet reprehenderit dolore. Amet et laborum nostrud do mollit ullamco aute exercitation quis consectetur laboris labore anim ea." },
                { "twitterid": "@Clemons", "text": "Excepteur et anim sunt anim deserunt tempor velit voluptate dolor magna id non. Cupidatat tempor dolore minim ex." },
                { "twitterid": "@Maldonado", "text": "Duis et consectetur ea do occaecat et id amet. Qui tempor non reprehenderit sint nostrud ad laboris." },
                { "twitterid": "@Horton", "text": "Reprehenderit tempor amet commodo do dolore laboris aute. Proident ad ad excepteur id occaecat." },
                { "twitterid": "@Margie", "text": "Eu ullamco duis magna non. Ad culpa pariatur cillum tempor minim Lorem." },
                { "twitterid": "@Villarreal", "text": "Fugiat aliqua proident ex cillum. Velit consectetur consectetur aute dolor esse ex laborum." },
                { "twitterid": "@Carrillo", "text": "Consequat adipisicing eiusmod labore aliqua sunt tempor commodo deserunt sint dolore et anim exercitation. Consequat adipisicing cillum est fugiat." },
                { "twitterid": "@Earlene", "text": "Laboris in exercitation anim aliquip aliqua reprehenderit laboris ut occaecat in proident. Consectetur non dolore amet tempor commodo magna nostrud adipisicing incididunt esse incididunt amet eu." },
                { "twitterid": "@Bertha", "text": "Commodo veniam cillum ullamco sint voluptate. Excepteur officia cillum labore excepteur sit." },
                { "twitterid": "@Delacruz", "text": "Irure pariatur consequat ea laborum aliqua culpa amet sit occaecat ad Lorem cillum dolor. Proident do eiusmod ipsum proident id dolore anim laborum labore non adipisicing Lorem." },
                { "twitterid": "@Sharlene", "text": "Fugiat adipisicing et quis laborum esse eiusmod magna incididunt minim. Veniam ullamco commodo tempor et cupidatat minim tempor." },
                { "twitterid": "@Jeanne", "text": "Officia consectetur laboris eiusmod sint sint non culpa irure. Nisi quis laborum qui nulla ea consectetur laboris consequat ea ut fugiat voluptate non." },
                { "twitterid": "@Nannie", "text": "Voluptate laborum mollit nisi ad incididunt et ipsum fugiat voluptate enim fugiat. Ex consequat ex eiusmod quis sunt dolore Lorem Lorem." },
                { "twitterid": "@Charlene", "text": "Commodo nulla officia sint et excepteur voluptate nostrud aute commodo laboris non. Nisi eu officia culpa magna anim minim tempor consectetur." },
                { "twitterid": "@Effie", "text": "Consectetur est velit ipsum aute et irure voluptate. Incididunt officia ullamco duis eu consequat ad." }
            ]
    };

    var twtr = new TwitterControl(".tweets", tweetData.tweets);
    twtr.startTimer(12000);
});