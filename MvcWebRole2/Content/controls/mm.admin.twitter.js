
var PageSize = 25;
var PageCounter = 0;
var TwitterList = [];

var Twitter = function () {
    Twitter.prototype.GetMovieInfoContainer = function (id, title) {
        var container = $("<div/>").attr("id", id).attr("class", "basic-movie-info");
        var sectionTitle = $("<div/>").html(title).attr("class", "twitter-section-title");
        $(container).append(sectionTitle);
        return container;
    }

    Twitter.prototype.GetTwitterGrid = function (newsList) {
        TwitterList = newsList;
        newsList = JSON.parse(newsList);
        console.log(newsList);
        //TwitterList = newsList;

        if (newsList == null || newsList == "undefined") {
            newsList = [];
        }

        var container = $("<div/>").attr("class", "twitter-container");
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
        var j = 0;
        for (i = PageCounter; i < newsList.length; i++) {
            
            if (j == PageSize) {
                PageCounter = i;
                break;
            }
            j++;

            var isChecked = false;
            if (newsList[i].IsActive) isChecked = true;

            var checkbox = $("<input/>").attr("type", "checkbox").attr("id", newsList[i].TwitterId).attr("checked", isChecked);

            var gridRow = $("<div/>").attr("class", "twitter-grid-row");

            var gridRowData1 = $("<div/>").attr("class", "twitter-grid-row-data1").append(checkbox);
            var gridRowData2 = $("<div/>").attr("class", "twitter-grid-row-data2").html(newsList[i].TextMessage);
            if (newsList[i].FromUserId == null || newsList[i].FromUserId == "") newsList[i].FromUserId = "-";
            var gridRowData3 = $("<div/>").attr("class", "twitter-grid-row-data3").html(newsList[i].FromUserId);
            var date = new Date(parseInt(newsList[i].Created_At.substr(6)));
            var gridRowData4 = $("<div/>").attr("class", "twitter-grid-row-data4").html(date);

            $(gridRow).append(gridRowData1);
            $(gridRow).append(gridRowData2);
            $(gridRow).append(gridRowData3);
            $(gridRow).append(gridRowData4);

            if (newsList[i].TextMessage != null && newsList[i].TextMessage != undefined && newsList[i].TextMessage != "") {
                $(grid).append(gridRow);
            }
        }

        //pager
        var gridPager = $("<div/>").attr("class", "twitter-grid-header").append($("<a/>").attr("style", "float:right;color:white;cursor:pointer;").html("Next").click(function () {
            $(".twitter-container").html("");
            $(".twitter-container").append(new Twitter().GetTwitterGrid(TwitterList));
        }));

        $(gridPager).append($("<a/>").attr("style", "float:left;color:white;cursor:pointer;").html("Previous").click(function () {
            $(".twitter-container").html("");
            $(".twitter-container").append(new Twitter().GetTwitterPreviousGrid(TwitterList));
        }));

        $(grid).append(gridPager);

        return $(container).append(sectionTitle).append(grid);
    }

    Twitter.prototype.DeleteTwitt = function () {

        var cnfm = confirm("Are you sure to delete seleted twits?");

        if (cnfm) {
            var SeletedIds = [];

            $(".twitter-grid").find(".twitter-grid-row").each(function () {
                if ($(this).find(".twitter-grid-row-data1").find("input[type=\"checkbox\"]").prop("checked")) {
                    SeletedIds.push($(this).find(".twitter-grid-row-data1").find("input[type=\"checkbox\"]").attr("id"));
                }
            });

            if (SeletedIds.length > 0) {
                var xmlData = JSON.stringify(SeletedIds);

                CallController("Home/DeleteTwitt", "data", xmlData, function () {

                    $(".twitter-container").html("");

                    CallHandler("api/Twitter?start=0&page=0", function (data) {
                        $(".twitter-container").append(new Twitter().GetTwitterGrid(data));
                    });

                    $("#status").html("Selected twitts deleted successfully!");
                });
            }
        }
    }

    Twitter.prototype.SetTwittActive = function () {
        var SeletedIds = [];

        $(".twitter-grid").find(".twitter-grid-row").each(function () {
            if ($(this).find(".twitter-grid-row-data1").find("input[type=\"checkbox\"]").prop("checked")) {
                SeletedIds.push($(this).find(".twitter-grid-row-data1").find("input[type=\"checkbox\"]").attr("id"));
            }
        });

        if (SeletedIds.length > 0) {
            var xmlData = JSON.stringify(SeletedIds);

            CallController("Home/SetActiveTwitt", "data", xmlData, function () {

                $(".twitter-container").html("");

                CallHandler("api/Twitter?start=0&page=0", function (data) {
                    $(".twitter-container").append(new Twitter().GetTwitterGrid(data));
                });

                $("#status").html("Selected twitts activate successfully!");
            });
        }
    }

    Twitter.prototype.GetTwitterPreviousGrid = function (newsList) {
        
        newsList = JSON.parse(newsList);
        console.log(newsList);
        //TwitterList = newsList;

        if (newsList == null || newsList == "undefined") {
            newsList = [];
        }

        var container = $("<div/>").attr("class", "twitter-container");
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
        var j = 0;
        for (i = PageCounter; i < newsList.length, i >= 0; i--) {
            
            if (j == PageSize) {
                PageCounter = i;
                break;
            }

            j++;

            var isChecked = false;
            if (newsList[i].IsActive) isChecked = true;

            var checkbox = $("<input/>").attr("type", "checkbox").attr("id", newsList[i].TwitterId).attr("checked", isChecked);

            var gridRow = $("<div/>").attr("class", "twitter-grid-row");

            var gridRowData1 = $("<div/>").attr("class", "twitter-grid-row-data1").append(checkbox);
            var gridRowData2 = $("<div/>").attr("class", "twitter-grid-row-data2").html(newsList[i].TextMessage);
            if (newsList[i].FromUserId == null || newsList[i].FromUserId == "") newsList[i].FromUserId = "-";
            var gridRowData3 = $("<div/>").attr("class", "twitter-grid-row-data3").html(newsList[i].FromUserId);
            var date = new Date(parseInt(newsList[i].Created_At.substr(6)));
            var gridRowData4 = $("<div/>").attr("class", "twitter-grid-row-data4").html(date);

            $(gridRow).append(gridRowData1);
            $(gridRow).append(gridRowData2);
            $(gridRow).append(gridRowData3);
            $(gridRow).append(gridRowData4);

            if (newsList[i].TextMessage != null && newsList[i].TextMessage != undefined && newsList[i].TextMessage != "") {
                $(grid).append(gridRow);
            }
        }

        //pager
        var gridPager = $("<div/>").attr("class", "twitter-grid-header").append($("<a/>").attr("style", "float:right;color:white;cursor:pointer;").html("Next").click(function () {
            $(".twitter-container").html("");
            $(".twitter-container").append(new Twitter().GetTwitterGrid(TwitterList));
        }));

        $(gridPager).append($("<a/>").attr("style", "float:left;color:white;cursor:pointer;").html("Previous").click(function () {
            $(".twitter-container").html("");
            $(".twitter-container").append(new Twitter().GetTwitterPreviousGrid(TwitterList));
        }));

        $(grid).append(gridPager);

        return $(container).append(sectionTitle).append(grid);
    }

    Twitter.prototype.CrawlTwitts = function () {
        CallController("Home/CrawlTwitts", "data", "", function () {
            $("#status").html("Successfully crawl news!");
        });
    }
}