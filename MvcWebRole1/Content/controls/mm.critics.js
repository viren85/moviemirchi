var critics = [
        { href: 'rajeev-masand', title: 'Rajeev Masand', name: 'Rajeev Masand', poster: 'rajeev-masand.jpg', aff: 'CNN IBN' },
        { href: 'tarun-adarsh', title: 'Tarun Adarsh', name: 'Tarun Adarsh', poster: 'tarun-adarsh.jpg', aff: 'Bollywood Hungama, Trade Guide' },
        { href: 'anupama-chopra', title: 'Anupama Chopra', name: 'Anupama Chopra', poster: 'anupama-chopra.jpg', aff: 'Hindustan Times' },
        { href: 'shubra-gupta', title: 'Shubra Gupta', name: 'Shubra Gupta', poster: 'shubra-gupta.jpg', aff: 'Indian Express' },
        { href: 'tushar-joshi', title: 'Tushar Joshi', name: 'Tushar Joshi', poster: 'tushar-joshi.jpg', aff: 'DNA' },
        { href: 'raja-sen', title: 'Raja Sen', name: 'Raja Sen', poster: 'raja-sen.jpg', aff: 'rajasen.com' },
        { href: 'shubha-shetty', title: 'Shubha Shetty', name: 'Shubha Shetty', poster: 'shubha-shetty.jpg', aff: 'Mid-day' },
        { href: 'pratim-gupta', title: 'Pratim Gupta', name: 'Pratim Gupta', poster: 'pratim-gupta.jpg', aff: 'Telegraph, Kolkata' },
        { href: 'vajir-singh', title: 'Vajir Singh', name: 'Vajir Singh', poster: 'vajir-singh.jpg', aff: 'Box office India' },
        { href: 'komal-nahta', title: 'Komal Nahta', name: 'Komal Nahta', poster: 'komal-nahta.jpg', aff: 'Film Information' },
        { href: 'sudhish-kamath', title: 'Sudhish Kamath', name: 'Sudhish Kamath', poster: 'sudhish-kamath.jpg', aff: 'The Hindu' },
        { href: 'sarit-ray', title: 'Sarit Ray', name: 'Sarit Ray', poster: 'sarit-ray.jpg', aff: 'Hindustan Times' },
        { href: 'gaurav-malani', title: 'Gaurav Malani', name: 'Gaurav Malani', poster: 'gaurav-malani.jpg', aff: 'Times of India' },
        { href: 'aseem-chhabra', title: 'Aseem Chhabra', name: 'Aseem Chhabra', poster: 'aseem-chhabra.jpg', aff: 'Rediff' },
        { href: 'saibal-chatterjee', title: 'Saibal Chatterjee', name: 'Saibal Chatterjee', poster: 'saibal-chatterjee.jpg', aff: 'NDTV' },
        { href: 'rachit-gupta', title: 'Rachit Gupta', name: 'Rachit Gupta', poster: 'rachit-gupta.jpg', aff: 'Filmfare' },
        { href: 'mihir-fadnavis', title: 'Mihir Fadnavis', name: 'Mihir Fadnavis', poster: 'mihir-fadnavis.jpg', aff: 'FirstPost' },
        { href: 'taran-adarsh', title: 'Taran Adarsh', name: 'Taran Adarsh', poster: 'taran-adarsh.jpg', aff: 'Bollywood Hungama, Trade Guide' },
];

