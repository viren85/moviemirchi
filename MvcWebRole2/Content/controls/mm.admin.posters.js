var Posters = function () {
    var Counter = -1;

    Posters.prototype.GetPosterContainer = function (posters) {
        var container = $("<div/>").attr("class", "form-container");
        var sectionTitle = new MovieInformation().GetMovieInfoContainer("poster-section-title", "Posters");

        // Default poster
        // Upload files
        // List of already uploaded posters

        return $(container).append(sectionTitle).append(this.UploadNewPoster()).append(this.GetAllPoster(posters));
    }

    Posters.prototype.GetDefaultPoster = function () {

    }

    Posters.prototype.UploadNewPoster = function () {
        return new FormBuilder().GetFileUploadControl("poster-upload", "Upload Poster", "Upload Poster");
    }

    Posters.prototype.GetAllPoster = function (posters, type) {
        //console.log(posters);
        var container = $("<div/>").attr("class", "poster-container");
        if (posters != null && posters != undefined) {
            for (i = 0; i < posters.length; i++) {
                var singlePoster = $("<div/>").attr("class", "single-poster").attr("id", "singleposter_" + i).attr("onmouseover", "show(" + i + ");").attr("onmouseout", "hide(" + i + ");");
                var isChecked = false;
                if (i == posters.length - 1) { isChecked = true; }

                var rad = new FormBuilder().GetRadioButton(posters[i], "", "posters", isChecked);

                var img = $("<img/>").attr("src", PUBLIC_BLOB_URL + posters[i]);

                // edit poster div
                var editPoster = $("<div/>").attr("id", i).append(new FormBuilder().GetCheckBox("isActive", "Active", true));
                $(editPoster).append($("<div/>").attr("class", "btn btn-danger").attr("style", "float:right;margin-top: 57%;").attr("onclick", "RemoveDiv(" + i + ");").html("Delete"));
                $(editPoster).attr("class", "edit-poster");
                $(singlePoster).append(editPoster);

                $(container).append($(singlePoster).append(rad).append(img));
                this.Counter = i;
            }
        }
        return container;
    }

    Posters.prototype.AddSinglePoster = function (poster, type) {
        var container = $(".poster-container");
        if (poster != null && poster != undefined) {
            if (type == "poster") {
                this.Counter = this.Counter + 1;
                var singlePoster = $("<div/>").attr("class", "single-poster").attr("id", "singleposter_" + this.Counter).attr("onmouseover", "show(" + this.Counter + ");").attr("onmouseout", "hide(" + this.Counter + ");");
                var rad = new FormBuilder().GetRadioButton(poster, "", "posters", false);
                var img = $("<img/>").attr("src", PUBLIC_BLOB_URL + poster);
                //var img = $("<img/>").attr("src", poster);

                // edit poster div
                var editPoster = $("<div/>").attr("id", this.Counter).append(new FormBuilder().GetCheckBox("isActive", "", true));
                $(editPoster).append($("<div/>").attr("class", "btn btn-danger").attr("style", "float:right").attr("onclick", "RemoveDiv(" + this.Counter + ");").html("Delete"));
                $(editPoster).attr("class", "edit-poster");
                $(singlePoster).append(editPoster);

                $(container).append($(singlePoster).append(rad).append(img));
            }
            else {
                var singlePoster = $("<div/>").attr("class", "single-poster");
                var rad = new FormBuilder().GetRadioButton(poster, "", "posters", false);
                var img = $("<img/>").attr("src", PUBLIC_BLOB_URL + poster);

                $(container).append($(singlePoster).append(rad).append(img));
            }
        }
    }
}

function show(id) {
    id = "#" + id;
    $(id).show();
}
function hide(id) {
    id = "#" + id;
    $(id).hide();
}

function RemoveDiv(counter) {
    var id = "#singleposter_" + counter;
    $("#singleposter_" + counter).remove();
}