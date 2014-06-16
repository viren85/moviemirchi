var News = function () {

    News.prototype.GetMovieInfoContainer = function (id, title) {
        var container = $("<div/>").attr("id", id).attr("class", "basic-movie-info");
        var sectionTitle = $("<div/>").html(title).attr("class", "news-section-title");
        $(container).append(sectionTitle);
        return container;
    }

    News.prototype.GetNewsGrid = function (newsList) {
        newsList = JSON.parse(newsList);
        if (newsList == null || newsList == "undefined") {
            newsList = [];
        }

        var container = $("<div/>").attr("class", "news-container");
        //var sectionTitle = new MovieInformation().GetMovieInfoContainer("artist-section-title", "Manage News");
        var sectionTitle = this.GetMovieInfoContainer("artist-section-title", "Manage News");

        var grid = $("<div/>").attr("class", "news-grid").attr("id", "sortable");
        var gridHead = $("<div/>").attr("class", "news-grid-header");

        var gridCol1 = $("<div/>").attr("class", "news-grid-column1").append($("<input/>").attr("type", "checkbox").attr("id", "chkHeader").click(function () {            
            if ($("#chkHeader").prop("checked")) {
                $(".news-grid").find(".news-grid-row").each(function () {
                    $(this).find(".news-grid-row-data1").find("input[type=\"checkbox\"]").prop("checked", true);
                });
            }
            else {
                $(".news-grid").find(".news-grid-row").each(function () {
                    $(this).find(".news-grid-row-data1").find("input[type=\"checkbox\"]").prop("checked", false);
                });
            }
        }));

        var gridCol2 = $("<div/>").attr("class", "news-grid-column2").html("News Title");
        var gridCol3 = $("<div/>").attr("class", "news-grid-column3").html("From");
        var gridCol4 = $("<div/>").attr("class", "news-grid-column4").html("Publish Date");

        $(gridHead).append(gridCol1).append(gridCol2).append(gridCol3).append(gridCol4);

        $(grid).append(gridHead);

        for (i = 0; i < newsList.length; i++) {

            var checkbox = $("<input/>").attr("type", "checkbox");
            var gridRow = $("<div/>").attr("class", "news-grid-row");

            var gridRowData1 = $("<div/>").attr("class", "news-grid-row-data1").append(checkbox);
            var gridRowData2 = $("<div/>").attr("class", "news-grid-row-data2").html(newsList[i].Title);
            var gridRowData3 = $("<div/>").attr("class", "news-grid-row-data3").html(newsList[i].Source);
            var gridRowData4 = $("<div/>").attr("class", "news-grid-row-data4").html(newsList[i].PublishDate.substring(0, 25));

            $(gridRow).append(gridRowData1);
            $(gridRow).append(gridRowData2);
            $(gridRow).append(gridRowData3);
            $(gridRow).append(gridRowData4);

            if (newsList[i].Title != null && newsList[i].Title != undefined && newsList[i].Title != "") {
                $(grid).append(gridRow);
            }
        }

        return $(container).append(sectionTitle).append(grid);
        //return $(container).append(grid);
    }
}