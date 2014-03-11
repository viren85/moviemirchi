// http://www.imdb.com/search/title?at=0&languages=hi%7C1&sort=moviemeter,asc&start=1&title_type=feature

var getmoviesJSON = function () {
 	var MovieType = function (el) {
	 var _el = el;
	 
	 var identityFn = function (v) { return v; };
	 var trimFn = function (v) { return v.trim(); };
	 
	 var getSafeText = function (el) {
		return el === null ? "" : el.innerText;
	 }
	 
	 var getArray = function (obj, fn) {
	    var _fn = (typeof fn == "undefined" || fn === null) ? identityFn : fn;
		var arr = new Array();
        for(var i = 0; i < obj.length; ++i) { 
	        arr.push(_fn(obj[i]));
		}
	    return arr;
	 };
	 
	 var getTitleTag = function(el) {
	    var titleEl = _el.querySelector(".title");
		var aTags = titleEl !== null ? titleEl.getElementsByTagName("a") : [];
		return aTags.length >= 2 ? aTags[1] : null;
	 };
	 
	var getLink = function(el, text) {
		var aTags = getArray(el.querySelectorAll("a"));
		var aTag = aTags.filter(function (t) {
			return t !== null && t.innerText === text;
		});
		var url = aTag === null ? "" : aTag[0].getAttribute("href");
		return url === "" ? "" : ("http://www.imdb.com" + url);
	};
	
	var getNameLinkArray = function(el, arr) {
		var results = new Array();
		for(var i = 0; i < arr.length; ++i) { 
			var _t = arr[i].trim();
			results.push({
				Name: _t,
				Link: getLink(el, _t),
			})
		}
		return results;
	};
	 
	 return {
		name : function() {
			return getSafeText(getTitleTag(_el));
		},
		link: function() {
			var aTag = getTitleTag(_el);
			var url = aTag === null ? "" : aTag.getAttribute("href");
			return url === "" ? "" : ("http://www.imdb.com" + url);
		},
		genre : function() {
			return getNameLinkArray(_el, getSafeText(_el.querySelector(".genre")).split("|"));
		},
		outline : function() {
			return getSafeText(_el.querySelector(".outline"));
		},
		credit : function() {
			var creditText = getSafeText(_el.querySelector(".credit"));
			var credits = creditText.split("Dir:");
			if (credits.length == 2) {
			    credits = credits[1].split("With:");
			    if (credits.length == 2) {
			        var director = credits[0];
			        var artist = credits[1];
				
				    return {
				        director: getNameLinkArray(_el, director.split(",")),
					    artist: getNameLinkArray(_el, artist.split(",")),
				    };
			    }
			}
			return {
			    artist: credits,
			};
		},
		rating : function() {
		    var rating = _el.querySelector(".user_rating .rating");
			var title = rating.getAttribute("title");
			var score = "?";
			var rate = "?";
			var maxrate = "?"
			var votes = "?";
			if (title !== null) {
				title = title.split(" ");
				score = title.filter(function(t) { 
					return t.indexOf("/") !== -1; 
				})[0].trim();
				maxrate = score.split("/")[1].trim();
				rate = score.split("/")[0].trim();
				votes = title.filter(function(t) { 
					return t.indexOf("(") !== -1; 
				})[0].split("(")[1];
			}
			
			return {
			    score: score,
			    rate: rate,
				maxrate: maxrate,
				votes: votes,
			};
		},
	  };
	};

	var Movie = function (el) {
	  var _el = el;
	  
	  var movie = new MovieType(_el);
	  return {
		name: movie.name(),
		link: movie.link(),
		genre: movie.genre(),
		outline: movie.outline(),
		credit: movie.credit(),
		rating: movie.rating(),
	  };
	};

	var scrapeMovies = function (el) {
		var movies = new Array();
		var resultsEl = el.querySelector(".results");
		if(resultsEl !== null) {
			var moviesEl = resultsEl.querySelectorAll(".detailed");
			if(resultsEl !== null) {
				for(var m = 0; m < moviesEl.length; ++m) {
				  movies.push(new Movie(moviesEl[m]));
				}
			}
		}
		return movies;
	};

	var movies = scrapeMovies(document.body);
	var moviesJSON = JSON.stringify(movies);
	return moviesJSON;
};

getmoviesJSON();