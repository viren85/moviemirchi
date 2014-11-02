function PrepareReviewerPage() {
    var json = [
                    { "title": "Reviewer", "section": "review-list-tube" },
                    { "title": "Latest Reviews", "section": "review-list-now-playing-tube" },
                    { "title": "Previous Reviews", "section": "review-list-other-tube" }
    ];

    $(".nav-bar-container").append(GetNavBar(json));

    var name = GetEntityName(document.location.href, "reviewer");
    var nameArray = name.split('-');
    var finalName = "";
    for (i = 0; i < nameArray.length; i++) {
        finalName += toPascalCase(nameArray[i]) + "-";
    }

    if (nameArray.length > 0) {
        finalName = finalName.substring(0, finalName.length - 1);
        if (finalName.indexOf('%7c') > -1) {
            var index = finalName.indexOf('%7c') + 3;
            var last = finalName.substring(index);
            finalName = finalName.substring(0, index) + toPascalCase(last);
        }
    }

    LoadReviewsByReviewer(finalName);

    RecentlyViewedCookies.add({ name: name, type: 'reviewer', url: "/reviewer/" + name });
}