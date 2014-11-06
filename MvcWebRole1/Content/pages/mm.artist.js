// This method loads the artist page content
function PrepareArtistPage() {
    var json = [
                { "title": "Artist", "section": "artist_detail-tube" },
                { "title": "Upcoming Movies", "section": "upcoming-movie-list-tube" },
                { "title": "Now Playing Movies", "section": "nowplaying-movie-list-tube" },
                { "title": "Previous Movies", "section": "previous-movie-list-tube" },
                { "title": "Photos", "section": "movie-poster-details-tube" },
                { "title": "Tweets", "section": "tweets-tube" },
                { "title": "Recently Viewed", "section": "recent-container-tube" }
    ];

    $(".nav-bar-container").append(GetNavBar(json));

    var name = GetEntityName(document.location.href, "artists");
    var artist = name.split('-').join(' ');

    var fileName = "/Images/Loading.GIF";
    $("<div id=\"artist_detail-tube\" class=\"section-title large-fonts\">" + new Util().toPascalCase(artist) + "</div>").insertBefore($(".movies .artist-bio"));
    $(".movies .artist-bio").append(ShowPersonBio(""));
    $(".movies").append(GetTubeControl("Upcoming Movies", "upcoming-movie-list", "upcoming-movies-pager"));
    $(".movies").append(GetTubeControl("Now Playing Movies", "nowplaying-movie-list", "nowplaying-movies-pager"));
    $(".movies").append(GetTubeControl("Previous Movies", "previous-movie-list", "previous-movies-pager"));
    $(".movies").append(GetTubeControl("Photos", "movie-poster-details", "posters-pager"));
    $(".movies").append(GetTubeControl("Tweets", "tweets", "tweet-pager", "width30"));
    $(".movies").append(GetTubeControl("Recently Viewed", "recent-container", "recent-pager"));

    $(".section-title").each(function () {
        new Util().AppendLoadImage($(this));
    });

    var apiPath = "/api/ArtistMovies?type=movie&name=" + artist;
    var artistBioPath = "/api/ArtistMovies?type=bio&name=" + artist;
    CallHandler(apiPath, PopulateMovies);
    CallHandler(artistBioPath, PopulateArtistsBio);
    LoadTweets("artist", name);
    LoadRecentVisits();
    new Util().RemoveLoadImage($("#tweets-tube"));
    
    RecentlyViewedCookies.add({ name: name, type: 'artist', url: "/artists/" + name });
}

// This method loads artists default poster, artists bio and all the posters
function PopulateArtistsBio(response) {
    try {
        var data = JSON.parse(response);

        if (data.Status != undefined || data.Status == "Error") {
            $(".intro-text").css("margin-left", "0px").html(data.UserMessage);
            $("#artist_detail-tube").each(function () {
                new Util().RemoveLoadImage($(this));
            });

            $("#movie-poster-details-tube").hide();
        }
        else {
            if (data != null) {
                new Util().RemoveLoadImage($("#artist_detail-tube"));
                if (data.Bio == "")
                    $(".intro-text").html("Movie Mirchi is gathering bio-data for this artist, will be updated soon.");
                else
                    $(".intro-text").css("margin-left", "0px").html(data.Bio);

                var posters = JSON.parse(data.Posters);

                if (posters != null && posters.length > 0) {
                    new Util().RemoveLoadImage($("#movie-poster-details-tube"));
                    $(".bio-pic").append($("<img/>").attr("src", PUBLIC_BLOB_URL + posters[posters.length - 1]));
                    InitBio();

                    if (posters.length > 1) {
                        PopulatePosters(data.Posters, data.ArtistName, null);
                        ArrangeImages($(".movie-poster-details"));
                    }
                    else {
                        $("#movie-poster-details-tube").hide();
                        $(".top-nav-bar").find("li").each(function () {
                            if ($(this).attr("link-id") == "movie-poster-details-tube") {
                                $(this).remove();
                            }
                        });
                    }

                    $(".gallery a[rel^='prettyPhoto']").prettyPhoto({
                        animation_speed: 'normal',
                        theme: 'dark_square',
                        slideshow: false,
                        autoplay_slideshow: false,
                        show_title: true,
                        keyboard_shortcuts: true,
                        social_tools: false,
                        allow_resize: true,
                    });
                }
                else {
                    $(".bio-pic img").attr("src", "/Images/user.png").removeAttr("style").css("width", "140px").css("float", "left");;
                    $(".movie-poster-details").parent().hide();
                    $("#movie-poster-details-tube").hide();
                    $(".top-nav-bar").find("li").each(function () {
                        if ($(this).attr("link-id") == "movie-poster-details-tube") {
                            $(this).remove();
                        }
                    });
                }
            }
            else {
                $(".intro-text").css("margin-left", "0px").html("Currently this artist does not have any biography on <a href=\"/\">Movie Mirchi</a>");
            }
        }
    } catch (e) {
        $(".intro-text").css("margin-left", "0px").html("Unable to get artist's details.");

        $(".section-title").each(function () {
            new Util().RemoveLoadImage($(this));
        });
    }
}

