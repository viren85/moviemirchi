function PopulatingUserFavorite() {
    var url = "api/GetCastByType?t=all&l=3";
    CallHandler(url, OnSuccessPopulatingUserFavorite);
}

function OnSuccessPopulatingUserFavorite(result) {
    try {
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
        else {
            $("#fav_actor").html(result.UserMessage);
        }
    } catch (e) {
        $("#fav_actor").html("");
    }
}


function SaveUserFavorite() {
    var favArtists = [];
    var favCritics = [];
    var favGenre = [];
    var feedbackRate = 1;
    var FavoriteList = [];

    $(".feedback-rating span").each(function () {
        if ($(this).attr("class") != "feedback-default" && $(this).attr("class") != "feedback-last") {
            feedbackRate = $(this).html();
        }
    });

    FavoriteList.push({ "Type": "Rate", "Name": feedbackRate });

    $("#artist-pref-screen ul li").each(function () {
        if ($(this).attr("class") == "selected") {
            FavoriteList.push({ "Type": "Actor", "Name": $(this).find(".artist-name").html() });
        }
    });

    $("#critics-pref-screen ul li").each(function () {
        if ($(this).attr("class") == "selected") {
            FavoriteList.push({ "Type": "Critics", "Name": $(this).find(".artist-name").html() });
        }
    });

    $("#genre-pref-screen ul.movie-genre li").each(function () {
        
        if ($(this).attr("selected") == "selected") {
            FavoriteList.push({ "Type": "Genre", "Name": $(this).find(".movie-genre").html() });
        }
    });

    var userId = $("#hfUserId").val();
    var cFavoriteId = new Util().GetCookie("favoriteId");

    CallHandler("api/SaveUserFavorite?u=" + userId + "&c=" + cFavoriteId + "&d=" + encodeURI(JSON.stringify(FavoriteList)), OnSuccessSaveUserFavorite);

    //favArtists

    /*var actorName = $("#txtSearchActor").val();
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

    var cFavoriteId = new Util().GetCookie("favoriteId");

    if (isValid) {
        CallHandler("api/SaveUserFavorite?u=" + userId + "&c=" + cFavoriteId + "&d=" + encodeURI(JSON.stringify(FavoriteList)), OnSuccessSaveUserFavorite);
    }
    else {
        $("#favStatus").attr("style", "display:block");
        $("#favStatus").html("Please enter actor name or select genre");
    }*/
}

function OnSuccessSaveUserFavorite(result) {
    try {
        result = JSON.parse(result);

        if (result.Status == "Ok") {

            if (result.Message == "Set Cookie") {
                new Util().SetCookie("favoriteId", result.FavoriteId, 365);
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
            $("#favStatus").html(result.UserMessage);

            if (result.Message == "No Updated") {
                $("#favStatus").html("You already set your favorites. Login for update.");
            }

            var intverval = setInterval(function () {
                $("#favStatus").hide(500);
                $("#favStatus").html("");
                clearInterval(intverval);
            }, 10000);
        }
    } catch (e) {
        $("#favStatus").html("Unable to save your favorites. Please try agail later");
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