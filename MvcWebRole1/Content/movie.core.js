//var BASE_URL = "http://127.0.0.1:81/";
var BASE_URL = "";
var TILE_MODE = 0; // 0 = Old Tile with Hover effect, 1 = New Tile with Slide Effect
var MovieCounter = 4;
var MovieIndexer = 0;
var Movie = 4;

var ReviewsDetails = [];
var Index = 0;

var MOVIES = [];

var MOUSE_ON;

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
    try {
        result = JSON.parse(result);
        new Util().RemoveLoadImage($("#movie-list-tube"));

        if (result.Status != undefined || result.Status == "Error") {
            $(".movie-list").html(result.UserMessage);
        }
        else {
            if (result.length > 0) {
                MOVIES = result;

                // adding images        
                for (var i = 0; i < result.length; i++) {
                    var list = PopulatingMovies(result[i], "movie-list");
                }

                /*The image width/height shall be calculated once the image is fully loaded*/
                var width = $(document).width();

                var defaultRatio = (TILE_MODE == 0) ? 400 : 300;
                var defaultTileHeight = (TILE_MODE == 0) ? 340 : 300;
                var defaultTileWidth = (TILE_MODE == 0) ? 225 : 200;

                var ratio = 0;
                var newWidth = 0;
                $(".movie-list").find("img:first").each(function () {
                    ratio = this.width / this.height;
                    newWidth = defaultRatio * ratio;
                });
                $(".movie-list").find("img").each(function () {
                    var $this = $(this);
                    $this.height(defaultTileHeight + "px");

                    if (newWidth > defaultTileWidth) {
                        $this.width(defaultTileWidth + "px");
                    } else {
                        $this.width(newWidth + "px");
                    }
                });

                ScaleElement($(".movie-list ul"));
                /*if (TILE_MODE == 0 && $(window).width() > 767)
                    ScaleElement($(".movie-list ul"));
                else
                    ScaleNewTileElement($(".movie-list ul"));
                */
                // movie-list
                //PreparePaginationControl($(".movie-list"));
                if ($(window).width() < 768) {
                    // Comment next two line and uncomment new pager line to repro unresponsive page issue
                    /*$(".movie-list ul li").hide();
                    $(".movie-list ul li:first").show();*/
                    var pager = new Pager($(".movie-list"), "#now-pager");
                }
                else {
                    PreparePaginationControl($(".movie-list"));
                }

                $(window).resize(function () {
                    if ($(window).width() < 785) {
                        var pager = new Pager($(".movie-list"), "#now-pager");
                        $("#now-pager .pager-container").remove();
                        //$("#upcoming-pager .pager-container").remove();
                        //$("#critics-pager .pager-container").remove();
                    }
                    else {
                        PreparePaginationControl($(".movie-list"));
                        //PreparePaginationControl($(".upcoming-movie-list"));
                        //var pagerJson = { "pagerContainerId": "critics-pager", "tileWidth": "90" };
                        //PreparePaginationControl($(".critics-container"), pagerJson);
                    }
                });
            }
        }
    } catch (e) {
        $(".movie-list").html("Unable to get movies");
    }
}

function onSuccessLoadUpcomingMovies(result) {
    try {
        result = JSON.parse(result);

        new Util().RemoveLoadImage($("#upcoming-movie-list-tube"));
        //$(".upcoming-movie-list").find("img").hide();

        if (result.Status != undefined || result.Status == "Error") {
            $(".upcoming-movie-list").html(result.UserMessage);
        }
        else {
            if (result.length > 0) {
                //MOVIES = result;

                // adding images        
                for (var i = 0; i < result.length; i++) {
                    var list = PopulatingMovies(result[i], "upcoming-movie-list");
                }

                /*The image width/height shall be calculated once the image is fully loaded*/
                var width = $(document).width();

                var defaultRatio = (TILE_MODE == 0) ? 400 : 300;
                var defaultTileHeight = (TILE_MODE == 0) ? 380 : 300;
                var defaultTileWidth = (TILE_MODE == 0) ? 225 : 200;

                var ratio = 0;
                var newWidth = 0;
                $(".upcoming-movie-list").find("img:first").each(function () {
                    ratio = this.width / this.height;
                    newWidth = defaultRatio * ratio;
                });
                $(".upcoming-movie-list").find("img").each(function () {
                    var $this = $(this);
                    $this.height(defaultTileHeight + "px");

                    if (newWidth > defaultTileWidth) {
                        $this.width(defaultTileWidth + "px");
                    } else {
                        $this.width(newWidth + "px");
                    }
                });

                ScaleElement($(".upcoming-movie-list ul"));
                /*if (TILE_MODE == 0 && $(window).width() > 767)
                    ScaleElement($(".upcoming-movie-list ul"));
                else
                    ScaleNewTileElement($(".upcoming-movie-list ul"));
                */
                // movie-list
                //PreparePaginationControl($(".upcoming-movie-list"), { pagerContainerId: "upcoming-pager" });
                //var pager = new Pager($(".upcoming-movie-list"), "#upcoming-pager");
                if ($(window).width() < 768) {
                    // Comment next two line and uncomment new pager line to repro unresponsive page issue
                    /*$(".upcoming-movie-list ul li").hide();
                    $(".upcoming-movie-list ul li:first").show();*/
                    var pager = new Pager($(".upcoming-movie-list"), "#upcoming-pager");
                }
                else {
                    PreparePaginationControl($(".upcoming-movie-list"), { pagerContainerId: "upcoming-pager" });
                }

                $(window).resize(function () {
                    if ($(window).width() < 785) {
                        $("#upcoming-pager .pager-container").remove();
                        var pager = new Pager($(".upcoming-movie-list"), "#upcoming-pager");
                    }
                    else {
                        PreparePaginationControl($(".upcoming-movie-list"), { pagerContainerId: "upcoming-pager" });
                    }
                });
            }
        }
    } catch (e) {
        $(".upcoming-movie-list").html("Unable to get movies");
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
        links += "<a href='/" + type.toLowerCase() + "/" + genre[i].trim().split(' ').join('-').split('.').join('').toLowerCase() + "'>" + genre[i].trim() + "</a> | "
    }

    links = links.substring(0, links.lastIndexOf("|"));
    return links;
}

function ScaleNewTileElement(element) {
    var currentElement = null;
    $(element).find("li.movie #picAndCaption").each(function () {
        $(this).hover(function () {
            MOUSE_ON = $(this).attr("id");
            var that = $(this);
            setTimeout(function () {

                if (MOUSE_ON == $(that).attr("id")) {
                    $(that).find("#hover").css("position", "absolute").css("top", "70px").css("height", "250px");
                    $(that).find("#hover .movie-songs").show();
                    MOUSE_ON = "";
                }
            }, 100);
        },
        function () {
            var that = $(this);
            $(that).find("#hover").css("position", "relative").css("top", "auto").css("height", "auto");
            $(that).find("#hover").find(".movie-songs").hide();
        });

        $(element).find("li.movie #picAndCaption #hover").each(function () {
            $(this).hover(function (event) {
                event.stopPropagation();
            },
            function () {
                $(this).css("position", "relative").css("top", "auto").css("height", "auto");
                $(this).find(".movie-songs").hide();

            });
        });
    });
}