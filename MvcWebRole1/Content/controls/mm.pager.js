
function PreparePaginationControl(rotatorControl, pagerOptions) {

    // Initialise the control options. When any option is not provided, it will initiated with default values.
    var options = (function (options) {

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

        if (options === null || typeof options === "undefined" || options === {}) {
            options = defaultOptions;
        } else {
            if (!options.minPages) {
                options.minPages = defaultOptions.minPages;
            }

            if (!options.maxPages) {
                options.maxPages = defaultOptions.maxPages;
            }

            if (!options.minControlWidth) {
                options.minControlWidth = defaultOptions.minControlWidth;
            }

            if (!options.maxControlWidth) {
                options.maxControlWidth = defaultOptions.maxControlWidth;
            }

            if (!options.tileWidth) {
                options.tileWidth = defaultOptions.tileWidth;
            }

            if (!options.pagerType) {
                options.pagerType = defaultOptions.pagerType;
            }

            if (!options.pagerPosition) {
                options.pagerPosition = defaultOptions.pagerPosition;
            }

            if (!options.effect) {
                options.effect = defaultOptions.effect;
            }

            if (!options.direction) {
                options.direction = defaultOptions.direction;
            }

            if (!options.useDefaultTileCount) {
                options.useDefaultTileCount = defaultOptions.useDefaultTileCount;
            }

            if (!(options.clickHandler && $.isFunction(options.clickHandler))) {
                options.clickHandler = defaultOptions.clickHandler;
            }

            if (!options.pagerContainerId) {
                options.pagerContainerId = defaultOptions.pagerContainerId;
            }

            if (!options.pagerContainer) {
                options.pagerContainer = defaultOptions.pagerContainer;
            }

            if (!options.pageSize) {
                options.pageSize = defaultOptions.pageSize;
            }

            if (!options.tilesInPage) {
                options.tilesInPage = defaultOptions.tilesInPage;
            }

            if (!options.totalTileCount) {
                options.totalTileCount = defaultOptions.totalTileCount;
            }

            if (!options.pageCount) {
                options.pageCount = defaultOptions.pageCount;
            }

            if (!options.activeTileStartIndex) {
                options.activeTileStartIndex = defaultOptions.activeTileStartIndex;
            }
        }

        return options;
    })(pagerOptions);

    (function (rotatorControl, pagerOptions) {

        // If we have content which could be displayed in single page, we don't need any pagination control.
        if (options.pageCount < 2) {
            return;
        }

        //// TODO: Viren - move this class to another JS file and include it here - make the right call
        //// TODO: Viren - fix this css so that the arrows looks grayed out and then back to life using appropriate css classes
        var ArrowManager = function (left, right, pagesCount) {
            var _left = left;
            var _right = right;
            var _pagesCount = pagesCount;

            ArrowManager.prototype.disable = function ($el) {
                $el.css("background-color", "#FF0000");
            };

            ArrowManager.prototype.enable = function ($el) {
                $el.css("background-color", "#00FF00");
            };

            ArrowManager.prototype.manage = function (activeIndex) {
                this.manageLeft(activeIndex);
                this.manageRight(activeIndex);
            };

            ArrowManager.prototype.manageLeft = function (activeIndex) {
                // == instead of === : We want to compare "x" with x, without using parseInt
                if (activeIndex == 1) {
                    this.disable(_left);
                } else {
                    this.enable(_left);
                }
            };

            ArrowManager.prototype.manageRight = function (activeIndex) {
                // == instead of === : We want to compare "x" with x, without using parseInt
                if (activeIndex == _pagesCount) {
                    this.disable(_right);
                } else {
                    this.enable(_right);
                }
            };
        };

        // Get Pagination control
        var GetPaginationControl = function (rotatorControl, options) {

            var pagerContainer = $("<div/>").attr("class", options.pagerContainer);
            var leftArrow = $("<div/>").attr("class", "pager-left-arrow").html("<div class='left-arrow-icon'></div>");
            var rightArrow = $("<div/>").attr("class", "pager-right-arrow").html("<div class='right-arrow-icon'></div>");

            // Add pages
            {
                pagerContainer.append(leftArrow);

                for (var i = 1; i <= options.pageCount; i++) {
                    var pager = $("<div/>").attr("class", "page-index").attr("page-index", i);
                    pagerContainer.append(pager);
                }

                pagerContainer.append(rightArrow);
            }

            var arrowManager = new ArrowManager(leftArrow, rightArrow, options.pageCount);
            var pages = pagerContainer.find(".page-index");
            var pagesParent = $(pages[0]).parent();
            var activePageIndex = function () {
                return pagesParent.find(".active-page-index").attr("page-index");
            };

            // Set first as active
            $(pages[0]).attr("class", "page-index active-page-index");

            // Manage arrows once
            arrowManager.manage(activePageIndex());

            // Click handlers
            {
                $(leftArrow).click(function () {

                    var curr = pages.filter(function () {
                        // == instead of === : We want to compare "x" with x, without using parseInt
                        return ($(this).attr("page-index") == currentPage);
                    })[0];
                    if (curr) {
                        var prev = $(curr).prev();
                        if (prev) {
                            $(prev).click();
                            arrowManager.manage(activePageIndex());
                        }
                    }
                });

                $(rightArrow).click(function () {

                    var curr = pages.filter(function () {
                        // == instead of === : We want to compare "x" with x, without using parseInt
                        return ($(this).attr("page-index") == currentPage);
                    })[0];
                    if (curr) {
                        var next = $(curr).next();
                        if (next) {
                            $(next).click();
                            arrowManager.manage(activePageIndex());
                        }
                    }
                });

                pages.each(function () {
                    $(this).click(function () {

                        options.activeTileStartIndex = (($(this).attr("page-index") - 1) * options.tilesInPage);
                        currentPage = $(this).attr("page-index");

                        // Set inactive/active
                        pagesParent.find(".active-page-index").attr("class", "page-index");
                        $(this).attr("class", "page-index active-page-index");

                        // Show/hide tiles 
                        var showIndex = options.activeTileStartIndex + options.tilesInPage - 1;

                        $(rotatorControl).find("li").each(function (tileIndex) {
                            if (tileIndex >= options.activeTileStartIndex && tileIndex <= showIndex) {
                                $(this).show();
                            } else {
                                $(this).hide();
                            }

                            // TODO: Cleanup this CODE SMELL
                            // some times the poster images of second page (and onwards) does not get default width.
                            // Hence explicitly assigning the width to all the poster images in tube
                            if ($(this).find("img.movie-poster").width() == 0) {
                                $(this).find("img.movie-poster").css("width", "263px"); // need to get rid of hardcoded width
                            }
                        });

                        arrowManager.manage(activePageIndex());
                    })
                });
            }

            return pagerContainer;
        };

        //// Start

        var currentPage = 1;

        var li = $(rotatorControl).find("li");

        // 1. Hide all the li element which are present in ul
        li.hide();

        // 2. calculate the page size, calculate the tile size, calculate the # of pages required to showcase the gallery
        if (!options.useDefaultTileCount) {
            options.totalTileCount = li.length;
        }

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

        // Show/hide tiles 
        var showIndex = options.activeTileStartIndex + options.tilesInPage - 1;

        li.each(function (tileIndex) {
            if (tileIndex >= options.activeTileStartIndex && tileIndex <= showIndex) {
                $(this).show();
            } else {
                $(this).hide();
            }
        });
    })(rotatorControl, options);

}
