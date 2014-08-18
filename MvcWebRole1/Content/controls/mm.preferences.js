var UserPreferences = function () {
    var GetPreferenceButton = function () {
        var btn = $('<div/>').addClass("feedback-button").html("<div id='feedback-1'>What do you like?</div><div id='feedback-2' class='hide'>Movies</div><div id='feedback-3' class='hide'>Artists</div><div id='feedback-4' class='hide'>Critics</div>").click(function () {
            $("#user-pref").modal('show');
        });

        $('body').append(GetUserFeedbackScreen());
        //$('body').append(GetRecommendationScreen());

        $(".feedback-pager a").click(function () {
            // TODO - Fix the pager
            var screenId = $(this).parent().parent().attr("id");

            $('.screen').hide();
            var id = $(this).attr("id");
            id = id.substring(15);

            switch (id) {
                case "1":
                    $('#user-pref-screen').show();
                    break;
                case "2":
                    $('#artist-pref-screen').show();
                    $('#artist-pref-screen .feedback-content').attr('class', 'feedback-content');
                    break;
                case "3":
                    $('#genre-pref-screen').show();
                    $('#genre-pref-screen .feedback-content').attr('class', 'feedback-content');
                    break;
            }

            $("#" + screenId + " .feedback-pager a").attr("class", "inactive");

            for (i = 1; i <= parseInt(id) ; i++) {
                $("#" + screenId + " #feedback-pager-" + i).attr("class", "active");
            }
        });

        $(".movie-genre li").click(function () {
            var className = $(this).attr("class");
            if (className.indexOf("-active") <= -1) {
                $(this).attr("class", $(this).attr("class") + "-active");
            }
            else {
                $(this).attr("class", $(this).attr("class").substring(0, $(this).attr("class").indexOf("-active")));
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

        });

        $(".artist-pref-screen").click(function () {

        });

        return btn;
    }

    var GetUserFeedbackScreen = function () {
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
                                    GetUserPreferencePager() +
                                "</div></div>" +
                            "<div class=\"screen\" id=\"artist-pref-screen\">" +
                                "<div class=\"feedback-title\">Your favorite artists:</div>" +
                                "<div class=\"feedback-content\">" +
                                "<ul>" +
                                    "<li><div class=\"artist-picture\"><img src=\"https://moviemirchistorage.blob.core.windows.net/posters/deepika-padukone-poster-19.jpg\" /></div><div class=\"artist-name\">Deepika</div></li>" +
                                    "<li><div class=\"artist-picture\"><img src=\"https://moviemirchistorage.blob.core.windows.net/posters/ranveer-singh-poster-13.jpg\" /></div><div class=\"artist-name\">Ranveer</div></li>" +
                                    "<li><div class=\"artist-picture\"><img src=\"https://moviemirchistorage.blob.core.windows.net/posters/ranbir-kapoor-poster-43.jpg\" /></div><div class=\"artist-name\">Ranbeer</div></li>" +
                                    "<li><div class=\"artist-picture\"><img src=\"https://moviemirchistorage.blob.core.windows.net/posters/priyanka-chopra-poster-27.jpg\" /></div><div class=\"artist-name\">Priyanka</div></li>" +
                                "</ul>" +
                                GetUserPreferencePager() +
                            "</div></div>" +
                            "<div class=\"screen\" id=\"genre-pref-screen\">" +
                                "<div class=\"feedback-title\">Your favorite Genre:</div>" +
                                "<div class=\"feedback-content\">" +
                                "<ul class=\"movie-genre\">" +
                                    "<li class=\"orange\"><div class=\"movie-genre\">Action</div></li>" +
                                    "<li class=\"green\"><div class=\"movie-genre\">Romance</div></li>" +
                                    "<li class=\"blue\"><div class=\"movie-genre\">Sci-Fi</div></li>" +
                                    "<li class=\"red\"><div class=\"movie-genre\">Drama</div></li>" +
                                    "<li class=\"maroon\"><div class=\"movie-genre\">Musical</div></li>" +
                                    "<li class=\"yellow\"><div class=\"movie-genre\">Thriller</div></li>" +
                                "</ul>" +
                                GetUserPreferencePager() +
                            "</div></div>" +
                            "</div>" + "</div>" +
            "</div></div></div>");
        return feedback;
    }

    var GetUserPreferencePager = function () {
        var pager = "<div class=\"feedback-pager\">" +
            "<a class=\"active\" id=\"feedback-pager-1\"></a>" +
            "<a class=\"inactive\" id=\"feedback-pager-2\"></a>" +
            "<a class=\"inactive\" id=\"feedback-pager-3\"></a>" +
            "<a class=\"inactive\" id=\"feedback-pager-4\"></a>" +
            "</div>";
        return pager;
    }

    return GetPreferenceButton();
}