$(document).ready(function () {
    $(".clear-search-bar").click(function () {
        $("#target").val("");
        $("#targetUL").hide();
        $("#home-search").val("");
        $(this).hide();
    });

    //We have used keyup event to track the user enter value in the textbox.
    $("#target").keyup(function () {
        $("#home-search").val("");
        //Fetching the textbox value.
        var query = $(this).val().replace(".", "");

        //clear-search-bar
        if (query.length > 0)
            $("#search-results").show();
        else
            $("#search-results").hide();

        getItems(query);
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

        if (data.length < 1 || data.length == undefined) {
            $("#targetDiv").append($("<ul id='targetUL' style='display:none;'></ul>"));
        }
        else {
            //appending an UL element to show the values.
            $("#search-results").append($("<ul id='targetUL' style='display:block;'></ul>"));
            //Removing previously added li elements to the list.
            $("#search-results").find("li").remove();
            //We are iterating over the list returned by the json and for each element we are creating a li element and appending the li element to ul element.
            var searchResultCounter = 0;

            $.each(data, function (i, value) {
                $(".home-search-bar").find(".clear-search-bar").show();
                //On click of li element we are calling a method.
                if (searchResultCounter < 6) {
                    searchResultCounter++;
                    var li = $("<li>");
                    var divImage = $("<div>");
                    $(divImage).attr("style", "min-width: 15%; min-height: 50px; float: left;");
                    var img = $("<img/>")
                    img.attr("class", "img-thumbnail");
                    img.attr("style", "width: 50px; height: 50px;margin-right: 1%");

                    var description = JSON.parse(value.Description);

                    var actors = "";

                    for (var i = 0; i < description.length; i++) {
                        actors += description[i] + ", ";
                    }

                    if (actors.length > 0) {
                        actors = actors.substring(0, actors.lastIndexOf(","));
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
                    $(divTitleDesc).attr("style", "width: 80%;float: left;");

                    $(divTitleDesc).html("<span style='width:100%; font-weight: bold;float: left;font-size: 16px;'>" + value.Title + "</span><span style='width:100%;float: left;color: #666666;font-size: 11px;margin-top: 2px;'>" + value.Type + "</span>");

                    var anchor = $("<a>");
                    $(anchor).attr("href", "/Movie/" + value.Link);

                    $(anchor).append(divImage);
                    $(anchor).append(divTitleDesc);

                    $(li).append(anchor);

                    $("#targetUL").append(li);

                    //$("#targetUL").append($("<li class='targetLI' onclick='javascript:appendTextToTextBox(this)'>" + value.Title + "</li>"));
                }
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