var GetReviewControl = function (containerClass, review) {
    var html =
                "<div class=\"arrow_container\">" +
                    "<div class=\"left\">" +
                        "<div class=\"info\">" +
                            "<div class=\"reviewer\">" +
                                "<img src=\"http://www.rajeevmasand.com/assets/images/rajabout.jpg\" style=\"height:100px;width:100px\" />" +
                                "<div class=\"name\">Rajeev Masand</div>" +
                                "<div class=\"affiliation\">Indy Times | Bolly Times</div>" +
                                "<div class=\"other\">" +
                                    "<div class=\"topcritic\">Top Critic</div>" +
                                "</div>" +
                            "</div>" +
                        "</div>" +
                    "</div>" +
                    "<div class=\"right\">" +
                        "<div class=\"mirchimeter\">3 out of 5</div>" +
                        "<div class=\"review\">" +
                            "<div class=\"arrow_box\">" +
                                "<div class=\"content\"><blockquote class=\"quote\">DiCaprio has hinted before that comedy might be his natural calling -- think of Catch Me If You Can -- but his energy here is not just fun, it's discovery.The Wolf of Wall Street is a magnificent black comedy: fast, funny, and remarkably filthy.</blockquote></div>" +
                            "</div>" +
                        "</div>" +
                    "</div>" +
                    "<div class=\"clear\"></div>" +
                "</div>";

    $("." + containerClass).append(html);
}