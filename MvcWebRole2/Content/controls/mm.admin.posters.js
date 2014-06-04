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
        var container = $("<div/>").attr("class", "poster-container");
        if (posters != null && posters != undefined) {
            for (i = 0; i < posters.length; i++) {
                var singlePoster = $("<div/>").attr("class", "single-poster");
                var rad = new FormBuilder().GetRadioButton("poster-" + (i + 1), "", "posters", false);
                var img = $("<img/>").attr("src", PUBLIC_BASE_URL + "/Posters/Images/" + posters[i]);
                $(container).append($(singlePoster).append(rad).append(img));
            }
        }
        return container;
    }
}