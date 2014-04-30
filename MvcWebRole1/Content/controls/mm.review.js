function GetReviewControl(containerClass, movieReviews) {

    var review = movieReviews;

    if (review != undefined && review != null && review.length > 0) {

        for (k = 0; k < review.length ; k++) {
            console.log(k + "," + review[k].ReviewerName);
            var reviewText = review[k].Review.length > 250 ? review[k].Review.substring(0, 250) + "..." : review[k].Review;
            
            // TODO - Need to get the correct picture of reviewer based on their name. Currently the pictures are hardcoded.
            var html =
                "<div class=\"arrow_container\">" +
                    "<div class=\"left\">" +
                        "<div class=\"info\">" +
                            "<div class=\"reviewer\">" +
                                "<img src=\"" + GetReviewerPic(review[k].ReviewerName) + "\" style=\"height:100px;width:100px\" />" +
                                "<div class=\"reviewer-name\"><a href=\"javascript:void()\">" + review[k].ReviewerName + "</a></div>" +
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
                                "<div class=\"review-content\"><blockquote class=\"quote\">" + reviewText + "</blockquote><div class=\"more-link\"><a href=\"javascript:void(0)\">More...</a></div></div>" +

                            "</div>" +
                        "</div>" +
                    "</div>" +
                    "<div class=\"clear\"></div>" +
                "</div>";
            $("." + containerClass).append(html)
        }
    }
    else {
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
}

function GetReviewerPic(reviewerName) {
    switch (reviewerName) {
        case "Anupama Chopra":
            return "/posters/images/anupama-chopra.jpg";
        case "Omar Qureshi":
            return "/posters/images/omar-qureshi.jpg";
        case "Khalid Mohamed":
            return "/posters/images/khalid-mohamed.jpg";
        case "Taran Adarsh":
            return "/posters/images/tarun-adarsh.jpg";
        case "Rajeev Masand":
            return "http://www.rajeevmasand.com/assets/images/rajabout.jpg";
        default:
            return "../images/user.png";
    }
}