function LoadSingleMovie(movieId) {
    var path = "../api/MovieInfo?q=" + movieId;
    var reviewPath = "../api/MovieReview?q=" + movieId;

    CallHandler(path, ShowMovie);
    //CallHandler(reviewPath, ShowMovieReviews);
}

var ShowMovie = function (data) {
    var result = JSON.parse(data);

    if (result.Movie != undefined) {
        $(".movies").append(GetTubeControl(result.Movie.Name, "movie-list", "movie-pager"));

        //$(".tube-container").append($(".movie-details"));
        //$(".movie-list").append($(".link-container"));
        PopulatingMovies(result.Movie, "movie-list");
        ScaleElement1($(".movie-list ul"));

        // Show all posters of current movie
        var poster = [], reviews = [];

        poster = result.Movie.Posters;
        reviews = result.MovieReviews;

        ShowMovieDetails(result.Movie);
        PopulatePosters(poster, result.Movie.Name);
        ArrangeImages($(".movie-poster-details"));
        ShowMovieReviews(reviews);
        PrepareGenreLinks();

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

    //$(".content").append(GetTubeControl("Tweets", "tweets", "tweet-pager"));

    //LoadTweets();
    // TODO: Clean this code smell
    ////var twtr = new TwitterControl(".tweets");
    ////twtr.startTimer(12000);
}

var ShowMovieDetails = function (movie) {
    var movieDetalis = $("<div/>").addClass("movie-description");

    var directors = "", directorsList = "";
    var writers = "", writerList = "";
    var producers = "", producersList = "";
    var music = "", musicList = "";
    var cast = "", actorList = "";
    var artistCounter = 0;
    var songsList = "";

    var casts = [];
    casts = JSON.parse(movie.Casts);

    if (casts != "undefined" && casts != null && casts.length > 0) {
        for (var c = 0; c < casts.length; c++) {
            if (casts[c].role.toLowerCase() == "director" && casts[c].name != null && directors.indexOf(casts[c].name) == -1) {
                if (casts[c].charactername == null) {
                    directors += "<a href=\"/Artists/" + casts[c].name.split(' ').join('-') + "\" title='click here to view profile'>" + casts[c].name + "</a>, ";
                    directorsList += "<li class='team-item'><a href=\"/Artists/" + casts[c].name.split(' ').join('-') + "\">" + casts[c].name + "</a></li>";
                }
            }
            else if (casts[c].role.toLowerCase() == "writer" && casts[c].name != null && writers.indexOf(casts[c].name) == -1) {
                writers += "<a  href=\"/Artists/" + casts[c].name.split(' ').join('-') + "\" title='click here to view profile'>" + casts[c].name + "</a>, ";
                writerList += "<li class='team-item'><a href=\"/Artists/" + casts[c].name.split(' ').join('-') + "\">" + casts[c].name + "</a></li>";
            }
            else if (casts[c].role.toLowerCase() == "music" && casts[c].name != null && music.indexOf(casts[c].name) == -1) {
                if (casts[c].charactername == null) {
                    music += "<a  href=\"/Artists/" + casts[c].name.split(' ').join('-') + "\" title='click here to view profile'>" + casts[c].name + "</a>, ";
                    musicList += "<li class='team-item'><a href=\"/Artists/" + casts[c].name.split(' ').join('-') + "\">" + casts[c].name + "</a></li>";
                }
            }
            else if (casts[c].role.toLowerCase() == "producer" && casts[c].name != null && producers.indexOf(casts[c].name) == -1) {
                if (casts[c].charactername == "producer") {
                    producers += "<a href=\"/Artists/" + casts[c].name.split(' ').join('-') + "\" title='click here to view profile'>" + casts[c].name + "</a>, ";
                    producersList += "<li class='team-item'><a href=\"/Artists/" + casts[c].name.split(' ').join('-') + "\">" + casts[c].name + "</a></li>";
                }
            }
            else if (casts[c].role.toLowerCase() == "actor" && cast.indexOf(casts[c].name) == -1 && artistCounter < 8) {
                cast += "<a  href=\"/Artists/" + casts[c].name.split(' ').join('-') + "\" title='click here to view profile'> " + casts[c].name + "</a>, ";
                actorList += "<li class='cast-item'><span class='cast-details'><a href=\"/Artists/" + casts[c].name.split(' ').join('-') + "\">" + casts[c].name + "</a></span><span class='cast-details-right'>" + casts[c].charactername + "</span></li>";
                artistCounter++;
            }
        }
    }
    else {
        // Need to remove element if we dont have data to render it on screen
        //$("#item3").remove();
    }

    $(movieDetalis).append(new RatingControl().GetRatingControl(JSON.parse(movie.MyScore)));
    $(movieDetalis).append(GetMovieSynopsis(movie.Synopsis));
    $(movieDetalis).append(GetMovieGenre(movie.Genre));
    $(movieDetalis).append(GetMovieCast(CleanCastString(cast)));
    $(movieDetalis).append(GetMovieDirector(CleanCastString(directors)));
    $(movieDetalis).append(GetMovieProducer(CleanCastString(producers)));
    $(movieDetalis).append(GetMovieMusicDirector(CleanCastString(music)));
    $(movieDetalis).append(GetMovieWriter(CleanCastString(writers)));
    $(movieDetalis).append(GetMovieStats(movie.Stats));
    $(".tube-container:first").append(movieDetalis);
}

// images is JSON object
var PopulatePosters = function (images, movieName) {
    var poster = [];
    poster = JSON.parse(images);

    if (poster != "undefined" && poster != null && poster.length > 1) {        
        
        var ul = $("<ul/>").attr("class", "gallery clearfix");

        for (var p = 0; p < poster.length; p++) {
            var img = $("<img/>")
            img.attr("class", "gallery-image");
            img.attr("alt", movieName);            
            img.attr("src", PUBLIC_BLOB_URL + poster[p]);
            img.error(function () {
                $(this).hide();
            });

            var li = $("<li/>");
            var a = $("<a/>").attr("href", PUBLIC_BLOB_URL + poster[p] + "?lol=lol").attr("rel", "prettyPhoto[gallery]");

            $(a).append(img);
            $(li).append(a);
            $(ul).append(li);
            //$(".movie-poster-details").append(img);
        }

        $(".movie-poster-details").append(ul);

        $(".link-container").show();
    }
    else {
        $(".movie-poster-details").hide();
        $(".link-container").find("div.section-title").each(function () {
            if ($(this).html() == "Posters") {
                $(this).hide();
            }
        });
    }
}

var ShowMovieReviews = function (review) {
    // VS - For production, following line shall be uncommented. Other line is used for demo purposes, 
    // when movies does not have any associated reivews
    //if (review != "undefined" && review != null && review.length > 0) {
    if (review != "undefined" && review != null) {
        $(".link-container").show();
        //(review.length > 0) ? GetReviewControl("movie-review-details", review) : GetDefaultReviewControl("movie-review-details", review);                        
        GetReviewControl("movie-review-details", review);
        if (review.length <= 0)
            $(".movie-review-details").html("<b>Currently this movie does not have any reviews.</b>");
    }
    else {
        $(".movie-review-details").hide();
        $(".link-container").find("div.section-title").each(function () {
            if ($(this).html() == "Reviews") {
                $(this).hide();
            }
        });
    }
}

var GetMovieGenre = function (genre) {
    genre = genre.length == 0 ? "-" : genre;
    return GetMovieDataHolder("Genre:", genre);
}

var GetMovieCast = function (movieCast) {
    movieCast = movieCast.length == 0 ? "-" : movieCast;
    return GetMovieDataHolder("Stars:", movieCast);
}

var GetMovieDirector = function (movieCast) {
    movieCast = movieCast.length == 0 ? "-" : movieCast;
    return GetMovieDataHolder("Directors:", movieCast);
}

var GetMovieProducer = function (movieCast) {
    movieCast = movieCast.length == 0 ? "-" : movieCast;
    return GetMovieDataHolder("Producers:", movieCast);
}

var GetMovieMusicDirector = function (movieCast) {
    movieCast = movieCast.length == 0 ? "-" : movieCast;
    return GetMovieDataHolder("Music Director:", movieCast);
}

var GetMovieWriter = function (movieCast) {
    movieCast = movieCast.length == 0 ? "-" : movieCast;
    return GetMovieDataHolder("Writer:", movieCast);
}

var GetMovieStats = function (movieStats) {
    movieStats = movieStats.length == 0 ? "-" : movieStats;
    return GetMovieDataHolder("Statistics:", movieStats);
}

var GetMovieSynopsis = function (synopsis) {
    synopsis = synopsis.length == 0 ? "-" : synopsis;
    return GetMovieDataHolder("Synopsis:", synopsis);
}

var GetMovieDataHolder = function (label, data) {
    return "<div class=\"movie-data-row\"><span class=\"movie-data-label\">" + label + "</span><span class=\"movie-data\">" + data + "<span/></div>";
}

var CleanCastString = function (str) {
    if (str.length > 0) {
        str = str.substring(0, str.lastIndexOf(","));
    }

    return str;
}