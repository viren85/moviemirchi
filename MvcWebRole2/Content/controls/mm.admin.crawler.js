var Crawler = function () {

    var AFFILIATION = ["Hindustan Times", "Film Fare", "Bollywood Hungama"]; // after this data come from xml file

    Crawler.prototype.BuildForm = function () {
        var formContainer = $("<div/>").attr("class", "form-container");

        var isEnabled = new FormBuilder().GetCheckBox("isEnabled", "Enabled", false);

        var movieName = new FormBuilder().GetTextField("txtMovieName", "Movie Name", "Movie Name");
        var month_Year = new FormBuilder().GetTextField("txtMonth", "Month/Year", "Month/Year");
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
            saveXmlFile();
        }));
        $(buttonContainer).append($("<div/>").attr("class", "btn btn-primary").attr("style", "margin-right: 10px;").html("Save & Crawl"));

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

    Crawler.prototype.SaveXmlFileCrawl = function () {
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

            CallController("Home/CreateXMLFile", "data", xmlData, function () { $("#status").html("XML file generated successfully!"); });
        }
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