﻿function CallHandler(queryString, OnComp) {
    $.ajax({
        url: queryString,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        responseType: "json",
        cache: false,
        success: OnComp,
        error: OnFail
    });
    return false;
}

function OnFail() { }

function GetQueryStringsForHtmPage() {
    var assoc = {};
    var decode = function (s) { return decodeURIComponent(s.replace(/\+/g, " ")); };
    var queryString = document.location.search.substring(1);

    var keyValues = queryString.split('&');

    for (var i in keyValues) {
        var key = keyValues[i].split('=');
        if (key.length > 1) {
            assoc[decode(key[0])] = decode(key[1]);
        }
    }

    return assoc;
}

function LoadCurrentMovies() {
    var path = "api/Movies?type=current";
    CallHandler(path, onSuccessLoadCurrentMovies);
}

var MOVIES = [];

function onSuccessLoadCurrentMovies(result) {
    result = JSON.parse(result);

    console.log(result);

    if (result.length > 0) {

        MOVIES = result;

        // adding images        
        for (var i = 0; i < result.length; i++) {
            PopulatingMovies(result[i]);
            PopulatingMoviesTitle(result[i]);
            if (i == 4)
                break;
        }

        if (result.length > 5) {
            $("#nextMovies").show();
            //$("#previousMovies").show();
        }
    }
}
var MovieCounter = 5;
var MovieIndexer = 0;

function NextMovies() {
    if (MovieIndexer < MOVIES.length) {
        $("#previousMovies").show();
        $("#currentmovie").html("");
        $("#movieTitle").html("");

        MovieIndexer++;

        for (var i = MovieIndexer; i < MOVIES.length; i++) {
            MovieCounter++;
            PopulatingMovies(MOVIES[i]);
            PopulatingMoviesTitle(MOVIES[i]);

            if (MovieCounter == MOVIES.length) {
                $("#nextMovies").hide();
            }

            if (MovieCounter % 5 == 0) {
                break;
            }
        }
    }
}
function PreviousMovies() {
    MovieIndexer--;
    if (MovieIndexer >= 0) {
        $("#nextMovies").show();
        $("#currentmovie").html("");
        $("#movieTitle").html("");

        for (var i = MovieIndexer; i < MOVIES.length; i++) {
            MovieCounter--;
            PopulatingMovies(MOVIES[i]);
            PopulatingMoviesTitle(MOVIES[i]);

            if (MovieCounter == 5) {
                $("#previousMovies").hide();
            }

            if (MovieCounter % 5 == 0) {
                break;
            }
        }

        //MovieIndexer--;
    }
}

function PopulatingMovies(movie) {
    var movieContainer = $("#currentmovie");

    var img = $("<img/>")
    img.attr("class", "img-thumbnail");
    img.attr("style", "width: 19%; height: 150px;margin-right: 1%");
    img.attr("data-src", "holder.js/200x200");
    img.attr("alt", movie.Name);

    var poster = [];
    poster = JSON.parse(movie.Posters);

    console.log(poster);

    img.attr("src", poster[0].url);

    var anchor = $("<a/>");
    anchor.attr("href", "Movie?name=" + movie.UniqueName);
    anchor.attr("title", movie.Name)
    anchor.append(img);

    movieContainer.append(anchor);
}

function PopulatingMoviesTitle(movieTitle) {
    var name = $("<div/>");
    name.attr("class", "movie-title");
    name.attr("style", "margin-right: 1%;");
    name.html("<a href='Movie?name=" + movieTitle.UniqueName + "'>" + movieTitle.Name + "</a>");

    var parent = $("#movieTitle");

    parent.append(name);
}

function LoadSingleMovie(movieId) {
    var path = "api/MovieInfo?q=" + movieId;

    CallHandler(path, onSuccessLoadSingleMovie);
}

