var Songs = function () {
    Songs.prototype.GetSongsGrid = function (songsList) {
        
        if (songsList == null || songsList == "undefined") {
            songsList = [];
        }

        var container = $("<div/>").attr("class", "songs-container");
        var sectionTitle = new MovieInformation().GetMovieInfoContainer("songs-section-title", "Songs");
        
        var addButton = $("<div/>").attr("title", "add new song").attr("class", "btn btn-success").html("<b>+</b>").attr("style", "position: absolute;top: 12px;right: 2px;float: left;").attr("data-toggle", "modal").attr("data-target", "#mySongModal").click(function () {
            //$("#txtSongTitle").val($("#txtFriendly").val());
        });

        var grid = $("<div/>").attr("class", "songs-grid").attr("id", "songs-sortable");
        var gridHead = $("<div/>").attr("class", "songs-grid-header");

        var gridCol1 = $("<div/>").attr("class", "songs-grid-column").html("Title");
        var gridCol2 = $("<div/>").attr("class", "songs-grid-column").html("Lyrics").attr("style", "display:none");
        var gridCol3 = $("<div/>").attr("class", "songs-grid-column").html("Link");

        $(gridHead).append(gridCol1).append(gridCol2).append(gridCol3);
        $(grid).append(gridHead);

        for (i = 0; i < songsList.length; i++) {
            var gridRow = $("<div/>").attr("class", "songs-grid-row");
            var gridRowData1 = $("<div/>").attr("class", "songs-grid-row-data1").attr("id", "grid-row-data1_" + i).html(songsList[i].SongTitle).attr("cmpsd", songsList[i].Composed).attr("prfmr", songsList[i].Performer).attr("artist", songsList[i].Artist);
            var gridRowData2 = $("<div/>").attr("class", "songs-grid-row-data2").html(songsList[i].Lyrics).attr("recte", songsList[i].Recite).attr("crtsy", songsList[i].Courtsey).attr("style", "display:none");

            var linkText = "<a href=\"#\" data-toggle=\"modal\" data-target=\".bs-example-modal-lg\" onclick=\"new Songs().PopulatePopup(" + i + ");\">click to add link<a/>";

            if (songsList[i].YoutubeURL != null && songsList[i].YoutubeURL != undefined && songsList[i].YoutubeURL != "")
                linkText = songsList[i].YoutubeURL + " <a href=\"#\" data-toggle=\"modal\" data-target=\".bs-example-modal-lg\" onclick=\"new Songs().PopulatePopup(" + i + ");\">change<a/>";

            var gridRowData3 = $("<div/>").attr("id", "songs-row-data3_" + i).attr("class", "songs-grid-row-data3").attr("style", "cursor:pointer").html(linkText);

            if (songsList[i].Thumb != null && songsList[i].Thumb != undefined && songsList[i].Thumb != "")
                $(gridRowData3).attr("thumb", songsList[i].Thumb);
                //thumb

            $(gridRow).append(gridRowData1);
            $(gridRow).append(gridRowData2);
            $(gridRow).append(gridRowData3);

            if (songsList[i].SongTitle != null && songsList[i].SongTitle != undefined && songsList[i].SongTitle != "") {
                $(grid).append(gridRow);
            }
        }

        return $(container).append(sectionTitle).append(addButton).append(grid);
    }
    
    Songs.prototype.PopulatePopup = function (counter) {
        var col1 = $("#grid-row-data1_" + counter);
        $("#myModalLabel").empty();
        $("#myModalLabel").html("Attach link to '" + $(col1).html() + " " + $("#txtFriendly").val() + "'");
        $("#myModalLabel").append($("<input/>").attr("type", "hidden").attr("id", "hf-col3-id").val("#songs-row-data3_" + counter));
        $("#search-container").empty();
        $("#VideoFrame").hide();
        $("#query").val($(col1).html() + " " + $("#txtFriendly").val());
        $("#hfType").val("song");
    }
}