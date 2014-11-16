function LoadRecentVisits() {
    var items = RecentlyViewedCookies.get();

    if (!items || items.length <= 0) {
        $(".recent-container").remove();
    } else {
        var list = $("<ul/>").addClass("recent-list");
        $.each(items, function (k, v) {
            var src = v.src ? v.src : "https://moviemirchistorage.blob.core.windows.net/posters/default-movie.jpg";
            var item = $("<li/>").html("<a href=\"" + v.url + "\"><img src=\"" + src + "\" /><br/><span>" + v.name + "</span></a>");
            $(list).append(item);
        });

        $(".recent-container").append(list);
        //new Util().RemoveLoadImage($(".recent-container"));
    }
}