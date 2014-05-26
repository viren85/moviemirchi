$(document).ready(function () {
    $(".clear-search-bar").click(function () {
        $("#targetUL").hide();
        $("#home-search").val("");
        $(this).hide();
    });

    $(document).click(function () {
        $(".home-search-bar .clear-search-bar").hide();
        $("#search-results").hide();
        $("#home-search").val("");
    });

    $("#home-search").keyup(function (e) {
        $("#target").val("");
        var query = $(this).val().replace(".", "");

        if (query.length > 3) {
            $(".home-search-bar .clear-search-bar").show();
        } else {
            $(".home-search-bar .clear-search-bar").hide();
            $("#search-results").hide();
        }

        // kyeCode is 27 for 'ESC' keypress. On Esc we want to dismiss search
        if (e.keyCode === 27) {
            $(".home-search-bar.clear-search-bar").hide();
            $("#search-results").hide();
            $("#home-search").val("");
        }
            // We want to force search when the click button is pressed
            // On keyup originalEvent is set to an Event
            // When keyup is called from click, originalEvent is not set

            // keyCode is 13 for 'Enter' keypress. On Enter we want to treat it as click on Search button
        else if (query.length > 3 || !e.originalEvent || e.keyCode === 13) {
            getItems(query);
            $("#search-results").show();
        }
    });

    $("#home-search").click(function (e) {
        e.stopPropagation();
        return false;
    });

    $(".search-button").click(function (e) {
        $("#targetUL").remove();
        $("#home-search").keyup();
        $(".home-search-bar .clear-search-bar").show();
        e.stopPropagation();
        return false;
    });
});

