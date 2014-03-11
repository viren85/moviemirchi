using DataStoreLib.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Script.Serialization;
using DataStoreLib.Models;

namespace MvcWebRole1.Controllers.api
{
    public class MovieInfoController : BaseController
    {
        #region Commented code
        // get : api/MovieInfo?movieId={id}
        /*protected override string ProcessRequest()
        {
            var qpParams = HttpUtility.ParseQueryString(this.Request.RequestUri.Query);
            if (string.IsNullOrEmpty(qpParams["movieId"]))
            {
                throw new ArgumentException("movieId is not present");
            }
            return @"{ 
	""movieId"" : ""guid"",
	""poster"" : {
		""height"" : 300,
		""width"" : 200,
		""url"" = ""test""
	},
    ""name"" : ""blah blah"",
	""rating"" : {
		""system"" : 5,
		""critic"" : 6,
		""hot"" : ""no""
	},
	""info"" : {
		""synopsis"" : ""this is a brilliant scary movie"",
		""cast"" : [{
				""name"" : ""ben affleck"",
				""charactername"" : ""mickey"",
				""image"" : {
					""height"" : 300,
					""width"" : 200,
					""url"" = ""test""
				},
				""role"" : ""producer""
			}, {
				""name"" : ""jerry afflect"",
				""charactername"" : ""mouse"",
				""image"" : {
					""height"" : 300,
					""width"" : 200,
					""url"" = ""test""
				},
				""role"" : ""actor""
			}
		],
		""stats"" : {
			""budget"" : ""30,000"",
			""boxoffice"" : ""50000""
		},
		""multimedia"" : {
			""songs"" : [{
					""name"" : ""chaiyya chaiyya"",
					""url"" : ""songtest""
				}, {
					""name"" : ""chaiyya chaiyya"",
					""url"" : ""songtest""
				}
			],
			""trailers"" : [{
					""name"" : ""best movie"",
					""url"" : ""trailertest""
				}, {
					""name"" : ""chaiyya chaiyya"",
					""url"" : ""songtest""
				}
			],
			""pics"" : [{
					""caption"" : ""test caption"",
					""image"" : {
						""height"" : 300,
						""width"" : 200,
						""url"" = ""test""
					}
				}
			]
		}
	}
	""reviews"" : [{
			""name"" : ""khan"",
			""rating"" : {
				""system"" : 5,
				""critic"" : 6,
				""hot"" : ""no""
			},
			""summary"" : ""this is awesome"",
			""outlink"" : ""outlinktest""
		}
	]
}";
        }*/
        #endregion

        // get : api/MovieInfo?q={movieId}
        protected override string ProcessRequest()
        {
            JavaScriptSerializer json = new JavaScriptSerializer();

            MovieInfo movieInfo = new MovieInfo();

            var qpParams = HttpUtility.ParseQueryString(this.Request.RequestUri.Query);
            if (string.IsNullOrEmpty(qpParams["q"]))
            {
                throw new ArgumentException("movieId is not present");
            }

            string name = qpParams["q"].ToString();

            var tableMgr = new TableManager();
            //var movie = tableMgr.GetMovieById(movieId);
            var movie = tableMgr.GetMovieByUniqueName(name);

            if (movie != null)
            {
                movieInfo.movieId = movie.MovieId;
                movieInfo.Movie = movie;

                var reviews = movie.GetReviewIds();

                var reviewList = tableMgr.GetReviewsByMovieId(movieInfo.movieId);

                List<ReviewEntity> userReviews = new List<ReviewEntity>();

                if (reviewList != null)
                {
                    foreach (var review in reviewList)
                    {
                        userReviews.Add(review.Value);
                    }
                }

                movieInfo.MovieReviews = userReviews;
            }

            return json.Serialize(movieInfo);
        }
    }

    public class MovieInfo
    {
        public MovieEntity Movie { get; set; }
        public List<ReviewEntity> MovieReviews { get; set; }
        public List<MovieEntity> MoviesList { get; set; }
        public string movieId { get; set; }
        public string name { get; set; }
        public PosterInfo poster { get; set; }
        public Rating rating { get; set; }
        public Info info { get; set; }
        public List<Review> reviews { get; set; }
    }

    public class PosterInfo
    {
        public int height { get; set; }
        public int width { get; set; }
        public string url { get; set; }
    }

    public class Rating
    {
        public int system { get; set; }
        public int critic { get; set; }
        public string hot { get; set; }
    }

    public class Info
    {
        public string synopsis { get; set; }
        public List<Cast> cast { get; set; }
        public Stats stats { get; set; }

        public Multimedia multimedia { get; set; }
    }

    public class Cast
    {
        public string name { get; set; }
        public string charactername { get; set; }
        public PosterInfo image { get; set; }
        public string role { get; set; }
    }

    public class Stats
    {
        public string budget { get; set; }
        public string boxoffice { get; set; }
    }

    public class Multimedia
    {
        public List<SongTrailer> songs { get; set; }
        public List<SongTrailer> trailers { get; set; }
        public List<Picture> pics { get; set; }
    }

    public class SongTrailer
    {
        public string name { get; set; }
        public string url { get; set; }
    }

    public class Picture
    {
        public string caption { get; set; }
        public PosterInfo pics { get; set; }
    }

    public class Review
    {
        public string name { get; set; }
        public Rating rating { get; set; }
        public string summary { get; set; }
        public string outlink { get; set; }
    }
}
