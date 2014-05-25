$(document).ready(function () {
    $(".clear-search-bar").click(function () {
        $("#targetUL").hide();
        $("#home-search").val("");
        $(this).hide();
    });

    $("#home-search").keyup(function (e) {
        $("#target").val("");
        var query = $(this).val().replace(".", "");

        if (query.length > 0) {
            $("#search-bar .clear-search-bar").show();
        } else {
            $("#search-bar .clear-search-bar").hide();
        }

        getItems(query);
    });

    $(".search-button").click(function () {
        $("#targetUL").remove();
        $("#home-search").keyup();
    });
});

function getItems(query) {
    var searchPath = "AutoComplete/AutoCompleteMovies?query=" + query;
    CallHandler(searchPath, PopulateSearchResult);
}

function PopulateSearchResult(response) {
    if (response) {
        if ($("#targetUL")) {
            //If the UL element is not null or undefined we are clearing it, so that the result is appended in new UL every next time.
            $("#targetUL").remove();
        }

        //assigning json response data to local variable. It is basically list of values.
        data = response;

        if (!data || !data.length || data.length < 1) {
            $("#search-results").append($("<ul id='targetUL' style='display: block'><li style='height: 35px'>No results found for '" + $("#home-search").val() + "'.</li></ul>"));
        } else {
            //appending an UL element to show the values.
            $("#search-results").append($("<ul id='targetUL' style='display:block;'></ul>"));
            //Removing previously added li elements to the list.
            $("#search-results").find("li").remove();
            //We are iterating over the list returned by the json and for each element we are creating a li element and appending the li element to ul element.
            var searchResultCounter = 0;
            var query = $("#home-search").val().toLowerCase();
            new SearchResults(data).Init();
        }
    } else {
        //If data is null the we are removing the li and ul elements.
        $("#targetUL").find("li").remove();
        $("#targetUL").remove();
    }

    if ($("#target").val() === "") {
        $("#search-results").append($("#targetUL"));
    }
}

//This method appends the text oc clicked li element to textbox.
function appendTextToTextBox(e) {
    //Getting the text of selected li element.
    var textToappend = e.innerText;
    //setting the value attribute of textbox with selected li element.
    $("#target").val(textToappend);
    //Removing the ul element once selected element is set to textbox.
    $("#targetUL").remove();
    $("#targetUL").css("display", "none");
}

