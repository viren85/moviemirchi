function PrepareMoviePage() {
    var json = [
                    { "title": "Movie", "section": "movie-list-tube" },
                    { "title": "Posters", "section": "mov_poster" },
                    { "title": "Songs", "section": "movie_songs" },
                    { "title": "Trailers", "section": "movie_trailers" },
                    { "title": "Reviews", "section": "movie-review-details-tube" },
                    { "title": "Tweets", "section": "tweets-tube" }
    ];

    $(".nav-bar-container").append(GetNavBar(json));

    var url = document.location.href;
    var name = url.substring(url.indexOf("movie/") + "movie/".length);
    if (name.indexOf("#") > -1) {
        name = name.substring(0, name.indexOf("#"))
    }
    if (name.indexOf("?") > -1) {
        name = name.substring(0, name.indexOf("?"));
    }

    LoadSingleMovie(name);

    $(".tweet-details").append(GetTubeControl("Tweets", "tweets", "tweet-pager", "width30"));
    $(".review-details").append(GetTubeControl("Reviews", "movie-review-details", "review-pager"));
    LoadTweets("movie", name);

    $(".section-title").each(function () {
        new Util().AppendLoadImage($(this));
    });

    // Hide loading image from tweets and news section after 5 sec
    setTimeout(function () {
        new Util().RemoveLoadImage($("#tweets-tube"));
    }, 4000);
}

function PrepareHomePage() {
    var json = [
                    { "title": "Now Playing", "section": "movie-list-tube" },
                    { "title": "Upcoming Releases", "section": "upcoming-movie-list-tube" },
                    { "title": "News", "section": "news-container-tube" },
                    { "title": "Tweets", "section": "tweets-tube" },
                    { "title": "Critics", "section": "critics-container-tube" }
    ];

    $(".nav-bar-container").append(GetNavBar(json));


    $(".movies").append(GetTubeControl("Now Playing", "movie-list", "now-pager"));
    $(".movies").append(GetTubeControl("Upcoming Releases", "upcoming-movie-list", "upcoming-pager"));
    $(".movies").append(GetTubeControl("News", "news-container", "news-pager", "width60"));
    $(".movies").append(GetTubeControl("Tweets", "tweets", "tweet-pager", "width30"));
    $(".movies").append(GetTubeControl("Top Critics", "critics-container", "critics-pager", "width60"));

    $(".section-title").each(function () {
        new Util().AppendLoadImage($(this));
    });

    new Util().RemoveLoadImage($("#critics-container-tube"));

    LoadCritics();
    LoadNews();
    LoadCurrentMovies();
    LoadUpcomingMovies();
    LoadTweets();
}