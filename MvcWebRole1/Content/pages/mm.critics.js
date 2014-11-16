function PrepareReviewerPage() {
    /*var json = [
                    { "title": "Reviewer", "section": "review-list-tube" },
                    { "title": "Latest Reviews", "section": "review-list-now-playing-tube" },
                    { "title": "Previous Reviews", "section": "review-list-other-tube" },
                    { "title": "Recently Viewed", "section": "recent-container-tube" }
    ];

    $(".nav-bar-container").append(GetNavBar(json));*/

    var name = GetEntityName(document.location.href, "reviewer");
    var nameArray = name.split('-');
    var finalName = "";
    for (i = 0; i < nameArray.length; i++) {
        finalName += new Util().toPascalCase(nameArray[i]) + "-";
    }

    if (nameArray.length > 0) {
        finalName = finalName.substring(0, finalName.length - 1);
        if (finalName.indexOf('%7c') > -1) {
            var index = finalName.indexOf('%7c') + 3;
            var last = finalName.substring(index);
            finalName = finalName.substring(0, index) + new Util().toPascalCase(last);
        }
    }

    LoadReviewsByReviewer(finalName);
}

var TrackRecentCriticsVisit = function (name) {
    var src = $(".bio-pic").find("img").attr("src");
    RecentlyViewedCookies.add({ name: name, type: 'reviewer', url: "/reviewer/" + name.replace(' ', '-'), src: src });
}