function onSuccessLoadSingleMovie(result) {
    result = JSON.parse(result);

    console.log(result);

    if (result.Movie != undefined) {

        $("#title").html("<span style='font-size: 16px; font-weight: bold'>" + result.Movie.Name + "</span> (" + result.Movie.Year + ")");

        var poster = [];
        poster = JSON.parse(result.Movie.Posters);

        if (poster.length > 0) {
            //showing movies posters
            for (var p = 0; p < poster.length; p++) {
                var img = $("<img/>")
                img.attr("class", "img-thumbnail");
                img.attr("style", "width: 150px; height: 150px;margin-right: 1%");
                img.attr("data-src", "holder.js/200x200");
                img.attr("alt", result.Movie.Name);
                img.attr("src", poster[p].url);

                $("#posters").append(img);
            }
        }

        $("#genre").html("<b>Genre :</b> " + result.Movie.Genre);
        $("#synopsis").html("<b>Synopsis :</b> " + result.Movie.Synopsis);

        var directors = ""; var writers = ""; var cast = "";

        var casts = [];
        casts = JSON.parse(result.Movie.Casts);

        if (casts.length > 0) {
            for (var c = 0; c < casts.length; c++) {

                if (casts[c].role.toLowerCase() == "director") {
                    directors += "<a href='javascript:void(0);' title='click here to view profile'>" + casts[c].name + "</a>, ";
                }
                else if (casts[c].role.toLowerCase() == "writer") {
                    writers += "<a href='javascript:void(0);' title='click here to view profile'>" + casts[c].name + "</a>, ";
                }
                else {
                    cast += "<a href='javascript:void(0);' title='click here to view profile'> " + casts[c].name + "</a>, ";
                }
            }
        }

        if (directors.length > 0) {
            directors = directors.substring(0, directors.lastIndexOf(","));
        }
        if (cast.length > 0) {
            cast = cast.substring(0, cast.lastIndexOf(","));
        }
        if (writers.length > 0) {
            writers = writers.substring(0, writers.lastIndexOf(","));
        }

        $("#director").html("<b>Director :</b> " + directors);
        $("#writer").html("<b>Writer :</b> " + writers);
        $("#cast").html("<b>Cast :</b> " + cast);

        var ratings = [];
        ratings = JSON.parse(result.Movie.Ratings);

        console.log(ratings);
        var rating = 3;
        if (ratings.critic != undefined)
            rating = ratings.critic;

        //$("#rating").html("<b>Rating :</b>  System = " + ratings.system + ", Critics = " + ratings.critic + ", Hot = " + ratings.hot);
        $("#rating").html("<b style='float: left; margin-top: 3%; margin-right: 3%'>Rating :</b><div class='titlePageSprite star-box-giga-star'> " + rating + " </div>");

        // populating Reviews
        var reviews = [];
        //reviews = JSON.parse(result.MovieReviews);
        reviews = result.MovieReviews;

        if (reviews.length > 0) {

            var ul = $("#reviews");

            for (var r = 0; r < reviews.length; r++) {
                var li = $("<li>");
                li.attr("class", "activityContainer");

                var div = $("<div>");
                div.attr("class", "reviewer");

                var img = $("<img/>")
                img.attr("class", "img-thumbnail");
                img.attr("style", "width: 50px; height: 50px;");
                img.attr("data-src", "holder.js/200x200");
                img.attr("alt", reviews[r].ReviewerName);
                img.attr("src", reviews[r].OutLink);
                img.attr("title", reviews[r].ReviewerName);

                var anchor = $("<a/>");
                anchor.attr("href", "Movie/Reviewer?name=" + reviews[r].ReviewerName);
                anchor.append(img);

                var span = $("<span>");
                span.attr("style", "width:100%; font-weight: bold;");
                span.attr("title", reviews[r].ReviewerName);

                var reviewerName = reviews[r].ReviewerName;

                if (reviews[r].ReviewerName.length > 11) {
                    reviews[r].ReviewerName = reviews[r].ReviewerName.substring(0, 11) + "...";
                }

                span.html("<a href='Movie/Reviewer?name=" + reviewerName + "'>" + reviews[r].ReviewerName + "</a>");
                var br = $("<br/>");
                //div.append(img);
                div.append(anchor);
                div.append(br);
                div.append(span);

                var review = $("<div>");
                review.attr("class", "review");
                if (reviews[r].Review.length > 190) {
                    reviews[r].Review = reviews[r].Review.substring(0, 190) + "...";
                }
                review.html("<a href='Movie/Reviewer?name=" + reviewerName + "' >" + reviews[r].Review + "</a>");

                li.append(div);
                li.append(review);

                ul.append(li);
            }
        }
        else {
            $("#reviews").html("<li class='activityContainer'>No reviews</li>");
        }
    }
}

