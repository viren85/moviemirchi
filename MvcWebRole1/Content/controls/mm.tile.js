function PopulatingMovies(movie, container) {
    var movieContainer = $("." + container + " ul");

    $("." + container).addClass("tile-type-" + TILE_MODE);

    var poster = [];
    poster = JSON.parse(movie.Posters);
    var src = (poster != null && poster.length > 0) ? PUBLIC_BLOB_URL + poster[poster.length - 1] : PUBLIC_BLOB_URL + "default-movie.jpg";

    var anchor = $("<a/>");
    var list = $("<li/>");

    list.attr("class", "movie")
    //anchor.attr("href", "Movie?name=" + movie.UniqueName);
    anchor.attr("href", "/Movie/" + movie.UniqueName);
    anchor.attr("title", movie.Name);
    //anchor.append(img);
    var synopsis = movie.Synopsis.length > 500 ? movie.Synopsis.substring(0, 500) + "..." : movie.Synopsis;

    var html = "";
    var strSongs = "", songs = [];
    songs = JSON.parse(movie.Songs);
    if (songs != undefined && songs.length > 0) {
        for (var sIndex = 0; sIndex < songs.length; sIndex++) {
            strSongs += "<div><span>" + songs[sIndex].SongTitle + "</span><span class='play'></span></div>";

            if (sIndex == 2) break;
        }
    }

    console.log(movie);
    var criticRating;
    if (movie.MyScore == "" || movie.MyScore == undefined || JSON.parse(movie.MyScore).criticrating == undefined || JSON.parse(movie.MyScore).criticrating == "") {
        criticRating = 0;
    } else {        
        criticRating = parseInt(JSON.parse(movie.MyScore).criticrating) / 10;
    }
    
    console.log(criticRating);

    if (TILE_MODE == 0) {
        html = "<div id=\"picAndCaption\" class=\"viewingDiv " + movie.UniqueName + "\">" +
                    "<div id=\"imageContainer\" class=\"viewer\" style=\"height: 400px;\">" +
                        "<img id=\"imageEl\" onerror=\"LoadDefaultImage(this);\" onload=\"MovieImageLoaded(this);\" class=\"movie-poster shownImage\" title=\"" + movie.Name + "\" alt=\"" + movie.Name + "\" src=\"" + src + "\" style=\"margin: auto;\"/>" +
                        "<div class=\"captionAndNavigate\">" +
                            "<div id=\"captionCredit\" class=\"multimediaCaption\">" +
                                "<div id=\"photoCaption\">" +
                                    "<div class=\"img-movie-name img-movie-name-tile-type-" + TILE_MODE + "\">" + movie.Name + "</div>" +
                                    "<div class=\"img-movie-genre img-movie-genre-tile-type-" + TILE_MODE + "\">" + movie.Genre + "</div>" +
                                    "<div class=\"img-movie-date img-movie-date-tile-type-" + TILE_MODE + "\">" + movie.Month + "</div>" +
                                    //GetMovieRateControl(movie.Ratings) +
                                    GetMovieRateControl(criticRating) +
                                    "<div class=\"movie-synopsis\" style=\"display: none;\">" + synopsis + "</div>" +
                                "</div>" +
                            "</div>" +
                        "</div>" +
                    "</div>" +
                "</div>";
    }
    else {
        html =
                "<div id=\"picAndCaption\" class=\"viewingDiv " + movie.UniqueName + "\">" +
                    "<div id=\"imageContainer\" class=\"viewer\" style=\"height: 300px;\">" +
                        "<img id=\"imageEl\" onerror=\"LoadDefaultImage(this);\" onload=\"MovieImageLoaded(this);\" class=\"movie-poster shownImage\" title=\"" + movie.Name + "\" alt=\"" + movie.Name + "\" src=\"" + src + "\" style=\"margin: auto;\"/>" +
                    "</div>" +
                    "<div id=\"hover\" style=\"width: 200px; padding: 4%; background-color: white; float: left; height: 175px;border: 1px solid #ddd; box-shadow: -3px 3px 5px #ccc;\">" +
                                "<div class=\"img-movie-name\">" + movie.Name + "</div>" +
                                "<div class=\"img-movie-genre\">" + movie.Genre + "</div>" +
                                "<div class=\"img-movie-date\">" + movie.Month + "</div>" +
                                //GetRateControl(movie.Ratings) +
                                GetMovieRateControl(criticRating) +
                                "<div class=\"movie-songs\" style=\"display: none;\">" + strSongs
                                /*"<div><span>Tu hi Junoon</span><span class='play'></span></div>" +
                                "<div><span>Malang</span><span class='play'></span></div>" +
                                "<div><span>Kamli</span><span class='play'></span></div>"*/
                                + "</div>" +
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
        var width = $(document).width();
        var imgWidth = parseInt($(img).css("width").replace("px"));
        var imgHeight = parseInt($(img).css("height").replace("px"));

        var ratio = imgWidth / imgHeight;
        //var newWidth = 400 * ratio;

        // When image is of small size, it leaves lot of white spaces next to tile. When image is of large size (Dhoom), it overlaps the next image
        // Hence keeping the height + width of fix size.
        var newWidth = (TILE_MODE == 0) ? 263 : 200;
        var newHeight = (TILE_MODE == 0) ? 400 : 300;

        $(img).css("width", newWidth + "px").css("height", newHeight + "px");
    }
}

function LoadDefaultImage(element) {
    //$(element).attr("src", "/Posters/Images/default-movie.jpg"); 
    $(element).attr("src", PUBLIC_BLOB_URL + "default-movie.jpg");
    var width = $(document).width();
    var imgWidth = parseInt($(element).css("width").replace("px"));
    var imgHeight = parseInt($(element).css("height").replace("px"));

    var ratio = imgWidth / imgHeight;
    var newWidth = 263;
    $(element).css("width", newWidth + "px").css("height", "400px");
}