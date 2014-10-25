function PopulatingMovies(movie, container, options) {
    var movieContainer = $("." + container + " ul");

    var poster = [];
    poster = JSON.parse(movie.Posters);
    var src = (poster != null && poster.length > 0) ? PUBLIC_BLOB_URL + poster[poster.length - 1] : PUBLIC_BLOB_URL + "default-movie.jpg";

    var list = $("<li itemscope itemtype=\"http://schema.org/Movie\" class=\"movie\"></li>");
    var anchor = $("<a title=\"" + movie.Name + "\"></a>");
    if (options && options.disableClick) {
    } else {
        anchor.attr("href", "/movie/" + movie.UniqueName);
    }
    anchor.prepend("<meta itemprop=\"url\" content=\"/movie/" + movie.UniqueName + "\">");

    var synopsis = movie.Synopsis!= null && movie.Synopsis.length > 500 ? movie.Synopsis.substring(0, 500) + "..." : movie.Synopsis;

    var criticRating;
    var hide = false;
    if (!movie.MyScore || movie.MyScore === "") {
        criticRating = 0;
        hide = true;
    } else {

        criticRating = JSON.parse(movie.MyScore).criticrating;
        if (!criticRating || criticRating === "") {
            criticRating = 0;
            hide = true;
        } else {
            criticRating = parseInt(criticRating) / 10;
        }
    }

    if (TILE_MODE == 0) {
        // TODO: Fix condition for review indicator
        html = "<div id=\"picAndCaption\" class=\"viewingDiv " + movie.UniqueName + "\">" +
                    "<div id=\"imageContainer\" class=\"viewer\" style=\"height: 340px;\">" +
                        "<img itemprop=\"image\" id=\"imageEl\" onerror=\"LoadDefaultImage(this);\" onload=\"MovieImageLoaded(this);\" class=\"movie-poster shownImage\" title=\"" + movie.Name + "\" alt=\"" + movie.Name + "\" src=\"" + src + "\" style=\"margin: auto;\"/>" +
                        "<div class=\"captionAndNavigate\">" +
                            "<div id=\"captionCredit\" class=\"multimediaCaption\">" +
                                "<div id=\"photoCaption\">" +
                                    "<meta itemprop=\"name\" content=\"" + movie.Name + "\">" +
                                    "<meta itemprop=\"datePublished\" content=\"" + movie.Month + "\">" +
                                    "<div class=\"img-movie-name img-movie-name-tile-type-" + TILE_MODE + "\">" + movie.Name + "</div>" +
                                    "<div class=\"img-movie-genre img-movie-genre-tile-type-" + TILE_MODE + "\">" + movie.Genre + "</div>" +
                                    "<div class=\"img-movie-date img-movie-date-tile-type-" + TILE_MODE + "\">" + movie.Month + "</div>" +
                                    (!hide ? GetMovieRateControl(criticRating, movie.Ratings) : "") +
                                    "<div class=\"additives\">" +
                                        "<div class=\"aleft\">" +
                                            "<span class=\"myglyphicon " + ((movie.Trailers && movie.Trailers !== "[]" && movie.Trailers.indexOf("YoutubeURL") > -1) ? "trailer" : "") + "\"></span>" +
                                        "</div>" +
                                        "<div class=\"aright\">" +
                                            "<span class=\"myglyphicon " + ((movie.Songs && movie.Songs !== "[]" && movie.Songs.indexOf("YoutubeURL") > -1) ? "song" : "") + "\"></span>" +
                                        "</div>" +
                                        "<div class=\"aleft\">" +
                                            "<span class=\"myglyphicon " + ((movie.Posters && movie.Posters !== "[]" && JSON.parse(movie.Posters).length > 1) ? "photo" : "") + "\"></span>" +
                                        "</div>" +
                                        "<div class=\"aright\">" +
                                            "<span class=\"myglyphicon " + (1 === 0 ? "review" : "") + "\"></span>" +
                                        "</div>" +
                                    "</div>" +
                                    "<div class=\"movie-synopsis\" style=\"display: none;\">" + synopsis + "</div>" +
                                "</div>" +
                            "</div>" +
                        "</div>" +
                    "</div>" +
                "</div>";
    }

    anchor.append(html);
    list.append(anchor);
    movieContainer.append(list);

    return list;
}

function MovieImageLoaded(img) {
    if (img && $(img)[0]) {
        // When image is of small size, it leaves lot of white spaces next to tile. When image is of large size (Dhoom), it overlaps the next image
        // Hence keeping the height + width of fix size.
        var newWidth = (TILE_MODE == 0) ? 225 : 200;
        var newHeight = (TILE_MODE == 0) ? 340 : 300;

        $(img).css("width", newWidth + "px").css("height", newHeight + "px");
        $(img).parent("li.movie").show();
    }
}

function LoadDefaultImage(element) {
    $(element).attr("src", PUBLIC_BLOB_URL + "default-movie.jpg");
    var width = $(document).width();
    var imgWidth = parseInt($(element).css("width").replace("px"));
    var imgHeight = parseInt($(element).css("height").replace("px"));

    var ratio = imgWidth / imgHeight;
    var newWidth = 225;
    $(element).css("width", newWidth + "px").css("height", "340px");
}