function GetReviewerAndReviews(name, movie) {
    var path = "../api/ReviewerInfo?name=" + name;

    CallHandler(path, onSuccessPopulateReviewsAndReviews);
}

var ReviewsDetails = [];
var Index = 0;

function onSuccessPopulateReviewsAndReviews(result) {
    result = JSON.parse(result);
    console.log(result);

    if (result != undefined) {
        // adding image
        var img = $("<img/>")
        img.attr("class", "img-thumbnail");
        img.attr("style", "width: 150px; height: 150px;");
        img.attr("data-src", "holder.js/200x200");
        img.attr("alt", result.Name);
        img.attr("src", result.OutLink);
        img.attr("title", result.Name);
        $("#img").append(img);

        // addmin name
        $("#name").html("<h2>" + result.Name + "</h2>");

        // adding affiliation
        var affiliation = []; affiliation = JSON.parse(result.Affilation);
        if (affiliation != undefined) {
            for (var a = 0; a < affiliation.length; a++) {
                var img = $("<img/>")
                img.attr("class", "img-thumbnail");
                img.attr("style", "width: 30px; height: 30px;margin-right: 1%");
                img.attr("data-src", "holder.js/200x200");
                img.attr("alt", affiliation[a].name);
                img.attr("src", affiliation[a].logoimage);
                img.attr("title", affiliation[a].name);

                var anchor = $("<a/>");
                anchor.attr("href", affiliation[a].link);
                anchor.attr("target", "_blank");
                anchor.append(img);

                $("#affiliation").append(anchor);
            }
        }
        else {
            $("#affiliation").html("");
        }

        ReviewsDetails = result.ReviewsDetails;

        if (ReviewsDetails != undefined) {
            PopulateReview(ReviewsDetails[0]);
            $("#next").attr("onclick", "Next();");
            $("#previous").attr("onclick", "Previous();");

            if (ReviewsDetails.length == 1) {
                $("#next").hide();
                $("#previous").hide();
            }
            else {
                $("#next").show();
                $("#previous").hide();
            }
        }
    }
}

function PopulateReview(review) {
    $("#moviename").html("<h2>" + review.MovieName + "</h2>");
    $("#rating").html(review.CriticsRating);
    $("#reivew").html(review.Review);
    $("#title").html("Review - " + review.MovieName);
}

function Next() {
    Index++;
    $("#previous").show();
    if (Index < ReviewsDetails.length) {
        PopulateReview(ReviewsDetails[Index]);
    }

    if (Index == ReviewsDetails.length - 1) {
        $("#next").hide();
        $("#previous").show();
        //Index--;
    }
}

function Previous() {
    Index--;
    $("#next").show();
    if (Index >= 0) {
        PopulateReview(ReviewsDetails[Index]);
    }

    if (Index == 0) {
        $("#previous").hide();
        $("#next").show();
        //Index++;
    }

    return;
}

/* -------------------- Creating Picture controls---------------------- */
function AddPictureControls() {
    var counter = 0;

    if ($("#hfPictures").val() != "") {
        counter = $("#hfPictures").val();
    }

    var pictureContainer = $("#picture");
    var well = $("<div/>");
    well.attr("id", "well_" + counter);
    well.attr("class", "well");
    well.attr("style", "padding: 10px;margin-bottom: 5px;");

    var url = $("<input/>");
    url.attr("type", "text");
    url.attr("placeholder", "Picture URL");
    url.attr("style", "width:30%;margin-right:4px");
    url.attr("id", "picImgUrl_" + counter);
    url.attr("name", "picImgUrl_" + counter);
    well.append(url);

    var name = $("<input/>");
    name.attr("type", "text");
    name.attr("placeholder", "Picture Caption");
    name.attr("style", "width:30%;margin-right:4px");
    name.attr("id", "picName_" + counter);
    name.attr("name", "picName_" + counter);
    well.append(name);

    var height = $("<input/>");
    height.attr("type", "text");
    height.attr("placeholder", "Height");
    height.attr("class", "HWtextBoxes");
    height.attr("style", "margin-right:4px");
    height.attr("id", "picImgHeight_" + counter);
    height.attr("name", "picImgHeight_" + counter);
    well.append(height);

    var width = $("<input/>");
    width.attr("type", "text");
    width.attr("placeholder", "Width");
    width.attr("class", "HWtextBoxes");
    width.attr("style", "margin-right:4px");
    width.attr("id", "picImgWidth_" + counter);
    width.attr("name", "picImgWidth_" + counter);
    well.append(width);

    var close = $("<div/>");
    close.attr("class", "btn btn-danger");
    close.attr("onclick", "RemovePicture(" + counter + ")");
    close.html("X");
    well.append(close);

    pictureContainer.append(well);
    counter++;
    $("#hfPictures").val(counter);
}

