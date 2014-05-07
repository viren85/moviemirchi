
var BASE_URL = "http://127.255.0.1:81/";

var MovieCounter = 4;
var MovieIndexer = 0;
var Movie = 4;

var ReviewsDetails = [];
var Index = 0;

var MOVIES = [];

function CallHandler(queryString, OnComp) {
    $.ajax({
        url: BASE_URL + queryString,
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

function LoadUpcomingMovies() {
    var path = "api/Movies?type=upcoming";
    CallHandler(path, onSuccessLoadUpcomingMovies);
}

function onSuccessLoadCurrentMovies(result) {
    result = JSON.parse(result);

    if (result.length > 0) {
        MOVIES = result;

        // adding images        
        for (var i = 0; i < result.length; i++) {
            var list = PopulatingMovies(result[i], "movie-list");
        }

        /*The image width/height shall be calculated once the image is fully loaded*/
        var width = $(document).width();

        $(".movie-list").find("img").each(function () {
            var ratio = this.width / this.height;
            var newWidth = 400 * ratio;
            $(this).width(newWidth + "px").height("380px");

            if (newWidth > 250)
                $(this).width("250px");
        });

        ScaleElement($(".movie-list ul"));

        // movie-list
        PreparePaginationControl($(".movie-list"));
        //PreparePaginationControl($(".news-container"));
    }
}

function onSuccessLoadUpcomingMovies(result) {
    result = JSON.parse(result);

    if (result.length > 0) {
        //MOVIES = result;

        // adding images        
        for (var i = 0; i < result.length; i++) {
            var list = PopulatingMovies(result[i], "upcoming-movie-list");
        }

        /*The image width/height shall be calculated once the image is fully loaded*/
        var width = $(document).width();

        $(".upcoming-movie-list").find("img").each(function () {
            var ratio = this.width / this.height;
            var newWidth = 400 * ratio;
            $(this).width(newWidth + "px").height("380px");

            if (newWidth > 250)
                $(this).width("250px");
        });

        ScaleElement($(".upcoming-movie-list ul"));

        // movie-list
        PreparePaginationControl($(".upcoming-movie-list"), { pagerContainerId: "upcoming-pager" });
        //PreparePaginationControl($(".news-container"));
    }
}


function NextMovies() {
    if (MovieIndexer < MOVIES.length) {
        $("#previousMovies").show();
        $("#currentmovie").html("");
        $("#movieTitle").html("");

        MovieIndexer++;
        Movie++;

        //if (MovieCounter == MOVIES.length) {
        if (Movie == MOVIES.length) {
            $("#nextMovies").hide();
        }

        var movieContainer = $(".movie-list ul");
        $(movieContainer).html("");

        for (var i = MovieIndexer; i < MOVIES.length; i++) {
            MovieCounter++;
            PopulatingMovies(MOVIES[i]);
            //PopulatingMoviesTitle(MOVIES[i]);

            if (MovieCounter % 4 == 0) {
                break;
            }
        }

        var width = $(document).width();

        $(".movie-list").find("img").each(function () {

            var ratio = this.width / this.height;
            var newWidth = 400 * ratio;
            $(this).width(newWidth + "px").height("400px");

            if (newWidth > 270)
                $(this).width("270px");
        });
    }
}

function PreviousMovies() {
    MovieIndexer--;
    Movie--;

    if (MovieIndexer >= 0) {
        $("#nextMovies").show();
        $("#currentmovie").html("");
        $("#movieTitle").html("");

        var movieContainer = $(".movie-list ul");
        $(movieContainer).html("");

        if (Movie == 4) {
            $("#previousMovies").hide();
        }

        for (var i = MovieIndexer; i < MOVIES.length; i++) {
            MovieCounter--;
            PopulatingMovies(MOVIES[i]);

            if (MovieCounter % 4 == 0) {
                break;
            }
        }
    }

    if (MovieIndexer < 0) {
        MovieIndexer = 0;
    }

    var width = $(document).width();

    $(".movie-list").find("img").each(function () {

        var ratio = this.width / this.height;
        var newWidth = 400 * ratio;
        $(this).width(newWidth + "px").height("400px");

        if (newWidth > 270)
            $(this).width("270px");
    });
}

function showCaption(element) {
    /*var movieDiv = $("." + unqname)[0];
    var picCaption = $("#captionCredit", movieDiv)[0];
    picCaption.style.display = "inline";*/
    $(element).find(".captionAndNavigate").each(function () {
        $(this).height("370px");
    });

    $(element).find(".movie-synopsis").each(function () {
        $(this).show();
    });

    /* $(element).css("-webkit-transform", "scale(1.05)");
     $(element).css("-ms-transform", "scale(1.05)");
     $(element).css("-moz-transform", "scale(1.05)");
     $(element).css("-o-transform", "scale(1.05)");*/
}

function hideCaption(element) {
    /*var movieDiv = $("." + unqname)[0];
    var picCaption = $("#captionCredit", movieDiv)[0];
    picCaption.style.display = "none";*/
    $(element).find(".captionAndNavigate").each(function () {
        $(this).height("auto");
    });

    $(element).find(".movie-synopsis").each(function () {
        $(this).hide();
    });
}

function PopulatingMoviesTitle(movieTitle, parentElment) {
    ////var name = $("<div/>");
    ////var movieName = $("<span/>");
    ////name.attr("class", "movie-title");
    ////movieName.html(movieTitle.Name).attr("style", "position: absolute;z-index: 2;color: white;font-size: 24px;text-align: center;width: 18%;margin-top: 20%;");
    ////parentElment.append(name);
    ////parentElment.append(movieName);
}

// VS - Commenting the previous implementation of movie page using flipboard effect.
/*
function LoadSingleMovie(movieId) {
    var path = "../api/MovieInfo?q=" + movieId;
    var reviewPath = "../api/MovieReview?q=" + movieId;

    CallHandler(path, onSuccessLoadSingleMovie);
    CallHandler(reviewPath, onSuccessLoadMovieReviews);
}

function onSuccessLoadSingleMovie(result) {
    result = JSON.parse(result);

    if (result.Movie != undefined) {

        $(".content .movie-name").html(result.Movie.Name);// + "(" + result.Movie.Year + ")");

        var poster = [];
        poster = JSON.parse(result.Movie.Posters);

        if (poster != "undefined" && poster != null && poster.length > 0) {
            $("img.home-poster").attr("src", "/Posters/Images/" + poster[poster.length - 1]);

            //showing movies posters
            for (var p = 0; p < poster.length; p++) {
                var img = $("<img/>")
                img.attr("class", "gallery-image");
                img.attr("alt", result.Movie.Name);
                img.attr("src", "/Posters/Images/" + poster[p]);
                img.error(function () {
                    $(this).hide();
                });

                $("#imagearea").append(img);
            }
        }
        else {
            $("img.home-poster").attr("src", "/Posters/Images/default-movie.jpg");

            if (poster.length < 2)
                $("#item2").remove();
        }

        /*else if (poster.length < 2) {
            $("#imagearea").parent(".bb-item").remove();
        }* /

        $("div.genre").html(result.Movie.Genre);
        $("p.story-plot").html(result.Movie.Synopsis);

        var directors = "", directorsList = "";
        var writers = "", writerList = "";
        var producers = "", producersList = "";
        var music = "", musicList = "";
        var cast = "", actorList = "";
        var songsList = "";

        var casts = [], songs = [];
        casts = JSON.parse(result.Movie.Casts);
        songs = JSON.parse(result.Movie.Songs);

        if (casts != "undefined" && casts != null && casts.length > 0) {
            for (var c = 0; c < casts.length; c++) {

                if (casts[c].role.toLowerCase() == "director" && casts[c].name != null) {
                    if (casts[c].charactername == null) {
                        directors += "<a href='javascript:void(0);' title='click here to view profile'>" + casts[c].name + "</a>, ";
                        directorsList += "<li class='team-item'><a>" + casts[c].name + "</a></li>";
                    }
                }
                else if (casts[c].role.toLowerCase() == "writer" && casts[c].name != null) {
                    writers += "<a href='javascript:void(0);' title='click here to view profile'>" + casts[c].name + "</a>, ";
                    writerList += "<li class='team-item'><a>" + casts[c].name + "</a></li>";
                }
                else if (casts[c].role.toLowerCase() == "music" && casts[c].name != null) {
                    if (casts[c].charactername == null) {
                        music += "<a href='javascript:void(0);' title='click here to view profile'>" + casts[c].name + "</a>, ";
                        musicList += "<li class='team-item'><a>" + casts[c].name + "</a></li>";
                    }
                }
                else if (casts[c].role.toLowerCase() == "producer" && casts[c].name != null) {
                    if (casts[c].charactername == "producer") {
                        producers += "<a href='javascript:void(0);' title='click here to view profile'>" + casts[c].name + "</a>, ";
                        producersList += "<li class='team-item'><a>" + casts[c].name + "</a></li>";
                    }
                }
                else if (casts[c].role.toLowerCase() == "actor") {
                    cast += "<a href='javascript:void(0);' title='click here to view profile'> " + casts[c].name + "</a>, ";
                    actorList += "<li class='cast-item'><span class='cast-details'><a>" + casts[c].name + "</a></span><span class='cast-details-right'>" + casts[c].charactername + "</span></li>";
                }
            }
        }
        else {
            $("#item3").remove();
        }

        if (songs != "undefined" && songs != null && songs.length > 0) {
            for (var s = 0; s < songs.length; s++) {
                if (songs[s].SongTitle != "") {
                    songsList += "<li class='song-item'>";
                    songsList += "<div class='song-details'><span>Title: </span>" + songs[s].SongTitle + "</div>";

                    if (songs[s].Performer != "") {
                        songsList += "<div class='song-details'><span>Performer: </span>" + songs[s].Performer + "</div>";
                    }

                    if (songs[s].Lyrics != "") {
                        songsList += "<div class='song-details'><span>Lyrics: </span>" + songs[s].Lyrics + "</div>";
                    }

                    songsList += "</li>";
                }
            }
        }
        else {
            $("#item4").remove();
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
        if (music.length > 0) {
            music = music.substring(0, music.lastIndexOf(","));
        }
        if (producers.length > 0) {
            producers = producers.substring(0, producers.lastIndexOf(","));
        }


        $("#director").html("<span class='role-name'>Director:</span><span class='role-details'>" + directors + "</span>");
        $("#writer").html("<span class='role-name'>Writer :</span><span class='role-details'>" + writers + "</span>");
        $("#producer").html("<span class='role-name'>Producer:</span><span class='role-details'>" + producers + "</span>");
        $("#music").html("<span class='role-name'>Music Director:</span><span class='role-details'>" + music + "</span>");
        //$("#cast").html("<b>Cast :</b> " + cast);

        $("#cast-list").html(actorList);
        $("#director-list").html(directorsList);
        $("#producer-list").html(producersList);
        $("#writer-list").html(writerList);
        $("#music-list").html(musicList);
        $("#song-list").html(songsList);


        var ratings = [];
        ratings = JSON.parse(result.Movie.Ratings);

        $(".critic-rating").html(result.Movie.Ratings);
        $("#stats").html(result.Movie.Stats);

        var rate = Math.round(result.Movie.Ratings);

        for (var i = 1; i <= rate; i++) {
            var star = $("<img/>").addClass("star-image").attr("src", "../Images/small_gold_star.png");
            $(".star").append(star);
        }

        var width = $(document).width();
        $("#imagearea").find("img").each(function () {
            var ratio = this.width / this.height;
            var newWidth = 200 * ratio;
            $(this).width(newWidth + "px").height("200px");
        });

        InitPage();
        Page.init();
    }
}*/

function onSuccessLoadMovieReviews(result) {

    // populating Reviews
    result = JSON.parse(result);

    if (result.MovieReviews != undefined) {
        var reviews = [];
        //reviews = JSON.parse(result.MovieReviews);
        reviews = result.MovieReviews;

        if (reviews.length > 0) {

            var ul = $("#reviewer-list");

            // Clean loader image and any other child elements before adding fresh list of the reviewers
            $(ul).html("");
            $("#detailed-review").html("");

            for (var r = 0; r < reviews.length; r++) {
                var li = $("<li>");
                var div = $("<div>");
                div.attr("class", "reviewer");
                div.attr("review-id", r);

                div.click(function () {
                    var id = $(this).attr("review-id");
                    $("#detailed-review").find("div.critic-movie-review").each(function () {
                        if ($(this).attr("review-id") == id) {
                            $(this).show();
                        }
                        else {
                            $(this).hide();
                        }
                    });

                    $("#detailed-review").find("div.critic-review").each(function () {
                        if ($(this).attr("review-id") == id) {
                            $(this).show();
                        }
                        else {
                            $(this).hide();
                        }
                    });

                    $(this).parent().parent().find("li").css("background-color", "#FFF");
                    $(this).parent().css("background-color", "#EEE");
                });

                var img = $("<img/>")
                img.attr("alt", reviews[r].ReviewerName);

                if (reviews[r].OutLink != "")
                    img.attr("src", reviews[r].OutLink);
                else
                    img.attr("src", "/Images/user.png");

                img.attr("title", reviews[r].ReviewerName);

                var anchor = $("<a/>");
                anchor.attr("href", "Movie/Reviewer?name=" + reviews[r].ReviewerName);
                anchor.append(img);

                var reviewer = $("<div/>");
                reviewer.attr("class", "reviewer-name");
                reviewer.attr("title", reviews[r].ReviewerName);

                var reviewerName = reviews[r].ReviewerName;

                if (reviews[r].ReviewerName.length > 31) {
                    reviews[r].ReviewerName = reviews[r].ReviewerName.substring(0, 31) + "...";
                }

                reviewer.html("<a href='Movie/Reviewer?name=" + reviewerName + "'>" + reviews[r].ReviewerName + "</a>");

                //div.append(img);
                div.append(anchor);
                div.append(reviewer);

                var review = $("<div>");
                review.attr("class", "reviewer-affiliation");
                review.html(reviews[r].Affiliation);

                div.append(review);
                li.append(div);

                ul.append(li);

                var reviewDetail = $("<div/>").html(reviews[r].Review).attr("class", "critic-movie-review").attr("review-id", r).hide();

                var criticReview = GetCriticDetails(reviews[r].ReviewerName, reviews[r].OutLink, reviews[r].Affiliation, reviews[r].ReviewerRating);
                $(criticReview).attr("review-id", r);
                $("#detailed-review").append(criticReview);


                $("#detailed-review").append(reviewDetail);

                if (r == 0) {
                    $("#reviewer-list").find("li:first").css("background-color", "#EEE");
                    reviewDetail.show();
                }
            }
        }
        else {
            $("#reviewer-list").html("<li>Currently this movie does not have any reviews by critics.</li>");
            //$("#item5").remove();
        }
    }
}

function GetCriticDetails(criticName, criticImagePath, criticAffiliation, criticRatingNumber) {
    var critic = $("<div/>").attr("class", "critic-review");

    var criticName = $("<div/>").attr("class", "critic-name").html(criticName);
    var criticImage = $("<img/>").attr("class", "critic-review-image");
    var criticAffiliation = $("<div/>").attr("class", "critic-affiliation").html(criticAffiliation);
    var criticRate = $("<div/>").attr("class", "critic-rate-container");
    var criticRating = $("<div/>").attr("class", "critic-rate").html("Rating: " + criticRatingNumber);
    var criticRatingStars = $("<div/>").css("float", "right").css("width", "100%");

    var rate = Math.round(criticRatingNumber);

    for (var i = 1; i <= rate; i++) {
        var star = $("<img/>").addClass("star-image").attr("src", "../Images/small_gold_star.png");
        $(criticRatingStars).append(star);
    }

    $(criticRate).append(criticRating);
    $(criticRate).append(criticRatingStars);



    if (criticImagePath != "") {
        $(criticImage).attr("src", criticImagePath);
    }
    else {
        $(criticImage).attr("src", "/Images/user.png");
    }

    $(critic).append(criticImage);
    $(critic).append(criticName);
    $(critic).append(criticAffiliation);
    $(critic).append(criticRate);
    return critic;
}

function GetReviewerAndReviews(name, movie) {
    var path = "api/ReviewerInfo?name=" + name;

    CallHandler(path, onSuccessPopulateReviewsAndReviews);
}

function onSuccessPopulateReviewsAndReviews(result) {
    result = JSON.parse(result);


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
    //well.attr("style", "padding: 10px;margin-bottom: 5px;");

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
        if ($("#picImgUrl_" + p).val() != undefined) {
            var imgUrl = $("#picImgUrl_" + p).val();
            var caption = $("#picName_" + p).val();
            var imgHight = $("#picImgHeight_" + p).val();
            var imgWidth = $("#picImgWidth_" + p).val();

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
    //well.attr("style", "padding: 10px;margin-bottom: 5px;");

    var name = $("<input/>");
    name.attr("type", "text");
    name.attr("placeholder", "Name");
    name.attr("style", "width:30%;margin-right:4px");
    name.attr("id", "castName_" + counter);
    name.attr("name", "castName_" + counter);
    well.append(name);

    var charName = $("<input/>");
    charName.attr("type", "text");
    charName.attr("placeholder", "Character Name");
    charName.attr("style", "width:30%;margin-right:4px");
    charName.attr("id", "charName_" + counter);
    charName.attr("name", "charName_" + counter);
    well.append(charName);

    var select = $("<select/>");
    select.attr("id", "roles_" + counter);
    select.attr("name", "roles_" + counter);

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
        if ($("#castName_" + p).val() != undefined) {
            var name = $("#castName_" + p).val();
            var character = $("#charName_" + p).val();

            var imgUrl = $("#castImgUrl_" + p).val();
            var imgHight = $("#castImgHeight_" + p).val();
            var imgWidth = $("#castImgWidth_" + p).val();

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
    //well.attr("style", "padding: 10px;margin-bottom: 5px;");

    var url = $("<input/>");
    url.attr("type", "text");
    url.attr("placeholder", "Poster URL");
    url.attr("style", "width:30%;margin-right:4px");
    url.attr("id", "posterUrl_" + counter);
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
            var imgUrl = $("#posterUrl_" + p).val();
            var imgHight = $("#posterHeight_" + p).val();
            var imgWidth = $("#posterWidth_" + p).val();

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
    //well.attr("style", "padding: 10px;margin-bottom: 5px;");

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
            var songName = $("#songName_" + p).val();
            var songUrl = $("#songUrl_" + p).val();

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
    //well.attr("style", "padding: 10px;margin-bottom: 5px;");

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
            var trailerName = $("#movieTrailerName_" + p).val();
            var trailerUrl = $("#movieTrailerUrl_" + p).val();

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

function ClearMoviesControl() {
    $("#bottomError").html("");
    $("#bottomError").show();

    $("#movieName").val("");
    $("#movieAltName").val("");
    $("#synops").val("");
    $('#genre :selected').text("--Genre--");

    $('#month :selected').text("-- Select Month--");
    $("#year").val("");

    ClearCastControl();
    ClearPictureControl();
    ClearPosterControl();
    ClearSongsControl();
    ClearTrailerControl();

    //clear ratings controls 
    $("#systemRating").val("");
    $("#criticRating").val("");
    $("#hot").prop("checked", true);

    // clear budget and boxoffice control
    $("#budget").val("");
    $("#boxOffice").val("");
}

function ClearCastControl() {
    var counter = 0;

    $("#castError").html("");
    $("#castError").hide();

    if ($("#hfCast").val() != "") {
        counter = $("#hfCast").val();
    }

    for (var p = 0; p < counter; p++) {
        if ($("#cast_well_" + p) != undefined) {
            if (p == 0) {
                $("#castName_" + p).val("");
                $("#charName_" + p).val("");
                $("#roles_" + p + " :selected").text("--Select Role--");
                $("#castImgUrl_" + p).val("");
                $("#castImgHeight_" + p).val("");
                $("#castImgWidth_" + p).val("");
            } else {
                $("#cast_well_" + p).remove();
            }
        }
    }
}

function ClearPictureControl() {
    var counter = 0;

    $("#pictureError").html("");
    $("#pictureError").hide();

    if ($("#hfPictures").val() != "") {
        counter = $("#hfPictures").val();
    }

    for (var p = 0; p < counter; p++) {
        if ($("#well_" + p) != undefined) {
            if (p == 0) {
                $("#picImgUrl_" + p).val("");
                $("#picName_" + p).val("");
                $("#picImgHeight_" + p).val("");
                $("#picImgWidth_" + p).val("");
            } else {
                $("#well_" + p).remove();
            }
        }
    }
}

function ClearPosterControl() {
    var counter = 0;

    $("#posterError").html("");
    $("#posterError").hide();

    if ($("#hfPoster").val() != "") {
        counter = $("#hfPoster").val();
    }

    for (var p = 0; p < counter; p++) {
        if ($("#poster_well_" + p) != undefined) {
            if (p == 0) {
                $("#posterUrl_" + p).val("");
                $("#posterHeight_" + p).val("");
                $("#posterWidth_" + p).val("");
            } else {
                $("#poster_well_" + p).remove();
            }
        }
    }
}

function ClearSongsControl() {
    var counter = 0;

    $("#songsError").html("");
    $("#songsError").hide();

    if ($("#hfSongs").val() != "") {
        counter = $("#hfSongs").val();
    }

    for (var p = 0; p < counter; p++) {
        if ($("#songs_well_" + p) != undefined) {
            if (p == 0) {
                $("#songName_" + p).val("");
                $("#songUrl_" + p).val("");
            } else {
                $("#songs_well_" + p).remove();
            }
        }
    }
}
function ClearTrailerControl() {
    var counter = 0;

    $("#trailerError").html("");
    $("#trailerError").hide();

    if ($("#hfTrailer").val() != "") {
        counter = $("#hfTrailer").val();
    }

    for (var p = 0; p < counter; p++) {
        if ($("#trailer_well_" + p) != undefined) {
            if (p == 0) {
                $("#movieTrailerName_" + p).val("");
                $("#movieTrailerUrl_" + p).val("");
            } else {
                $("#trailer_well_" + p).remove();
            }
        }
    }
}

/*Creating Affilation Control*******************/
function AddAffilationControl() {
    var counter = 0;

    if ($("#hfAffilation").val() != "") {
        counter = $("#hfAffilation").val();
    }

    var affilationContainer = $("#affilation");

    var well = $("<div/>");
    well.attr("id", "affilation_well_" + counter);
    well.attr("class", "col-sm-12");
    well.attr("style", "display:block;");

    //generating affilation name
    var affilationName = $("<input/>");
    affilationName.attr("style", "width: 35%;padding: 1%;border-radius: 4px;margin-left: 2%;margin-bottom: 1%;border-width: 1px;");
    affilationName.attr("name", "affilationName");
    affilationName.attr("type", "text");
    affilationName.attr("placeholder", "Affilation Name");
    affilationName.attr("id", "affilationName_" + counter);
    well.append(affilationName);
    well.append($("<br>"));



    //generating website Name
    var websiteName = $("<input/>");
    websiteName.attr("style", "width: 35%;padding: 1%;border-radius: 4px;margin-left: 2%;margin-bottom: 1%;border-width: 1px;");
    websiteName.attr("type", "text");
    websiteName.attr("placeholder", "Website Name");
    websiteName.attr("name", "websiteName");
    websiteName.attr("id", "websiteName_" + counter);
    well.append(websiteName);
    well.append($("<br>"));

    //generating website Link
    var websiteLink = $("<input/>");
    websiteLink.attr("style", "width: 35%;padding: 1%;border-radius: 4px;margin-left: 2%;margin-bottom: 1%;border-width: 1px;");
    websiteLink.attr("type", "text");
    websiteLink.attr("placeholder", "Website Link");
    websiteLink.attr("name", "websiteLink");
    websiteLink.attr("id", "websiteLink_" + counter);
    well.append(websiteLink);
    well.append($("<br>"));

    //generating logo link
    var logoLink = $("<input/>");
    logoLink.attr("style", "width: 35%;padding: 1%;border-radius: 4px;margin-left: 2%;margin-bottom: 1%;border-width: 1px;");
    logoLink.attr("type", "text");
    logoLink.attr("placeholder", "Logo Link");
    logoLink.attr("name", "logoLink");
    logoLink.attr("id", "logoLink_" + counter);
    well.append(logoLink);
    well.append($("<br>"));

    //generating country
    var country = $("<input/>");
    country.attr("style", "width: 35%;padding: 1%;border-radius: 4px;margin-left: 2%;margin-bottom: 1%;border-width: 1px;");
    country.attr("type", "text");
    country.attr("placeholder", "Country");
    country.attr("name", "Country");
    country.attr("id", "country_" + counter);
    well.append(country);


    //
    var close = $("<input/>");
    close.attr("type", "button");
    close.attr("style", "margin-left:1%;");
    close.attr("class", "btn btn-danger");
    close.attr("value", "Remove");
    close.attr("name", "ramove");
    close.attr("onClick", "RemoveAffilation(" + counter + ")");
    //close.html("X");
    well.append(close);

    //remove div
    // var close1 = $("<div/>");
    //close1.attr("class", "btn btn-danger");
    //close1.attr("onClick", "RemoveAffilation(" + counter + ")");
    //close1.html("X");
    // well.append(close1);

    affilationContainer.append(well);
    counter++;

    $("#hfAffilation").val(counter);
}

function RemoveAffilation(counter) {
    var well = $("#affilation_well_" + counter);
    well.remove();
}
/*End*/

function SaveUserFavorite() {
    var actorName = $("#txtSearchActor").val();
    var directorName = $("#txtSearchDirector").val();
    var musicDirectorName = $("#txtSearchMusicDirector").val();
    var userId = $("#hfUserId").val();

    var isValid = false;

    var FavoriteList = [];

    $("#fav_actor").find('input[type="checkbox"]').each(function () {
        if ($(this).attr("checked")) {
            isValid = true;
            var acName = $(this).next("label").html();
            FavoriteList.push({ "Type": "Actor", "Name": acName });
        }
    });

    $("#fav_director").find('input[type="checkbox"]').each(function () {
        if ($(this).attr("checked")) {
            isValid = true;
            var dirName = $(this).next("label").html();
            //alert(dirName);
            FavoriteList.push({ "Type": "Director", "Name": dirName });
        }
    });

    $("#fav_musicdirector").find('input[type="checkbox"]').each(function () {
        if ($(this).attr("checked")) {
            isValid = true;
            var musDirName = $(this).next("label").html();
            //alert(musDirName);
            FavoriteList.push({ "Type": "Music Director", "Name": musDirName });
        }
    });

    $("#fav_genre").find('input[type="checkbox"]').each(function () {
        if ($(this).attr("checked")) {
            isValid = true;
            var genre = $(this).next("label").html();
            //alert(genre);
            FavoriteList.push({ "Type": "Genre", "Name": genre });
        }
    });

    if (actorName != "") {
        isValid = true;
        FavoriteList.push({ "Type": "Actor", "Name": actorName });
    }

    if (directorName != "") {
        isValid = true;
        FavoriteList.push({ "Type": "Director", "Name": directorName });
    }

    if (musicDirectorName != "") {
        isValid = true;
        FavoriteList.push({ "Type": "Music Director", "Name": musicDirectorName });
    }

    if (userId != "") {
        //setCookie("favoriteId", "userid", 365);
    }

    var cFavoriteId = getCookie("favoriteId");

    if (isValid) {
        CallHandler("api/SaveUserFavorite?u=" + userId + "&c=" + cFavoriteId + "&d=" + encodeURI(JSON.stringify(FavoriteList)), OnSuccessSaveUserFavorite);
    }
    else {
        $("#favStatus").attr("style", "display:block");
        $("#favStatus").html("Please enter actor name or select genre");
    }
}

function OnSuccessSaveUserFavorite(result) {
    result = JSON.parse(result);

    if (result.Status == "Ok") {

        if (result.Message == "Set Cookie") {
            setCookie("favoriteId", result.FavoriteId, 365);
        }

        ClearUserFavoriteControls();

        $("#favStatus").removeAttr("class");
        $("#favStatus").attr("class", "alert alert-success");
        $("#favStatus").attr("style", "display:block");
        $("#favStatus").html("Successfully saved your favorite list.");

        var intverval = setInterval(function () {
            $(".user-fav").slideToggle("slow");
            clearInterval(intverval);
            $("#favStatus").html("");
            $("#favStatus").hide();
        }, 10000);

    }
    else {
        $("#favStatus").attr("style", "display:block");
        $("#favStatus").html("Ops! there is some error. Please try again.");

        if (result.Message == "No Updated") {
            $("#favStatus").html("You already set your favorites. Login for update.");
        }

        var intverval = setInterval(function () {
            $("#favStatus").hide(500);
            $("#favStatus").html("");
            clearInterval(intverval);
        }, 10000);
    }
}

function ClearUserFavoriteControls() {
    $("#fav_actor").find('input[type="checkbox"]').each(function () {
        $(this).attr("checked", false);
    });

    $("#fav_director").find('input[type="checkbox"]').each(function () {
        $(this).attr("checked", false);
    });

    $("#fav_musicdirector").find('input[type="checkbox"]').each(function () {
        $(this).attr("checked", false);
    });

    $("#fav_genre").find('input[type="checkbox"]').each(function () {
        $(this).attr("checked", false);
    });

    $("#txtSearchActor").val("");
    $("#txtSearchDirector").val("");
    $("#txtSearchMusicDirector").val("");
}

/* -------------- Validate email address ---------------- */
function IsEmailValid(emailText) {
    var pattern = /^[a-zA-Z0-9\-_]+(\.[a-zA-Z0-9\-_]+)*@[a-z0-9]+(\-[a-z0-9]+)*(\.[a-z0-9]+(\-[a-z0-9]+)*)*\.[a-z]{2,4}$/;
    if (pattern.test(emailText)) {
        return true;
    } else {
        return false;
    }
}

/*------ Login ----------------------------------------*/
function authenticateUser() {
    var isValid = true;
    try {
        var username = $("#signin_email").val();
        var loginPassword = $("#signin_password").val();

        if (username == "") {
            $("#loginError").html("Email address require.");
            $("#loginError").show();
            $("#signin_email").focus();
            isValid = false;
            return;
        }

        if (loginPassword == "") {
            $("#loginError").html("Password require.");
            $("#loginError").show();
            $("#signin_password").focus();
            isValid = false;
            return;
        }
        //CallHandler("Login/UserLogin", encodeURI(JSON.stringify(hfLogin)));

        if (isValid) {
            var hflogin = ({ "UserName": username, "Email": username, "Password": loginPassword });
            // $("#hfLogin").val(JSON.stringify(hfLogin));

            $.ajax({
                url: BASE_URL + 'Login/UserLogin',
                data: { "hfLogin": JSON.stringify(hflogin) },
                type: 'Post',
                dataType: 'json',
                success: ShowSuccessMessageLogin,
                error: function (xhr, status, error) {
                }
            });
        }


    } catch (ex) {
        $("#loginError").html("There some error please try again.");
        $("#loginError").show();
    }


}

function ShowSuccessMessageLogin(result) {
    if (result.Status == "Ok") {
        window.location = BASE_URL + 'Home/Index';
    } else if (result.Status == "Require") {
        $("#loginError").html("Username and Password require.");
        $("#loginError").show();
        ClearLoginformData();
    } else if (result.Status == "Error") {
        $("#loginError").html("Login Failed, Try again.");
        $("#loginError").show();
    }
}


function ClearLoginformData() {

    $("#signin_email").val("");
    $("#signin_password").val("");
    $("#loginError").hide("");
    //$("#Login"). data-dismiss("modal");
}


/*Register the user from popup  -------------------*/
function RegisterUser() {
    var isValid = true;
    try {
        var fname = $("#FirstName").val();
        var lname = $("#LastName").val();
        var email = $("#Email1").val();
        var pwd = $("#password2").val();
        var confirmPassword = $("#password3").val();

        if (fname == "") {
            $("#registerError").html("Please provide First Name.");
            $("#registerError").show();
            $("#FirstName").focus();
            isValid = false;
            return;
        }


        if (lname == "") {
            $("#registerError").html("Please provide Last Name.");
            $("#registerError").show();
            $("#LastName").focus();
            isValid = false;
            return;
        }

        if (email == "") {
            $("#registerError").html("Please provide email address.");
            $("#registerError").show();
            $("#Email1").focus();
            isValid = false;
            return;
        }

        if (pwd == "") {
            $("#registerError").html("Please provide password.");
            $("#registerError").show();
            $("#password2").focus();
            isValid = false;
            return;
        }

        if (confirmPassword == "") {
            $("#registerError").html("Please provide confirm password.");
            $("#registerError").show();
            $("#password3").focus();
            isValid = false;
            return;
        }

        if (!IsEmailValid(email)) {
            $("#registerError").html("Please provide valid email address.");
            $("#registerError").show();
            $("#Email1").focus();
            isValid = false;
            return;
        }

        if (pwd != confirmPassword) {
            $("#registerError").html("Password and confirm password does not match.");
            $("#registerError").show();
            $("#password3").focus();
            isValid = false;
            return;
        }

        if (isValid) {
            var user = {
                "FirstName": fname,
                "LastName": lname,
                "UserName": email,
                "Email": email,
                "Password": pwd,
                "Mobile": confirmPassword // mobile no use as confirm password
            };
            //$("#hfAffilations").val(JSON.stringify(user));
            $.ajax({

                url: 'Login/Register',
                data: { "userJson": JSON.stringify(user) },
                type: 'POST',
                dataType: 'json',
                success: ShowSuccessMessage,
                error: function (xhr, status, error) {
                }
            });
        }
    } catch (e) {
        $("#registerError").html("There some error please try again.");
        $("#registerError").show();
    }
}

function ShowSuccessMessage(result) {

    if (result.Status == "Ok") {
        $("#successStatusR").html("You are successfully register. Please login to access your account.");
        $("#successStatusR").show();
        ClearformData();
    } else if (result.Status == "Error") {
        if (result.Message != undefined) {
            $("#registerError").html(result.Message);
        }

        $("#registerError").show();
    }
    else {
        $("#registerError").html("Unable to register. Please try again after some time.");
        $("#registerError").show();
    }


}

function ClearformData() {
    $("#FirstName").val("");
    $("#LastName").val("");
    $("#Email1").val("");
    $("#password2").val("");
    $("#password3").val("");
    $("#registerError").hide("");
}

/* end Login*/


/*--------------Populating user favorit----------------*/
function PopulatingUserFavorite() {
    var url = "api/GetCastByType?t=all&l=3";
    CallHandler(url, OnSuccessPopulatingUserFavorite);
}


function OnSuccessPopulatingUserFavorite(result) {
    result = JSON.parse(result);

    if (result.Status == "Ok") {
        /* getting actor */
        for (var a = 0; a < result.Actor.length; a++) {
            //alert(result.Actor[a].name);
            var span = $("<span/>");
            var checkBox = $("<input/>");
            $(checkBox).attr("type", "checkbox");
            $(checkBox).attr("id", "fav_actor_" + a);

            var checkBoxLabel = $("<label/>");
            $(checkBoxLabel).attr("for", "fav_actor_" + a);
            $(checkBoxLabel).html(result.Actor[a].name);

            $(span).append(checkBox);
            $(span).append(checkBoxLabel);

            $("#fav_actor").append(span);
        }
        // adding textbox for search acotr
        var span = $("<span/>");
        $(span).attr("class", "fav-span");
        var searchActor = $("<input/>");
        $(searchActor).attr("type", "text");
        $(searchActor).attr("id", "txtSearchActor");
        $(searchActor).attr("placeholder", "Actor Name...");
        $(searchActor).attr("class", "form-control");

        $(searchActor).keyup(function (e) {
            //Fetching the textbox value.
            if (e.keyCode == 40 || e.keyCode == 38) {
            }
            else {
                var query = $(this).val();
                //Calling GetItems method.
                autoCompleteTextBox1(query, "AutoComplete/AutoCompleteActors", "autoCompleteActor", "actorUL", "txtSearchActor", "hfMovieId");
            }
        });

        var autoCompleteActor = $("<div>");
        $(autoCompleteActor).attr("id", "autoCompleteActor");

        $(autoCompleteActor).append(searchActor);

        $(span).append(autoCompleteActor);
        //$(span).append(searchActor);
        $("#fav_actor").append(span);

        /* getting director */
        for (var d = 0; d < result.Director.length; d++) {

            var span = $("<span/>");
            var checkBox = $("<input/>");
            $(checkBox).attr("type", "checkbox");
            $(checkBox).attr("id", "fav_director_" + d);

            var checkBoxLabel = $("<label/>");
            $(checkBoxLabel).attr("for", "fav_director_" + d);
            $(checkBoxLabel).html(result.Director[d].name);

            $(span).append(checkBox);
            $(span).append(checkBoxLabel);

            $("#fav_director").append(span);
        }
        // adding textbox for search acotr
        var dSpan = $("<span/>");
        $(dSpan).attr("class", "fav-span");
        var searchDirector = $("<input/>");
        $(searchDirector).attr("type", "text");
        $(searchDirector).attr("id", "txtSearchDirector");
        $(searchDirector).attr("placeholder", "Director Name...");
        $(searchDirector).attr("class", "form-control");

        $(searchDirector).keyup(function (e) {
            //Fetching the textbox value.
            if (e.keyCode == 40 || e.keyCode == 38) {
            }
            else {
                var query = $(this).val();
                //Calling GetItems method.
                autoCompleteTextBox1(query, "AutoComplete/AutoCompleteDirectors", "autoCompleteDirector", "directorUL", "txtSearchDirector", "hfMovieId");
            }
        });

        var autoCompleteDirector = $("<div>");
        $(autoCompleteDirector).attr("id", "autoCompleteDirector");

        $(autoCompleteDirector).append(searchDirector);

        $(dSpan).append(autoCompleteDirector);

        //$(dSpan).append(searchDirector);
        $("#fav_director").append(dSpan);

        /* getting music director*/
        for (var md = 0; md < result.MusicDirector.length; md++) {

            var span = $("<span/>");
            var checkBox = $("<input/>");
            $(checkBox).attr("type", "checkbox");
            $(checkBox).attr("id", "fav_musicdirector_" + md);

            var checkBoxLabel = $("<label/>");
            $(checkBoxLabel).attr("for", "fav_musicdirector_" + md);
            $(checkBoxLabel).html(result.MusicDirector[md].name);

            $(span).append(checkBox);
            $(span).append(checkBoxLabel);

            $("#fav_musicdirector").append(span);
        }

        // adding textbox for search acotr
        var mdSpan = $("<span/>");
        $(mdSpan).attr("class", "fav-span");
        var searchMusicDirector = $("<input/>");
        $(searchMusicDirector).attr("type", "text");
        $(searchMusicDirector).attr("id", "txtSearchMusicDirector");
        $(searchMusicDirector).attr("placeholder", "Music Director Name...");
        $(searchMusicDirector).attr("class", "form-control");

        $(searchMusicDirector).keyup(function (e) {
            //Fetching the textbox value.
            if (e.keyCode == 40 || e.keyCode == 38) {
            }
            else {
                var query = $(this).val();
                //Calling GetItems method.
                autoCompleteTextBox1(query, "AutoComplete/AutoCompleteMusicDirectors", "autoCompleteMusicDirector", "musicDirectorUL", "txtSearchMusicDirector", "hfMovieId");
            }
        });

        var autoCompleteMusicDirector = $("<div>");
        $(autoCompleteMusicDirector).attr("id", "autoCompleteMusicDirector");

        $(autoCompleteMusicDirector).append(searchMusicDirector);

        $(mdSpan).append(autoCompleteMusicDirector);

        //$(mdSpan).append(searchMusicDirector);
        $("#fav_musicdirector").append(mdSpan);
    }
}
/*--------------end user favorit----------------*/

/*--------------cookies related functions----------------*/
function setCookie(cname, cvalue, exdays) {
    var d = new Date();
    d.setTime(d.getTime() + (exdays * 24 * 60 * 60 * 1000));
    var expires = "expires=" + d.toGMTString();
    document.cookie = cname + "=" + cvalue + "; " + expires;
}

function getCookie(cname) {
    var name = cname + "=";
    var ca = document.cookie.split(';');
    for (var i = 0; i < ca.length; i++) {
        var c = ca[i].trim();
        if (c.indexOf(name) == 0) return c.substring(name.length, c.length);
    }
    return "";
}
/*--------------end cookies related functions----------------*/