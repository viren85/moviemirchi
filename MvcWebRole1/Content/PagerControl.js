var currentPage = 1;

/*function PrepareMovieListItem() {
    PreparePaginationControl($("#rotator"));
}*/

var defaultOptions = {
    minPages: 3,
    maxPages: 10,
    minControlWidth: 50, // in %
    maxControlWidth: 100, // in %
    tileWidth: 333, // in pixels
    pagerType: "circle", // Square shall be another option
    pagerPosition: "center", // left/right shall be another option
    effect: "slide", // fade shall be another effect
    direction: "left", // right shall be another animation direction
    pagerContainerId: "pager", // id of the div which will hold the pager control
    pagerContainer: "pager-container",// container div for pager elements
    pageSize: $(document).width(),
    tilesInPage: 4,
    totalTileCount: 13,
    useDefaultTileCount: false,
    pageCount: 4,
    activeTileStartIndex: 0,
    clickHandler: function () { }
};

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


    var options = Init(pagerOptions);

    // 1. Hide all the li element which are present in ul
    // 2. calculate the page size, calculate the tile size, calculate the # of pages required to showcase the gallery
    // 3. Prepare pager control - assign link to each of the pagination link
    /*if (options == null || options == "undefined") {
        options = defaultOptions;
    }*/

    $(rotatorControl).find("li").hide();

    if (!options.useDefaultTileCount)
        options.totalTileCount = $(rotatorControl).find("li").length;

    // Calculate the margin - which will be left on right/left side of page
    var margin = Math.round(options.pageSize / 10);
    options.pageSize = options.pageSize - (margin * 2);

    options.tilesInPage = Math.floor(options.pageSize / options.tileWidth);
    options.pageCount = Math.floor(options.totalTileCount / options.tilesInPage);

    $("#" + options.pagerContainerId).html("");
    $("#" + options.pagerContainerId).append(GetPaginationControl(rotatorControl, options));

    activeTileStartIndex = ((currentPage - 1) * options.tilesInPage) + 1;
    
    var tileIndex = 0;
    var isTileDisplayed = false;
    $(rotatorControl).find("li").each(function () {

        if (tileIndex == options.activeTileStartIndex) {
            $(this).show();
            isTileDisplayed = true;
        }
        else if (isTileDisplayed && (tileIndex <= options.activeTileStartIndex + options.tilesInPage)) {
            $(this).show();
        }
        else {
            $(this).hide();
        }

        tileIndex++;
    });
}

function GetPaginationControl(rotatorControl, options) {
    var pagerContainer = $("<div/>").attr("class", options.pagerContainer);

    if (options.pageCount < 2) {
        return;
    }

    for (var i = 0; i < options.pageCount; i++) {
        var pager;

        if (i == 0)
            pager = $("<div/>").attr("class", "page-index active-page-index").attr("page-index", (i + 1));
        else
            pager = $("<div/>").attr("class", "page-index").attr("page-index", (i + 1));


        $(pager).click(function () {
            options.activeTileStartIndex = (($(this).attr("page-index") - 1) * options.tilesInPage);

            var tileIndex = 0;
            var isTileDisplayed = false;

            $("." + options.pagerContainer).find("div").each(function () {
                $(this).attr("class", "page-index");
            });

            $(this).attr("class", "page-index active-page-index");

            //PagerClick();

            $(rotatorControl).find("li").each(function () {

                if (tileIndex == options.activeTileStartIndex) {
                    $(this).show();
                    isTileDisplayed = true;
                }
                else if (isTileDisplayed && (tileIndex <= options.activeTileStartIndex + options.tilesInPage)) {
                    $(this).show();
                }
                else {
                    $(this).hide();
                }

                if ($(this).find("img.movie-poster").width() == 0) {
                    $(this).find("img.movie-poster").css("width", "263px");
                }

                tileIndex++;
            });
        });

        pagerContainer.append(pager);
    }

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