var SearchResults = function (obj) {
    // var resultData = JSON.parse(obj);
    var resultData = obj;
    var entityList = [];
    var searchResultCounter = 0;

    SearchResults.prototype.GetSearchQuery = function () {
        return $("#home-search").val().toLowerCase().split(".").join("");
    };

    SearchResults.prototype.Init = function () {

        var that = this;
        resultData.forEach(function (result, index) {
            if (index < 6) {
                that.GetItems(result);
            }
        });
    };

    SearchResults.prototype.GetEntities = function (singleEntity) {
        if (singleEntity) {
            var key = this.GetSearchQuery();

            if (singleEntity.Description && singleEntity.Description.toLowerCase().indexOf(key) > -1) {
                this.GetArtistItem(singleEntity);
            }
        }
    };

    SearchResults.prototype.GetItems = function (singleEntity) {
        if (singleEntity) {
            var key = this.GetSearchQuery();

            if (singleEntity.Title && singleEntity.Title.toLowerCase().indexOf(key) > -1 && !this.IsEntityAdded(singleEntity.Title) && searchResultCounter < 6) {

                // This is movie entity hence show the movie item
                this.GetMovieItem(singleEntity);
                entityList.push(singleEntity.Title);
                searchResultCounter++;
            } else if (singleEntity.Description && singleEntity.Description.toLowerCase().indexOf(key) > -1) {

                // This is artist entity hence show the artist item
                this.GetArtistItem(singleEntity);

                if (!this.IsEntityAdded(singleEntity.Title) && searchResultCounter < 6) {
                    this.GetMovieItem(singleEntity, true, "artists");
                    entityList.push(singleEntity.Title);
                    searchResultCounter++;
                }
            } else if (singleEntity.Critics && singleEntity.Critics.toLowerCase().indexOf(key) > -1 && searchResultCounter < 6) {

                // This is critics entity hence show the critics item
                if (!this.IsEntityAdded(singleEntity.Title) && searchResultCounter < 6) {
                    this.GetCriticsItem(singleEntity);
                }
            }

            else if (singleEntity.Type && singleEntity.Type.toLowerCase().indexOf(key) > -1) {

                // This is genre entity hence show the genre item
                this.GetGenreItem(singleEntity);
            }
        }
    }

    SearchResults.prototype.GetMovieItem = function (singleEntity, isSecondary, type) {
        var li = $("<li>");
        var divTitleDesc = $("<div>");
        var anchor = $("<a>");

        $(divTitleDesc).attr("class", "search-result-desc");
        $(divTitleDesc).html("<span class='search-result-title'>" + singleEntity.Title + "</span>");

        if (isSecondary) {
            switch (type) {
                case "artists":
                    var artist = this.GetProcessedArtists(this.GetMatchArtistName(singleEntity));
                    if (artist !== "")
                        $(divTitleDesc).html($(divTitleDesc).html() + "<span class='search-result-text'><b>Artist</b>: " + GetLinks(artist, "Artists") + "</span>");
                    break;
                case "genre":
                    $(divTitleDesc).html("<span class='search-result-text'><b>Genre</b>: " + GetLinks(singleEntity.Type, "Genre") + "</span>");
                    break;
            }
        }

        $(anchor).attr("href", "/Movie/" + singleEntity.Link);
        $(anchor).append(GetImageElement(singleEntity, "movie"));
        $(anchor).append(divTitleDesc);

        $(li).append(anchor);
        $("#targetUL").append(li);
    }

    SearchResults.prototype.GetArtistItem = function (singleEntity) {

        if (searchResultCounter >= 6) {
            return;
        }

        var artistName = this.GetMatchArtistName(singleEntity);
        var that = this;
        artistName = artistName.filter(function (ar) {
            return !that.IsEntityAdded(ar);
        });

        artistName.forEach(function (artist) {
            var li = $("<li>");
            var divTitleDesc = $("<div>");
            var anchor = $("<a>");

            $(divTitleDesc).attr("class", "search-result-desc");
            $(divTitleDesc).html("<span class='search-result-title'>" + artist + "</span>");

            $(anchor).attr("href", "/Artists/" + artist.split(" ").join("-"));
            $(anchor).append(GetImageElement(singleEntity, "artist"));
            $(anchor).append(divTitleDesc);

            $(li).append(anchor);
            $("#targetUL").append(li);

            entityList.push(artist);
            searchResultCounter++;
        });
    }

    SearchResults.prototype.GetCriticsItem = function (singleEntity) {

        if (searchResultCounter >= 6) {
            return;
        }

        var criticsName = this.GetMatchCriticsName(singleEntity);
        var that = this;
        criticsName = criticsName.filter(function (ar) {
            return !that.IsEntityAdded(ar);
        });

        criticsName.forEach(function (critics) {
            var li = $("<li>");
            var divTitleDesc = $("<div>");
            var anchor = $("<a>");

            $(divTitleDesc).attr("class", "search-result-desc");
            $(divTitleDesc).html("<span class='search-result-title'>" + critics + "</span>");

            $(anchor).attr("href", "/Movie/Reviewer/" + singleEntity.Link);
            $(anchor).append(GetImageElement(singleEntity, "critics"));
            $(anchor).append(divTitleDesc);

            $(li).append(anchor);
            $("#targetUL").append(li);

            entityList.push(critics);
            searchResultCounter++;
        });
    }

    SearchResults.prototype.GetGenreItem = function (singleEntity) {
        if (searchResultCounter >= 6) {
            return;
        }

        var li = $("<li>");
        var divTitleDesc = $("<div>");
        var anchor = $("<a>");

        $(divTitleDesc).attr("class", "search-result-desc");
        $(divTitleDesc).html("<span class='search-result-title'>" + singleEntity.Title + "</span><span class='search-result-text'><b>Genre</b>: " + GetLinks(singleEntity.Type, "/Movie/Reviewer") + "</span>");

        $(anchor).attr("href", "/Genre/" + singleEntity.Link);
        $(anchor).append(GetImageElement(singleEntity, "movie"));
        $(anchor).append(divTitleDesc);

        $(li).append(anchor);

        $("#targetUL").append(li);
        searchResultCounter++;
    }

    SearchResults.prototype.IsEntityAdded = function (title) {
        return $.inArray(title, entityList) > -1;
    }

    SearchResults.prototype.GetMatchArtistName = function (singleEntity) {
        var query = this.GetSearchQuery();

        var artists = JSON.parse(singleEntity.Description);
        var match = artists.filter(function (ar) {
            if (ar) {
                return ar.toLowerCase().indexOf(query) === 0;
            }
        });

        return match;
    }

    SearchResults.prototype.GetMatchCriticsName = function (singleEntity) {
        var query = this.GetSearchQuery();

        var critics = JSON.parse(singleEntity.Critics);
        var match = critics.filter(function (cr) {
            if (cr) {
                return cr.toLowerCase().indexOf(query) === 0;
            }
        });

        return match;
    }

    SearchResults.prototype.GetProcessedArtists = function (artists) {
        var query = this.GetSearchQuery();

        var filtArtists = artists.filter(function (a) {
            return !a && a.toLowerCase().indexOf(query) > -1;
        });

        return $.unique(filtArtists.length > 0 ? filtArtists : artists).join("| ");
    }
}

function GetImageElement(singleEntity, type) {
    var img = $("<img/>");
    var divImage = $("<div>");

    $(divImage).attr("class", "search-image");
    img.attr("class", "img-thumbnail");
    img.attr("class", "movie-poster");

    if (type === "movie" && singleEntity.TitleImageURL !== "") {
        img.attr("src", "/Posters/Images/" + singleEntity.TitleImageURL);
    } else if (type === "artist" || type === "critics") {
        img.attr("src", "/Images/user.png");
        img.attr("class", "person-poster");
    } else {
        img.attr("src", "/Posters/Images/default-movie.jpg");
    }

    divImage.append(img);
    return divImage;
}

function GetCriticsLinks(singleEntity) {

}

function GetArtistsLinks(singleEntity) {
    var query = this.GetSearchQuery();

    var artists = JSON.parse(singleEntity.Description);
    var match = artists.filter(function (ar) {
        if (ar) {
            return ar.toLowerCase().indexOf(query) === 0;
        }
    });

    return GetLinks(match.join("|"), "Artists");
}