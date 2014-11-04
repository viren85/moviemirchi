(function (i, s, o, g, r, a, m) {
    i['GoogleAnalyticsObject'] = r; i[r] = i[r] || function () {
        (i[r].q = i[r].q || []).push(arguments)
    }, i[r].l = 1 * new Date(); a = s.createElement(o),
    m = s.getElementsByTagName(o)[0]; a.async = 1; a.src = g; m.parentNode.insertBefore(a, m)
})(window, document, 'script', '//www.google-analytics.com/analytics.js', 'ga');

ga('create', 'UA-52983576-1', 'auto');
ga('require', 'displayfeatures');
ga('send', 'pageview');

var USER_ID = "";

var trackNewsLink = function (url) {
    ga('send', 'event', 'news', 'click', url);
}

var trackReviewLink = function (url) {
    ga('send', 'event', 'review', 'click', url);
}

var trackVideoLink = function (url) {
    ga('send', 'event', 'video', 'click', url);
}

var trackSongLink = function (url) {
    ga('send', 'event', 'song', 'click', url);
}

var trackSearchLink = function (pageName, searchTerm) {
    ga('send', 'event', 'search', 'click', pageName + "-" + searchTerm);
}

var trackPhotoLink = function (url) {
    ga('send', 'event', 'photo', 'click', url);
}