﻿// Place holder is text displayed in search bar - e.g. Search Movies, Search Artists, Search Critics etc.
// Search type is - movies, artists, critics

var Search = function (placeholder, searchtype) {
    var type = searchtype;
    var resultContainer;
    var that = this;
    var MOVIES;
    var CURRENT_MOVIE;
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

    Search.prototype.GetSearchBar = function () {
        var searchContainer = $("<div/>").attr("class", "search-container");
        var txtSearch = $("<input/>").attr("class", "search-text").attr("placeholder", placeholder);
        var btnSearch = $("<div/>").attr("class", "btn btn-success").html("Go");

        $(txtSearch).keypress(function () {
            if ($(this).val().length > 2) {
                that.GetSearchResults($(".search-result-container"));
            }
        });

        return $(searchContainer).append(txtSearch).append(btnSearch);
    }

    Search.prototype.GetSearchResultContainer = function () {
        var searchContainer = $("<div/>").attr("class", "search-result-container");
        return $(searchContainer);
    }

    Search.prototype.GetSearchResults = function (searchResultContainer) {
        // Call the search API from here
        resultContainer = searchResultContainer;
        var searchQuery = $(".search-text").val();
        var queryString = "";
        if (searchQuery != "") {
            queryString = "?q=" + searchQuery;
        }

        CallHandler("api/Movies" + queryString, this.PopulateSearchResults);
        //this.PopulateSearchResults("[]");
    }

    Search.prototype.PopulateSearchResults = function (data) {
        // Prepare search result UI from this function
        var json = JSON.parse(data);
        MOVIES = json;
        $(resultContainer).children("ul").remove();
        //Comment following line once API is functional
        //json = testData;
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
                    img = $("<img/>").attr("src", PUBLIC_BASE_URL + "/Posters/Images/" + posters[posters.length - 1]).attr("class", "search-item-img");
                }
                else {
                    img = $("<img/>").attr("src", PUBLIC_BASE_URL + "/Posters/Images/default-movie.jpg").attr("class", "search-item-img");
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

            $(resultContainer).append(searchResultList);
        }
    }

    Search.prototype.PopulateMovieDetails = function (uname) {
        for (var i = 0; i < MOVIES.length; i++) {
            if (MOVIES[i].UniqueName == uname) {
                CURRENT_MOVIE = MOVIES[i];
                //alert("movie details for " + uname + " is here");
                $("#txtUnique").val(MOVIES[i].UniqueName);
                $("#txtFriendly").val(MOVIES[i].Name);
                $("#txtSynopsis").val(MOVIES[i].Synopsis);
                $("#txtBudget").val(MOVIES[i].Stats.replace("&nbsp;", " "));
                $("#txtState").val(MOVIES[i].State);

                $(".form-container").find("radio").each(function () { $(this).prop('checked', false); });

                if (MOVIES[i].State == "upcoming") {
                    $("#rbUpcoming").prop('checked', true);
                }
                else if (MOVIES[i].State == "now-playing") {
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

                $(".artists-container").html("");
                if (MOVIES[i].Casts != "" && MOVIES[i].Casts != undefined) {
                    var artist = JSON.parse(MOVIES[i].Casts);
                    $(".artists-container").append(new Artists().GetArtistGrid(artist));

                    /*$("#sortable").sortable({
                        axis: "y",
                        revert: true,
                        scroll: false,
                        placeholder: "sortable-placeholder",
                        cursor: "move"
                    });*/

                    $(function () {
                        $("#sortable").sortable({ cursor: "move" });
                        $("#sortable").disableSelection();
                    });
                }

                $(".posters-container").html("");

                if (MOVIES[i].Posters != "" && MOVIES[i].Posters != undefined) {
                    var posters = JSON.parse(MOVIES[i].Posters);
                    $(".posters-container").append(new Posters().GetPosterContainer(posters));
                }
                // upload files
                //$("#poster-upload").attr("onchange", "UploadSelectedFile(this)");
                //$("#poster-upload").attr("onchange", function () { alert("file uplaed") });
                //$("#poster-upload").attr("onchange", "search.UploadSelectedFile(this);");

               /* $('#poster-upload').fileupload({
                    dataType: 'json',
                    url: '/Home/UploadFile',
                    autoUpload: true,
                    done: function (e, data) {
                        $('.file_name').html(data.result.name);
                        $('.file_type').html(data.result.type);
                        $('.file_size').html(data.result.size);
                    }
                }).on('fileuploadprogressall', function (e, data) {
                    var progress = parseInt(data.loaded / data.total * 100, 10);
                    $('.progress .progress-bar').css('width', progress + '%');
                });
                */
                break;
            }
        }

        console.log(CURRENT_MOVIE);
    }


    /*Search.prototype.UploadSelectedFile = function (element) {

        $('#poster-upload').fileupload({
            dataType: 'json',
            url: '/Home/UploadFile',
            autoUpload: true,
            done: function (e, data) {
                $('.file_name').html(data.result.name);
                $('.file_type').html(data.result.type);
                $('.file_size').html(data.result.size);
            }
        }).on('fileuploadprogressall', function (e, data) {
            var progress = parseInt(data.loaded / data.total * 100, 10);
            $('.progress .progress-bar').css('width', progress + '%');
        });
    }*/
}

