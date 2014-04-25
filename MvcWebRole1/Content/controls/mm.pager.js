var currentPage = 1;

// Defualt control options. When caller does not provide any of these options, the default values will be used.
var defaultOptions = {
    minPages: 3,
    maxPages: 10,
    minControlWidth: 50, // in %
    maxControlWidth: 100, // in %
    tileWidth: 280, // in pixels
    pagerType: "circle", // Square shall be another option
    pagerPosition: "center", // left/right shall be another option
    effect: "slide", // fade shall be another effect
    direction: "left", // right shall be another animation direction
    pagerContainerId: "now-pager", // id of the div which will hold the pager control
    pagerContainer: "pager-container",// container div for pager elements
    pageSize: $(document).width(), // Page size. This size will be used to calculate the # of tiles which could be placed in single row.
    tilesInPage: 4, // # of tiles in single row without overflowing the content
    totalTileCount: 13, // # of tiles to be displayed. This count is specially useful for pagination.
    useDefaultTileCount: false, 
    pageCount: 4, // total # of pages with tiles. This value will be calculated dynamically based on # of List Items and per tile width
    activeTileStartIndex: 0, // This value is used to track the current page in pagination
    clickHandler: function () { } // For future use.
};

// Initialise the control options. When any option is not provided, it will initiated with default values.
function Init(options) {
    if (options == null || options == "undefined") {
        options = defaultOptions;
    }
    else {
        if (options.minPages == null || options.minPages == "undefined") {
            options.minPages = defaultOptions.minPages;
        }

        if (options.maxPages == null || options.maxPages == "undefined") {
            options.maxPages = defaultOptions.maxPages;
        }

        if (options.minControlWidth == null || options.minControlWidth == "undefined") {
            options.minControlWidth = defaultOptions.minControlWidth;
        }

        if (options.maxControlWidth == null || options.maxControlWidth == "undefined") {
            options.maxControlWidth = defaultOptions.maxControlWidth;
        }

        if (options.tileWidth == null || options.tileWidth == "undefined") {
            options.tileWidth = defaultOptions.tileWidth;
        }

        if (options.pagerType == null || options.pagerType == "undefined") {
            options.pagerType = defaultOptions.pagerType;
        }

        if (options.pagerPosition == null || options.pagerPosition == "undefined") {
            options.pagerPosition = defaultOptions.pagerPosition;
        }

        if (options.effect == null || options.effect == "undefined") {
            options.effect = defaultOptions.effect;
        }
        if (options.direction == null || options.direction == "undefined") {
            options.direction = defaultOptions.direction;
        }
        if (options.useDefaultTileCount == null || options.useDefaultTileCount == "undefined") {
            options.useDefaultTileCount = defaultOptions.useDefaultTileCount;
        }

        if (options.clickHandler == null || options.clickHandler == "undefined") {
            options.clickHandler = defaultOptions.clickHandler;
        }
        if (options.pagerContainerId == null || options.pagerContainerId == "undefined") {
            options.pagerContainerId = defaultOptions.pagerContainerId;
        }

        if (options.pagerContainer == null || options.pagerContainer == "undefined") {
            options.pagerContainer = defaultOptions.pagerContainer;
        }

        if (options.pageSize == null || options.pageSize == "undefined") {
            options.pageSize = defaultOptions.pageSize;
        }

        if (options.tilesInPage == null || options.tilesInPage == "undefined") {
            options.tilesInPage = defaultOptions.tilesInPage;
        }

        if (options.totalTileCount == null || options.totalTileCount == "undefined") {
            options.totalTileCount = defaultOptions.totalTileCount;
        }

        if (options.pageCount == null || options.pageCount == "undefined") {
            options.pageCount = defaultOptions.pageCount;
        }

        if (options.activeTileStartIndex == null || options.activeTileStartIndex == "undefined") {
            options.activeTileStartIndex = defaultOptions.activeTileStartIndex;
        }
    }

    return options;
}

