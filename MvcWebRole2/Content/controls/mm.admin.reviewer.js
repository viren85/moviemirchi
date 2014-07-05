var Reviewer = function () {
    Reviewer.prototype.BuildForm = function () {
        var formContainer = $("<div/>").attr("class", "form-container");

        var isEnabled = new FormBuilder().GetCheckBox("isEnabled", "Enabled", false);
        var reviewerName = new FormBuilder().GetTextField("txtReviewerName", "Reviewer Name", "Reviewer Name");
        var affilation = new FormBuilder().GetTextField("txtAffilation", "Affilation", "Affilation");

        $(formContainer).append(isEnabled);
        $(formContainer).append(reviewerName);
        $(formContainer).append(affilation);

        return formContainer;
    }

    Reviewer.prototype.PopulateReviewerDetails = function (reviewer) {
        $("#txtReviewerName").val(reviewer.ReviewerName);
        $("#txtAffilation").val(reviewer.Affilation);

        $(".shortcut-container").html("");
        $(".shortcut-container").append($("<a/>").html("Save changes").attr("onclick", "updateCritics()").attr("class", "btn btn-success").attr("title", "click here to save all the changes."));
        $(".shortcut-container").append($("<div>").attr("id", "status"));

        var posters = [];

        $(".posters-container").html("");
        $(".posters-container").append(new Posters().GetPosterContainer(posters));

        $("#poster-upload").attr("onchange", "UploadSelectedFile(this, '#txtReviewerName','reviewerPhotot');");

        new Posters().AddSinglePoster(reviewer.ReviewerImage);
    }

    Reviewer.prototype.UpdateReviewer = function (reviewer) {

        var reviewerName = $("#txtReviewerName").val();
        var affiliation = $("#txtAffilation").val();

        if (reviewerName != undefined && reviewerName != "") {
            reviewer.ReviewerName = reviewerName;
        }

        if (affiliation != undefined && affiliation != "") {
            reviewer.Affilation = affiliation;
        }

        var objReviewer = {
            "ReviewerId": reviewer.ReviewerId,
            "ReviewerName": reviewer.ReviewerName,
            "ReviewerImage": "",
            "Affilation": reviewer.Affilation
        };

        // save movie
        var reviewerData = JSON.stringify(objReviewer);

        CallController("../Reviewer/AddReviewer", "data", reviewerData, function () { $("#status").html("Critics details saved successfully!"); });
    }
}