function GetReviewControl(containerClass, movieReviews) {
    var review = movieReviews;

    if (review != undefined && review != null && review.length > 0) {

        for (k = 0; k < review.length ; k++) {
            var reviewText = review[k].Review.length > 250 ? review[k].Review.substring(0, 250) + "..." : review[k].Review;

            // TODO - Need to get the correct picture of reviewer based on their name. Currently the pictures are hardcoded.
            var html =
                    "<div class=\"arrow_container\">" +
                        "<div class=\"left\">" +
                            "<div class=\"info\">" +
                                "<div class=\"reviewer\">" +
                                    "<img src=\"" + GetReviewerPic(review[k].ReviewerName) + "\" style=\"height:100px;width:100px\" />" +
                                    "<div class=\"reviewer-name\"><a href=\"/movie/reviewer/" + CleanName(review[k].ReviewerName) + "\">" + review[k].ReviewerName + "</a></div>" +
                                    "<div class=\"affiliation\"><a href=\"javascript:void()\">" + review[k].Affiliation + "</a></div>" +
                                    "<div class=\"other\">" +
                                        "<div class=\"topcritic\">Top Critic</div>" +
                                    "</div>" +
                                "</div>" +
                            "</div>" +
                        "</div>" +
                        "<div class=\"right\">" +
                            "<div class=\"mirchimeter\">" + GetRateControl(review[k].ReviewerRating) + "</div>" +
                            "<div class=\"review\">" +
                                "<div class=\"arrow_box\">" +
                                    "<div class=\"review-content\"><blockquote class=\"quote\">" + reviewText + "</blockquote><div class=\"more-link\"><a target=\"_new\" href=\"" + review[k].OutLink + "\">More...</a></div></div>" +

                                "</div>" +
                            "</div>" +
                        "</div>" +
                        "<div class=\"clear\"></div>" +
                    "</div>";

            console.log(review[k]);
            $("." + containerClass).append(html)
        }
    }
}

var GetDefaultReviewControl = function (containerClass, movieReviews, isReviewer) {
    var html =
                "<div class=\"arrow_container\">" +
                    "<div class=\"left\">" +
                        "<div class=\"info\">" +
                            "<div class=\"reviewer\">" +
                                "<img src=\"" + GetReviewerPic("Rajeev Masand") + "\" style=\"height:100px;width:100px\" />" +
                                "<div class=\"reviewer-name\"><a href=\"javascript:void()\">Rajeev Masand</a></div>" +
                                "<div class=\"affiliation\"><a href=\"javascript:void()\">Indy Times</a> | <a href=\"javascript:void()\">Bolly Times</a></div>" +
                                "<div class=\"other\">" +
                                    "<div class=\"topcritic\">Top Critic</div>" +
                                "</div>" +
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
    var review = movieReviews;

    if (review != undefined && review != null && review.length > 0) {

        for (k = 0; k < review.length ; k++) {
            var reviewText = review[k].Review.length > 250 ? review[k].Review.substring(0, 250) + "..." : review[k].Review;
            var uniqueName = CleanName(review[k].MovieName);
            // TODO - Need to get the correct picture of reviewer based on their name. Currently the pictures are hardcoded.
            var html =
                    "<li>" +
            "<div class=\"arrow_container\">" +
                "<div class=\"left\">" +
                    "<div class=\"info\">" +
                        "<div class=\"reviewer\">" +
                            "<img src=\"" + GetMoviePoster(review[k].MoviePoster, review[k].MovieName) + "\" style=\"height:235px;width:150px\" onerror=\"this.src='/Posters/Images/default-movie.jpg'\" />" +
                        "</div>" +
                    "</div>" +
                "</div>" +
                "<div class=\"right\">" +
                    "<div class=\"review-movie-name\"><a href=\"/movie/" + uniqueName + "\">" + review[k].MovieName + "</a></div>" +
                    "<div class=\"mirchimeter\">" + GetRateControl(review[k].ReviewerRating) + "</div>" +
                    "<div class=\"review\">" +
                        "<div class=\"arrow_box\">" +
                            "<div class=\"review-content\"><blockquote class=\"quote\">" + reviewText + "</blockquote><div class=\"more-link\"><a target=\"_new\" href=\"javascript:void(0)\">More...</a></div></div>" +

                        "</div>" +
                    "</div>" +
                "</div>" +
                "<div class=\"clear\"></div>" +
            "</div>" +
            "</li>";

            $(".review-list ul").append(html)
        }
    }
}

function GetReviewerPic(reviewerName) {
    var basePath = "/posters/images/critic/";
    reviewerName = basePath + reviewerName.replace(" ", "-") + ".jpg";
    return reviewerName;
    /*switch (reviewerName) {
        case "Anupama Chopra":
            return "anupama-chopra.jpg";
        case "Omar Qureshi":
            return "omar-qureshi.jpg";
        case "Khalid Mohamed":
            return "khalid-mohamed.jpg";
        case "Taran Adarsh":
            return "tarun-adarsh.jpg";
        case "Rajeev Masand":
            return "http://www.rajeevmasand.com/assets/images/rajabout.jpg";
        default:
            return "../images/user.png";
    }*/
}

function GetMoviePoster(posters, movieName) {
    // TODO - poster object is not getting populated hence it returns the null poster object. Hence we had to add if/else block
    var posterPath;
    if (posters == null || posters == "undefined") {
        //posterPath = "/Posters/Images/default-movie.jpg";
        posterPath = "/Posters/Images/" + movieName.replace(" ", "-") + "-poster-1.jpg";
    }
    else {
        var posters = JSON.parse(posters);
        for (var j = posters.length - 1; j > -1; j--) {
            posterPath = "/Posters/Images/" + posters[j];
            break;
        }
    }

    return posterPath;
}

function CleanName(name) {
    return name.split(' ').join("-").split('.').join('');
}