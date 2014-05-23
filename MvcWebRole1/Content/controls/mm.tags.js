function LoadTags() {
    // call API to populate the tags - instead of using hardcoded values
    new Tags().InitTagCloud();
}

var Tags = function (tagJsonString) {
    var tags = null;

    if (tagJsonString != null && tagJsonString != "undefined")
        tags = JSON.parse(tagJsonString);

    if (tags == null)
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

    Tags.prototype.GetReviewerLink = function (text, link) {
        return "<a href='/movie/reviewer/" + link + "'>" + text + "</a>"
    }

    Tags.prototype.GetMovieLink = function (text, link) {
        return "<a href='/movie/" + link + "'>" + text + "</a>"
    }

    Tags.prototype.GetArtistsLink = function (text, link) {
        return "<a href='/Artists/" + link + "'>" + text + "</a>"
    }

    Tags.prototype.GetGenreLink = function (text, link) {
        return "<a href='/Genre/" + link + "'>" + text + "</a>"
    }

    Tags.prototype.PrepareTagGroup = function (jsonList) {
        var div = "<div class=\"tags\"><ul>";
        for (var i = 0; i < jsonList.length; i++) {
            switch (jsonList[i].Role) {
                case "Reviewer":
                    div += "<li class=\"tag" + jsonList[i].Weight + "\">" + this.GetReviewerLink(jsonList[i].Name, jsonList[i].UniqueName) + "</li>";
                    break;
                case "Movie":
                    div += "<li class=\"tag" + jsonList[i].Weight + "\">" + this.GetMovieLink(jsonList[i].Name, jsonList[i].UniqueName) + "</li>";
                    break;
                case "Artists":
                    div += "<li class=\"tag" + jsonList[i].Weight + "\">" + this.GetArtistsLink(jsonList[i].Name, jsonList[i].UniqueName) + "</li>";
                    break;
                case "Genre":
                    div += "<li class=\"tag" + jsonList[i].Weight + "\">" + this.GetGenreLink(jsonList[i].Name, jsonList[i].UniqueName) + "</li>";
                    break;
            }
        }

        div += "</ul></div>";
        $(".tags-container").append(div);
    }

    Tags.prototype.InitTagCloud = function () {
        for (var roleList = 0; roleList < tags.length; roleList++) {
            this.PrepareTagGroup(tags[roleList]);
        }
    }
}