// This method loads artists upcoming, now playing and previous movies
function PopulateMovies(response) {
    try {
        var data = JSON.parse(response);
        var now = 0, upcoming = 0, archived = 0;

        new Util().RemoveLoadImage($("#genre-name-tube"));
        new Util().RemoveLoadImage($("#upcoming-movie-list-tube"));
        new Util().RemoveLoadImage($("#nowplaying-movie-list-tube"));
        new Util().RemoveLoadImage($("#previous-movie-list-tube"));

        if (data.Status != undefined || data.Status == "Error") {
            $(".movie-list").html(data.UserMessage);
            $(".top-nav-bar").find("li").each(function () {
                if ($(this).attr("link-id") == "movie-list-tube") {
                    $(this).remove();
                }
            });
        }
        else {
            if (data != null && data.length > 0) {
                for (var i = 0; i < data.length; i++) {
                    //var list = PopulatingMovies(data[i], "movie-list");
                    if (data[i].State == "now playing" || data[i].State == "now-playing") {
                        var list = PopulatingMovies(data[i], "nowplaying-movie-list");
                        now++;
                    }
                    else if (data[i].State == "upcoming") {
                        var list = PopulatingMovies(data[i], "upcoming-movie-list");
                        upcoming++;
                    }
                    else if (data[i].State == "" || data[i].State == "released") {
                        // It creates 20+ pages for current data. I think we shall have some restriction
                        if (archived < 25) {
                            var list = PopulatingMovies(data[i], "previous-movie-list");
                            archived++;
                        }
                    }
                }

                /*The image width/height shall be calculated once the image is fully loaded*/
                var width = $(document).width();
                if (now == 0) {
                    $("#nowplaying-movie-list-tube").hide();
                    $(".top-nav-bar").find("li").each(function () {
                        if ($(this).attr("link-id") == "movie-list-tube") {
                            $(this).remove();
                        }
                    });
                }
                if (upcoming == 0) {
                    $("#upcoming-movie-list-tube").hide();
                    $(".top-nav-bar").find("li").each(function () {
                        if ($(this).attr("link-id") == "upcoming-movie-list-tube") {
                            $(this).remove();
                        }
                    });
                }
                if (archived == 0) {
                    $("#previous-movie-list-tube").hide();
                    $(".top-nav-bar").find("li").each(function () {
                        if ($(this).attr("link-id") == "previous-movie-list-tube") {
                            $(this).remove();
                        }
                    });
                }

                // TODO: Revisit my fix - there is nothing like ".movie-list" here, we have three over here
                $.each(["upcoming-", "nowplaying-", "previous-"], function (k, v) {

                    var $list = $("." + v + "movie-list");
                    $list.addClass("movie-list");

                    var ratio = 0;
                    var newWidth = 0;
                    $list.find("img:first").each(function () {
                        ratio = this.width / this.height;
                        newWidth = 400 * ratio;
                    });
                    $list.find("img").each(function () {
                        var $this = $(this);
                        $this.width(newWidth + "px").height("340px");

                        if (newWidth > 225) {
                            $this.width("225px");
                        }
                    });

                    ScaleElement($list.find("ul"));

                    var resizeHandler = function () {
                        if ($(window).width() < 768) {
                            new Pager($list, "#" + v + "movies-pager");
                        } else {
                            PreparePaginationControl($list, { pagerContainerId: "" + v + "movies-pager", tileWidth: "275" });
                        }
                    };

                    resizeHandler();
                    $(window).resize(resizeHandler);
                });
            }
            else {
                $(".top-nav-bar").find("li").each(function () {
                    if ($(this).attr("link-id") == "movie-list-tube") {
                        $(this).remove();
                    }
                });
            }
        }
    } catch (e) {
        $(".movie-list").html("Movie Mirchi is gathering movies for this artist, will be updated soon.");
    }
}