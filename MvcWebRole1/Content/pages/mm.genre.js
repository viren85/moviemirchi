function PrepareGenrePage() {
    /*var json = [
                    { "title": "Now Playing", "section": "nowplaying-movie-list-tube" },
                    { "title": "Upcoming", "section": "upcoming-movie-list-tube" },
                    { "title": "Previous", "section": "previous-movie-list-tube" },
                    { "title": "Recently Viewed", "section": "recent-container-tube" }
    ];

    $(".nav-bar-container").append(GetNavBar(json));*/

    var name = GetEntityName(document.location.href, "genre");
    name = new Util().toPascalCase(name);

    $(".movies").append("<div class=\"movie-details\" style=\"margin-left: 0px\"><div class=\"tube-tilte\"  style=\"width: 200px\">" + name + " Movies</div></div>");
    $(".movies").append("<div class=\"movie-list\"></div>");
    $(".movies").append("<div class=\"upcoming-movie-list\"></div>");
    $(".movies").append("<div class=\"previous-movie-list\"></div>");
    $(".movies").append("<div class=\"recent-container\"><div class=\"tube-tilte\">Recent</div></div>");

    /*$(".movies").append(GetTubeControl(name + " Movies", "genre-name"));
    $(".movies").append(GetTubeControl("Now Playing", "nowplaying-movie-list", "nowplaying-movies-pager"));
    $(".movies").append(GetTubeControl("Upcoming Releases", "upcoming-movie-list", "upcoming-movies-pager"));
    $(".movies").append(GetTubeControl("Previous Movies", "previous-movie-list", "previous-movies-pager"));
    $(".movies").append(GetTubeControl("Recently Viewed", "recent-container", "recent-pager"));

    $(".section-title").each(function () {
        new Util().AppendLoadImage($(this));
    });*/

    var apiPath = "/api/GenreMovies?type=" + name;
    CallHandler(apiPath, PopulateMovies);

    LoadRecentVisits();
    TrackRecentGenreVisit(name);

}

var TrackRecentGenreVisit = function (name) {
    var src = $(".review-list").find("img").attr("src");
    RecentlyViewedCookies.add({ name: name, type: 'genre', url: "/genre/" + name.replace(' ', '-'), src: src });
}