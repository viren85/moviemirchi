﻿
var Pager = function (tileContainer, pagerContainerSelector) {
    var CURRENT_TILE = null;
    var TILE_CONTAINER = tileContainer;
    var PAGER_SELECTOR = pagerContainerSelector;
    var IS_FIRST_TILE = true;
    var IS_LAST_TILE = false;
    var TILE_COUNT = 1;

    var GetPager = function () {
        CURRENT_TILE = $(TILE_CONTAINER).find("li:first");

        CalculateTiles(pagerContainerSelector);
        GetNavArrows(TILE_CONTAINER);

        if (TILE_COUNT == Infinity)
            TILE_COUNT = 0;

        // Hide all tiles before we initiate the show the correct tiles
        $(TILE_CONTAINER).find("li").hide();
        $(TILE_CONTAINER).find("li:first").show(); // This line is required because first tile does not appear when resolution is high

        var TEMP_TILE = CURRENT_TILE;

        for (var i = 0; i < TILE_COUNT - 1; i++) {
            $(TEMP_TILE).next().show();
            CURRENT_TILE = TEMP_TILE;
            TEMP_TILE = $(TEMP_TILE).next();
        }

        // Since we have initialized the pager, it won't have any items in left. Hence hide it.
        $(pagerContainerSelector).find(".left-arrow").hide();

        // Since we don't have much tiles to be displayed hence hiding the right arrow
        if ($(window).width() > 767 && TILE_COUNT < 1) {
            $(pagerContainerSelector).find(".right-arrow").hide();
        }
        else if (TILE_COUNT == 1 && pagerContainerSelector == "#review-pager") {
            // Calculate actual tile counts
            var count = 0;
            $(".movie-review-details li").each(function () {
                count++;
            });

            if (count > 1) {
                $(pagerContainerSelector).show();
                $(pagerContainerSelector).find(".right-arrow").show();
            }
        }
        else if (TILE_COUNT == 1) {
            var count = 0;
            $(tileContainer).find("li").each(function () {
                count++;
            });

            if (count > 1) {
                $(pagerContainerSelector).show();
            }
            else {
                $(pagerContainerSelector).hide();
            }
        }
    }

    var GetNavArrows = function () {

        // Left Arrow Click Event - Show correct tiles along with hide left arrow in case of first tile, show right arrow.
        var leftArrow = $("<div/>").attr("class", "left-arrow").append($("<div/>").addClass("arrow-left")).click(function () {

            // Hide all tiles before we initiate the show the correct tiles
            $(CURRENT_TILE).parent().find("li").hide();
            var TEMP_TILE = CURRENT_TILE;

            if (TILE_COUNT == Infinity)
                TILE_COUNT = 0;

            TILE_COUNT = TILE_COUNT == 0 ? 1 : TILE_COUNT; // When resolution is too low to accomodate single tile, our logic calculates it to be 0. Hence setting it to 1, to show atleast one tile.

            // Since window could accomodate more than one tile - show them
            // Bug - Show all tiles which could fit in the screen when we are on first page
            for (var i = 0; i < TILE_COUNT; i++) {
                $(TEMP_TILE).prev().show();
                CURRENT_TILE = $(TEMP_TILE).prev();
                CURRENT_TILE = $(CURRENT_TILE).html() == undefined ? $(TEMP_TILE) : $(CURRENT_TILE);
                TEMP_TILE = $(TEMP_TILE).prev();
            }

            // Check if this is first tile, is yes set the flag. 
            IS_FIRST_TILE = ($(TEMP_TILE).html() == $(TILE_CONTAINER).find("li:first").html());

            if (IS_FIRST_TILE || $(TEMP_TILE).html() == undefined)
                $(this).hide();
            else
                $(this).show();

            $(this).next().show();

        });

        // Right Arrow Click Event - Show correct tiles along with hide right arrow in case of last tile, show left arrow.
        var rightArrow = $("<div/>").attr("class", "right-arrow").append($("<div/>").addClass("arrow-right")).click(function () {
            var TEMP_TILE = CURRENT_TILE;
            $(CURRENT_TILE).parent().find("li").hide();

            if (TILE_COUNT == Infinity)
                TILE_COUNT = 0;

            TILE_COUNT = TILE_COUNT <= 0 ? 1 : TILE_COUNT;

            // Bug - Show all tiles which could fit in the screen when we are on last page
            for (var i = 0; i < TILE_COUNT; i++) {
                $(TEMP_TILE).next().show();
                CURRENT_TILE = $(TEMP_TILE).next();
                CURRENT_TILE = $(CURRENT_TILE).html() == undefined ? $(TEMP_TILE) : $(CURRENT_TILE);
                TEMP_TILE = $(TEMP_TILE).next();
            }

            // Check if this is last tile, is yes set the flag. 
            IS_LAST_TILE = ($(TEMP_TILE).html() == $(TILE_CONTAINER).find("li:last").html());

            if (IS_LAST_TILE || $(TEMP_TILE).html() == undefined)
                $(this).hide();
            else
                $(this).show();


            $(pagerContainerSelector).find(".left-arrow").show();
        });

        $(PAGER_SELECTOR).html("");
        $(PAGER_SELECTOR).append(leftArrow);
        $(PAGER_SELECTOR).append(rightArrow);
    }

    var CalculateTiles = function (pagerContainer) {
        var tileWidth;
        var subFactor = 150;
        var windowWidth = $(window).width();
        var totalTiles = 0;

        if (windowWidth <= 400) {
            TILE_COUNT = 1;
            return;
        }

        // Calculate if we have only 1 tile to display
        $(pagerContainer).parent().find("ul li").each(function () {
            totalTiles++;
        });

        if (totalTiles < 2) {
            TILE_COUNT = 1;
            return;
        }

        if (pagerContainer != "#critics-pager") {
            if (pagerContainer == "#review-pager") 
                tileWidth = $(".movie-review-details li").width();
            else 
                tileWidth = $(".movie-list .movie").width();
        }
        else {
            tileWidth = $(".critics-container .reviewer").width();
            subFactor = 300;
        }

        var availableWidth = windowWidth - subFactor;
        var tiles = Math.floor(availableWidth / tileWidth);
        TILE_COUNT = tiles;
    }

    GetPager();

    $(window).resize(function () {
        var tileWidth = 0;
        var subFactor = 80;

        if ($(".movie-list .movie").width() == null) {
            tileWidth = $(".review-list-now-playing li").width() + 10;
            subFactor = 50;
        }
        else {
            tileWidth = $(".movie-list .movie").width() + 40;
        }

        var windowWidth = $(window).width();
        var availableWidth = windowWidth - subFactor;
        var tiles = Math.floor(availableWidth / tileWidth);
        //$(".movie-list .movie").hide();
        //$(".upcoming-movie-list .movie").hide();

        $(CURRENT_TILE).parent().find("li").hide();
        $(CURRENT_TILE).css("display", "inline");

        TILE_COUNT = tiles;

        var TEMP_TILE = CURRENT_TILE;
        for (var i = 0; i < tiles - 1; i++) {
            $(TEMP_TILE).next().css("display", "inline");
            TEMP_TILE = $(TEMP_TILE).next();
        }
    });
};