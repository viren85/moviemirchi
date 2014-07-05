var Artists = function () {
    var testArtists = [
                           {
                               "ArtistName": "Amitabh Bachhan",
                               "Role": "Actor",
                               "CharacterName": "Bhoot"
                           },
                           {
                               "ArtistName": "Hritik Roshan",
                               "Role": "Actor",
                               "CharacterName": "Krrish"
                           },
                           {
                               "ArtistName": "Priyanka Chopra",
                               "Role": "Actor",
                               "CharacterName": "Heroine"
                           },
                            {
                                "ArtistName": "Amitabh Bachhan",
                                "Role": "Actor",
                                "CharacterName": "Bhoot"
                            },
                           {
                               "ArtistName": "Hritik Roshan",
                               "Role": "Actor",
                               "CharacterName": "Krrish"
                           },
                           {
                               "ArtistName": "Priyanka Chopra",
                               "Role": "Actor",
                               "CharacterName": "Heroine"
                           }

    ];

    Artists.prototype.GetArtistGrid = function (artistList) {
        if (artistList == null || artistList == "undefined") {
            artistList = testArtists;
        }

        var container = $("<div/>").attr("class", "artists-container");
        var sectionTitle = new MovieInformation().GetMovieInfoContainer("artist-section-title", "Artists");

        var grid = $("<div/>").attr("class", "artist-grid").attr("id", "sortable");
        var gridHead = $("<div/>").attr("class", "artist-grid-header");

        var gridCol1 = $("<div/>").attr("class", "artist-grid-column").html("Artist Name");
        var gridCol2 = $("<div/>").attr("class", "artist-grid-column").html("Role");
        var gridCol3 = $("<div/>").attr("class", "artist-grid-column").html("Character");

        $(gridHead).append(gridCol1).append(gridCol2).append(gridCol3);
        $(grid).append(gridHead);

        for (i = 0; i < artistList.length; i++) {
            var gridRow = $("<div/>").attr("class", "artist-grid-row");

            var gridRowData1 = $("<div/>").attr("class", "artist-grid-row-data1").html(artistList[i].name);

            //var gridRowData2 = $("<div/>").attr("class", "artist-grid-row-data2").html(artistList[i].role);
            //var gridRowData3 = $("<div/>").attr("class", "artist-grid-row-data3").html(artistList[i].charactername);
            var gridRowData2 = $("<div/>").attr("class", "artist-grid-row-data2").append($("<input/>").attr("type", "text").attr("style", "min-width:80px;width:100px").attr("title", artistList[i].role).val(artistList[i].role));
            var gridRowData3 = $("<div/>").attr("class", "artist-grid-row-data3").append($("<input/>").attr("type", "text").attr("style", "min-width:80px;width:140px").attr("title", artistList[i].charactername).val(artistList[i].charactername));

            $(gridRow).append(gridRowData1);
            $(gridRow).append(gridRowData2);
            $(gridRow).append(gridRowData3);

            if (artistList[i].name != null && artistList[i].name != undefined && artistList[i].name != "") {
                $(grid).append(gridRow);
            }
        }

        return $(container).append(sectionTitle).append(grid);
    }

    Artists.prototype.BuildForm = function () {
        var formContainer = $("<div/>").attr("class", "form-container");

        var isEnabled = new FormBuilder().GetCheckBox("isEnabled", "Enabled", false);
        var uniqueName = new FormBuilder().GetTextField("txtUnique", "Unique Name", "Unique Name");
        var artistName = new FormBuilder().GetTextField("txtArtistName", "Artist Name", "Artist Name");
        var bio = new FormBuilder().GetTextArea("txtBio", "Bio", "Bio");
        var born = new FormBuilder().GetTextArea("txtBorn", "Born", "Born");
        var popularity = new FormBuilder().GetTextField("txtPopularity", "Popularity", "Popularity");

        $(formContainer).append(isEnabled);

        $(formContainer).append(uniqueName);
        $(formContainer).append(artistName);
        $(formContainer).append(bio);
        $(formContainer).append(born);
        $(formContainer).append(popularity);

        return formContainer;
    }

    Artists.prototype.PopulateArtistDetails = function (artist) {        
        console.log(artist);
        //basic info
        $("#txtUnique").val(artist.UniqueName);
        $("#txtArtistName").val(artist.ArtistName);
        $("#txtBio").val(artist.Bio);
        $("#txtBorn").val(artist.Born);
        $("#txtPopularity").val(artist.Popularity);

        // rating / my score
        if (artist.MyScore != "" && artist.MyScore != undefined) {
            var myScore = JSON.parse(artist.MyScore);
            $("#txtTeekhaRate").val(myScore.teekharating);
            $("#txtFeekaRate").val(myScore.feekharating);
            $("#txtMyScore").val(myScore.criticrating);
        }
        else {
            $("#txtTeekhaRate").val("");
            $("#txtFeekaRate").val("");
            $("#txtMyScore").val("");
        }

        $(".posters-container").html("");

        if (artist.Posters != "" && artist.Posters != undefined) {
            var posters = JSON.parse(artist.Posters);
            $(".posters-container").append(new Posters().GetPosterContainer(posters));
        }
        $(".shortcut-container").html("");
        $(".shortcut-container").append($("<a/>").html("Save changes").attr("onclick", "updateArtist()").attr("class", "btn btn-success").attr("title", "click here to save all the changes."));
        $(".shortcut-container").append($("<div>").attr("id", "status"));
        // upload files
        $("#poster-upload").attr("onchange", "UploadSelectedFile(this)");
    }

    Artists.prototype.UpdateArtistDetails = function (artist) {
        console.log(artist);

        var uniqueName = $("#txtUnique").val();
        var artistName = $("#txtArtistName").val();
        var bio = $("#txtBio").val();
        var born = $("#txtBorn").val(); 
        var popularity = $("#txtPopularity").val();

        if (uniqueName != undefined && uniqueName != "") {
            artist.UniqueName = uniqueName
        }

        if (artistName != undefined && artistName != "") {
            artist.ArtistName = artistName
        }

        if (bio != undefined && bio != "") {
            artist.Bio = bio
        }

        if (born != undefined && born != "") {
            artist.Born = born
        }

        if (popularity != undefined && popularity != "") {
            artist.Popularity = popularity
        }

        //get ratings
        var myScore = { "teekharating": $("#txtTeekhaRate").val(), "feekharating": $("#txtFeekaRate").val(), "criticrating": $("#txtMyScore").val() };
        artist.MyScore = JSON.stringify(myScore);
        
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
            artist.Posters = JSON.stringify(posters);

        var objArtist = {
            "ArtistId": artist.ArtistId,
            "ArtistName": artist.ArtistName,
            "UniqueName": artist.UniqueName,
            "Bio": artist.Bio,
            "Born": artist.Born,
            "MovieList": artist.MovieList,
            "Popularity": artist.Popularity,
            "Posters": artist.Posters,
            "MyScore": artist.MyScore,
            "JsonString": artist.JsonString
        };

        // save movie
        var artistData = JSON.stringify(objArtist);
        
        CallController("../Home/UpdateArtists", "data", artistData, function () { $("#status").html("Artist details saved successfully!"); });
    }
}