function PreparePaginationControl(rotatorControl, pagerOptions) {
    var tileIndex = 0;
    var isTileDisplayed = false;

    var options = Init(pagerOptions);
    
    // 1. Hide all the li element which are present in ul
    $(rotatorControl).find("li").hide();

    // 2. calculate the page size, calculate the tile size, calculate the # of pages required to showcase the gallery

    if (!options.useDefaultTileCount)
        options.totalTileCount = $(rotatorControl).find("li").length;

    // Calculate the margin - which will be left on right/left side of page
    var margin = Math.round(options.pageSize / 20);
    options.pageSize = options.pageSize - (margin * 2);

    options.tilesInPage = Math.floor(options.pageSize / options.tileWidth);
    options.pageCount = Math.ceil(options.totalTileCount / options.tilesInPage);
    activeTileStartIndex = ((currentPage - 1) * options.tilesInPage) + 1;

    // If we don't clear the pagination control, then it will keep appending the pagination indexes.
    $("#" + options.pagerContainerId).html("");

    // 3. Prepare pager control - assign link to each of the pagination link
    $("#" + options.pagerContainerId).append(GetPaginationControl(rotatorControl, options));

    // Show correct tiles 
    $(rotatorControl).find("li").each(function () {
        if (tileIndex == options.activeTileStartIndex) {
            $(this).show();
            isTileDisplayed = true;
        }
        else if (isTileDisplayed && (tileIndex <= (options.activeTileStartIndex + options.tilesInPage - 1))) {
            $(this).show();
        }
        else {
            $(this).hide();
        }

        tileIndex++;
    });
}

function GetPaginationControl(rotatorControl, options) {
    // If we have content which could be displayed in single page, we don't need any pagination control.
    if (options.pageCount < 2) {
        return;
    }

    var pagerContainer = $("<div/>").attr("class", options.pagerContainer);
    var leftArrow = $("<div/>").attr("class", "pager-left-arrow").html("<div class='left-arrow-icon'></div>").hide();
    var rightArrow = $("<div/>").attr("class", "pager-right-arrow").html("<div class='right-arrow-icon'></div>");

    $(leftArrow).click(function () {
        var previousElement;
        $(this).show();
        if (options.activeTileStartIndex == 0) {
            $(this).hide();
        }

        $(this).parent().find(".page-index").each(function () {
            if ($(this).attr("page-index") == currentPage) {
                $(previousElement).click();
            }

            previousElement = $(this);
        });
    });

    $(rightArrow).click(function () {
        var isElementReached = false;

        $(this).show();
        if (options.activeTileStartIndex == options.totalTileCount) {
            $(this).hide();
        }

        $(this).parent().find(".page-index").each(function () {
            if ($(this).attr("page-index") == currentPage && !isElementReached) {
                $(this).next().click();
                isElementReached = true;
            }
        });
    });

    pagerContainer.append(leftArrow);

    var pageCounter = options.pageCount;

    for (var i = 0; i < pageCounter; i++) {
        var pager;

        if (i == 0)
            pager = $("<div/>").attr("class", "page-index active-page-index").attr("page-index", (i + 1));
        else
            pager = $("<div/>").attr("class", "page-index").attr("page-index", (i + 1));


        $(pager).click(function () {
            var tileIndex = 0;
            var isTileDisplayed = false;

            options.activeTileStartIndex = (($(this).attr("page-index") - 1) * options.tilesInPage);
            currentPage = $(this).attr("page-index");
            
            $("." + options.pagerContainer).find("div").each(function () {
                if (!$(this).hasClass("pager-left-arrow") && !$(this).hasClass("pager-right-arrow") && !$(this).hasClass("left-arrow-icon") && !$(this).hasClass("right-arrow-icon"))
                    $(this).attr("class", "page-index");
            });

            $(this).attr("class", "page-index active-page-index");

            $(rotatorControl).find("li").each(function () {
                if (tileIndex == options.activeTileStartIndex) {
                    $(this).show();
                    isTileDisplayed = true;
                }
                else if (isTileDisplayed && (tileIndex <= (options.activeTileStartIndex + options.tilesInPage - 1))) {
                    $(this).show();
                }
                else {
                    $(this).hide();
                }

                // some times the poster images of second page (and onwards) does not get default width.
                // Hence explicitly assigning the width to all the poster images in tube
                if ($(this).find("img.movie-poster").width() == 0) {
                    $(this).find("img.movie-poster").css("width", "263px"); // need to get rid of hardcoded width
                }

                tileIndex++;
            });


            if (options.activeTileStartIndex == 0)
                $("." + options.pagerContainer + " .pager-left-arrow").hide();
            else
                $("." + options.pagerContainer + " .pager-left-arrow").show();

            if (options.totalTileCount == (options.activeTileStartIndex + 1))
                $("." + options.pagerContainer + " .pager-right-arrow").hide();
            else
                $("." + options.pagerContainer + " .pager-right-arrow").show();

        });

        pagerContainer.append(pager);
    }

    pagerContainer.append(rightArrow);
    return pagerContainer;
}

function HighlightActivePage(pagerContainer, pageIndex) {
    var index = 1;
    $(pagerContainer).find(".page-index").each(function () {
        if (index == pageIndex) {
            $(this).attr("class", "active-page-index page-index");
        }
        else {
            $(this).attr("class", "page-index");
        }
    });
}