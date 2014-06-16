var Twitter = function () {

    Twitter.prototype.GetMovieInfoContainer = function (id, title) {
        var container = $("<div/>").attr("id", id).attr("class", "basic-movie-info");
        var sectionTitle = $("<div/>").html(title).attr("class", "twitter-section-title");
        $(container).append(sectionTitle);
        return container;
    }

    Twitter.prototype.GetTwitterGrid = function (newsList) {
        newsList = JSON.parse(newsList);
        if (newsList == null || newsList == "undefined") {
            newsList = [];
        }

        var container = $("<div/>").attr("class", "twitter-container");
        //var sectionTitle = new MovieInformation().GetMovieInfoContainer("artist-section-title", "Manage News");
        var sectionTitle = this.GetMovieInfoContainer("twitter-section-title", "Manage Twitts");

        var grid = $("<div/>").attr("class", "twitter-grid").attr("id", "sortable");
        var gridHead = $("<div/>").attr("class", "twitter-grid-header");

        var gridCol1 = $("<div/>").attr("class", "twitter-grid-column1").append($("<input/>").attr("type", "checkbox").attr("id", "chkHeader").click(function () {
            if ($("#chkHeader").prop("checked")) {
                $(".twitter-grid").find(".twitter-grid-row").each(function () {
                    $(this).find(".twitter-grid-row-data1").find("input[type=\"checkbox\"]").prop("checked", true);
                });
            }
            else {
                $(".twitter-grid").find(".twitter-grid-row").each(function () {
                    $(this).find(".twitter-grid-row-data1").find("input[type=\"checkbox\"]").prop("checked", false);
                });
            }
        }));

        var gridCol2 = $("<div/>").attr("class", "twitter-grid-column2").html("Twitter Message");
        var gridCol3 = $("<div/>").attr("class", "twitter-grid-column3").html("From");
        var gridCol4 = $("<div/>").attr("class", "twitter-grid-column4").html("Publish Date");

        $(gridHead).append(gridCol1).append(gridCol2).append(gridCol3).append(gridCol4);

        $(grid).append(gridHead);

        for (i = 0; i < newsList.length; i++) {

            var checkbox = $("<input/>").attr("type", "checkbox");
            var gridRow = $("<div/>").attr("class", "twitter-grid-row");

            var gridRowData1 = $("<div/>").attr("class", "twitter-grid-row-data1").append(checkbox);
            var gridRowData2 = $("<div/>").attr("class", "twitter-grid-row-data2").html(newsList[i].TextMessage);
            var gridRowData3 = $("<div/>").attr("class", "twitter-grid-row-data3").html(newsList[i].FromUserId);
            var gridRowData4 = $("<div/>").attr("class", "twitter-grid-row-data4").html(newsList[i].Created_At);

            $(gridRow).append(gridRowData1);
            $(gridRow).append(gridRowData2);
            $(gridRow).append(gridRowData3);
            $(gridRow).append(gridRowData4);

            if (newsList[i].TextMessage != null && newsList[i].TextMessage != undefined && newsList[i].TextMessage != "") {
                $(grid).append(gridRow);
            }
        }

        return $(container).append(sectionTitle).append(grid);
        //return $(container).append(grid);
    }
}