function RemovePicture(counter) {
    var well = $("#well_" + counter);
    well.remove();
}

function GetPictureJson() {
    var counter = 0;

    if ($("#hfPictures").val() != "") {
        counter = $("#hfPictures").val();
    }

    var picture = [];

    for (var p = 0; p < counter; p++) {
        if ($("#picImgUrl_" + p) != undefined) {
            var imgUrl = $("#picImgUrl_" + p);
            var caption = $("#picName_" + p);
            var imgHight = $("#picImgHeight_" + p);
            var imgWidth = $("#picImgWidth_" + p);

            if (imgUrl == "" || caption == "" || imgHight == "" || imgWidth == "") {
                $("#pictureError").html("All fields are mandatory. Please enter all values");
                $("#pictureError").show();

                return "";
            }
            else {
                var picImg = { "height": imgHight, "width": imgWidth, "url": imgUrl };

                picture.push({ "caption": caption, "image": picImg });
            }
        }
    }

    return JSON.stringify(picture);
}
/*-------------------end-----------------------*/

/*-----------------creating cast controls--------------------*/
function AddCastControls() {
    var counter = 0;

    if ($("#hfCast").val() != "") {
        counter = $("#hfCast").val();
    }

    var pictureContainer = $("#casts");
    var well = $("<div/>");
    well.attr("id", "cast_well_" + counter);
    well.attr("class", "well");
    well.attr("style", "padding: 10px;margin-bottom: 5px;");

    var name = $("<input/>");
    name.attr("type", "text");
    name.attr("placeholder", "Name");
    name.attr("style", "width:30%;margin-right:4px");
    name.attr("id", "castName_" + counter);
    name.attr("name", "castName_" + counter);
    well.append(name);

    var charName = $("<input/>");
    charName.attr("type", "text");
    charName.attr("placeholder", "Picture Caption");
    charName.attr("style", "width:30%;margin-right:4px");
    charName.attr("id", "charName_" + counter);
    charName.attr("name", "charName_" + counter);
    well.append(charName);

    var select = $("<select/>");
    select.attr("id", "role_" + counter);
    select.attr("name", "role_" + counter);

    var option1 = $("<option>");
    option1.attr("value", "0");
    option1.attr("selected", "true");
    option1.html("--Select Role--");
    select.append(option1);

    var option2 = $("<option>");
    option2.attr("value", "Actor");
    option2.html("Actor");
    select.append(option2);

    var option3 = $("<option>");
    option3.attr("value", "Director");
    option3.html("Director");
    select.append(option3);

    var option4 = $("<option>");
    option4.attr("value", "Producer");
    option4.html("Producer");
    select.append(option4);

    var option5 = $("<option>");
    option5.attr("value", "Writer");
    option5.html("Writer");
    select.append(option5);

    well.append(select);

    well.append($("<br>"));

    var label = $("<label/>");
    label.attr("id", "label_" + counter);
    label.html("Cast Image :");
    well.append(label);

    var url = $("<input/>");
    url.attr("type", "text");
    url.attr("placeholder", "Image Url");
    url.attr("style", "width:30%;margin-right:4px");
    url.attr("id", "castImgUrl_" + counter);
    url.attr("name", "castImgUrl_" + counter);
    well.append(url);

    var height = $("<input/>");
    height.attr("type", "text");
    height.attr("placeholder", "Height");
    height.attr("class", "HWtextBoxes");
    height.attr("style", "margin-right:4px");
    height.attr("id", "castImgHeight_" + counter);
    height.attr("name", "castImgHeight_" + counter);
    well.append(height);

    var width = $("<input/>");
    width.attr("type", "text");
    width.attr("placeholder", "Width");
    width.attr("class", "HWtextBoxes");
    width.attr("style", "margin-right:4px");
    width.attr("id", "castImgWidth_" + counter);
    width.attr("name", "castImgWidth_" + counter);
    well.append(width);

    var close = $("<div/>");
    close.attr("class", "btn btn-danger");
    close.attr("onclick", "RemoveCast(" + counter + ")");
    close.html("X");
    well.append(close);

    pictureContainer.append(well);
    counter++;
    $("#hfCast").val(counter);
}

