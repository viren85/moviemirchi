var BingPosters = function () {

    BingPosters.prototype.GetPosterContainer = function (posters) {
        var container = $("<div/>").attr("class", "form-container");
        var sectionTitle = new MovieInformation().GetMovieInfoContainer("poster-section-title", "Bing Images");

        return $(container).append(sectionTitle).append(this.GetAllPoster(posters));
    }


    BingPosters.prototype.GetAllPoster = function (posters) {

        var container = $("<div/>").attr("class", "poster-container");

        if (posters != null && posters != undefined && posters.length > 0) {
            var isChecked = false;
            for (i = 0; i < posters.length; i++) {
                var innerContainer = $("<div/>").attr("class", "bing-img-container");
                var square = new FormBuilder().GetCheckBox(posters[i], "", isChecked);
                var img = $("<img/>").attr("src", posters[i]).attr("class", "single-poster");

                $(innerContainer).append(square).append(img);

                //$(container).append(innerContainer).append(square).append(img);
                $(container).append(innerContainer);
                Counter = i;
            }
        }
        else {
            $(container).append($("<div/>").html("No result found for '" + $("#txtSearchTerm").val() + "'")).attr("style", "  width: 90%;margin-left: 3%;");;
        }

        return container;
    }
}





