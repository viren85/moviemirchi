var UserPreferences = function () {
    var GetPreferenceButton = function () {
        var btn = $('<div/>').addClass("feedback-button").html("<div id='feedback-1'>What do you like?</div><div id='feedback-2' class='hide'>Movies</div><div id='feedback-3' class='hide'>Artists</div><div id='feedback-4' class='hide'>Critics</div>").click(function () {
            $("#user-pref").modal('show');
        });

        $('body').append(GetUserFeedbackScreen());
        //$('body').append(GetRecommendationScreen());

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
                "<div class=\"modal-dialog\" id=\"popup\">" +
                    "<div class=\"modal-content\">" +
                        "<div class=\"modal-header\" id=\"popup-header\">" +
                            "<button type=\"button\" class=\"close\" data-dismiss=\"modal\"><span aria-hidden=\"true\">&times;</span><span class=\"sr-only\">Close</span></button>" +
                            "<span class=\"icon-feedback\"></span><h3 class=\"modal-title\">What do you like?</h3><span>Your feedback help us know where we can do better.</span>" +
                        "</div>" +
                        "<div class=\"modal-body\">" +
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
                            "</div>" + "</div>" +
            "</div></div></div>");
        return feedback;
    }

    /*var GetRecommendationScreen = function () {
        var feedback = $("<div class=\"modal fade\" id=\"artist-pref\" role=\"dialog\" aria-labelledby=\"myModalLabel2\" aria-haspopup=\"false\">" +
                "<div class=\"modal-dialog\" id=\"popup2\">" +
                    "<div class=\"modal-content\">" +
                        "<div class=\"modal-header\" id=\"popup-header2\">" +
                            "<button type=\"button\" class=\"close\" data-dismiss=\"modal\"><span aria-hidden=\"true\">&times;</span><span class=\"sr-only\">Close</span></button>" +
                            "<span class=\"icon-feedback\"></span><h3 class=\"modal-title\">What do you like?</h3><span>Your feedback help us know where we can do better.</span>" +
                        "</div>" +
                        "<div class=\"modal-body\">" +
                            "<div class=\"feedback-title\">Your favorite artists:</div>" +
                            "<div class=\"feedback-content\">" +
                                "<ul>" +
                                    "<li><div class=\"artist-picture\"><img src=\"https://moviemirchistorage.blob.core.windows.net/posters/deepika-padukone-poster-19.jpg\" /></div><div class=\"artist-name\">Deepika Padukone</div></li>" +
                                    "<li><div class=\"artist-picture\"><img src=\"https://moviemirchistorage.blob.core.windows.net/posters/ranveer-singh-poster-13.jpg\" /></div><div class=\"artist-name\">Ranveer Singh</div></li>" +
                                    "<li><div class=\"artist-picture\"><img src=\"https://moviemirchistorage.blob.core.windows.net/posters/ranbir-kapoor-poster-43.jpg\" /></div><div class=\"artist-name\">Ranbeer Kapoor</div></li>" +
                                    "<li><div class=\"artist-picture\"><img src=\"https://moviemirchistorage.blob.core.windows.net/posters/priyanka-chopra-poster-27.jpg\" /></div><div class=\"artist-name\">Priyanka Chopra</div></li>" +
                                "</ul>"+
                            "</div></div>" +
            "</div></div></div>");

        return feedback;
    }

    var GetPreferencesScreen = function () {

    }*/

    var GetUserPreferencePager = function () {
        var pager = "<div class=\"feedback-pager\">" +
            "<a class=\"active\"  onclick=\"$('.screen').hide();$('#user-pref-screen').show();\"></a>" +
            "<a class=\"inactive\" onclick=\"$('.screen').hide();$('#artist-pref-screen').show();$('#artist-pref-screen .feedback-content').attr('class','feedback-content')\"></a>" +
            "<a class=\"inactive\" onclick=\"$('.screen').hide();$('#artist-pref').show();\"></a>" +
            "</div>";
        return pager;
    }

    return GetPreferenceButton();
}