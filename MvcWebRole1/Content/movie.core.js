//var BASE_URL = "http://127.0.0.1:81/";
var BASE_URL = "";

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
        console.log(1);
        // adding images        
        for (var i = 0; i < result.length; i++) {
            var list = PopulatingMovies(result[i], "movie-list");
        }
        console.log(2);
        /*The image width/height shall be calculated once the image is fully loaded*/
        var width = $(document).width();

        $(".movie-list").find("img").each(function () {
            var ratio = this.width / this.height;
            var newWidth = 300 * ratio;
            $(this).width(newWidth + "px").height("300px");

            if (newWidth > 200)
                $(this).width("200px");
        });
        console.log(3);
        ScaleElement1($(".movie-list ul"));

        // movie-list
        PreparePaginationControl($(".movie-list"));
        console.log(4);
        //PreparePaginationControl($(".news-container"));

        $(window).resize(function () {
            PreparePaginationControl($(".movie-list"));
        });
        console.log(5);
    }
}

function onSuccessLoadUpcomingMovies(result) {
    result = JSON.parse(result);
    console.log(6);
    if (result.length > 0) {
        //MOVIES = result;

        // adding images        
        for (var i = 0; i < result.length; i++) {
            var list = PopulatingMovies(result[i], "upcoming-movie-list");
        }
        console.log(7);
        /*The image width/height shall be calculated once the image is fully loaded*/
        var width = $(document).width();

        $(".upcoming-movie-list").find("img").each(function () {
            var ratio = this.width / this.height;
            var newWidth = 300 * ratio;
            $(this).width(newWidth + "px").height("300px");

            if (newWidth > 200)
                $(this).width("200px");
        });
        console.log(8);
        ScaleElement1($(".upcoming-movie-list ul"));

        // movie-list
        PreparePaginationControl($(".upcoming-movie-list"), { pagerContainerId: "upcoming-pager" });
        console.log(9);
        $(window).resize(function () {
            PreparePaginationControl($(".upcoming-movie-list"), { pagerContainerId: "upcoming-pager" });
        });
        console.log(10);
    }
}

function PrepareGenreLinks() {
    $("span.movie-data-label").each(function () {
        if ($(this).html() == "Genre:") {
            var html = $(this).next().text();
            var links = GetLinks(html, "Genre");
            $(this).next().html(links);
        }
    });
}

function GetLinks(html, type) {
    if (html == null || html == "undefined") {
        return;
    }

    var genre = html.split("|");
    var links = "";
    for (i = 0; i < genre.length; i++) {
        links += "<a href='/" + type + "/" + genre[i].trim().split(' ').join('-') + "'>" + genre[i].trim() + "</a> | "
    }

    links = links.substring(0, links.lastIndexOf("|"));
    return links;
}

function ScaleElement1(element) {
    var currentElement = null;
    $(element).find("li.movie").each(function () {

        $(this).find("#picAndCaption, #hover").hover(function () {
            var element = this;
            $(element).find("#hover").each(function () {
                $(this).css("position", "absolute").css("top", "70px").css("height", "230px");
                /*$(this).find(".img-movie-name").each(function () { $(this).show(); });
                $(this).find(".img-movie-genre").each(function () { $(this).show(); });
                $(this).find(".img-movie-date").each(function () { $(this).show(); });
                $(this).find(".movie-songs ul").each(function () { $(this).show(); });*/
            });
        },       
        function () {
            var element = this;
            $(element).find("#hover").each(function () {
                //setTimeout(function(){},200);
                /*$(this).find(".img-movie-name").each(function () { $(this).hide(); });
                $(this).find(".img-movie-genre").each(function () { $(this).hide(); });
                $(this).find(".img-movie-date").each(function () { $(this).hide(); });
                $(this).find(".movie-songs ul").each(function () { $(this).hide(); });*/
                $(this).css("position", "relative").css("top", "auto").css("height", "auto");
            });

        });
    });
}