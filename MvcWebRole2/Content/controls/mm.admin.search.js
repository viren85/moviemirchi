// Place holder is text displayed in search bar - e.g. Search Movies, Search Artists, Search Critics etc.
// Search type is - movies, artists, critics

var Search = function (placeholder, searchtype) {
    var type = searchtype;
    var resultContainer;
    var that = this;
    var MOVIES;
    var CURRENT_MOVIE;
    var Counter = 0;
    var ARTISTS;
    var CURRENT_ARTIST;

    var CRITICS;
    var CURRENT_CRITIC;

    var CRAWLFILES;
    var CURRENT_CRAWLFILE;

    var testData =
        [
            {
                "Name": "Yeh Jawani Hai Deewani",
                "Posters": ["poster-1.jpg", "poster-2.jpg", "poster-3.jpg"],
                "Year": "2013"
            },
            {
                "Name": "Dhoom 3",
                "Posters": ["poster-1.jpg", "poster-2.jpg", "poster-3.jpg"],
                "Year": "2013"
            },
            {
                "Name": "Bhaag Milkha Bhaag",
                "Posters": ["poster-1.jpg", "poster-2.jpg", "poster-3.jpg"],
                "Year": "2013"
            },
            {
                "Name": "Goliyon ki Rasleela Ram-Leela",
                "Posters": ["poster-1.jpg", "poster-2.jpg", "poster-3.jpg"],
                "Year": "2013"
            },
            {
                "Name": "Chennai Express",
                "Posters": ["poster-1.jpg", "poster-2.jpg", "poster-3.jpg"],
                "Year": "2013"
            },
            {
                "Name": "Chennai Express",
                "Posters": ["poster-1.jpg", "poster-2.jpg", "poster-3.jpg"],
                "Year": "2013"
            },
            {
                "Name": "Chennai Express",
                "Posters": ["poster-1.jpg", "poster-2.jpg", "poster-3.jpg"],
                "Year": "2013"
            },
            {
                "Name": "Chennai Express",
                "Posters": ["poster-1.jpg", "poster-2.jpg", "poster-3.jpg"],
                "Year": "2013"
            }
        ];

    Search.prototype.GetSearchBar = function (type) {
        var searchContainer = $("<div/>").attr("class", "search-container");
        var txtSearch = $("<input/>").attr("class", "search-text").attr("placeholder", placeholder);
        var btnSearch = $("<div/>").attr("class", "btn btn-success").html("Go");

        $(txtSearch).keypress(function () {
            if ($(this).val().length > 2) {
                that.GetSearchResults($(".search-result-container"), type);
            }
        });

        $(txtSearch).keyup(function () {
            if ($(this).val().length == 0) {
                that.GetSearchResults($(".search-result-container"), type);
            }
        });

        return $(searchContainer).append(txtSearch).append(btnSearch);
    }

    Search.prototype.GetSearchResultContainer = function () {
        var searchContainer = $("<div/>").attr("class", "search-result-container");
        return $(searchContainer);
    }

    Search.prototype.GetSearchResults = function (searchResultContainer, type) {
        // Call the search API from here
        resultContainer = searchResultContainer;
        var searchQuery = $(".search-text").val();
        var queryString = "";
        if (searchQuery != "") {
            queryString = "?q=" + searchQuery;
        }

        switch (type) {
            case "movies":
                CallHandler("api/Movies" + queryString, this.PopulateSearchResults);
                break;
            case "artists":
                CallHandler("../api/Artists" + queryString, this.PopulateArtistsResults);
                break;
            case "critics":
                CallHandler("../api/Reviewer" + queryString, this.PopulateCriticsResult);
                break;
            case "crawler":
                CallHandler("../api/CrawlerFiles" + queryString, this.PopulateCrawlerResults);
                break;
            case "news":
                CallHandler("../api/News?start=0&page=20", this.PopulateNewsResults);
                break;
            case "twitter":
                CallHandler("../api/Twitter?start=0&page=0", this.PopulateTwitterResults);
                break;
            default:
                CallHandler("api/Movies" + queryString, this.PopulateSearchResults);
                break;
        }

        //this.PopulateSearchResults("[]");
    }

    Search.prototype.PopulateArtistsResults = function (data) {
        var json = JSON.parse(data);
        ARTISTS = json;
        $(resultContainer).children("ul").remove();

        if (json != null) {
            var searchResultList = $("<ul/>").attr("class", "search-result-list");

            for (i = 0; i < json.length; i++) {
                //if (json[i].Name.indexOf($(".search-text").val()) > -1) {
                var item = $("<li/>").attr("class", "search-result-list-item").attr("un", json[i].ArtistId).click(function () {
                    $(".content-container").show();
                    //that.PopulateMovieDetails($(this).attr("un"));
                    that.PopulateArtistDetail($(this).attr("un"));
                });

                var img;
                var posters = JSON.parse(json[i].Posters);

                if (posters.length > 0) {
                    img = $("<img/>").attr("src", PUBLIC_BLOB_URL + posters[posters.length - 1]).attr("class", "search-item-img");
                }
                else {
                    img = $("<img/>").attr("src", PUBLIC_BLOB_URL + "default-movie.jpg").attr("class", "search-item-img");
                }

                var movieTitle = $("<div/>").attr("class", "search-movie-name").html(json[i].ArtistName);
                var year = $("<div/>").attr("class", "search-movie-year").html("");
                $(item).append(img);
                $(item).append(movieTitle);
                $(item).append(year);
                $(searchResultList).append(item);
                //}
            }

            if (resultContainer == null || resultContainer == "undefined") {
                resultContainer = $(".search-result-container");
            }

            if (json.length == 0) {
                var item1 = $("<li/>").attr("class", "search-result-list-item");

                var text = $(".search-text").val() == "" ? "Currently you haven't added any artist" : "Currently you haven't added \"" + $(".search-text").val() + "\" artist";

                var empty = $("<div/>").attr("class", "search-movie-name").attr("style", "width: 90%;margin-left: 5%;").html(text);
                $(item1).append(empty);
                $(item1).append($("<div/>").attr("class", "search-movie-year").html(""));
                $(searchResultList).append(item1);
            }

            $(resultContainer).append(searchResultList);
        }
    }

    Search.prototype.PopulateSearchResults = function (data) {
        // Prepare search result UI from this function
        var json = JSON.parse(data);
        MOVIES = json;
        $(resultContainer).children("ul").remove();
        //Comment following line once API is functional        
        if (json != null) {
            var searchResultList = $("<ul/>").attr("class", "search-result-list");

            for (i = 0; i < json.length; i++) {
                //if (json[i].Name.indexOf($(".search-text").val()) > -1) {
                var item = $("<li/>").attr("class", "search-result-list-item").attr("un", json[i].UniqueName).click(function () {
                    $(".content-container").show();
                    that.PopulateMovieDetails($(this).attr("un"));
                });

                var img;
                var posters = JSON.parse(json[i].Posters);

                if (posters.length > 0) {
                    img = $("<img/>").attr("src", PUBLIC_BLOB_URL + posters[posters.length - 1]).attr("class", "search-item-img");
                }
                else {
                    img = $("<img/>").attr("src", PUBLIC_BLOB_URL + "default-movie.jpg").attr("class", "search-item-img");
                }

                var movieTitle = $("<div/>").attr("class", "search-movie-name").html(json[i].Name);
                var year = $("<div/>").attr("class", "search-movie-year").html(json[i].Year);
                $(item).append(img);
                $(item).append(movieTitle);
                $(item).append(year);
                $(searchResultList).append(item);
                //}
            }

            if (resultContainer == null || resultContainer == "undefined") {
                resultContainer = $(".search-result-container");
            }

            if (json.length == 0) {
                var item1 = $("<li/>").attr("class", "search-result-list-item");

                var text = $(".search-text").val() == "" ? "Currently you haven't added any movie" : "Currently you haven't added \"" + $(".search-text").val() + "\" movie";

                var empty = $("<div/>").attr("class", "search-movie-name").attr("style", "width: 90%;margin-left: 5%;").html(text);
                $(item1).append(empty);
                $(item1).append($("<div/>").attr("class", "search-movie-year").html(""));
                $(searchResultList).append(item1);
            }

            $(resultContainer).append(searchResultList);
        }
    }

    Search.prototype.PopulateMovieDetails = function (uname) {
        for (var i = 0; i < MOVIES.length; i++) {
            if (MOVIES[i].UniqueName == uname) {
                CURRENT_MOVIE = MOVIES[i]; // assign selected movie to current movie variable      
                console.log(CURRENT_MOVIE);
                $("#txtUnique").val(MOVIES[i].UniqueName);
                $("#txtFriendly").val(MOVIES[i].Name);
                $("#txtSynopsis").val(MOVIES[i].Synopsis);
                $("#txtBudget").val(MOVIES[i].Stats.replace("&nbsp;", " "));
                $("#txtState").val(MOVIES[i].State);

                $(".form-container").find("radio").each(function () { $(this).prop('checked', false); });

                if (MOVIES[i].State == "upcoming") {
                    $("#rbUpcoming").prop('checked', true);
                }
                else if (MOVIES[i].State == "now-playing" || MOVIES[i].State == "now playing") {
                    $("#rbNowPlaying").prop('checked', true);
                }
                else {
                    $("#rbReleased").prop('checked', true);
                }

                if (MOVIES[i].MyScore != "" && MOVIES[i].MyScore != undefined) {
                    var myScore = JSON.parse(MOVIES[i].MyScore);
                    $("#txtTeekhaRate").val(myScore.teekharating);
                    $("#txtFeekaRate").val(myScore.feekharating);
                    $("#txtMyScore").val(myScore.criticrating);
                }
                else {
                    $("#txtTeekhaRate").val("");
                    $("#txtFeekaRate").val("");
                    $("#txtMyScore").val("");
                }

                // populate cast
                $(".artists-container").html("");
                if (MOVIES[i].Casts != "" && MOVIES[i].Casts != undefined) {
                    var artist = JSON.parse(MOVIES[i].Casts);
                    $(".artists-container").append(new Artists().GetArtistGrid(artist));

                    $(function () {
                        $("#sortable").sortable({ cursor: "move" });
                        $("#sortable").disableSelection();
                    });
                }

                //populate songs
                $(".songs-container").html("");
                if (MOVIES[i].Songs != "" && MOVIES[i].Songs != undefined) {
                    var songs = JSON.parse(MOVIES[i].Songs);
                    $(".songs-container").append(new Songs().GetSongsGrid(songs));

                    $(function () {
                        $("#songs-sortable").sortable({ cursor: "move" });
                        $("#songs-sortable").disableSelection();
                    });
                }


                $(".posters-container").html("");

                if (MOVIES[i].Posters != "" && MOVIES[i].Posters != undefined) {
                    var posters = JSON.parse(MOVIES[i].Posters);
                    $(".posters-container").append(new Posters().GetPosterContainer(posters));
                }

                $(".shortcut-container").html("");
                $(".shortcut-container").append($("<a/>").html("Save changes").attr("onclick", "updateMovie()").attr("class", "btn btn-success").attr("title", "click here to save all the changes."));
                $(".shortcut-container").append($("<div>").attr("id", "status"));
                // upload files
                $("#poster-upload").attr("onchange", "UploadSelectedFile(this, '#txtUnique','poster')");
                break;
            }
        }

        //console.log(CURRENT_MOVIE);
    }

    Search.prototype.UpdateMovie = function () {
        //console.log(CURRENT_MOVIE);

        var uniqueName = $("#txtUnique").val();
        var friendlyName = $("#txtFriendly").val();
        var synopsis = $("#txtSynopsis").val();
        var stats = $("#txtBudget").val();

        if (uniqueName != undefined && uniqueName != "") {
            CURRENT_MOVIE.UniqueName = uniqueName
        }

        if (friendlyName != undefined && friendlyName != "") {
            CURRENT_MOVIE.Name = friendlyName
        }

        if (synopsis != undefined && synopsis != "") {
            CURRENT_MOVIE.Synopsis = synopsis
        }

        if (stats != undefined && stats != "") {
            CURRENT_MOVIE.Stats = stats
        }

        // get state
        $(".basic-form-container").find(".form-container").find("input[type='radio']").each(function () {
            if ($(this).prop('checked') == true) {
                CURRENT_MOVIE.State = $(this).attr('value').toLowerCase();
            }
        });

        //get ratings
        var myScore = { "teekharating": $("#txtTeekhaRate").val(), "feekharating": $("#txtFeekaRate").val(), "criticrating": $("#txtMyScore").val() };
        CURRENT_MOVIE.MyScore = JSON.stringify(myScore);

        //console.log(CURRENT_MOVIE.MyScore);

        //get artings list
        var artists = [];
        $(".artist-grid").find(".artist-grid-row").each(function () {
            var name = $(this).find(".artist-grid-row-data1").html();
            var role = $(this).find(".artist-grid-row-data2").find("input").val();
            var charName = $(this).find(".artist-grid-row-data3").find("input").val();

            artists.push({ "name": name, "role": role, "charactername": charName });
        });

        if (artists.length > 0)
            CURRENT_MOVIE.Casts = JSON.stringify(artists);

        //get songs list
        var songs = [];
        $(".songs-grid").find(".songs-grid-row").each(function () {
            var title = $(this).find(".songs-grid-row-data1").html();
            var composed = $(this).find(".songs-grid-row-data1").attr("cmpsd");
            var performer = $(this).find(".songs-grid-row-data1").attr("prfmr");
            var artist = $(this).find(".songs-grid-row-data1").attr("artist");

            var lyrics = $(this).find(".songs-grid-row-data2").html();
            var recite = $(this).find(".songs-grid-row-data2").attr("recte");
            var courtsey = $(this).find(".songs-grid-row-data2").attr("crtsy");

            var youtubeEmbedURL = $(this).find(".songs-grid-row-data3").html();
            alert(youtubeEmbedURL);
            var index = youtubeEmbedURL.indexOf("<a");
            alert(index);
            if (index > 0) youtubeEmbedURL = youtubeEmbedURL.substring(0, index);

            alert(youtubeEmbedURL);
            var youtubeThamb = $(this).find(".songs-grid-row-data3").attr("thamb");

            songs.push({ "SongTitle": title, "Lyrics": lyrics, "Composed": composed, "Performer": performer, "Recite": recite, "Courtsey": courtsey, "YoutubeURL": youtubeEmbedURL, "Thamb": youtubeThamb, "Artist": artist });
        });

        if (songs.length > 0)
            CURRENT_MOVIE.Songs = JSON.stringify(songs);

        alert(CURRENT_MOVIE.Songs);
        //geting posters
        var posters = [];
        var selectPoster = null;
        $(".poster-container").find(".single-poster").each(function () {
            if ($(this).find("input[type='radio']").prop("checked") == true) {
                selectPoster = $(this).find("input[type='radio']").attr("id");
            }
            else {
                var poster = $(this).find("input[type='radio']").attr("id");
                posters.push(poster);
            }
        });
        if (selectPoster != null)
            posters.push(selectPoster);

        if (posters.length > 0)
            CURRENT_MOVIE.Posters = JSON.stringify(posters);

        //console.log(CURRENT_MOVIE);

        var movie = {
            "MovieId": CURRENT_MOVIE.MovieId,
            "Name": CURRENT_MOVIE.Name,
            "AltNames": CURRENT_MOVIE.AltNames,
            "Posters": CURRENT_MOVIE.Posters,
            "Ratings": CURRENT_MOVIE.Ratings,
            "Synopsis": CURRENT_MOVIE.Synopsis,
            "Casts": CURRENT_MOVIE.Casts,
            "Stats": CURRENT_MOVIE.Stats,
            "Songs": CURRENT_MOVIE.Songs,
            "Trailers": CURRENT_MOVIE.Trailers,
            "Pictures": CURRENT_MOVIE.Pictures,
            "Genre": CURRENT_MOVIE.Genre,
            "Month": CURRENT_MOVIE.Month,
            "Year": CURRENT_MOVIE.Year,
            "UniqueName": CURRENT_MOVIE.UniqueName,
            "State": CURRENT_MOVIE.State,
            "MyScore": CURRENT_MOVIE.MyScore,
            "JsonString": CURRENT_MOVIE.JsonString,
            "Popularity": CURRENT_MOVIE.Popularity
        };

        // save movie
        var movie1 = JSON.stringify(movie);
        //console.log(movie1);
        $("#status").html("<b>Saving...<b/>");
        CallController("Movie/AddMovie", "hfMovie", movie1, function () { $("#status").html("Movie details saved successfully!") });
    }

    // for artist
    Search.prototype.PopulateArtistDetail = function (uname) {
        for (var i = 0; i < ARTISTS.length; i++) {
            if (ARTISTS[i].ArtistId == uname) {
                new Artists().PopulateArtistDetails(ARTISTS[i]);
                CURRENT_ARTIST = ARTISTS[i];
            }
        }
    }

    Search.prototype.UpdateArtist = function () {
        new Artists().UpdateArtistDetails(CURRENT_ARTIST);
    }
    //end

    Search.prototype.PopulateCriticsResult = function (data) {
        var json = JSON.parse(data);
        CRITICS = json;
        $(resultContainer).children("ul").remove();

        if (json != null) {
            var searchResultList = $("<ul/>").attr("class", "search-result-list");

            for (i = 0; i < json.length; i++) {
                var item = $("<li/>").attr("class", "search-result-list-item").attr("un", json[i].ReviewerId).click(function () {
                    $(".content-container").show();
                    that.PopulateCriticsDetail($(this).attr("un"));
                });

                var img;

                img = $("<img/>").attr("src", PUBLIC_BLOB_URL + json[i].ReviewerImage).attr("class", "search-item-img");

                var movieTitle = $("<div/>").attr("class", "search-movie-name").html(json[i].ReviewerName);
                var year = $("<div/>").attr("class", "search-movie-year").html("");
                $(item).append(img);
                $(item).append(movieTitle);
                $(item).append(year);
                $(searchResultList).append(item);
            }

            if (resultContainer == null || resultContainer == "undefined") {
                resultContainer = $(".search-result-container");
            }

            if (json.length == 0) {
                var item1 = $("<li/>").attr("class", "search-result-list-item");

                var text = $(".search-text").val() == "" ? "Currently you haven't added any critic" : "Currently you haven't added \"" + $(".search-text").val() + "\" critic";

                var empty = $("<div/>").attr("class", "search-movie-name").attr("style", "width: 90%;margin-left: 5%;").html(text);
                $(item1).append(empty);
                $(item1).append($("<div/>").attr("class", "search-movie-year").html(""));
                $(searchResultList).append(item1);
            }

            $(resultContainer).append(searchResultList);
        }
    }

    Search.prototype.PopulateCriticsDetail = function (criticsId) {
        for (var i = 0; i < CRITICS.length; i++) {
            if (CRITICS[i].ReviewerId == criticsId) {
                new Reviewer().PopulateReviewerDetails(CRITICS[i]);
                CURRENT_CRITIC = CRITICS[i];
            }
        }
    }

    Search.prototype.UpdateCritics = function () {
        new Reviewer().UpdateReviewer(CURRENT_CRITIC);
    }

    // crawler
    Search.prototype.PopulateCrawlerResults = function (data) {
        // Prepare search result UI from this function
        console.log(data);
        var json = JSON.parse(data);
        CRAWLFILES = json;
        $(resultContainer).children("ul").remove();

        //Comment following line once API is functional
        //json = testData;
        if (json != null) {
            var searchResultList = $("<ul/>").attr("class", "search-result-list");

            for (i = 0; i < json.length; i++) {
                //if (json[i].Name.indexOf($(".search-text").val()) > -1) {
                var item = $("<li/>").attr("class", "search-result-list-item").attr("un", json[i].MovieId).click(function () {
                    that.PopulateCrawlerFileDetail($(this).attr("un"));
                });

                var img;

                img = $("<img/>").attr("src", PUBLIC_BLOB_URL + "default-movie.jpg").attr("class", "search-item-img");

                var movieTitle = $("<div/>").attr("class", "search-movie-name").html(json[i].MovieName);
                var year = $("<div/>").attr("class", "search-movie-year").html(json[i].Month + " " + json[i].Year);
                $(item).append(img);
                $(item).append(movieTitle);
                $(item).append(year);
                $(searchResultList).append(item);
            }

            if (resultContainer == null || resultContainer == "undefined") {
                resultContainer = $(".search-result-container");
            }

            if (json.length == 0) {
                var item1 = $("<li/>").attr("class", "search-result-list-item");

                var text = $(".search-text").val() == "" ? "Currently you haven't added any movie for this month" : "Currently you haven't added \"" + $(".search-text").val() + "\" movie";

                var empty = $("<div/>").attr("class", "search-movie-name").attr("style", "width: 90%;margin-left: 5%;").html(text);
                $(item1).append(empty);
                $(item1).append($("<div/>").attr("class", "search-movie-year").html(""));
                $(searchResultList).append(item1);
            }

            $(resultContainer).append(searchResultList);
        }
    }

    Search.prototype.SaveXMLFile = function (isCrawl) {
        new Crawler().SaveXmlFileCrawl(isCrawl);
    }

    Search.prototype.PopulateNewsResults = function (data) {
        $(".content-container").show();
        $(".news-container").append(new News().GetNewsGrid(data));
    }

    Search.prototype.PopulateTwitterResults = function (data) {
        $(".content-container").show();
        $(".twitter-container").append(new Twitter().GetTwitterGrid(data));
    }

    Search.prototype.PopulateCrawlerFileDetail = function (movieId) {
        for (var i = 0; i < CRAWLFILES.length; i++) {
            if (CRAWLFILES[i].MovieId == movieId) {
                new Crawler().PopulateCrawlerFileDetails(CRAWLFILES[i]);
                CURRENT_CRAWLFILES = CRAWLFILES[i];
            }
        }
    }
}

function updateMovie() { search.UpdateMovie(); }
function updateArtist() { search.UpdateArtist(); }
function updateCritics() { search.UpdateCritics(); }
function saveXmlFile(isCrawl) { search.SaveXMLFile(isCrawl); }
