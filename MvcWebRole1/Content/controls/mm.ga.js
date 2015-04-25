// Google analytics is set up in the layout

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