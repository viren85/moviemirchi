
namespace CloudMovie.APIRole.API
{
    using DataStoreLib.Models;
    using DataStoreLib.Storage;
    using DataStoreLib.Utils;
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Linq;
    using System.Web;
    using System.Web.Mvc;
    using System.Web.Script.Serialization;

    public class GetUserPopularController : BaseController
    {
        private static Lazy<JavaScriptSerializer> jsonSerializer = new Lazy<JavaScriptSerializer>(() => new JavaScriptSerializer());
        private static Lazy<string> jsonError = new Lazy<string>(() =>
            jsonSerializer.Value.Serialize(
                new
                {
                    Status = "Error",
                    UserMessage = "Unable to get popular elements.",
                    ActualError = "",
                })
            );

        protected override string ProcessRequest()
        {
            string userFeedbackTags;
            if (!CacheManager.TryGet<string>(CacheConstants.UserFeedback, out userFeedbackTags))
            {
                try
                {
                    // Get top actors
                    // Get top music directors
                    // Currently we don't have any mechanism to map/track the role of artists
                    // E.g. AR Rehman is for music director. Complexity arises for Farhan Akhtar kind of person (multi talented)

                    // hard code genre & critics
                    var tableMgr = new TableManager();
                    List<ArtistEntity> artistList = tableMgr.GetAllArtist(string.Empty).ToList();

                    foreach (ArtistEntity ae in artistList)
                    {
                        ae.ArtistId = string.Empty;
                        ae.Bio = string.Empty;
                        ae.Born = string.Empty;
                        ae.Popularity = string.IsNullOrEmpty(ae.Popularity) ? "0" : ae.Popularity;
                        ae.JsonString = string.Empty;
                        ae.ETag = string.Empty;
                        ae.PartitionKey = string.Empty;
                        ae.RowKey = string.Empty;
                    }

                    IEnumerable<ArtistEntity> artistList1 = artistList.OrderByDescending(a => int.Parse(a.Popularity)).Take(5);

                    //IEnumerable<ArtistEntity> artistList = tableMgr.GetAllArtist(string.Empty).OrderByDescending(a => int.Parse(a.Popularity)).Take(5);

                    var roleReviewer = new string[] { "Anupama Chopra", "Taran Adarsh", "Rajeev Masand", "Komal Nahta" }
                        .Select(r => new
                        {
                            UniqueName = r.ToLower().Replace(" ", "-"),
                            Name = r,
                            Role = "Reviewer",
                            Weight = "3",
                        });

                    //IDictionary<string, MovieEntity> movieEntities = tableMgr.GetAllMovies();
                    // Music directors
                    // Get music directors name
                    // search in DB against list of these names - find which one is most popular based on the
                    // popularity column in artists table

                    // directors & producers
                    // Get director + producers name
                    // search in DB agains list of these names - find which one is most popular based on the 
                    // popularity column in artist table

                    /*var musicDirectors = movieEntities
                        .SelectMany(m => m.Value.GetMusicDirectors())
                        .GroupBy(g => g)
                        .OrderByDescending(g => g.Count())
                        .Take(4);

                    var directors = movieEntities
                        .SelectMany(m => m.Value.GetProducers()
                                          .Concat(m.Value.GetDirectors()))
                        .GroupBy(g => g)
                        .OrderByDescending(g => g.Count())
                        .Take(4);
                            
                    */

                    var popular = string.Format(
                        "[{0},{1}]",
                        jsonSerializer.Value.Serialize(artistList1),
                        jsonSerializer.Value.Serialize(roleReviewer));
                    //jsonSerializer.Value.Serialize(musicDirectors),
                    //jsonSerializer.Value.Serialize(directors));

                    userFeedbackTags = popular;

                    CacheManager.Add<string>(CacheConstants.UserFeedback, userFeedbackTags);
                }
                catch (Exception)
                {
                    // if any error occured then return User friendly message with system error message
                    return jsonError.Value;
                }
            }

            return userFeedbackTags;
        }
    }
}
