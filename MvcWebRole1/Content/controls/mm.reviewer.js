function LoadReviewsByReviewer(reviewer) {
    reviewer = reviewer.replace("-", " ");
    var reviewPath = "../api/ReviewerInfo?name=" + reviewer;
    CallHandler(reviewPath, ShowReviews);
}

var ShowReviews = function (data) {
    var result = JSON.parse(data);
if (result.ReviewsDetails != undefined && result.ReviewsDetails != null && result.ReviewsDetails.length > 0) {
        $(".movies").append(GetTubeControl("Reviews By " + result.Name, "review-list", "review-pager"));

        var reviews = [];

        reviews = result.ReviewsDetails;
        ShowReviewsByReviewer(reviews);
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

