
namespace CloudMovie.APIRole.API
{
    using DataStoreLib.Constants;
    using DataStoreLib.Models;
    using DataStoreLib.Storage;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using System.Web.Script.Serialization;

    public class GetCastByTypeController : BaseController
    {
        List<Cast> tempCast = new List<Cast>();

        // get : api/GetCastByType?t={type=[all, actor, director, music director etc]}&l={limit/count eg 5,10,100 so on default 100}
        protected override string ProcessRequest()
        {
            JavaScriptSerializer json = new JavaScriptSerializer();

            try
            {
                var tblMgr = new TableManager();
                var qpParam = HttpUtility.ParseQueryString(this.Request.RequestUri.Query);

                string type = "all";
                int limit = 100;

                if (!string.IsNullOrEmpty(qpParam["t"]))
                {
                    type = qpParam["t"].ToString().ToLower();
                }

                if (!string.IsNullOrEmpty(qpParam["l"]))
                {
                    limit = Convert.ToInt32(qpParam["l"].ToString());
                }

                if (tempCast == null || tempCast.Count == 0)
                {
                    var movies = tblMgr.GetAllMovies();

                    foreach (var movie in movies)
                    {
                        List<Cast> castList = json.Deserialize(movie.Value.Casts, typeof(List<Cast>)) as List<Cast>;

                        if (castList != null)
                        {
                            foreach (var cast in castList)
                            {
                                if (!tempCast.Exists(c => c.name == cast.name))
                                {
                                    tempCast.Add(cast);
                                }
                            }
                        }
                    }
                }

                if (type == "all")
                {
                    var actor = (from u in tempCast
                                 where u.role.ToLower() == "actor" || u.role.ToLower() == "actress"
                                 select u).Distinct().Take(limit);

                    var directors = (from u in tempCast
                                     where u.role.ToLower() == "director"
                                     select u).Distinct().Take(limit);

                    var musicDirectors = (from u in tempCast
                                          where u.role.ToLower() == "music director"
                                          select u).Distinct().Take(limit);


                    return json.Serialize(new { Status = "Ok", Actor = actor, Director = directors, MusicDirector = musicDirectors });
                }
                else if (type == "actor")
                {
                    var actor = (from u in tempCast
                                 where u.role.ToLower() == "actor" || u.role.ToLower() == "actress"
                                 select u).Distinct().Take(limit);

                    return json.Serialize(new { Status = "Ok", Actor = actor });
                }
                else if (type == "director")
                {
                    var directors = (from u in tempCast
                                     where u.role.ToLower() == "director"
                                     select u).Distinct().Take(limit);

                    return json.Serialize(new { Status = "Ok", Director = directors });
                }
                else if (type == "musicdirector")
                {
                    var musicDirectors = (from u in tempCast
                                          where u.role.ToLower() == "music director"
                                          select u).Distinct().Take(limit);

                    return json.Serialize(new { Status = "Ok", MusicDirector = musicDirectors });
                }
            }
            catch (Exception ex)
            {
                return json.Serialize(new { Status = "Error", UserMessage = "Unable to get movie cast", ActualError = ex.Message });
            }

            return json.Serialize(new { Status = "Error", UserMessage = "Query string is empty." });
        }
    }
}
