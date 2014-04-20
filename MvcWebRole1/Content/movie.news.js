((function ShowNewsControl(url, selector) {

    var callback = function (entries) {

        entries.forEach(function (entry) {

            var getValue = function (k) {
                var el = entry[k];
                return el ? el : "";
            };

            var getDateSpan = function (k) {
                var ds = getValue(k);
                return (ds && ds !== "" && ds !== " ") ? ("<span>" + new Date(ds).toUTCString() + "</span>") : "";
            }

            var getSpan = function (k) {
                return "<span>" + getValue(k) + "</span>";
            };

            var html =
            "<div class='news-item'>" +
                "<div class='news-title'>" + getSpan("title") + "</div>" +
                "<div class='news-content'>" +
                    "<div class='news-date'>" + getDateSpan("publishedDate") + "</div>" +
                    "<div class='news-details'>" + getSpan("contentSnippet") + "</div>" +
                    "<div class='news-author'><a href=\"" + getValue("link") + "\">" + getSpan("author") + "</a></div>" +
                    
                "</div>" +
            "</div>";

            $(selector).append(html);
        });
    };

    var displayError = function (v) {
        var html =
            "<div class='news-item'><span class='error'>" + v + "</span></div>";
        $(selector).append(html);
    };

    $.ajax({
        url: 'http://ajax.googleapis.com/ajax/services/feed/load?v=1.0&num=10&callback=?&q=' + encodeURIComponent(url),
        dataType: 'json',
        success: function (data) {
            if (data && data.responseData && data.responseData.feed && data.responseData.feed.entries) {
                callback(data.responseData.feed.entries);
            } else {
                displayError("Sorry we are unable to display NEWS at this time");
            }
        },
        error: function (err) {
            displayError("Sorry we are unable to display NEWS at this time");
        }
    });
})("http://www.bollywoodhungama.com/rss/news.xml", ".news-container"));