function getItems(query) {

    var searchPath = "AutoComplete/AutoCompleteMovies?query=" + query;
    CallHandler(searchPath, function (response) {

        if (response) {
            if ($("#targetUL")) {
                //If the UL element is not null or undefined we are clearing it, so that the result is appended in new UL every next time.
                $("#targetUL").remove();
            }

            //assigning json response data to local variable. It is basically list of values.
            var data = response;

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
    });
}

var SearchResults = function (searchResults) {

    var resultData = searchResults;
    var entityList = [];
    var searchResultCounter = 0;
    var MaxEntries = 6;

    SearchResults.prototype.GetSearchQuery = function () {
        return $("#home-search").val().toLowerCase().split(".").join("");
    };

    SearchResults.prototype.Init = function () {

        var that = this;
        resultData.forEach(function (result, index) {
            if (index < MaxEntries) {
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

            if (singleEntity.Title && singleEntity.Title.toLowerCase().indexOf(key) > -1 && !this.IsEntityAdded(singleEntity.Title) && searchResultCounter < MaxEntries) {

                // This is movie entity hence show the movie item
                this.GetMovieItem(singleEntity);
            } else if (singleEntity.Critics && singleEntity.Critics.toLowerCase().indexOf(key) > -1 && searchResultCounter < MaxEntries) {

                // This is critics entity hence show the critics item
                if (!this.IsEntityAdded(singleEntity.Title) && searchResultCounter < MaxEntries) {
                    this.GetCriticsItem(singleEntity);
                }
            } else if (singleEntity.Type && singleEntity.Type.toLowerCase().indexOf(key) > -1) {

                // This is genre entity hence show the genre item
                this.GetGenreItem(singleEntity);
            }

            if (singleEntity.Description && singleEntity.Description.toLowerCase().indexOf(key) > -1) {

                // This is artist entity hence show the artist item
                this.GetArtistItem(singleEntity);

                if (!this.IsEntityAdded(singleEntity.Title) && searchResultCounter < MaxEntries) {
                    this.GetMovieItem(singleEntity, true, "artists");
                }
            }
        }
    };

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
                    if (artist !== "") {
                        $(divTitleDesc).html($(divTitleDesc).html() + "<span class='search-result-text'><b>Artist</b>: " + GetLinks(artist, "Artists") + "</span>");
                    }
                    break;
                case "genre":
                    $(divTitleDesc).html("<span class='search-result-text'><b>Genre</b>: " + GetLinks(singleEntity.Type, "Genre") + "</span>");
                    break;
            }
        }

        $(anchor).attr("href", "/Movie/" + singleEntity.Link);
        $(anchor).append(this.GetImageElement(singleEntity, "movie"));
        $(anchor).append(divTitleDesc);

        $(li).append(anchor);
        $("#targetUL").append(li);

        entityList.push(singleEntity.Title);
        searchResultCounter++;
    };

    SearchResults.prototype.GetArtistItem = function (singleEntity) {

        if (searchResultCounter >= MaxEntries) {
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
            $(anchor).append(that.GetImageElement(singleEntity, "artist"));
            $(anchor).append(divTitleDesc);

            $(li).append(anchor);
            $("#targetUL").append(li);

            entityList.push(artist);
            searchResultCounter++;
        });
    }

    SearchResults.prototype.GetCriticsItem = function (singleEntity) {

        if (searchResultCounter >= MaxEntries) {
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

            $(anchor).attr("href", "/Movie/Reviewer/" + critics.split(" ").join("-"));
            $(anchor).append(that.GetImageElement(singleEntity, "critics"));
            $(anchor).append(divTitleDesc);

            $(li).append(anchor);
            $("#targetUL").append(li);

            entityList.push(critics);
            searchResultCounter++;
        });
    };

    SearchResults.prototype.GetGenreItem = function (singleEntity) {
        if (searchResultCounter >= MaxEntries) {
            return;
        }
        else {
            var gen = this.GetGenre(singleEntity);

            if (!this.IsEntityAdded(gen)) {
                entityList.push(gen);

                // add list item in search result
                var li = $("<li>");
                var divTitleDesc = $("<div>");
                var anchor = $("<a>");

                $(divTitleDesc).attr("class", "search-result-desc");
                $(divTitleDesc).html("<span class='search-result-title'>" + gen + "</span>");

                $(anchor).attr("href", "/Genre/" + gen);
                $(anchor).append(this.GetImageElement(singleEntity, "genre"));
                $(anchor).append(divTitleDesc);

                $(li).append(anchor);

                $("#targetUL").append(li);
                searchResultCounter++;
            }
        }

        var li = $("<li>");
        var divTitleDesc = $("<div>");
        var anchor = $("<a>");

        $(divTitleDesc).attr("class", "search-result-desc");
        $(divTitleDesc).html("<span class='search-result-title'>" + singleEntity.Title + "</span><span class='search-result-text'><b>Genre</b>: " + GetLinks(singleEntity.Type, "/Movie/Reviewer") + "</span>");

        $(anchor).attr("href", "/Genre/" + singleEntity.Link);
        $(anchor).append(this.GetImageElement(singleEntity, "movie"));
        $(anchor).append(divTitleDesc);

        $(li).append(anchor);

        $("#targetUL").append(li);
        searchResultCounter++;
    }

    SearchResults.prototype.IsEntityAdded = function (title) {
        return $.inArray(title, entityList) > -1;
    };

    SearchResults.prototype.FilterLambda = function (query) {
        return function (a) {
            if (a) {
                a = a.toLowerCase();
                if (a.indexOf(query) !== -1) {

                    // 'ali ' should match ' sajid ali khan'
                    // 'deep ' should match 'deep banarjee' and not match 'deepika padukone'
                    // As a side-effect 'a b' will now match 'xxa bxx' -> which is low-pri thus ok
                    if (query.indexOf(' ') !== -1) {
                        return true;
                    }

                    // 'padukone' should match 'deepika padukone'
                    // 'deep' should not match 'sandeep'
                    return (a.split(' ').filter(function (s) {
                        return s.indexOf(query) === 0;
                    }).length > 0);
                }
            }

            return false;
        }
    };

    SearchResults.prototype.GetMatchArtistName = function (singleEntity) {

        var query = this.GetSearchQuery();
        var artists = JSON.parse(singleEntity.Description);
        var match = artists.filter(SearchResults.prototype.FilterLambda(query));
        return $.unique(match);
    };

    SearchResults.prototype.GetMatchCriticsName = function (singleEntity) {

        var query = this.GetSearchQuery();
        var critics = JSON.parse(singleEntity.Critics);
        var match = critics.filter(SearchResults.prototype.FilterLambda(query));
        return $.unique(match);
    };

    SearchResults.prototype.GetProcessedArtists = function (artists) {

        var query = this.GetSearchQuery();
        var filtArtists = artists.filter(SearchResults.prototype.FilterLambda(query));
        return $.unique(filtArtists.length > 0 ? filtArtists : artists).join("| ");
    };

    SearchResults.prototype.GetGenre = function (singleEntity) {
        var query = this.GetSearchQuery();
        var genreList = singleEntity.Type.split("|");
        var matchedItem = "";
        genreList.forEach(function (item) {
            if (matchedItem == "" && item.trim().toLowerCase().indexOf(query) > -1) {
                matchedItem = item.trim();
            }
        });

        return matchedItem;
    };

    SearchResults.prototype.GetImageElement = function (singleEntity, type) {
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
    };
};