function RemoveCast(counter) {
    var well = $("#cast_well_" + counter);
    well.remove();
}

function GetCastJson() {
    var counter = 0;

    if ($("#hfCast").val() != "") {
        counter = $("#hfCast").val();
    }

    var cast = [];

    for (var p = 0; p < counter; p++) {
        if ($("#castName__" + p) != undefined) {
            var name = $("#castName__" + p);
            var character = $("#charName_" + p);

            var imgUrl = $("#castImgUrl_" + p);
            var imgHight = $("#castImgHeight_" + p);
            var imgWidth = $("#castImgWidth_" + p);

            var role = $("#roles_" + p + " :selected").text();

            if (name == "" || imgUrl == "" || character == "" || imgHight == "" || imgWidth == "" || role == "" || role == "--Select Role--") {
                $("#castError").html("All fields are mandatory. Please enter all values");
                $("#castError").show();

                return "";
            }
            else {
                var img = { "height": imgHight, "width": imgWidth, "url": imgUrl };

                cast.push({ "name": name, "charactername": character, "image": img, "role": role });
            }
        }
    }

    return JSON.stringify(cast);
}
/*-----------------------end---------------------*/

/*------------------Creating poster controls-------------------------*/

function AddPosterControls() {
    var counter = 0;

    if ($("#hfPoster").val() != "") {
        counter = $("#hfPoster").val();
    }

    var pictureContainer = $("#poster");
    var well = $("<div/>");
    well.attr("id", "poster_well_" + counter);
    well.attr("class", "well");
    well.attr("style", "padding: 10px;margin-bottom: 5px;");

    var url = $("<input/>");
    url.attr("type", "text");
    url.attr("placeholder", "Poster URL");
    url.attr("style", "width:30%;margin-right:4px");
    url.attr("id", "posterUrl__" + counter);
    url.attr("name", "posterUrl_" + counter);
    well.append(url);

    var height = $("<input/>");
    height.attr("type", "text");
    height.attr("placeholder", "Height");
    height.attr("class", "HWtextBoxes");
    height.attr("style", "margin-right:4px");
    height.attr("id", "posterHeight_" + counter);
    height.attr("name", "posterHeight_" + counter);
    well.append(height);

    var width = $("<input/>");
    width.attr("type", "text");
    width.attr("placeholder", "Width");
    width.attr("class", "HWtextBoxes");
    width.attr("style", "margin-right:4px");
    width.attr("id", "posterWidth_" + counter);
    width.attr("name", "posterWidth_" + counter);
    well.append(width);

    var close = $("<div/>");
    close.attr("class", "btn btn-danger");
    close.attr("onclick", "RemovePoster(" + counter + ")");
    close.html("X");
    well.append(close);

    pictureContainer.append(well);
    counter++;
    $("#hfPoster").val(counter);
}

function RemovePoster(counter) {
    var well = $("#poster_well_" + counter);
    well.remove();
}

function GetPosterJson() {
    var counter = 0;

    if ($("#hfPoster").val() != "") {
        counter = $("#hfPoster").val();
    }

    var poster = [];

    for (var p = 0; p < counter; p++) {
        if ($("#posterUrl_" + p) != undefined) {
            var imgUrl = $("#posterUrl_" + p);
            var imgHight = $("#posterHeight_" + p);
            var imgWidth = $("#posterWidth_" + p);

            if (imgUrl == "" || imgHight == "" || imgWidth == "") {
                $("#posterError").html("All fields are mandatory. Please enter all values");
                $("#posterError").show();

                return "";
            }
            else {
                poster.push({ "height": imgHight, "width": imgWidth, "url": imgUrl });
            }
        }
    }

    return JSON.stringify(poster);
}

/* --------------------end--------------------- */
/* --------------------Creating controls for songs--------------------- */

