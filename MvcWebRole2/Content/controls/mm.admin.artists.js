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
        //Below feilds added later on by Ranveer
        var nickName = new FormBuilder().GetTextField("txtNickName", "Nick Name", "Nick Name");
        var age = new FormBuilder().GetTextField("txtAge", "Age", "Age");
        var familyRelation = new FormBuilder().GetTextArea("txtFamilyRelation", "Family Relation", "Family Relation"); //json
        var dateOfBirth = new FormBuilder().GetTextField("txtDateOfBirth", "Date Of Birth", "Date Of Birth");
        var bornCity = new FormBuilder().GetTextField("txtBornCity", "Born City", "Born City");
        var zodiacSign = new FormBuilder().GetTextField("txtZodiacSign", "Zodiac Sign", "Zodiac Sign");
        var hobbies = new FormBuilder().GetTextArea("txtHobbies", "Hobbies", "Hobbies"); //json
        var educationalDetails = new FormBuilder().GetTextArea("txtEducationalDetails", "Educational Details", "Educational Details"); //json
        var socialActivities = new FormBuilder().GetTextArea("txtSocialActivities", "Social Activities", "Social Activities");  //json
        var debut = new FormBuilder().GetTextField("txtDebut", "Debut", "Debut");
        var rememberForMovies = new FormBuilder().GetTextArea("txtRememberForMovies", "Remember For Movies", "Remember For Movies"); //json
        var awards = new FormBuilder().GetTextArea("txtAwards", "Awards", "Awards");  //json
        var facebook = new FormBuilder().GetTextField("txtFacebook", "Facebook", "Facebook");
        var twitter = new FormBuilder().GetTextField("txtTwitter", "Twitter", "Twitter");
        var instagram = new FormBuilder().GetTextField("txtInstagram", "Instagram", "Instagram");
        var summary = new FormBuilder().GetTextArea("txtSummary", "Summary", "Summary");

        $(formContainer).append(isEnabled);

        $(formContainer).append(uniqueName);
        $(formContainer).append(artistName);
        $(formContainer).append(bio);
        $(formContainer).append(born);
        $(formContainer).append(popularity);

        // added by ry
        $(formContainer).append(nickName);
        $(formContainer).append(age);
        $(formContainer).append($("<div/>").attr("id", "familylink").attr("style", "width: 100%;").append(this.AddFamilyRelation(true)));
        $(formContainer).append(dateOfBirth);
        $(formContainer).append(bornCity);
        $(formContainer).append(zodiacSign);
        $(formContainer).append(hobbies);
        $(formContainer).append($("<div/>").attr("id", "education").attr("style", "width: 100%;").append(this.AddEducationalDetail(true)));
        $(formContainer).append(socialActivities);
        $(formContainer).append($("<div/>").attr("id", "debut").attr("style", "width: 100%;").append(this.AddDebut(true)));
        $(formContainer).append($("<div/>").attr("id", "remembered").attr("style", "width: 100%;").append(this.AddRememberedFor(true)));
        $(formContainer).append($("<div/>").attr("id", "awards").attr("style", "width: 100%;").append(this.AddAwards(true)));
        $(formContainer).append(facebook);
        $(formContainer).append(twitter);
        $(formContainer).append(instagram);
        $(formContainer).append(summary);

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
        //added by vasim
        $("#txtNickName").val(artist.ArtistNickName);
        $("#txtAge").val(artist.Age);
        $("#txtDateOfBirth").val(artist.DateOfBirth);
        $("#txtBornCity").val(artist.BornCity);
        $("#txtZodiacSign").val(artist.ZodiacSign);
        $("#txtHobbies").val(artist.Hobbies);
        $("#txtSocialActivities").val(artist.SocialActivities);
        $("#txtFacebook").val(artist.FacebookURL);
        $("#txtTwitter").val(artist.TwitterHandle);
        $("#txtInstagram").val(artist.InstagramURL);
        $("#txtSummary").val(artist.Summary);

        if (artist.FamilyRelation == null || artist.FamilyRelation == undefined)
            artist.FamilyRelation = "[]";
        
        if (artist.EducationDetails == null || artist.EducationDetails == undefined)
            artist.EducationDetails = "[]";

        if (artist.DebutFilms == null || artist.DebutFilms == undefined)
            artist.DebutFilms = "[]";

        if (artist.RememberForMovies == null || artist.RememberForMovies == undefined)
            artist.RememberForMovies = "[]";

        if (artist.Awards == null || artist.Awards == undefined)
            artist.Awards = "[]";
        
        //populate family relation
        $("#familylink").html("");
        var familyRelation = JSON.parse(artist.FamilyRelation);

        if (familyRelation.length > 0) {
            for (var i = 0; i < familyRelation.length; i++) {
                if (i == 0)
                    $("#familylink").append(new Artists().PopulateFamilyRelation(true, familyRelation[i]));
                else
                    $("#familylink").append(new Artists().PopulateFamilyRelation(false, familyRelation[i]));
            }
        }
        else {
            $("#familylink").append(new Artists().PopulateFamilyRelation(true));
        }

        // populate education details
        $("#education").html("");
        var education = JSON.parse(artist.EducationDetails);

        if (education.length > 0) {
            for (var i = 0; i < education.length; i++) {
                if (i == 0)
                    $("#education").append(new Artists().PopulateEducationalDetail(true, education[i]));
                else
                    $("#education").append(new Artists().PopulateEducationalDetail(false, education[i]));
            }
        }
        else {
            $("#education").append(new Artists().PopulateEducationalDetail(true));
        }

        //populate debut films
        $("#debut").html("");
        var debutFilm = JSON.parse(artist.DebutFilms);

        if (debutFilm.length > 0) {
            for (var i = 0; i < debutFilm.length; i++) {
                if (i == 0)
                    $("#debut").append(new Artists().PopulateDebut(true, debutFilm[i]));
                else
                    $("#debut").append(new Artists().PopulateDebut(false, debutFilm[i]));
            }
        }
        else {
            $("#debut").append(new Artists().PopulateDebut(true));
        }

        // remember movies
        $("#remembered").html("");
        var rememberFilm = JSON.parse(artist.RememberForMovies);

        if (rememberFilm.length > 0) {
            for (var i = 0; i < rememberFilm.length; i++) {
                if (i == 0)
                    $("#remembered").append(new Artists().PopulateRememberedFor(true, rememberFilm[i]));
                else
                    $("#remembered").append(new Artists().PopulateRememberedFor(false, rememberFilm[i]));
            }
        }
        else {
            $("#remembered").append(new Artists().PopulateRememberedFor(true));
        }

        // Awards awards
        $("#awards").html("");
        var awards = JSON.parse(artist.Awards);

        if (awards.length > 0) {
            for (var i = 0; i < awards.length; i++) {
                if (i == 0)
                    $("#awards").append(new Artists().PopulateAwards(true, awards[i]));
                else
                    $("#awards").append(new Artists().PopulateAwards(false, awards[i]));
            }
        }
        else {
            $("#awards").append(new Artists().PopulateAwards(true));
        }

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

        //get value for new properties
        artist.ArtistNickName = $("#txtNickName").val();
        artist.Age = $("#txtAge").val();
        artist.DateOfBirth = $("#txtDateOfBirth").val();
        artist.BornCity = $("#txtBornCity").val();
        artist.ZodiacSign = $("#txtZodiacSign").val();
        artist.Hobbies = $("#txtHobbies").val();
        artist.SocialActivities = $("#txtSocialActivities").val();
        artist.FacebookURL = $("#txtFacebook").val();
        artist.TwitterHandle = $("#txtTwitter").val();
        artist.InstagramURL = $("#txtInstagram").val();
        artist.Summary = $("#txtSummary").val();

        //get controlls value
        var familyRelations = [];
        var educationDetails = [];
        var debutFilms = [];
        var rememberFilms = [];
        var awards = [];

        $("#familylink").find(".Relation").each(function () {
            var value = $(this).find("input").val();
            if (value != "" && value != undefined)
                familyRelations.push(value);
        });

        $("#education").find(".Educational").each(function () {
            var value = $(this).find("input").val();
            if (value != "" && value != undefined)
                educationDetails.push(value);
        });

        $("#debut").find(".Debut_Start").each(function () {
            var value = $(this).find("input").val();
            if (value != "" && value != undefined)
                debutFilms.push(value);
        });

        $("#remembered").find(".Remember").each(function () {
            var value = $(this).find("input").val();
            if (value != "" && value != undefined)
                rememberFilms.push(value);
        });

        $("#awards").find(".awards-received").each(function () {
            var value = $(this).find("input").val();
            if (value != "" && value != undefined)
                awards.push(value);
        });

        artist.FamilyRelation = JSON.stringify(familyRelations);
        artist.EducationDetails = JSON.stringify(educationDetails);
        artist.DebutFilms = JSON.stringify(debutFilms);
        artist.RememberForMovies = JSON.stringify(rememberFilms);
        artist.Awards = JSON.stringify(awards);

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
            "TwitterHandle": artist.TwitterHandle,
            "JsonString": artist.JsonString,
            //added by vasim for extras info
            "ArtistNickName": artist.ArtistNickName,
            "Age": artist.Age,
            "FamilyRelation": artist.FamilyRelation,
            "DateOfBirth": artist.DateOfBirth,
            "BornCity": artist.BornCity,
            "ZodiacSign": artist.ZodiacSign,
            "Hobbies": artist.Hobbies,
            "EducationDetails": artist.EducationDetails,
            "SocialActivities": artist.SocialActivities,
            "DebutFilms": artist.DebutFilms,
            "RememberForMovies": artist.RememberForMovies,
            "Awards": artist.Awards,
            "FacebookURL": artist.FacebookURL,
            "InstagramURL": artist.InstagramURL,
            "Summary": artist.Summary
            //end
        };

        //UpdateArtist
        //CallController("../Home/UpdateArtists", "data", artistData, function () { $("#status").html("Artist details saved successfully!"); });
        CallController("api/UpdateArtist", "data", objArtist, function () { $("#status").html("Artist details saved successfully!"); });
    }

    // added on 23-feb-2015
    // Family Relation
    Artists.prototype.AddFamilyRelation = function (isAddButton) {
        var familyRelation = $("<div/>").attr("style", "width: 100%;margin: px;").attr("class", "Relation");
        $(familyRelation).append($("<span/>").text("Family Relationship"));

        $(familyRelation).append($("<input/>").attr("type", "text").attr("style", "width:50%").attr("placeholder", "Family Link"));

        var button = $("<div/>");

        if (isAddButton) {
            $(button).attr("class", "btn btn-success").html("Add ...").click(function () {
                $("#familylink").append(new Artists().AddFamilyRelation(false));
            });

        } else {
            $(button).attr("class", "btn btn-danger").html("X").click(function () {
                $(familyRelation).remove();
            });
        }

        $(familyRelation).append(button);

        return familyRelation;
    }

    //generate controls
    // Educational Detail
    Artists.prototype.AddEducationalDetail = function (isAddButton) {
        var educationalDetail = $("<div/>").attr("style", "width: 100%;margin: px;").attr("class", "Educational");
        $(educationalDetail).append($("<span/>").text("Educational Detail"));
        $(educationalDetail).append($("<input/>").attr("type", "text").attr("style", "width:50%").attr("placeholder", "Education"));

        var button = $("<div/>");

        if (isAddButton) {
            $(button).attr("class", "btn btn-success").html("Add ...").click(function () {
                $("#education").append(new Artists().AddEducationalDetail(false));
            });

        } else {
            $(button).attr("class", "btn btn-danger").html("X").click(function () {
                $(educationalDetail).remove();
            });
        }

        $(educationalDetail).append(button);

        return educationalDetail;
    }

    // Debut Film
    Artists.prototype.AddDebut = function (isAddButton) {
        var debut = $("<div/>").attr("style", "width: 100%;margin: px;").attr("class", "Debut_Start");
        $(debut).append($("<span/>").text("Debut"));
        $(debut).append($("<input/>").attr("type", "text").attr("style", "width:50%").attr("placeholder", "Debut"));




        var button = $("<div/>");

        if (isAddButton) {
            $(button).attr("class", "btn btn-success").html("Add ...").click(function () {
                $("#debut").append(new Artists().AddDebut(false));
            });

        } else {
            $(button).attr("class", "btn btn-danger").html("X").click(function () {
                $(debut).remove();
            });
        }

        $(debut).append(button);

        return debut;
    }

    // Remembered For
    Artists.prototype.AddRememberedFor = function (isAddButton) {
        var rememberedFor = $("<div/>").attr("style", "width: 100%;margin: px;").attr("class", "Remember");
        $(rememberedFor).append($("<span/>").text("Remembered For"));
        $(rememberedFor).append($("<input/>").attr("type", "text").attr("style", "width:50%").attr("placeholder", "Remembered"));

        var button = $("<div/>");

        if (isAddButton) {
            $(button).attr("class", "btn btn-success").html("Add ...").click(function () {
                $("#remembered").append(new Artists().AddRememberedFor(false));
            });

        } else {
            $(button).attr("class", "btn btn-danger").html("X").click(function () {
                $(rememberedFor).remove();
            });
        }

        $(rememberedFor).append(button);

        return rememberedFor;
    }

    // Awards
    Artists.prototype.AddAwards = function (isAddButton) {
        var awards = $("<div/>").attr("style", "width: 100%;margin: px;").attr("class", "awards-received");
        $(awards).append($("<span/>").text("Awards"));
        $(awards).append($("<input/>").attr("type", "text").attr("style", "width:50%").attr("placeholder", "Awards"));

        var button = $("<div/>");

        if (isAddButton) {
            $(button).attr("class", "btn btn-success").html("Add ...").click(function () {
                $("#awards").append(new Artists().AddAwards(false));
            });

        } else {
            $(button).attr("class", "btn btn-danger").html("X").click(function () {
                $(awards).remove();
            });
        }

        $(awards).append(button);

        return awards;
    }

    //populate controls
    Artists.prototype.PopulateFamilyRelation = function (isAddButton, family) {
        var familyRelation = $("<div/>").attr("style", "width: 100%;margin: px;").attr("class", "Relation");
        $(familyRelation).append($("<span/>").text("Family Relationship"));

        $(familyRelation).append($("<input/>").attr("type", "text").attr("style", "width:50%").attr("placeholder", "Family Link").val(family));

        var button = $("<div/>");

        if (isAddButton) {
            $(button).attr("class", "btn btn-success").html("Add ...").click(function () {
                $("#familylink").append(new Artists().AddFamilyRelation(false));
            });

        } else {
            $(button).attr("class", "btn btn-danger").html("X").click(function () {
                $(familyRelation).remove();
            });
        }

        $(familyRelation).append(button);

        return familyRelation;
    }

    // Educational Detail
    Artists.prototype.PopulateEducationalDetail = function (isAddButton, educatoion) {
        var educationalDetail = $("<div/>").attr("style", "width: 100%;margin: px;").attr("class", "Educational");
        $(educationalDetail).append($("<span/>").text("Educational Detail"));
        $(educationalDetail).append($("<input/>").attr("type", "text").attr("style", "width:50%").attr("placeholder", "Education").val(educatoion));

        var button = $("<div/>");

        if (isAddButton) {
            $(button).attr("class", "btn btn-success").html("Add ...").click(function () {
                $("#education").append(new Artists().AddEducationalDetail(false));
            });

        } else {
            $(button).attr("class", "btn btn-danger").html("X").click(function () {
                $(educationalDetail).remove();
            });
        }

        $(educationalDetail).append(button);

        return educationalDetail;
    }

    // Debut Film
    Artists.prototype.PopulateDebut = function (isAddButton, debutMovie) {
        var debut = $("<div/>").attr("style", "width: 100%;margin: px;").attr("class", "Debut_Start");
        $(debut).append($("<span/>").text("Debut"));
        $(debut).append($("<input/>").attr("type", "text").attr("style", "width:50%").attr("placeholder", "Debut").val(debutMovie));

        var button = $("<div/>");

        if (isAddButton) {
            $(button).attr("class", "btn btn-success").html("Add ...").click(function () {
                $("#debut").append(new Artists().AddDebut(false));
            });

        } else {
            $(button).attr("class", "btn btn-danger").html("X").click(function () {
                $(debut).remove();
            });
        }

        $(debut).append(button);

        return debut;
    }

    // Remembered For
    Artists.prototype.PopulateRememberedFor = function (isAddButton, rememberMovie) {
        var rememberedFor = $("<div/>").attr("style", "width: 100%;margin: px;").attr("class", "Remember");
        $(rememberedFor).append($("<span/>").text("Remembered For"));
        $(rememberedFor).append($("<input/>").attr("type", "text").attr("style", "width:50%").attr("placeholder", "Remembered").val(rememberMovie));

        var button = $("<div/>");

        if (isAddButton) {
            $(button).attr("class", "btn btn-success").html("Add ...").click(function () {
                $("#remembered").append(new Artists().AddRememberedFor(false));
            });

        } else {
            $(button).attr("class", "btn btn-danger").html("X").click(function () {
                $(rememberedFor).remove();
            });
        }

        $(rememberedFor).append(button);

        return rememberedFor;
    }

    // Awards
    Artists.prototype.PopulateAwards = function (isAddButton, award) {
        var awards = $("<div/>").attr("style", "width: 100%;margin: px;").attr("class", "awards-received");
        $(awards).append($("<span/>").text("Awards"));
        $(awards).append($("<input/>").attr("type", "text").attr("style", "width:50%").attr("placeholder", "Awards").val(award));

        var button = $("<div/>");

        if (isAddButton) {
            $(button).attr("class", "btn btn-success").html("Add ...").click(function () {
                $("#awards").append(new Artists().AddAwards(false));
            });

        } else {
            $(button).attr("class", "btn btn-danger").html("X").click(function () {
                $(awards).remove();
            });
        }

        $(awards).append(button);

        return awards;
    }
}