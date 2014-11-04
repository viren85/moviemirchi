function LoadRecentVisits() {
    var items = RecentlyViewedCookies.get();
    
    if (!items || items.length <= 0) {
        $("#recent-container-tube").remove();
    }
    else {
        // Have place to show last 10 items
        for (i = 0; i < items.length && i <= 9; i++) {
            var name = new Util().toPascalCase(items[i].name.replace(/-/g, " "));
            var item = $("<div/>").html("<a href=\"" + items[i].url + "\">" + name + "</a>");
            $(".recent-container").append(item);
        }

        new Util().RemoveLoadImage($("#recent-container-tube"));
    }
}