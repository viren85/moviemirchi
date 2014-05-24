$(document).ready(function () {
    $(".clear-search-bar").click(function () {
        $("#targetUL").hide();
        $("#home-search").val("");
        $(this).hide();
    });

    $("#home-search").keyup(function (e) {
        $("#target").val("");
        var query = $(this).val().replace(".", "");

        if (query.length > 0)
            $("#search-bar .clear-search-bar").show();
        else
            $("#search-bar .clear-search-bar").hide();

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
    if (response != null) {
        if ($("#targetUL") != undefined) {
            //If the UL element is not null or undefined we are clearing it, so that the result is appended in new UL every next time.
            $("#targetUL").remove();
        }
        //assigning json response data to local variable. It is basically list of values.
        data = response;

        if (data.length < 1 || data.length == "undefined") {
            $("#search-results").append($("<ul id='targetUL' style='display: block'><li style='height: 35px'>No results found for " + $("#home-search").val() + "</li></ul>"));
        }
        else {
            //appending an UL element to show the values.
            $("#search-results").append($("<ul id='targetUL' style='display:block;'></ul>"));
            //Removing previously added li elements to the list.
            $("#search-results").find("li").remove();
            //We are iterating over the list returned by the json and for each element we are creating a li element and appending the li element to ul element.
            var searchResultCounter = 0;
            var query = $("#home-search").val().toLowerCase();
            new SearchResults(data).Init();
            $.each(data, function (i, value) {

                /*$(".home-search-bar").find(".clear-search-bar").show();
                //On click of li element we are calling a method.
                if (searchResultCounter < 6) {
                    searchResultCounter++;
                    var li = $("<li>");
                    var divImage = $("<div>");
                    $(divImage).attr("class", "search-image");
                    var img = $("<img/>")
                    img.attr("class", "img-thumbnail");

                    var description = JSON.parse(value.Description);
                    var reviewer = JSON.parse(value.Critics);
                    var actors = "";
                    var critics = "";
                    var counter = 0;

                    for (var i = 0; i < description.length && counter < 3; i++) {
                        if (description[i] != null && description[i].toLowerCase().indexOf(query) > -1 && actors.indexOf(description[i]) < 0) {
                            actors += description[i] + "| ";
                            counter++;
                        }
                    }

                    if (actors == "") {
                        for (var i = 0; i < description.length && counter < 3; i++) {
                            actors += description[i] + "| ";
                            counter++;
                        }
                    }

                    if (actors.length > 0) {
                        actors = actors.substring(0, actors.lastIndexOf("|"));
                    }

                    counter = 0;
                    if (reviewer != null) {
                        for (var i = 0; i < reviewer.length && counter < 3; i++) {
                            if (reviewer[i] != null) {
                                critics += reviewer[i] + "| ";
                                counter++;
                            }
                        }
                    }
                    else {
                        critics += "Anupama Chopra | Taran Adarsh | Rajeev Masand | ";
                    }

                    if (critics.length > 0) {
                        critics = critics.substring(0, critics.lastIndexOf("|"));
                    }

                    img.attr("class", "movie-poster");
                    if (value.TitleImageURL != "") {
                        img.attr("src", "/Posters/Images/" + value.TitleImageURL);
                    }
                    else {
                        img.attr("src", "/Posters/Images/default-movie.jpg");
                    }

                    divImage.append(img);

                    var divTitleDesc = $("<div>");
                    $(divTitleDesc).attr("class", "search-result-desc");

                    $(divTitleDesc).html("<span class='search-result-title'>" + value.Title + "</span><span class='search-result-text'><b>Genre</b>: " + GetLinks(value.Type, "Genre") + "</span><span class='search-result-text'><b>Artists</b>: " + GetLinks(actors, "Artists") + "</span><span class='search-result-text'><b>Critics</b>: " + GetLinks(critics, "Movie/Reviewer") + "</span>");

                    var anchor = $("<a>");
                    $(anchor).attr("href", "/Movie/" + value.Link);

                    $(anchor).append(divImage);
                    $(anchor).append(divTitleDesc);

                    $(li).append(anchor);

                    $("#targetUL").append(li);
                }
                */
            });
        }
    }
    else {
        //If data is null the we are removing the li and ul elements.
        $("#targetUL").find("li").remove();
        $("#targetUL").remove();
    }

    if ($("#target").val() == "")
        $("#search-results").append($("#targetUL"));
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

    SearchResults.prototype.Init = function () {
        for (var i = 0; i < resultData.length && i < 6; i++) {
            this.GetItems(resultData[i]);
        }
    }

    SearchResults.prototype.GetItems = function (singleEntity) {
        var key = $("#home-search").val().toLowerCase().split(".").join("");

        if (singleEntity.Title.toLowerCase().indexOf(key) > -1) {
            // This is movie entity hence show the movie item
            this.GetMovieItem(singleEntity);
        }
        else if (singleEntity.Description.toLowerCase().indexOf(key) > -1) {
            // This is artist entity hence show the artist item
            this.GetArtistItem(singleEntity);
        }
        else if (singleEntity.Critics.toLowerCase().indexOf(key) > -1) {
            // This is critics entity hence show the critics item
            this.GetCriticsItem(singleEntity);
        }
        else if (singleEntity.Type.toLowerCase().indexOf(key) > -1) {
            // This is genre entity hence show the genre item
            this.GetGenreItem(singleEntity);
        }
    }

    SearchResults.prototype.GetMovieItem = function (singleEntity) {
        var li = $("<li>");
        var divTitleDesc = $("<div>");
        var anchor = $("<a>");

        $(divTitleDesc).attr("class", "search-result-desc");
        $(divTitleDesc).html("<span class='search-result-title'>" + singleEntity.Title + "</span>");

        $(anchor).attr("href", "/Movie/" + singleEntity.Link);
        $(anchor).append(GetImageElement(singleEntity));
        $(anchor).append(divTitleDesc);

        $(li).append(anchor);

        $("#targetUL").append(li);
    }

    SearchResults.prototype.GetArtistItem = function (singleEntity) {
        var li = $("<li>");
        var divTitleDesc = $("<div>");
        var anchor = $("<a>");

        $(divTitleDesc).attr("class", "search-result-desc");
        $(divTitleDesc).html("<span class='search-result-title'>" + GetArtistsLinks(singleEntity) + "</span>");

        $(anchor).attr("href", "/Artists/" + singleEntity.Link);
        $(anchor).append(GetImageElement(singleEntity));
        $(anchor).append(divTitleDesc);

        $(li).append(anchor);

        $("#targetUL").append(li);
    }

    SearchResults.prototype.GetCriticsItem = function (singleEntity) {
        var li = $("<li>");
        var divTitleDesc = $("<div>");
        var anchor = $("<a>");

        $(divTitleDesc).attr("class", "search-result-desc");
        $(divTitleDesc).html("<span class='search-result-title'>" + singleEntity.Critics + "</span>");

        $(anchor).attr("href", "/Movie/Reviewer/" + singleEntity.Link);
        $(anchor).append(GetImageElement(singleEntity));
        $(anchor).append(divTitleDesc);

        $(li).append(anchor);

        $("#targetUL").append(li);
    }

    SearchResults.prototype.GetGenreItem = function (singleEntity) {
        var li = $("<li>");
        var divTitleDesc = $("<div>");
        var anchor = $("<a>");

        $(divTitleDesc).attr("class", "search-result-desc");
        $(divTitleDesc).html("<span class='search-result-title'>" + singleEntity.Type + "</span>");

        $(anchor).attr("href", "/Genre/" + singleEntity.Link);
        $(anchor).append(GetImageElement(singleEntity));
        $(anchor).append(divTitleDesc);

        $(li).append(anchor);

        $("#targetUL").append(li);
    }
}

function GetImageElement(singleEntity) {
    var img = $("<img/>");
    var divImage = $("<div>");
    $(divImage).attr("class", "search-image");

    img.attr("class", "img-thumbnail");
    img.attr("class", "movie-poster");
    if (singleEntity.TitleImageURL != "") {
        img.attr("src", "/Posters/Images/" + singleEntity.TitleImageURL);
    }
    else {
        img.attr("src", "/Posters/Images/default-movie.jpg");
    }

    divImage.append(img);
    return divImage;
}

function GetCriticsLinks(singleEntity) {

}

function GetArtistsLinks(singleEntity) {
    var actors = "";
    var counter = 0;
    var description = singleEntity.Description;
    var query = $("#home-search").val().toLowerCase().split(".").join("");

    for (var i = 0; i < description.length && counter < 3; i++) {
        if (description[i] != null && description[i].toLowerCase().indexOf(query) > -1 && actors.indexOf(description[i]) < 0) {
            actors += description[i] + "| ";
            counter++;
        }
    }

    if (actors == "") {
        for (var i = 0; i < description.length && counter < 3; i++) {
            actors += description[i] + "| ";
            counter++;
        }
    }

    if (actors.length > 0) {
        actors = actors.substring(0, actors.lastIndexOf("|"));
    }

    return GetLinks(actors, "Artists");
}