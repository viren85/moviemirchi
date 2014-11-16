function LoadReviewsByReviewer(reviewer) {
    reviewer = reviewer.split('-').join(' ');
    reviewer = reviewer.replace('%7c', '-');
    var reviewPath = "/api/ReviewerInfo?name=" + reviewer;
    CallHandler(reviewPath, ShowReviews);
}

var ShowReviews = function (data) {
    try {
        var result = JSON.parse(data);

        if (result.Status != undefined || result.Status == "Error") {
            $(".movies").html(result.UserMessage);
        }
        else {
            if (result.ReviewsDetails != undefined && result.ReviewsDetails != null && result.ReviewsDetails.length > 0) {
                //$(".movies").append(GetTubeControl(result.Name, "review-list", "review-pager"));

                var fileName = "/images/user.png";
                var name = result.Name;
                var affiliation = "";

                for (k = 0; k < critics.length; k++) {
                    if (critics[k] != null && critics[k] != undefined && critics[k].name == result.Name) {
                        fileName = PUBLIC_BLOB_URL + critics[k].poster;
                        affiliation = critics[k].aff;
                        break;
                    }
                }

                $(".movies").append("<div class=\"movie-details\" style=\"margin-left: 0px\"><div class=\"tube-tilte\"  style=\"width: 200px\">" + new Util().toPascalCase(name) + " </div><div class=\"artist-bio\">" + ShowPersonBio(affiliation) + "</div></div>");
                $(".bio-pic").append($("<img/>").attr("src", fileName).css("width", "100px").css("height", "120px"));
                $(".intro-text").css("margin-left", "0px").html("Currently this critic does not have any biography on <a href=\"/\">Movie Mirchi</a>");

                /*$(".movies").find(".review-list").each(function () {
                    $(this).prepend(ShowPersonBio(affiliation));
                    $(this).find("img").removeAttr("style").css("width", "225px").css("float", "left");
                    InitBio();
                    $(".bio-pic").append($("<img/>").attr("src", fileName));
                    // Need to populate this text from DB
                    $(".intro-text").css("margin-left", "0px").html("Currently this critic does not have any biography on <a href=\"/\">Movie Mirchi</a>");

                });*/

                var reviews = [];
                //var reviewTitle = GetTubeControl("Reviews", "reviews", "review-list-pager", null, "review_list_pagger");

                /*$(".review-list").find("ul:first").each(function () {
                    $("<div class=\"section-title large-fonts\" style=\"margin-left: 0%\">Reviews</div>").insertBefore(this);
                });*/

                $(".movies").append("<div class=\"review-list-now-playing\"></div>");
                $(".movies").append("<div class=\"review-list-other\"></div>");
                $(".movies").append("<div class=\"recent-container\"><div class=\"tube-tilte\">Recent</div></div>");

                reviews = result.ReviewsDetails;
                ShowReviewsByReviewer(reviews);
                //$(".movies").append(GetTubeControl("Recently Viewed", "recent-container", "recent-pager"));
                TrackRecentCriticsVisit(name);
                LoadRecentVisits();
            }
        }
    } catch (e) {
        
        $(".movies").html("Unable to get reviewer details.");
    }
                
    $(".footer").show();
}

var hasArchivedReviews = false;
var hasLatestReviews = false;

var ShowReviewsByReviewer = function (review) {
    // VS - For production, following line shall be uncommented. Other line is used for demo purposes, 
    // when movies does not have any associated reivews
    //if (review != "undefined" && review != null && review.length > 0) {

    //$(".movies").append(GetTubeControl("Latest Reviews", "review-list-now-playing", "now-pager"));
    /*$(".movies").append(GetTubeControl("Upcoming", "review-list-upcoming", "upcoming-pager"));*/
    //$(".movies").append(GetTubeControl("Previous Reviews", "review-list-other", "other-pager"));
    
    if (review != "undefined" && review != null) {
        $(".link-container").show();
        var np = [];
        var om = [];
        
        for (i = 0; i < review.length; i++) {
            if (review[i].MovieStatus == "now-playing" || review[i].MovieStatus == "now playing") {
                np.push(review[i]);
            }
            else if (review[i].MovieStatus == "" || review[i].MovieStatus == "released") {
                om.push(review[i]);
            }
        }
        
        $(".review-list-now-playing").append(LoadCriticReviewsTube(np, "Now Playing"));
        $(".review-list-other").append(LoadCriticReviewsTube(om, "Previous"));

        var tubeWidth = $(window).width() - Math.round($(window).width() / 3);
        $(".review-tube-container").css("width", tubeWidth + "px");

        InitTrailerTube(".review-list-now-playing");
        InitTrailerTube(".review-list-other");
    }
    else {
        $(".movie-review-details").hide();
        $(".link-container").find("div.section-title").each(function () {
            if ($(this).html() == "Reviews") {
                $(this).hide();
            }
        });
    }

    /*Pagination for movies */
    /*if ($(window).width() < 768) {
        new Pager($(".review-list-now-playing"), "#now-pager");
        new Pager($(".review-list-other"), "#other-pager");
    }
    else {
        PreparePaginationControl($(".review-list-now-playing"), { pagerContainerId: "now-pager", tileWidth: "360" });
        PreparePaginationControl($(".review-list-other"), { pagerContainerId: "other-pager", tileWidth: "360" });
    }

    $(window).resize(function () {
        if ($(window).width() < 768) {
            new Pager($(".review-list-now-playing"), "#now-pager");
            new Pager($(".review-list-other"), "#other-pager");
        }
        else {
            PreparePaginationControl($(".review-list-now-playing"), { pagerContainerId: "now-pager", tileWidth: "360" });
            PreparePaginationControl($(".review-list-other"), { pagerContainerId: "other-pager", tileWidth: "360" });
        }
    });

    if (!hasArchivedReviews) {
        $("#other_movie").hide();
    }
    if (!hasLatestReviews) {
        $("#now_playing").hide();
    }*/
}

