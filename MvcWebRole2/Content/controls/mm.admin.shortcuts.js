var Shortcuts = function () {
    var testShortcuts = [
                            { "Title": "Basic Information", "JumpId": "movie-basic-info" },
                            { "Title": "Rating", "JumpId": "movie-rating" },
                            { "Title": "Posters", "JumpId": "movie-posters" },
                            { "Title": "Artists", "JumpId": "movie-artist" },
                            { "Title": "Reviews", "JumpId": "movie-reviews" }
    ];

    Shortcuts.prototype.PrepareShortcuts = function (shortcutContainerClass, shortcutList) {
        var span = $("<span/>").html("Shortcuts:").attr("class", "shortcut-text");
        var list = $("<ul/>").attr("class", "shortcut-list");

        if (shortcutList == null || shortcutList == "undefined") {
            shortcutList = testShortcuts;
        }

        for (i = 0; i < shortcutList.length; i++) {
            var item = $("<li/>");
            var ahref = $("<a/>").attr("href", "#" + shortcutList[i].JumpId).html(shortcutList[i].Title).attr("title", shortcutList[i].Title);
            $(item).append(ahref);
            $(list).append(item);
        }
        
        $("." + shortcutContainerClass).append(span);
        $("." + shortcutContainerClass).append(list);
    }
}