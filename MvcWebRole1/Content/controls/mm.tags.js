function LoadTags() {
    // call API to populate the tags - instead of using hardcoded values
    // new Tags().InitTagCloud();

    var popularPath = "/api/Popular?type=all";
    CallHandler(popularPath, ShowTags);
}

var ShowTags = function (data) {
    try {
        var jData = JSON.parse(data);
        new Util().RemoveLoadImage($("#tags-container-tube"));
        if (jData.Status != undefined || jData.Status == "Error") {
            $(".tags-container").html(jData.UserMessage);
            $(".tags-container").attr("style", "padding:10px");
        }
        else {
            new Tags(data).InitTagCloud();
        }
    } catch (e) {
        $(".tags-container").html("Error occurred while getting popular tags.");
        $(".tags-container").attr("style", "padding:10px");
    }
}

var Tags = function (tagJsonString) {
    var tags = null;

    if (tagJsonString && tagJsonString != "") {
        tags = JSON.parse(tagJsonString);
    }

    if (!tags || tags === []) {
        tags = [
                [
                    { "UniqueName": "taran-adarsh", "Name": "Taran Adarsh", "Role": "Reviewer", "Weight": "3" },
                    { "UniqueName": "anupama-chopra", "Name": "Anupama Chopra", "Role": "Reviewer", "Weight": "4" },
                    { "UniqueName": "rajeev-masand", "Name": "Rajeev Masand", "Role": "Reviewer", "Weight": "2" }
                ],
                [
                    { "UniqueName": "Romance", "Name": "Romance", "Role": "Genre", "Weight": "5" },
                    { "UniqueName": "Action", "Name": "Action", "Role": "Genre", "Weight": "3" },
                    { "UniqueName": "Drama", "Name": "Drama", "Role": "Genre", "Weight": "1" }
                ]
        ];
    }

    Tags.prototype.InitTagCloud = function () {

        var pageName = new Util().GetPageName();
        var getReviewerLink = function (text, link) {
            return "<a href='/movie/reviewer/" + link.toLowerCase() + "?type=trend&src=" + pageName + "'>" + text + "</a>"
        };

        var getMovieLink = function (text, link) {
            return "<a href='/movie/" + link.toLowerCase() + "?type=trend&src=" + pageName + "'>" + text + "</a>"
        };

        var getArtistLink = function (text, link) {
            return "<a href='/artists/" + link.toLowerCase() + "?type=trend&src=" + pageName + "'>" + text + "</a>"
        };

        var getGenreLink = function (text, link) {
            return "<a href='/genre/" + link.toLowerCase() + "?type=trend&src=" + pageName + "'>" + text + "</a>"
        };

        var getLink = function (list) {
            switch (list.Role) {
                case "Reviewer":
                    return getReviewerLink(list.Name, list.UniqueName);
                case "Movie":
                    return getMovieLink(list.Name, list.UniqueName);
                case "Artists":
                    return getArtistLink(list.Name, list.UniqueName);
                case "Genre":
                    return getGenreLink(list.Name, list.UniqueName);
            }
        };

        tags.forEach(function (jsonList) {
            var lis = $.map(jsonList, function (list) {
                return "<li class=\"tag" + list.Weight + "\">" + getLink(list) + "</li>";
            });

            var html = "<div class=\"tags\"><ul>" + lis.join(', ') + "</ul></div>";
            $(".tags-container").append(html);
        });
    }
}