var Crawler = function () {

    var AFFILIATION = [
                        "CNN IBN",
                        "Bollywood Hungama",
                        "anupamachopra.com",
                        "Indian Express",
                        "DNA",
                        "rajasen.com",
                        "Mid Day",
                        "Telegraph",
                        "Box office India",
                        "Komal Nahta",
                        "The Hindu",
                        "Hindustan Times",
                        "Times of India",
                        "Rediff",
                        "NDTV",
                        "Filmfare",
                        "FirstPost",
                        "Mumbai Mirror"
    ]; // after this data come from xml file

    Crawler.prototype.BuildForm = function () {
        var formContainer = $("<div/>").attr("class", "form-container");

        var isEnabled = new FormBuilder().GetCheckBox("isEnabled", "Enabled", false);

        var movieName = new FormBuilder().GetTextField("txtMovieName", "Movie Name", "Movie Name");
        var month_Year = new FormBuilder().GetTextField("txtMonth", "Month Year", "Month/Year");
        var movieLink = new FormBuilder().GetTextField("txtMovieLink", "Movie Link", "Movie Link");

        //$(formContainer).append(isEnabled);

        $(function () {
            $("#txtMonth").datepicker({ dateFormat: "MM yy" });
        });

        $(formContainer).append(movieName);
        $(formContainer).append(month_Year);
        $(formContainer).append(movieLink);
        $(formContainer).append($("<div/>").attr("id", "reviewlinks").attr("style", "width: 120%;").append(this.AddReviewControl(true)));

        var buttonContainer = $("<div/>").attr("style", "width: 100%;").append($("<div/>").attr("class", "btn btn-success").attr("style", "margin-right: 10px;").html("Save").click(function () {
            saveXmlFile(false);
        }));
        $(buttonContainer).append($("<div/>").attr("class", "btn btn-primary").attr("style", "margin-right: 10px;").html("Save & Crawl").click(function () {
            saveXmlFile(true);
        }));

        $(buttonContainer).append($("<div/>").attr("class", "btn btn-danger").attr("style", "margin-right: 10px;").html("Cancel"));

        $(formContainer).append(buttonContainer);

        return formContainer;
    }

    Crawler.prototype.AddReviewControl = function (isAddButton) {

        var reviewControl = $("<div/>").attr("style", "width: 100%;margin: 10px;").attr("class", "singleReview");
        var combo = $("<select/>").attr("style", "float:left;padding: 4px;margin-top: 6px;border-radius: 4px;");

        combo.append("<option>--Select Affiliation--</option>");

        $.each(AFFILIATION, function (i, el) {
            combo.append("<option>" + el + "</option>");
        });

        $(reviewControl).append(combo);

        $(reviewControl).append($("<input/>").attr("type", "text").attr("style", "width:55%").attr("placeholder", "Review Link").change(function () {
            if ($(this).val() != "") {
                if (!IsValidURL($(this).val())) {
                    $(this).focus();
                    $(this).val("");
                    $(this).attr("style", "border-color:red;");
                }
                else {
                    $(this).attr("style", "border-color:#368BC1;");
                }

                if (!IsURLExists($(this).val())) {
                    $(this).focus();
                    $(this).val("");
                    $(this).attr("style", "border-color:red;");
                }
                else {
                    $(this).attr("style", "border-color:#368BC1;");
                }
            }
        }));

        var button = $("<div/>");

        if (isAddButton) {
            $(button).attr("class", "btn btn-success").html("Add ...").click(function () {
                $("#reviewlinks").append(new Crawler().AddReviewControl(false));
            });

        } else {
            $(button).attr("class", "btn btn-danger").html("X").click(function () {
                $(reviewControl).remove();
            });
        }

        $(reviewControl).append(button);

        return reviewControl;
    }

    Crawler.prototype.SaveXmlFileCrawl = function (isCrawl) {
        var movieName = $("#txtMovieName").val();
        var monthYear = $("#txtMonth").val();
        var movieLink = $("#txtMovieLink").val();

        var reviewsLink = [];

        $("#reviewlinks").find(".singleReview").each(function () {
            var name = $(this).find("select option:selected").text();
            var link = $(this).find("input").val();
            reviewsLink.push({ "Name": name, "Link": link });
        });

        //validate controls
        var isValid = true;
        if (movieName == "") {
            $("#status").attr("style", "color:red").html("Please enter movie name!");
            isValid = false;
        }
        if (monthYear == "") {
            $("#status").attr("style", "color:red").html("Please enter movie month / year!");
            isValid = false;
        }
        if (movieLink == "") {
            $("#status").attr("style", "color:red").html("Please enter movie link!");
            isValid = false;
        }

        if (hasDuplicates(reviewsLink)) {
            $("#status").attr("style", "color:red").html("Please select one review affiliation!");
            isValid = false;
        }

        if (isValid) {
            var objXmlData = {
                "MovieName": movieName,
                "MovieLink": movieLink,
                "Year": 00,
                "Month": monthYear,
                "Reviews": reviewsLink
            };

            var xmlData = JSON.stringify(objXmlData);

            var outPutMsg = "XML file generated successfully!";
            var path = "Home/CreateXMLFile";
            if (isCrawl){
                path = "Home/CreateXMLFileAndCrawl";
                outPutMsg = "XML file generated successfully and Crawl movie. It will take few minuts or more.";
            }

            CallController(path, "data", xmlData, function () {
                $("#status").html(outPutMsg);
                $(".search-result-container").children("ul").remove();
                CallHandler("api/CrawlerFiles", function (data) {
                    new Search().PopulateCrawlerResults(data);
                    $(".basic-form-container").html("");
                    $(".basic-form-container").append(new MovieInformation().GetMovieInfoContainer("movie-basic-info", "Add new movie to crawl"));
                    $(".basic-form-container").append(new Crawler().BuildForm());
                });
            });
        }
    }

    Crawler.prototype.PopulateCrawlerFileDetails = function (crawlFile) {
        console.log(crawlFile);

        $(".section-title").html("Change movie details to crawl");

        $("#txtMovieName").val(crawlFile.MovieName);
        $("#txtMonth").val(crawlFile.Month + " " + crawlFile.Year);
        $("#txtMovieLink").val(crawlFile.MovieLink);

        $("#reviewlinks").html("");
        var reviews = crawlFile.Reviews;

        if (reviews.length > 0) {
            for (var i = 0; i < reviews.length; i++) {
                if (i == 0)
                    $("#reviewlinks").append(new Crawler().PopulateReviewControl(true, reviews[i]));
                else
                    $("#reviewlinks").append(new Crawler().PopulateReviewControl(false, reviews[i]));
            }
        }
        else {
            $("#reviewlinks").append(new Crawler().AddReviewControl(true));
        }
    }

    Crawler.prototype.PopulateReviewControl = function (isAddButton, review) {

        var reviewControl = $("<div/>").attr("style", "width: 100%;margin: 10px;").attr("class", "singleReview");
        var combo = $("<select/>").attr("style", "float:left;padding: 4px;margin-top: 6px;border-radius: 4px;");

        combo.append("<option>--Select Affiliation--</option>");

        $.each(AFFILIATION, function (i, el) {
            if (el == review.Name)
                combo.append("<option selected='true'>" + el + "</option>");
            else
                combo.append("<option>" + el + "</option>");
        });

        $(reviewControl).append(combo);

        $(reviewControl).append($("<input/>").attr("type", "text").attr("style", "width:55%").attr("placeholder", "Review Link").change(function () {
            if ($(this).val() != "") {
                if (!IsValidURL($(this).val())) {
                    $(this).focus();
                    $(this).val("");
                    $(this).attr("style", "border-color:red;");
                }
                else {
                    $(this).attr("style", "border-color:#368BC1;");
                }

                if (!IsURLExists($(this).val())) {
                    $(this).focus();
                    $(this).val("");
                    $(this).attr("style", "border-color:red;");
                }
                else {
                    $(this).attr("style", "border-color:#368BC1;");
                }
            }
        }).val(review.Link));

        var button = $("<div/>");

        if (isAddButton) {
            isAddButton = false;
            $(button).attr("class", "btn btn-success").html("Add ...").click(function () {
                $("#reviewlinks").append(new Crawler().AddReviewControl(false));
            });

        } else {
            $(button).attr("class", "btn btn-danger").html("X").click(function () {
                $(reviewControl).remove();
            });
        }

        $(reviewControl).append(button);

        return reviewControl;
    }
}

function hasDuplicates(array) {
    var valuesSoFar = {};
    for (var i = 0; i < array.length; i++) {
        var value = array[i].Name;
        if (Object.prototype.hasOwnProperty.call(valuesSoFar, value)) {
            return true;
        }
        valuesSoFar[value] = true;
    }
    return false;
}