function AddSongsControls() {
    var counter = 0;

    if ($("#hfSongs").val() != "") {
        counter = $("#hfSongs").val();
    }

    var pictureContainer = $("#songs");
    var well = $("<div/>");
    well.attr("id", "songs_well_" + counter);
    well.attr("class", "well");
    well.attr("style", "padding: 10px;margin-bottom: 5px;");

    var songName = $("<input/>");
    songName.attr("type", "text");
    songName.attr("placeholder", "Song Name");
    songName.attr("style", "margin-right:4px; width:30%;");
    songName.attr("id", "songName_" + counter);
    songName.attr("name", "songName_" + counter);
    well.append(songName);

    var songUrl = $("<input/>");
    songUrl.attr("type", "text");
    songUrl.attr("placeholder", "Song URL");
    songUrl.attr("style", "width:30%;margin-right:4px");
    songUrl.attr("id", "songUrl_" + counter);
    songUrl.attr("name", "songUrl_" + counter);
    well.append(songUrl);

    var close = $("<div/>");
    close.attr("class", "btn btn-danger");
    close.attr("onclick", "RemoveSong(" + counter + ")");
    close.html("X");
    well.append(close);

    pictureContainer.append(well);
    counter++;
    $("#hfSongs").val(counter);
}

function RemoveSong(counter) {
    var well = $("#songs_well_" + counter);
    well.remove();
}

function GetSongJson() {
    var counter = 0;

    if ($("#hfSongs").val() != "") {
        counter = $("#hfSongs").val();
    }

    var songs = [];

    for (var p = 0; p < counter; p++) {
        if ($("#songName_" + p) != undefined) {
            var songName = $("#songName_" + p);
            var songUrl = $("#songUrl_" + p);

            if (songName == "" || songUrl == "") {
                $("#songsError").html("All fields are mandatory. Please enter all values");
                $("#songsError").show();

                return "";
            }
            else {
                songs.push({ "name": songName, "url": songUrl });
            }
        }
    }

    return JSON.stringify(songs);
}

/* --------------------end--------------------- */

/* --------------------Creating controls for trailer--------------------- */

function AddTrailerControls() {
    var counter = 0;

    if ($("#hfTrailer").val() != "") {
        counter = $("#hfTrailer").val();
    }

    var pictureContainer = $("#trailers");
    var well = $("<div/>");
    well.attr("id", "trailer_well_" + counter);
    well.attr("class", "well");
    well.attr("style", "padding: 10px;margin-bottom: 5px;");

    var songName = $("<input/>");
    songName.attr("type", "text");
    songName.attr("placeholder", "Trailer Name");
    songName.attr("style", "margin-right:4px; width:30%;");
    songName.attr("id", "movieTrailerName_" + counter);
    songName.attr("name", "movieTrailerName_" + counter);
    well.append(songName);

    var songUrl = $("<input/>");
    songUrl.attr("type", "text");
    songUrl.attr("placeholder", "Trailer URL");
    songUrl.attr("style", "width:30%;margin-right:4px");
    songUrl.attr("id", "movieTrailerUrl_" + counter);
    songUrl.attr("name", "movieTrailerUrl_" + counter);
    well.append(songUrl);

    var close = $("<div/>");
    close.attr("class", "btn btn-danger");
    close.attr("onclick", "RemoveTrailer(" + counter + ")");
    close.html("X");
    well.append(close);

    pictureContainer.append(well);
    counter++;
    $("#hfTrailer").val(counter);
}

function RemoveTrailer(counter) {
    var well = $("#trailer_well_" + counter);
    well.remove();
}

function GetTrailerJson() {
    var counter = 0;

    if ($("#hfTrailer").val() != "") {
        counter = $("#hfTrailer").val();
    }

    var trailer = [];

    for (var p = 0; p < counter; p++) {
        if ($("#movieTrailerName_" + p) != undefined) {
            var trailerName = $("#movieTrailerName_" + p);
            var trailerUrl = $("#movieTrailerUrl_" + p);

            if (trailerName == "" || trailerUrl == "") {
                $("#trailerError").html("All fields are mandatory. Please enter all values");
                $("#trailerError").show();

                return "";
            }
            else {
                trailer.push({ "name": trailerName, "url": trailerUrl });
            }
        }
    }

    return JSON.stringify(trailer);
}