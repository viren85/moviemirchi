function LoadRecentVisits() {
    var items = RecentlyViewedCookies.get();

    if (!items || items.length <= 0) {
        $("#recent-container-tube").remove();
    } else {

        $el = $(".recent-container");
        $.each(items, function (k, v) {
            var item = $("<div/>").html("<a href=\"" + v.url + "\">" + v.name + "</a>");
            $el.append(item);
        });

        new Util().RemoveLoadImage($("#recent-container-tube"));
    }
}