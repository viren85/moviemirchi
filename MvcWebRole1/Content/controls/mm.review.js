var GetReviewControl = function (containerClass, movieReviews) {
    if (movieReviews) {
        var reviewList = $("." + containerClass + " ul");
        movieReviews.forEach(function (review) {

            if (review.OutLink) {

                var reviewText = new Util().GetEllipsisText(review.Review, 200);

                var html =
                    "<li class=\"arrow_container\">" +
                        "<div class=\"left\">" +
                            "<div class=\"info\">" +
                                "<div class=\"reviewer\">" +
                                    "<a href=\"/movie/reviewer/" + FormPathFromName(review.ReviewerName) + "\">" +
                                        "<img src=\"" + GetReviewerPic(review.ReviewerName) + "\" style=\"height:100px;width:100px\" onerror=\"this.src='" + PUBLIC_BLOB_URL + "default-movie.jpg'\" />" +
                                    "</a>" +
                                    "<div class=\"reviewer-name\"><a href=\"/movie/reviewer/" + FormPathFromName(review.ReviewerName) + "\">" + review.ReviewerName + "</a></div>" +
                                    "<div class=\"affiliation\">" + review.Affiliation + "</div>" +
                                    /*"<div class=\"other\">" +
                                        "<div class=\"topcritic\">Top Critic</div>" +
                                    "</div>" +*/
                                "</div>" +
                            "</div>" +
                        "</div>" +
                        "<div class=\"right\">" +
                            "<div class=\"mirchimeter\">" + GetRateControl(review.CriticsRating) + "</div>" +
                            "<div class=\"review\">" +
                                "<div class=\"arrow_box\">" +
                                    "<div class=\"review-content\">" +
                                        "<blockquote class=\"quote\">" + reviewText + "</blockquote>" +
                                            "<div class=\"more-link\"><a target=\"_new\" href=\"" + review.OutLink + "\"  onclick=\"trackReviewLink('" + review.OutLink + "');\">More...</a></div>" +
                                    "</div>" +
                                "</div>" +
                            "</div>" +
                        "</div>" +
                        "<div class=\"clear\"></div>" +
                    "</li>";

                $(reviewList).append(html);
                $("." + containerClass).append(reviewList);
            }
        });
    }
};

var GetDefaultReviewControl = function (containerClass, movieReviews, isReviewer) {
    var html =
                "<div class=\"arrow_container\">" +
                    "<div class=\"left\">" +
                        "<div class=\"info\">" +
                            "<div class=\"reviewer\">" +
                                "<img src=\"" + GetReviewerPic("Rajeev Masand") + "\" style=\"height:100px;width:100px\" />" +
                                "<div class=\"reviewer-name\"><a href=\"javascript:void()\">Rajeev Masand</a></div>" +
                                "<div class=\"affiliation\"><a href=\"javascript:void()\">Indy Times</a> | <a href=\"javascript:void()\">Bolly Times</a></div>" +
                                /*"<div class=\"other\">" +
                                    "<div class=\"topcritic\">Top Critic</div>" +
                                "</div>" +*/
                            "</div>" +
                        "</div>" +
                    "</div>" +
                    "<div class=\"right\">" +
                        "<div class=\"mirchimeter\">" + GetRateControl(6) + "</div>" +
                        "<div class=\"review\">" +
                            "<div class=\"arrow_box\">" +
                                "<div class=\"review-content\"><blockquote class=\"quote\">DiCaprio has hinted before that comedy might be his natural calling -- think of Catch Me If You Can -- but his energy here is not just fun, it's discovery.The Wolf of Wall Street is a magnificent black comedy: fast, funny, and remarkably filthy.</blockquote><div class=\"more-link\"><a href=\"javascript:void(0)\">More...</a></div></div>" +

                            "</div>" +
                        "</div>" +
                    "</div>" +
                    "<div class=\"clear\"></div>" +
                "</div>";

    $("." + containerClass).append(html).append(html).append(html).append(html);
}

var GetReviewerReviews = function (containerClass, movieReviews) {
    if (movieReviews) {

        movieReviews.forEach(function (review) {

            if (review.OutLink) {

                var reviewText = new Util().GetEllipsisText(review.Review, 150);
                var uniqueName = FormPathFromName(review.MovieName);

                var html =
                    "<li>" +
                        "<div class=\"arrow_container\">" +
                            "<div class=\"critics-left\">" +
                                "<div class=\"info\">" +
                                    "<div class=\"reviewer\">" +
                                        "<a href=\"/movie/" + uniqueName + "\">" +
                                            "<img src=\"" + GetMoviePoster(review.MoviePoster, review.MovieName) + "\" style=\"height:235px;width:150px\" onerror=\"this.src='" + PUBLIC_BLOB_URL + "default-movie.jpg'\" />" +
                                        "</a>" +
                                    "</div>" +
                                "</div>" +
                            "</div>" +
                            "<div class=\"right\">" +
                                "<div class=\"review-movie-name\"><a href=\"/movie/" + uniqueName + "\">" + review.MovieName + "</a></div>" +
                                "<div class=\"mirchimeter\">" + GetRateControl(review.CriticsRating) + "</div>" +
                                "<div class=\"critics-review\">" +
                                    "<div class=\"arrow_box\">" +
                                        "<div class=\"review-content\">" +
                                            "<blockquote class=\"quote\">" + reviewText + "</blockquote>" +
                                            "<div class=\"critics-more-link\"><a target=\"_new\" href=\"" + review.OutLink + "\">More...</a></div>" +
                                        "</div>" +
                                    "</div>" +
                                "</div>" +
                            "</div>" +
                            "<div class=\"clear\"></div>" +
                        "</div>" +
                    "</li>";

                /*if (review.MovieStatus == "upcoming") {
                    $(".review-list-now-playing ul").append(html);
                }*/                
                if (review.MovieStatus == "now-playing" || review.MovieStatus == "now playing") {
                    hasLatestReviews = true;
                    $(".review-list-now-playing ul").append(html);
                }
                else if (review.MovieStatus == "" || review.MovieStatus == "released") {
                    hasArchivedReviews = true;
                    $(".review-list-other ul").append(html);
                }

                //$(".review-list ul").append(html);
            }
        });
    }
};

function GetReviewerPic(reviewerName) {
    return PUBLIC_BLOB_URL + reviewerName.replace(" ", "-").toLowerCase() + ".jpg";
}

function GetMoviePoster(posters, movieName) {
    // TODO - poster object is not getting populated hence it returns the null poster object. Hence we had to add if/else block
    var posterPath;
    if (!posters) {
        posterPath = PUBLIC_BLOB_URL + movieName.replace(" ", "-") + "-poster-1.jpg";
    } else {
        // TODO - fix this, doesn't seem right
        var posters = JSON.parse(posters);
        if (posters && posters.length && posters.length > 1) {
            posterPath = PUBLIC_BLOB_URL + posters[posters.length - 1];
        } else {
            posterPath = PUBLIC_BLOB_URL + movieName.split(" ").join("-") + "-poster-1.jpg";
        }
    }

    return posterPath;
}

function FormPathFromName(name) {
    return name.split(' ').join("-").split('.').join('');
}