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
        
        for (news in newsList) {
            var item = newsList[news];
            var isChecked = false;

            if (item.IsActive) isChecked = true;

            var checkbox = $("<input/>").attr("type", "checkbox").attr("id", item.NewsId).attr("checked", isChecked);
            var gridRow = $("<div/>").attr("class", "news-grid-row");

            var gridRowData1 = $("<div/>").attr("class", "news-grid-row-data1").append(checkbox);
            var gridRowData2 = $("<div/>").attr("class", "news-grid-row-data2").html(item.Title);
            var gridRowData3 = $("<div/>").attr("class", "news-grid-row-data3").html(item.Source);
            var gridRowData4 = $("<div/>").attr("class", "news-grid-row-data4").html(item.PublishDate.substring(0, 25));

            $(gridRow).append(gridRowData1);
            $(gridRow).append(gridRowData2);
            $(gridRow).append(gridRowData3);
            $(gridRow).append(gridRowData4);

            if (item.Title != null && item.Title != undefined && item.Title != "") {
                $(grid).append(gridRow);
            }
        }

        return $(container).append(sectionTitle).append(grid);
        //return $(container).append(grid);
    }

    News.prototype.DeleteNews = function () {

        var cnfm = confirm("Are you sure to delete seleted news?");

        if (cnfm) {
            var SeletedIds = [];

            $(".news-grid").find(".news-grid-row").each(function () {
                if ($(this).find(".news-grid-row-data1").find("input[type=\"checkbox\"]").prop("checked")) {
                    SeletedIds.push($(this).find(".news-grid-row-data1").find("input[type=\"checkbox\"]").attr("id"));
                }
            });

            if (SeletedIds.length > 0) {
                var xmlData = JSON.stringify(SeletedIds);

                CallController("/Home/DeleteNews", "data", xmlData, function () {

                    $(".news-container").html("");

                    CallHandler("api/News?start=0&page=20", function (data) {
                        $(".news-container").append(new News().GetNewsGrid(data));
                    });

                    $("#status").html("Selected news deleted successfully!");
                });
            }
        }
    }

    News.prototype.SetNewsActive = function () {
        var SeletedIds = [];

        $(".news-grid").find(".news-grid-row").each(function () {
            if ($(this).find(".news-grid-row-data1").find("input[type=\"checkbox\"]").prop("checked")) {
                SeletedIds.push($(this).find(".news-grid-row-data1").find("input[type=\"checkbox\"]").attr("id"));
            }
        });

        if (SeletedIds.length > 0) {
            var xmlData = JSON.stringify(SeletedIds);

            CallController("/Home/SetActiveNews", "data", xmlData, function () {

                $(".news-container").html("");

                CallHandler("api/News?start=0&page=20", function (data) {
                    $(".news-container").append(new News().GetNewsGrid(data));
                });

                $("#status").html("Selected news activate successfully!");
            });
        }
    }

    News.prototype.CrawlNews = function () {
        CallController("../Home/CrawlNews", "data", "", function () {
            $("#status").html("Successfully crawl news!");
        });
    }
}