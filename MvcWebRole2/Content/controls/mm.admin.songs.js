var Songs = function () {	
	Songs.prototype.GetSongsGrid = function (songsList) {

		if (songsList == null || songsList == "undefined") {
			songsList = [];
		}

		var container = $("<div/>").attr("class", "songs-container");
		var sectionTitle = new MovieInformation().GetMovieInfoContainer("songs-section-title", "Songs");

		var grid = $("<div/>").attr("class", "songs-grid").attr("id", "songs-sortable");
		var gridHead = $("<div/>").attr("class", "songs-grid-header");

		var gridCol1 = $("<div/>").attr("class", "songs-grid-column").html("Title");
		var gridCol2 = $("<div/>").attr("class", "songs-grid-column").html("Lyrics");
		var gridCol3 = $("<div/>").attr("class", "songs-grid-column").html("Link");

		$(gridHead).append(gridCol1).append(gridCol2).append(gridCol3);
		$(grid).append(gridHead);

		for (i = 0; i < songsList.length; i++) {
			var gridRow = $("<div/>").attr("class", "songs-grid-row");

			var gridRowData1 = $("<div/>").attr("class", "songs-grid-row-data1").html(songsList[i].SongTitle);

			var gridRowData2 = $("<div/>").attr("class", "artist-grid-row-data2").html(songsList[i].Lyrics);
			var gridRowData3 = $("<div/>").attr("class", "artist-grid-row-data3").html("youtube link");
			//var gridRowData2 = $("<div/>").attr("class", "songs-grid-row-data2").append($("<input/>").attr("type", "text").attr("style", "min-width:80px;width:100px").attr("title", songsList[i].role).val(songsList[i].role));
			//var gridRowData3 = $("<div/>").attr("class", "songs-grid-row-data3").append($("<input/>").attr("type", "text").attr("style", "min-width:80px;width:140px").attr("title", songsList[i].charactername).val(songsList[i].charactername));

			$(gridRow).append(gridRowData1);
			$(gridRow).append(gridRowData2);
			$(gridRow).append(gridRowData3);

			if (songsList[i].SongTitle != null && songsList[i].SongTitle != undefined && songsList[i].SongTitle != "") {
				$(grid).append(gridRow);
			}
		}

		return $(container).append(sectionTitle).append(grid);
	}
}