$(document).ready(function () {
    var ul = $(".critics-container").find("ul");//.css("height", "280px");
    var counter = 1;
    var style = "";
    var impCriticsCount = new Util().GetElementCount(60, 125);

    critics.forEach(function (critic) {
        if (counter <= impCriticsCount && counter < 6) {
            ul.append(
                "<li class=\"reviewer\">" +
                    "<a href=\"movie/reviewer/" + critic.href + "\" title=\"" + critic.title + " (" + critic.aff + ")" + "\">" +
                        "<div id=\"picAndCaption\" class=\"viewingDiv\">" +
                            "<div id=\"imageContainer\" class=\"viewer\">" +
                                "<img id=\"imageEl\" class=\"movie-poster shownImage\" title=\"" + critic.title + "(" + critic.aff + ")" + "\" alt=\"" + critic.title + " (" + critic.aff + ")" + "\" src=\"/Posters/Images/critic/" + critic.poster + "\" style=\"margin: auto; height: 125px; width: 125px; \" onerror=\"LoadDefaultCriticImage(this);\">" +
                            "</div>" +
                        "</div>" +
                        "<div class=\"critics-name\">" + critic.name + "</div>" +
                    "</a>" +
                "</li>");
        }
            /*else if (impCriticsCount == (counter + 1)) {
                ul.append(
                    "<li class=\"reviewer\" style=\"clear: left\"" + ">" +
                        "<a href=\"movie/reviewer/" + critic.href + "\" title=\"" + critic.title + " (" + critic.aff + ")" + "\">" +
                            "<div id=\"picAndCaption\" class=\"viewingDiv\">" +
                                "<div id=\"imageContainer\" class=\"viewer\">" +
                                    "<img id=\"imageEl\" class=\"movie-poster shownImage\" title=\"" + critic.title + "(" + critic.aff + ")" + "\" alt=\"" + critic.title + " (" + critic.aff + ")" + "\" src=\"/Posters/Images/critic/" + critic.poster + "\" style=\"margin: auto; height: 125px; width: 125px; \" onerror=\"LoadDefaultCriticImage(this);\">" +
                                "</div>" +
                            "</div>" +
                            "<div class=\"critics-name\">" + critic.name + "</div>" +
                        "</a>" +
                    "</li>");
    
            }*/

        else {
            var className = "reviewer reviewer-small";
            if (counter == (impCriticsCount + 1)) {
                className = "reviewer reviewer-small first-reviewer";
            }

            ul.append(
               "<li class=\"" + className + "\">" +
                   "<a href=\"movie/reviewer/" + critic.href + "\" title=\"" + critic.title + " (" + critic.aff + ")" + "\">" +
                       "<div id=\"picAndCaption\" class=\"viewingDiv\">" +
                           "<div id=\"imageContainer\" class=\"viewer\">" +
                               "<img id=\"imageEl\" class=\"movie-poster shownImage\" title=\"" + critic.title + " (" + critic.aff + ")" + "\" alt=\"" + critic.title + " (" + critic.aff + ")" + "\" src=\"/Posters/Images/critic/" + critic.poster + "\" style=\"margin: auto; height: 50px; width: 50px; \" onerror=\"LoadDefaultCriticImage(this);\">" +
                           "</div>" +
                       "</div>" +
                       "<div class=\"critics-name-small\">" + critic.name + "</div>" +
                   "</a>" +
               "</li>");
        }

        counter++;
    });

    /*var totalReviewers = $(".critics-container ul li.reviewer").length;
    var pagerJson = { "pagerContainer": "critics-container", "tilesInPage": 5, "totalTileCount": totalReviewers, "pagerContainerId": "critics-pager", "tileWidth": "220" };
    PreparePaginationControl($(".critics-container"), pagerJson);*/
});

$(window).resize(function () {
    $(".critics-container").find("ul li").each(function () {
        $(this).attr("class", "reviewer-small");
        $(this).find("img").each(function () {
            $(this).css("width", "50px").css("height", "50px");
        });

        $(this).find(".critics-name").each(function () {
            $(this).attr("class", "critics-name-small");
        });
    });

    //1. Calculate the 60% of critics-container
    //2. Calculate the # of elements possible to fit in in 60%
    //3. Assign class to these elements
    var width = $(document).width();
    width = Math.round(width / 100);
    width = width * 60;
    var elementCount = Math.floor(width / 125);

    var counter = 1;
    var className = "reviewer reviewer-small";

    $(".critics-container").find("ul li").each(function () {
        if (counter < elementCount) {
            $(this).attr("class", "reviewer");
            $(this).find("img").each(function () {
                $(this).css("width", "125px").css("height", "125px");
            });

            $(this).find(".critics-name-small").each(function () {
                $(this).attr("class", "critics-name");
            });
        }

        counter++;
    });

    $(".critics-container").find("ul li.reviewer-small:first").each(function () {
        $(this).attr("class", "reviewer reviewer-small first-reviewer");
    });
});