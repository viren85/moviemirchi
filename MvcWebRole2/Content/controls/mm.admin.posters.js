var Posters = function () {
    var testPosters = [
            "poster-1.jpg", "poster-1.jpg", "poster-1.jpg", "poster-1.jpg", "poster-1.jpg", "poster-1.jpg", "poster-1.jpg",
            "poster-1.jpg", "poster-1.jpg", "poster-1.jpg", "poster-1.jpg", "poster-1.jpg", "poster-1.jpg", "poster-1.jpg"
    ];

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

    Posters.prototype.GetAllPoster = function (posters) {
        //console.log(posters);
        var container = $("<div/>").attr("class", "poster-container");
        if (posters != null && posters != undefined) {
            for (i = 0; i < posters.length; i++) {
                var singlePoster = $("<div/>").attr("class", "single-poster").attr("id", "singleposter_" + i).attr("onmouseover", "show(" + i + ");").attr("onmouseout", "hide(" + i + ");");
                var isChecked = false;
                if (i == posters.length - 1) { isChecked = true; }

                var rad = new FormBuilder().GetRadioButton(posters[i], "", "posters", isChecked);

                var img = $("<img/>").attr("src", PUBLIC_BASE_URL + "/Posters/Images/" + posters[i]);

                // edit poster div
                var editPoster = $("<div/>").attr("id", i).append(new FormBuilder().GetCheckBox("isActive", "", true));
                $(editPoster).append($("<div/>").attr("class", "btn btn-danger").attr("style", "float:right").attr("onclick", "RemoveDiv(" + i + ");").html("Delete"));
                $(editPoster).attr("class", "edit-poster");
                $(singlePoster).append(editPoster);

                $(container).append($(singlePoster).append(rad).append(img));
                Counter = i;
            }

            console.log(Counter);
        }
        return container;
    }

    Posters.prototype.AddSinglePoster = function (poster) {
        var container = $(".poster-container");
        if (poster != null && poster != undefined) {
            Counter = Counter + 1;
            var singlePoster = $("<div/>").attr("class", "single-poster").attr("id", "singleposter_" + Counter).attr("onmouseover", "show(" + Counter + ");").attr("onmouseout", "hide(" + Counter + ");");
            var rad = new FormBuilder().GetRadioButton(poster, "", "posters", false);
            var img = $("<img/>").attr("src", PUBLIC_BASE_URL + "/Posters/Images/" + poster);

            // edit poster div
            var editPoster = $("<div/>").attr("id", Counter).append(new FormBuilder().GetCheckBox("isActive", "", true));
            $(editPoster).append($("<div/>").attr("class", "btn btn-danger").attr("style", "float:right").attr("onclick", "RemoveDiv(" + Counter + ");").html("Delete"));
            $(editPoster).attr("class", "edit-poster");
            $(singlePoster).append(editPoster);

            $(container).append($(singlePoster).append(rad).append(img));
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