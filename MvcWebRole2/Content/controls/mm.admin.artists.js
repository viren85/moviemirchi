var Artists = function () {
    var testArtists = [
                           {
                               "ArtistName": "Amitabh Bachhan",
                               "Role": "Actor",
                               "CharacterName": "Bhoot"
                           },
                           {
                               "ArtistName": "Hritik Roshan",
                               "Role": "Actor",
                               "CharacterName": "Krrish"
                           },
                           {
                               "ArtistName": "Priyanka Chopra",
                               "Role": "Actor",
                               "CharacterName": "Heroine"
                           },
                            {
                                "ArtistName": "Amitabh Bachhan",
                                "Role": "Actor",
                                "CharacterName": "Bhoot"
                            },
                           {
                               "ArtistName": "Hritik Roshan",
                               "Role": "Actor",
                               "CharacterName": "Krrish"
                           },
                           {
                               "ArtistName": "Priyanka Chopra",
                               "Role": "Actor",
                               "CharacterName": "Heroine"
                           }

    ];

    Artists.prototype.GetArtistGrid = function (artistList) {
        if (artistList == null || artistList == "undefined") {
            artistList = testArtists;
        }

        var container = $("<div/>").attr("class", "artists-container");
        var sectionTitle = new MovieInformation().GetMovieInfoContainer("artist-section-title", "Artists");

        var grid = $("<div/>").attr("class", "artist-grid");
        var gridHead = $("<div/>").attr("class", "artist-grid-header");

        var gridCol1 = $("<div/>").attr("class", "artist-grid-column").html("Artist Name");
        var gridCol2 = $("<div/>").attr("class", "artist-grid-column").html("Role");
        var gridCol3 = $("<div/>").attr("class", "artist-grid-column").html("Character");

        $(gridHead).append(gridCol1).append(gridCol2).append(gridCol3);
        $(grid).append(gridHead);
        for (i = 0; i < artistList.length; i++) {
            var gridRow = $("<div/>").attr("class", "artist-grid-row");
            var gridRowData1 = $("<div/>").attr("class", "artist-grid-row-data1").html(artistList[i].ArtistName);
            var gridRowData2 = $("<div/>").attr("class", "artist-grid-row-data2").html(artistList[i].Role);
            var gridRowData3 = $("<div/>").attr("class", "artist-grid-row-data3").html(artistList[i].CharacterName);
            $(gridRow).append(gridRowData1);
            $(gridRow).append(gridRowData2);
            $(gridRow).append(gridRowData3);
            $(grid).append(gridRow);
        }

        return $(container).append(sectionTitle).append(grid);
    }
}