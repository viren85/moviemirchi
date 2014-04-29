function LoadSingleMovie(movieId) {
    var path = "../api/MovieInfo?q=" + movieId;
    var reviewPath = "../api/MovieReview?q=" + movieId;
    CallHandler(path, ShowMovie);
    CallHandler(reviewPath, ShowMovieReviews);
}

function ShowMovie(result) {
    console.log(result);
    result = JSON.parse(result);

    if (result.Movie != undefined) {
        $(".movies").append(GetTubeControl(result.Movie.Name, "movie-list", "movie-pager"));
        $(".tube-container").append($(".movie-details"));
        $(".movie-list").append($(".link-container"));
        PopulatingMovies(result.Movie);
        ScaleElement($(".movie-list ul"));

        // Show all posters of current movie
        var poster = [], reviews = [];

        poster = result.Movie.Posters;
        reviews = result.MovieReviews;

        PopulatePosters(poster, result.Movie.Name);
        ArrangeImages($(".movie-poster-details"));
        // Show all reviews of current movie
        ShowMovieReviews(reviews);
    }
}

var ShowMovieReviews = function (review) {
    if (review != "undefined" && review != null && review.length > 0) {
        $(".link-container").show();
    }
    else {
        $(".movie-review-details").hide();
    }
}

// Poster is JSON object
var PopulatePosters = function (images, movieName) {
    var poster = [];
    poster = JSON.parse(images);
    if (poster != "undefined" && poster != null && poster.length > 0) {
        //$("img.home-poster").attr("src", "/Posters/Images/" + poster[poster.length - 1]);

        //showing movies posters
        for (var p = 0; p < poster.length; p++) {
            var img = $("<img/>")
            img.attr("class", "gallery-image");
            img.attr("alt", movieName);
            img.attr("src", "/Posters/Images/" + poster[p]);
            img.error(function () {
                $(this).hide();
            });

            $(".movie-poster-details").append(img);
        }

        $(".link-container").show();
    }
    else {
        $(".movie-poster-details").hide();
    }
}