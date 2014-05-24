function LoadTags() {
    // call API to populate the tags - instead of using hardcoded values
    // new Tags().InitTagCloud();

    var popularPath = "../api/Popular?type=all";
    CallHandler(popularPath, ShowTags);
}

var ShowTags = function (data) {
    new Tags(data).InitTagCloud();
}

var Tags = function (tagJsonString) {
    var tags = null;

    if (tagJsonString && tagJsonString != "") {
        tags = JSON.parse(tagJsonString);
    }

    if (tags == null) {
        tags = [
                [
                    { "UniqueName": "taran-adarsh", "Name": "Taran Adarsh", "Role": "Reviewer", "Weight": "3" },
                    { "UniqueName": "anupama-chopra", "Name": "Anupama Chopra", "Role": "Reviewer", "Weight": "4" },
                    { "UniqueName": "rachit-gupta", "Name": "Rachit Gupta", "Role": "Reviewer", "Weight": "2" }
                ],
                [
                    { "UniqueName": "mickey-virus", "Name": "Mickey Virus", "Role": "Movie", "Weight": "1" },
                    { "UniqueName": "krrish-3", "Name": "Krrish 3", "Role": "Movie", "Weight": "4" }
                ],
                [
                    { "UniqueName": "Deepika-Padukone", "Name": "Deepika Padukone", "Role": "Artists", "Weight": "5" },
                    { "UniqueName": "ranveer-singh", "Name": "Ranveer Singh", "Role": "Artists", "Weight": "4" },
                    { "UniqueName": "aditya-roy-kapoor", "Name": "Aditya Roy Kapoor", "Role": "Artists", "Weight": "1" },
                    { "UniqueName": "sanjay-leela-bhansali", "Name": "Sanjay Leela Bhansali", "Role": "Artists", "Weight": "2" }
                ],
                [
                    { "UniqueName": "Romance", "Name": "Romance", "Role": "Genre", "Weight": "5" },
                    { "UniqueName": "Action", "Name": "Action", "Role": "Genre", "Weight": "3" },
                    { "UniqueName": "Drama", "Name": "Drama", "Role": "Genre", "Weight": "1" }
                ]
        ];
    }

    Tags.prototype.InitTagCloud = function () {

        var getReviewerLink = function (text, link) {
            return "<a href='/movie/reviewer/" + link + "'>" + text + "</a>"
        };

        var getMovieLink = function (text, link) {
            return "<a href='/movie/" + link + "'>" + text + "</a>"
        };

        var getArtistLink = function (text, link) {
            return "<a href='/Artists/" + link + "'>" + text + "</a>"
        };

        var getGenreLink = function (text, link) {
            return "<a href='/Genre/" + link + "'>" + text + "</a>"
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

            var html = "<div class=\"tags\"><ul>" + lis.join() + "</ul></div>";
            $(".tags-container").append(html);
        });
    }
}