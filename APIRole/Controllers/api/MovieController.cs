
namespace CloudMovie.APIRole.API
{
    using DataStoreLib.Models;
    using DataStoreLib.Storage;
    using LuceneSearchLibrary;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using System.Web.Mvc;
    using System.Web.Script.Serialization;

    /// <summary>
    /// This API returns list of all the movies based on type. Type could be “current”, “all” “current” movies are having release date threshold of 1 month (configurable) 
    /// “all” movies type will return top 100 movies released recently.
    /// </summary>
    public class MovieController : BaseController
    {
        private static Lazy<JavaScriptSerializer> jsonSerializer = new Lazy<JavaScriptSerializer>(() => new JavaScriptSerializer());

        // get : api/Movies?type={current/all (default)}&resultlimit={default 100}          
        protected override string ProcessRequest()
        {
            return string.Empty;
        }

        [System.Web.Http.HttpGet]
        [System.Web.Http.AcceptVerbs("GET", "POST")]
        public ActionResult AddMovie(MovieEntity data)
        {
            MovieEntity movie = data;
            if (movie == null || string.IsNullOrEmpty(movie.Name))
            {
                return null;
            }

            try
            {
                //data = HttpContext.Current.Server.UrlDecode(data);
                JavaScriptSerializer json = new JavaScriptSerializer();
                //MovieEntity movie = json.Deserialize(data, typeof(MovieEntity)) as MovieEntity;

                if (movie != null)
                {
                    string uniqueName = movie.Name.Replace(" ", "-").Replace("&", "-and-").Replace(".", "").Replace("'", "").ToLower();

                    var tableMgr = new TableManager();
                    //MovieEntity oldEntity = tableMgr.GetMovieByUniqueName(movie.Name);
                    MovieEntity oldEntity = tableMgr.GetMovieByUniqueName(uniqueName);

                    if (oldEntity != null)
                    {
                        var rand = new Random();
                        int num = rand.Next(999999);

                        oldEntity.UniqueName = num.ToString() + oldEntity.UniqueName;

                        //tableMgr.UpdateMovieById(oldEntity);
                    }

                    MovieEntity entity = new MovieEntity();

                    //entity.RowKey = entity.MovieId = Guid.NewGuid().ToString();
                    entity.RowKey = entity.MovieId = movie.MovieId;
                    entity.Name = movie.Name;
                    entity.AltNames = movie.AltNames;
                    entity.Posters = movie.Posters;
                    entity.Ratings = movie.Ratings;
                    entity.Synopsis = movie.Synopsis;
                    entity.Casts = movie.Casts;
                    entity.Stats = movie.Stats;
                    entity.Songs = movie.Songs;
                    entity.Trailers = movie.Trailers;
                    entity.Pictures = movie.Pictures;
                    entity.Genre = movie.Genre;
                    entity.Month = movie.Month;
                    entity.Year = movie.Year;
                    entity.UniqueName = movie.UniqueName;
                    entity.State = movie.State;
                    entity.MyScore = movie.MyScore;
                    entity.JsonString = movie.JsonString;
                    entity.Popularity = movie.Popularity;

                    tableMgr.UpdateMovieById(entity);

                    //delete Entry in lucene search index
                    // Fix following method call - What shall be other param? 
                    LuceneSearch.ClearLuceneIndexRecord(movie.MovieId, "Id");
                    LuceneSearch.ClearLuceneIndexRecord(movie.UniqueName, "UniqueName");

                    string posterUrl = "default-movie.jpg";
                    string critics = string.Empty;

                    if (!string.IsNullOrEmpty(entity.Posters))
                    {
                        List<string> pList = json.Deserialize(entity.Posters, typeof(List<string>)) as List<string>;
                        if (pList != null && pList.Count > 0)
                            posterUrl = pList[pList.Count - 1];
                    }

                    var reviewDic = tableMgr.GetReviewsByMovieId(entity.MovieId);

                    if (reviewDic != null && reviewDic.Values != null && reviewDic.Values.Count > 0)
                    {
                        List<string> lCritics = new List<string>();

                        foreach (ReviewEntity re in reviewDic.Values)
                        {
                            lCritics.Add(re.ReviewerName);
                        }

                        critics = json.Serialize(lCritics);
                    }

                    // add updated entry in lucene search index
                    MovieSearchData movieSearchIndex = new MovieSearchData();
                    movieSearchIndex.Id = entity.RowKey;
                    movieSearchIndex.Title = entity.Name;
                    movieSearchIndex.Type = entity.Genre;
                    movieSearchIndex.TitleImageURL = posterUrl;
                    movieSearchIndex.UniqueName = entity.UniqueName;
                    movieSearchIndex.Description = movie.Casts;
                    movieSearchIndex.Critics = critics;
                    movieSearchIndex.Link = entity.UniqueName;
                    LuceneSearch.AddUpdateLuceneIndex(movieSearchIndex);
                }
            }
            catch (Exception ex)
            {
                return null;
            }

            return null;
            //return Json(new { Status = "Ok", actors = "Actors" }, JsonRequestBehavior.AllowGet);
        }
    }
}
