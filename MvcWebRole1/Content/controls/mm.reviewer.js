function LoadReviewsByReviewer(reviewer) {
    reviewer = reviewer.split('-').join(' ');
    var reviewPath = "../../api/ReviewerInfo?name=" + reviewer;
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
                $(".movies").append(GetTubeControl(result.Name, "review-list", "review-pager", null, "review_list"));

                var fileName = "/Images/user.png";
                var name = result.Name;
                var affiliation = "";

                for (k = 0; k < critics.length; k++) {
                    if (critics[k] != null && critics[k] != undefined && critics[k].name == result.Name) {
                        //fileName = "/Posters/Images/critic/" + critics[k].poster;
                        fileName = PUBLIC_BLOB_URL + critics[k].poster;
                        affiliation = critics[k].aff;
                        break;
                    }
                }

                $(".movies").find(".review-list").each(function () {
                    $(this).prepend(ShowPersonBio(fileName, name, "", affiliation));
                    $(this).find("img").removeAttr("style").css("width", "263px").css("float", "left");
                    InitBio();

                    // Need to populate this text from DB
                    $(".intro-text").html("Currently this critic does not have any biography on <a href=\"/Home\">Movie Mirchi</a>");

                });

                var reviews = [];
                var reviewTitle = GetTubeControl("Reviews", "reviews", "review-list-pager", null, "review_list_pagger");

                $(".review-list").find("ul:first").each(function () {
                    $("<div class=\"section-title large-fonts\" style=\"margin-left: 0%\">Reviews</div>").insertBefore(this);
                });

                reviews = result.ReviewsDetails;
                ShowReviewsByReviewer(reviews);
            }
        }
    } catch (e) {
        $(".movies").html("Unable to get reviewer details.");
    }
}

var ShowReviewsByReviewer = function (review) {
    // VS - For production, following line shall be uncommented. Other line is used for demo purposes, 
    // when movies does not have any associated reivews
    //if (review != "undefined" && review != null && review.length > 0) {
    if (review != "undefined" && review != null) {
        $(".link-container").show();
        GetReviewerReviews("movie-review-details", review);
    }
    else {
        $(".movie-review-details").hide();
        $(".link-container").find("div.section-title").each(function () {
            if ($(this).html() == "Reviews") {
                $(this).hide();
            }
        });
    }
}

