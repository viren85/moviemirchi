function PopulatingMovies(movie) {
    var movieContainer = $(".movie-list ul");

    var poster = [];
    poster = JSON.parse(movie.Posters);
    var src = (poster != null && poster.length > 0) ?
        "images/" + poster[poster.length - 1] :
        "images/default-movie.jpg";

    var anchor = $("<a/>");
    var list = $("<li/>");

    list.attr("class", "movie")
    //anchor.attr("href", "Movie?name=" + movie.UniqueName);
    anchor.attr("href", "Movie/" + movie.UniqueName);
    anchor.attr("title", movie.Name);
    //anchor.append(img);


    var synopsis = movie.Synopsis.length > 500 ? movie.Synopsis.substring(0, 500) + "..." : movie.Synopsis;

    var html =
    "<div id=\"picAndCaption\" class=\"viewingDiv " + movie.UniqueName + "\">" +
        "<div id=\"imageContainer\" class=\"viewer\" style=\"height: 300px;\">" +
            "<img id=\"imageEl\" onerror=\"LoadDefaultImage(this);\" onload=\"MovieImageLoaded(this);\" class=\"movie-poster shownImage\" title=\"" + movie.Name + "\" alt=\"" + movie.Name + "\" src=\"" + src + "\" style=\"margin: auto;\">" +
        "</div>" +
        "<div class=\"captionAndNavigate\" style=\"width:170px;padding: 15px;padding-top: 5px;padding-bottom:5px;\">" +
                "<div id=\"captionCredit\" style=\"width: 398px;\" class=\"multimediaCaption\">" +
                    "<div id=\"photoCaption\">" +
                        "<div class=\"img-movie-name\">" + movie.Name + "</div>" +
                        "<div class=\"img-movie-genre\">" + movie.Genre + "</div>" +
                        "<div class=\"img-movie-date\">" + movie.Month + "</div>" +
                        GetRateControl(movie.Ratings) +
                        "<div class=\"movie-songs\">" +
                        "<ul>" +
                        "<li><span>Tu hi Junoon</span><span class='play'></span></li>" +
                        "<li><span>Malang</span><span class='play'></span></li>" +
                        "<li><span>Kamli</span><span class='play'></span></li>" +
                        "</ul>"
                        + "</div>" +
                        "<div class=\"movie-synopsis\" style=\"display: none;\">" + synopsis + "</div>" +
                    "</div>" +
                "</div>" +
            "</div>" +
    "</div>";

    anchor.append(html);
    //anchor.append(GetRateControl(6));
    list.append(anchor);
    movieContainer.append(list);

    return list;
}

function MovieImageLoaded(img) {
    var width = $(document).width();
    var imgWidth = parseInt($(img).css("width").replace("px"));
    var imgHeight = parseInt($(img).css("height").replace("px"));

    var ratio = imgWidth / imgHeight;
    //var newWidth = 400 * ratio;

    // When image is of small size, it leaves lot of white spaces next to tile. When image is of large size (Dhoom), it overlaps the next image
    // Hence keeping the height + width of fix size.
    var newWidth = 200;

    /*
    if (newWidth > 263) {
        newWidth = 263;
    }
    else if (newWidth < 263) {
        newWidth = 263;
    }*/

    $(img).css("width", newWidth + "px").css("height", "300px");
}

var defaultLoaded = false;

function LoadDefaultImage(element) {
    if (!defaultLoaded) {
        $(element).attr("src", "images/default-movie.jpg");
        var width = $(document).width();
        var imgWidth = parseInt($(element).css("width").replace("px"));
        var imgHeight = parseInt($(element).css("height").replace("px"));

        var ratio = imgWidth / imgHeight;
        var newWidth = 200;
        $(element).css("width", newWidth + "px").css("height", "300px");
        defaultLoaded = true;
    }
}