var UserPreferences = function () {
    var GetPreferenceButton = function () {
        var btn = $('<div/>').addClass("feedback-button").html("<div id='feedback-1'>What do you like?</div><div id='feedback-2' class='hide'>Movies</div><div id='feedback-3' class='hide'>Artists</div><div id='feedback-4' class='hide'>Critics</div>").click(function () {
            var id = $("li.userid").attr("userid");
            
            if (id != "" && id != undefined) {
                $("#user-pref").modal('show');
                $("#hfUserId").val(id);
            }
            else
                $('#Login').modal('show');
        });

        $('body').append(GetUserFeedbackScreen());

        $(".feedback-next,.feedback-prev").click(function () {
            $('.screen').hide();
            $("#" + $(this).attr("show-id")).show();
        });

        $(".movie-genre li").click(function () {
            var className = $(this).attr("class");
            if (className.indexOf("-active") <= -1) {
                $(this).attr("class", $(this).attr("class") + "-active");
                $(this).attr("selected", "true");
            }
            else {
                $(this).attr("class", $(this).attr("class").substring(0, $(this).attr("class").indexOf("-active")));
                $(this).attr("selected", "false");
            }
        });

        $(".feedback-rating span").mouseover(function () {
            var id = parseInt($(this).html());
            var counter = 1;

            $(".feedback-rating span").each(function () {
                if (counter < id) {
                    $(this).attr("class", "feedback-" + counter);
                }
                else if (counter == id) {
                    $(this).attr("class", "feedback-" + counter);
                    $(".feedback-content").attr("class", "feedback-content feedback-" + counter);
                    $(".feedback-content div.feedback-rate").hide();
                    $(".feedback-content").find(".smily-" + counter).show().parent().show();
                    $(".feedback-content").find(".smily-" + counter + " .feedback-smily").show();
                }
                else {
                    $(this).attr("class", "feedback-default");
                }

                if (counter == 10 && id == counter) {
                    $(this).attr("class", "feedback-last feedback-" + counter);
                }
                else if (counter == 10) {
                    $(this).attr("class", "feedback-last");
                }

                counter++;
            });

            $("#user-pref-screen .feedback-next").show();
            $('#artist-pref-screen .feedback-content').attr('class', 'feedback-content');
            $('#genre-pref-screen .feedback-content').attr('class', 'feedback-content');
            $('#critics-pref-screen .feedback-content').attr('class', 'feedback-content');
        });

        return btn;
    }

    var GetUserFeedbackScreen = function () {
        // call API to get the actual dynamic data from the server
        //UserPreferences
        var url = "/api/GetUserPopular";
        CallHandler(url, OnSuccessPopulateUserFeedback);

        var feedback = $("<div class=\"modal fade\" id=\"user-pref\" role=\"dialog\" aria-labelledby=\"myModalLabel2\" aria-haspopup=\"false\">" +
                "<div class=\"modal-dialog\" id=\"user-popup\">" +
                    "<div class=\"modal-content\">" +
                        "<div class=\"modal-header\" id=\"popup-header\">" +
                            "<button type=\"button\" class=\"close\" data-dismiss=\"modal\"><span aria-hidden=\"true\">&times;</span><span class=\"sr-only\">Close</span></button>" +
                            "<span class=\"icon-feedback\"></span><h3 class=\"modal-title\">What do you like?</h3><span>Your feedback help us know where we can do better.</span>" +
                        "</div>" +
                        "<div class=\"modal-body\" style=\"width: 100%; background-color: #fff;\">" +
                            "<div class=\"screen\" id=\"user-pref-screen\">" +
                                "<div class=\"feedback-title\">Would you recommend Movie Mirchi to your friends?</div>" +
                                "<div class=\"feedback-rating\"><span class=\"one\">1</span><span class=\"two\">2</span><span class=\"three\">3</span><span class=\"four\">4</span><span class=\"five\">5</span><span class=\"six\">6</span><span class=\"seven\">7</span><span class=\"eight\">8</span><span class=\"nine\">9</span><span class=\"ten feedback-last\">10</span></div>" +
                                "<div class=\"feedback-content\">" +
                                    "<div class=\"feedback-rate\" style=\"display: block\"><div class=\"smily-default\"></div><div>Start over.</div></div>" +
                                    "<div class=\"feedback-rate\"><div class=\"smily smily-1\"></div><div class=\"feedback-smily\">Hell No!</div></div>" +
                                    "<div class=\"feedback-rate\"><div class=\"smily smily-2\"></div><div class=\"feedback-smily\">I won't.</div></div>" +
                                    "<div class=\"feedback-rate\"><div class=\"smily smily-3\"></div><div class=\"feedback-smily\">Not Yet.</div></div>" +
                                    "<div class=\"feedback-rate\"><div class=\"smily smily-4\"></div><div class=\"feedback-smily\">Maybe.</div></div>" +
                                    "<div class=\"feedback-rate\"><div class=\"smily smily-5\"></div><div class=\"feedback-smily\">Hmmm..</div></div>" +
                                    "<div class=\"feedback-rate\"><div class=\"smily smily-6\"></div><div class=\"feedback-smily\">Fine.</div></div>" +
                                    "<div class=\"feedback-rate\"><div class=\"smily smily-7\"></div><div class=\"feedback-smily\">Okay!</div></div>" +
                                    "<div class=\"feedback-rate\"><div class=\"smily smily-8\"></div><div class=\"feedback-smily\">Yes, I will.</div></div>" +
                                    "<div class=\"feedback-rate\"><div class=\"smily smily-9\"></div><div class=\"feedback-smily\">Definitely.</div></div>" +
                                    "<div class=\"feedback-rate\"><div class=\"smily smily-10\"></div><div class=\"feedback-smily\">I already have!</div></div>" +
                                    GetUserPreferencePager(1) +
                                    "<div style=\"display: none\" class=\"feedback-next\" show-id=\"artist-pref-screen\"><div>></div></div>" +
                                "</div></div>" +
                            "<div class=\"screen\" id=\"artist-pref-screen\">" +
                                "<div class=\"feedback-title\">Your favorite artists:</div>" +
                                "<div class=\"feedback-content\">" +
                                "<div class=\"feedback-prev\" show-id=\"user-pref-screen\"><div><</div></div>" +
                                "<ul></ul>" +
                                GetUserPreferencePager(2) +
                                "<div class=\"feedback-next\" show-id=\"genre-pref-screen\"><div>></div></div>" +
                            "</div></div>" +
                            "<div class=\"screen\" id=\"genre-pref-screen\">" +
                                "<div class=\"feedback-title\">Your favorite Genre:</div>" +
                                "<div class=\"feedback-content\">" +
                                "<div class=\"feedback-prev\" show-id=\"artist-pref-screen\"><div><</div></div>" +
                                "<ul class=\"movie-genre\">" +
                                    "<li class=\"orange\"><div class=\"movie-genre\">Action</div></li>" +
                                    "<li class=\"green\"><div class=\"movie-genre\">Romance</div></li>" +
                                    "<li class=\"blue\"><div class=\"movie-genre\">Thriller</div></li>" +
                                    "<li class=\"red\"><div class=\"movie-genre\">Drama</div></li>" +
                                    "<li class=\"maroon\"><div class=\"movie-genre\">Musical</div></li>" +
                                    "<li class=\"yellow\"><div class=\"movie-genre\">Comedy</div></li>" +
                                "</ul>" +
                                GetUserPreferencePager(3) +
                                 "<div class=\"feedback-next\" show-id=\"critics-pref-screen\"><div>></div></div>" +
                            "</div></div>" +
                            "<div class=\"screen\" id=\"critics-pref-screen\">" +
                                "<div class=\"feedback-title\">Your favorite Critics:</div>" +
                                "<div class=\"feedback-content\">" +
                                "<div class=\"feedback-prev\" show-id=\"genre-pref-screen\"><div><</div></div>" +
                                "<ul class=\"movie-critics\"></ul>" +
                                GetUserPreferencePager(4) +
                                "<div class=\"btn btn-success save-button\" onclick=\"SaveUserFavorite();\">Save</div>" +
                            "</div></div>" +

                            "</div></div>" +
            "</div></div></div>");
        return feedback;
    }

    var GetUserPreferencePager = function (activeID) {
        var pager = "<div class=\"feedback-pager\">";

        for (i = 1; i < 5; i++) {
            if (i <= activeID)
                pager += "<a class=\"active\" id=\"feedback-pager-" + i + "\"></a>";
            else
                pager += "<a class=\"inactive\" id=\"feedback-pager-" + i + "\"></a>";
        }

        pager += "</div>";
        return pager;
    }

    var OnSuccessPopulateUserFeedback = function (data) {
        var artists = JSON.parse(data);
        var artist = "";
        var critic = "";

        for (i = 0; i < artists[0].length; i++) {
            var posters = JSON.parse(artists[0][i].Posters);//.split(',');
            var path = "https://moviemirchistorage.blob.core.windows.net/posters/" + posters[posters.length - 1];
            var name = artists[0][i].ArtistName.split(' ')[0];
            artist += "<li><div class=\"artist-picture\"><img src=\"" + path + "\" /></div><div class=\"artist-name\" artist=\"" + artists[0][i].ArtistName + "\">" + name + "</div></li>";
        }

        for (i = 0; i < artists[1].length; i++) {
            var path = "https://moviemirchistorage.blob.core.windows.net/posters/" + artists[1][i].UniqueName + ".jpg";
            var name = artists[1][i].Name.split(' ')[0];
            critic += "<li><div class=\"critic-picture\"><img src=\"" + path + "\" /></div><div class=\"artist-name\" critic=\"" + artists[1][i].Name + "\">" + name + "</div></li>";
        }

        $("#artist-pref-screen ul").html(artist);
        $("#critics-pref-screen ul").html(critic);

        $("#artist-pref-screen ul li").click(function () {
            if ($(this).attr("class") == "selected") {
                $(this).attr("selected", "false");
                $(this).removeClass("selected");
            }
            else {
                $(this).attr("selected", "true");
                $(this).attr("class", "selected");
            }
        });

        $("#critics-pref-screen ul li").click(function () {
            if ($(this).attr("class") == "selected") {
                $(this).attr("selected", "false");
                $(this).removeClass("selected");
            }
            else {
                $(this).attr("selected", "true");
                $(this).attr("class", "selected");
            }
        });
    }

    return GetPreferenceButton();
}