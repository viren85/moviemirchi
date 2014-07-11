var Trailer = function () {
    Trailer.prototype.GetTrailerGrid = function (trailerList) {

        if (trailerList == null || trailerList == "undefined") {
            trailerList = [];
        }

        var container = $("<div/>").attr("class", "trailer-container");
        var sectionTitle = new MovieInformation().GetMovieInfoContainer("trailer-section-title", "Trailers");

        var addButton = $("<div/>").attr("title","add new trailer").attr("class", "btn btn-success").html("<b>+</b>").attr("style","position: absolute;top: 12px;right: 2px;float: left;");

        var grid = $("<div/>").attr("class", "trailer-grid").attr("id", "trailer-sortable");
        var gridHead = $("<div/>").attr("class", "trailer-grid-header");

        var gridCol1 = $("<div/>").attr("class", "trailer-grid-column").html("Title");
        var gridCol2 = $("<div/>").attr("class", "trailer-grid-column").html("Lyrics").attr("style", "display:none");
        var gridCol3 = $("<div/>").attr("class", "trailer-grid-column").html("Link");

        $(gridHead).append(gridCol1).append(gridCol2).append(gridCol3);
        $(grid).append(gridHead);

        for (i = 0; i < trailerList.length; i++) {
            var gridRow = $("<div/>").attr("class", "trailer-grid-row");
            var gridRowData1 = $("<div/>").attr("class", "trailer-grid-row-data1").attr("id", "trailer-grid-row-data1_" + i).html(trailerList[i].Title);
            var gridRowData2 = $("<div/>").attr("class", "trailer-grid-row-data2").html(trailerList[i].Thumb).attr("style", "display:none");

            var linkText = "<a href=\"#\" data-toggle=\"modal\" data-target=\".bs-example-modal-lg\" onclick=\"new Trailer().PopulatePopup(" + i + ");\">add Trailer<a/>";

            if (trailerList[i].YoutubeURL != null && trailerList[i].YoutubeURL != undefined && trailerList[i].YoutubeURL != "")
                linkText = trailerList[i].YoutubeURL + " <a href=\"#\" data-toggle=\"modal\" data-target=\".bs-example-modal-lg\" onclick=\"new Songs().PopulatePopup(" + i + ");\">change<a/>";

            var gridRowData3 = $("<div/>").attr("id", "trailer-row-data3_" + i).attr("class", "trailer-grid-row-data3").attr("style", "cursor:pointer").html(linkText);

            if (trailerList[i].Thumb != null && trailerList[i].Thumb != undefined && trailerList[i].Thumb != "")
                $(gridRowData3).attr("thumb", trailerList[i].Thumb);
            //thumb

            $(gridRow).append(gridRowData1);
            $(gridRow).append(gridRowData2);
            $(gridRow).append(gridRowData3);

            if (trailerList[i].Title != null && trailerList[i].Title != undefined && trailerList[i].Title != "") {
                $(grid).append(gridRow);
            }
        }

        return $(container).append(sectionTitle).append(addButton).append(grid);
        //return $(container).append(grid);
    }

    Trailer.prototype.PopulatePopup = function (counter) {
        var col1 = $("#trailer-grid-row-data1_" + counter);
        $("#myModalLabel").empty();
        $("#myModalLabel").html("Add trailer '" + $("#txtFriendly").val() + "'");
        $("#myModalLabel").append($("<input/>").attr("type", "hidden").attr("id", "hf-col3-id").val("#trailer-row-data3_" + counter));
        $("#search-container").empty();
        $("#VideoFrame").hide();
        $("#query").val($("#txtFriendly").val() + " official trailer");
    }
}