function PrepareMoviePage() {
    /*var json = [
                    { "title": "Movie", "section": "movie-list-tube" },
                    { "title": "Posters", "section": "mov_poster" },
                    { "title": "Songs", "section": "movie_songs" },
                    { "title": "Trailers", "section": "movie_trailers" },
                    { "title": "Reviews", "section": "movie-review-details-tube" },
                    { "title": "Tweets", "section": "tweets-tube" },
                    { "title": "Recently Viewed", "section": "recent-container-tube" }
    ];*/

    //$(".nav-bar-container").append(GetNavBar(json));

    var name = GetEntityName(document.location.href, "movie");

    LoadSingleMovie(name);

    //$(".tweet-details").append(GetTubeControl("Tweets", "tweets", "tweet-pager", "width30"));
    //$(".review-details").append(GetTubeControl("Reviews", "movie-review-details", "review-pager"));
    //$(".recent-details").append(GetTubeControl("Recently Viewed", "recent-container", "recent-pager", "width30"));

    //$(".movies").append("<div class=\"twitter-container\"><div class=\"tube-tilte\">Mirchi Feed</div></div>");
    $(".movies").append("<div class=\"movie-details\"></div>");
    $(".movies").append("<div class=\"movie-photos\"></div>");
    $(".movies").append("<div class=\"movie-songs\"><div class=\"tube-tilte\">Songs</div></div>");
    $(".movies").append("<div class=\"movie-trailers\"><div class=\"tube-tilte\">Trailers</div></div>");
    $(".movies").append("<div class=\"movie-reviews\"><div class=\"tube-tilte\">Reviews</div></div>");
    $(".movies").append("<div class=\"recent-container\"><div class=\"tube-tilte\">Recent</div></div>");

    LoadTweets("movie", name);

    $(".section-title").each(function () {
        new Util().AppendLoadImage($(this));
    });

    // Hide loading image from tweets and news section after 5 sec
    /*setTimeout(function () {
        new Util().RemoveLoadImage($("#tweets-tube"));
    }, 4000);*/

    LoadRecentVisits();
}

var TrackRecentMovieVisit = function (name) {
    var src = $(".movie-poster-container").find("img[class='movie-poster']").attr("src");
    RecentlyViewedCookies.add({ name: name, type: 'movie', url: "/movie/" + name.replace(' ', '-'), src: src });
}

var GetEntityName = function (url, page) {
    page = page + "/";
    var name = url.substring(url.indexOf(page) + page.length);
    if (name.indexOf("#") > -1) {
        name = name.substring(0, name.indexOf("#"))
    }
    if (name.indexOf("?") > -1) {
        name = name.substring(0, name.indexOf("?"));
    }
    return name;
};

var RecentlyViewedCookies = {
    add: (function () {
        var unique = unique || function (a) {
            var o = {}, i, l = a.length, r = [];
            for (i = 0; i < l; i += 1) o[a[i].url] = a[i];
            for (i in o) r.push(o[i]);
            return r;
        };
        return function (currentPage) {
            currentPage.name = new Util().toPascalCase(currentPage.name.replace(/-/g, " "));
            var arr = RecentlyViewedCookies.get();
            arr.unshift(currentPage);
            arr = unique(arr);
            $.cookie("RecentlyViewed", JSON.stringify(arr), { path: '/' });
        }
    })(),
    get: function () {
        var arr;
        var s = $.cookie("RecentlyViewed");
        try {
            arr = JSON.parse(s);
        } catch (e) {
        }
        arr = (arr || []);

        // We want only first 10 elements
        arr = arr.splice(0, 10);
        return arr;
    }
};

function PrepareHomePage() {
    /*var json = [
                    { "title": "Now Playing", "section": "movie-list-tube" },
                    { "title": "Upcoming Releases", "section": "upcoming-movie-list-tube" },
                    { "title": "News", "section": "news-container-tube" },
                    { "title": "Tweets", "section": "tweets-tube" },
                    { "title": "Critics", "section": "critics-container-tube" },
                    { "title": "Recently Viewed", "section": "recent-container-tube" }
    ];

    $(".nav-bar-container").append(GetNavBar(json));*/

    $(".movies").append("<div class=\"movie-list\"></div>");
    $(".movies").append("<div class=\"upcoming-movie-list\"></div>");
    $(".movies").append("<div class=\"twitter-container\"><div class=\"tube-tilte\">Mirchi Feed</div></div>");
    //$(".movies").append(GetTubeControl("Upcoming Releases", "upcoming-movie-list", "upcoming-pager"));
    //$(".movies").append(GetTubeControl("News", "news-container", "news-pager", "width60"));
    $(".movies").append("<div class=\"news-container\"><div class=\"tube-tilte\">News</div></div>");
    $(".movies").append("<div class=\"critics-container\"><div class=\"tube-tilte\">Critics</div></div>");

    //$(".movies").append(GetTubeControl("Top Critics", "critics-container", "critics-pager", "width60"));
    $(".movies").append("<div class=\"focus-container\"><div class=\"tube-tilte\">In Focus</div></div>");
    $(".movies").append("<div class=\"recent-container\"><div class=\"tube-tilte\">Recent</div></div>");

    $(".section-title").each(function () {
        new Util().AppendLoadImage($(this));
    });

    new Util().RemoveLoadImage($("#critics-container-tube"));

    LoadCritics();
    LoadNews();
    LoadCurrentMovies();
    LoadUpcomingMovies();

    if (!new Util().IsMobile()) {
        LoadTweets("home", "");
    }
    else {
        $(".twitter-container").remove();
        $(".focus-container").remove();
    }

    LoadRecentVisits();
}