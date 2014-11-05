var BASE_URL = "http://127.0.0.1:8081";
var WEB_BASE_URL = "http://127.0.0.1:81";
//var BASE_URL = "";

var TILE_MODE = 0; // 0 = Old Tile with Hover effect, 1 = New Tile with Slide Effect
var MovieCounter = 4;
var MovieIndexer = 0;
var Movie = 4;

var ReviewsDetails = [];
var Index = 0;

var MOUSE_ON;

function CallHandler(queryString, OnComp) {
    $.getJSON(BASE_URL + queryString, OnComp);
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
    var path = "/api/Movies?type=current";
    CallHandler(path, onSuccessLoadCurrentMovies);
}

function LoadUpcomingMovies() {
    var path = "/api/Movies?type=upcoming";
    CallHandler(path, onSuccessLoadUpcomingMovies);
}

function onSuccessLoadUpcomingMovies(result) {
    return LoadMovies(result, {
        tubeSelector: "#upcoming-movie-list-tube",
        listSelector: ".upcoming-movie-list",
        listClassName: "upcoming-movie-list",
        pagerSelector: "#upcoming-pager",
        pagerId: "upcoming-pager",
    });
}

function onSuccessLoadCurrentMovies(result) {
    return LoadMovies(result, {
        tubeSelector: "#movie-list-tube",
        listSelector: ".movie-list",
        listClassName: "movie-list",
        pagerSelector: "#now-pager",
        pagerId: "now-pager",
    });
}

function LoadMovies(result, options) {
    var $list = $(options.listSelector);

    if (!result || result === "") {
        $list.html("Unable to get movies");
    }

    try {
        result = JSON.parse(result);

        new Util().RemoveLoadImage($(options.tubeSelector));

        if (result.Status && result.Status === "Error") {
            $list.html(result.UserMessage);
        } else {
            if (result.length > 0) {
                // adding images
                $.each(result, function () {
                    try{
                        PopulatingMovies(this, options.listClassName);
                    }
                    catch (e) {

                    }
                })

                SetTileSize(options.listSelector);

                ScaleElement($(options.listSelector + " ul"));

                if ($(window).width() < 768) {
                    new Pager($list, options.pagerSelector);
                } else {
                    PreparePaginationControl($list, { pagerContainerId: options.pagerId, tileWidth: "275" });
                }

                $(window).resize(function () {
                    if ($(window).width() < 785) {
                        $(options.pagerSelector + " .pager-container").remove();
                        new Pager($list, options.pagerSelector);
                    } else {
                        PreparePaginationControl($list, { pagerContainerId: options.pagerId, tileWidth: "275" });
                    }
                });
            }
        }
    } catch (e) {
        $list.html("Unable to get movies");
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

function SetTileSize(tileSelector) {

    var $list = $(tileSelector);

    /*The image width/height shall be calculated once the image is fully loaded*/
    var width = $(document).width();

    var defaultRatio = (TILE_MODE == 0) ? 400 : 300;
    var defaultTileHeight = (TILE_MODE == 0) ? 340 : 300;
    var defaultTileWidth = (TILE_MODE == 0) ? 225 : 200;

    var ratio = 0;
    var newWidth = 0;
    $list.find("img:first").each(function () {
        ratio = this.width / this.height;
        newWidth = defaultRatio * ratio;
    });
    $list.find("img").each(function () {
        var $this = $(this);
        $this.height(defaultTileHeight + "px");

        if (newWidth > defaultTileWidth) {
            $this.width(defaultTileWidth + "px");
        } else {
            $this.width(newWidth + "px");
        }
    });
}