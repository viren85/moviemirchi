var Rating = function () {
    Rating.prototype.GetRatingForm = function () {
        var container = $("<div/>").attr("class", "rating-container");
        var sectionTitle = new MovieInformation().GetMovieInfoContainer("rating-section-title", "Rating");
        var teekha = new FormBuilder().GetTextField("txtTeekhaRate", "Teekha Rating", "Teekha");
        var feeka = new FormBuilder().GetTextField("txtFeekaRate", "Feeka Rating", "Feeka");
        var myScore = new FormBuilder().GetTextField("txtMyScore", "My Score", "My Score");
        $(container).append(sectionTitle).append(teekha).append(feeka).append(myScore);
        return container;
    }
}
