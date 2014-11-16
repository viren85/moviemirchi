

namespace CloudMovie.APIRole.Controllers.api
{
    using CloudMovie.APIRole.API;
    using CloudMovie.APIRole.UDT;
    using DataStoreLib.BlobStorage;
    using DataStoreLib.Models;
    using DataStoreLib.Storage;
    using DataStoreLib.Utils;
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Globalization;
    using System.IO;
    using System.Runtime.Serialization.Json;
    using System.Text;
    using System.Web;
    using System.Web.Mvc;
    using System.Web.Script.Serialization;
    using System.Xml;
    using Twitterizer;
    //using Twitterizer;

    public class CrawlTweetsController : BaseController
    {
        protected override string ProcessRequest()
        {
            JavaScriptSerializer json = new JavaScriptSerializer();
            var qpParams = HttpUtility.ParseQueryString(this.Request.RequestUri.Query);

            if (string.IsNullOrEmpty(qpParams["p"]))
            {
                throw new ArgumentException();
            }

            string page = qpParams["p"];
            string name = qpParams["n"];

            List<TwitterEntity> tweets = GetTweet(page, name);
            return json.Serialize(tweets);
        }

        private List<TwitterEntity> GetLatestTweets(string handle)
        {
            List<TwitterEntity> tweets = new List<TwitterEntity>();
            #region Twitter Library Setup
            OAuthTokens oaccesstkn = new OAuthTokens();
            string consumerKey = ConfigurationManager.AppSettings["TwitterConsumerKey"];
            string consumerSecret = ConfigurationManager.AppSettings["TwitterConsumerSecret"];

            oaccesstkn.AccessToken = ConfigurationManager.AppSettings["TwitterAccessToken"];
            oaccesstkn.AccessTokenSecret = ConfigurationManager.AppSettings["TwitterAccessSecret"]; ;
            oaccesstkn.ConsumerKey = consumerKey;
            oaccesstkn.ConsumerSecret = consumerSecret;
            #endregion

            try
            {
                #region Get Tweets From Twitter
                WebRequestBuilder webRequest = new WebRequestBuilder(new Uri(string.Format(ConfigurationManager.AppSettings["TwitterUrl"], handle, 500)), HTTPVerb.GET, oaccesstkn);

                string responseText;
                using (var response = webRequest.ExecuteRequest())
                {
                    using (var reader = new StreamReader(response.GetResponseStream()))
                    {
                        responseText = reader.ReadToEnd();
                    }
                }

                var brr = Encoding.UTF8.GetBytes(responseText);

                var streamReader = new StreamReader(new MemoryStream(brr));

                var serializer = new DataContractJsonSerializer(typeof(SearchResults));

                var tweetsResponse = (SearchResults)serializer.ReadObject(streamReader.BaseStream);

                streamReader.Close();
                #endregion

                foreach (var mmTweetData in tweetsResponse.Results)
                {
                    var tweetId = mmTweetData.Id.ToString();
                    try
                    {
                        var myTweet = new TwitterEntity
                        {
                            RowKey = Guid.NewGuid().ToString(),
                            TwitterId = tweetId,
                            TwitterIdString = mmTweetData.Id_Str,
                            TextMessage = mmTweetData.Text,
                            Source = mmTweetData.Source,
                            FromUser = mmTweetData.Source,
                            FromUserId = mmTweetData.ToUserName,
                            ProfileImageUrl = mmTweetData.User.ProfileImageUrl,
                            ProfileSecureImageUrl = mmTweetData.User.ProfileImageUrlHttps,
                            ReplyUserId = mmTweetData.User.FromUserId.ToString(),
                            ReplyScreenName = mmTweetData.User.FromUser,
                            ResultType = mmTweetData.SearchMetaData.ResultType,
                            LanguageCode = mmTweetData.SearchMetaData.IsoLanguageCode,
                            Created_At = ParseTwitterDateTime(mmTweetData.CreatedAt),
                            Status = "-1"
                        };

                        tweets.Add(myTweet);
                    }
                    catch (Exception)
                    {

                    }
                }
            }
            catch (Exception)
            {

            }

            return tweets;
        }

        private List<TwitterEntity> GetUpcomingMovieTweets()
        {
            List<TwitterEntity> twitterList = new List<TwitterEntity>();

            // Find the list of upcoming movies - Get their Twitter Handle
            if (CacheManager.TryGet(CacheConstants.TwitterJson + "Upcoming", out twitterList))
            {
                return twitterList;
            }
            else
            {
                if (twitterList == null)
                    twitterList = new List<TwitterEntity>();

                TableManager tbl = new TableManager();
                IEnumerable<MovieEntity> upcomingMovies = tbl.GetUpcomingMovies();
                if (upcomingMovies != null)
                {
                    foreach (MovieEntity movie in upcomingMovies)
                    {
                        if (!string.IsNullOrEmpty(movie.TwitterHandle))
                        {
                            // Call GetMovieTweets method
                            List<TwitterEntity> tweets = GetLatestTweets(movie.TwitterHandle);

                            // Add this movie to cache
                            if (tweets != null && tweets.Count > 0)
                            {
                                List<TwitterEntity> movieTweets = new List<TwitterEntity>();
                                if (CacheManager.TryGet(CacheConstants.TwitterJson + movie.UniqueName, out movieTweets))
                                {
                                    CacheManager.Remove(CacheConstants.TwitterJson + movie.UniqueName);
                                }

                                CacheManager.Add(CacheConstants.TwitterJson + movie.UniqueName, tweets);
                            }

                            if (tweets != null && tweets.Count > 5)
                            {
                                // Just keep top 5 tweets
                                tweets.RemoveRange(5, tweets.Count - 5);
                                twitterList.AddRange(tweets);
                            }
                            else if (tweets != null)
                                twitterList.AddRange(tweets); // Merge the tweet lists into one
                        }
                    }
                }

                if (twitterList.Count > 0)
                    CacheManager.Add(CacheConstants.TwitterJson + "Upcoming", twitterList);
            }

            return twitterList;
        }

        private List<TwitterEntity> GetNowPlayingMovieTweets()
        {
            List<TwitterEntity> twitterList = new List<TwitterEntity>();

            // Find the list of upcoming movies - Get their Twitter Handle
            if (CacheManager.TryGet(CacheConstants.TwitterJson + "NowPlaying", out twitterList))
            {
                return twitterList;
            }
            else
            {
                if (twitterList == null)
                    twitterList = new List<TwitterEntity>();

                TableManager tbl = new TableManager();
                IEnumerable<MovieEntity> upcomingMovies = tbl.GetCurrentMovies();
                if (upcomingMovies != null)
                {
                    foreach (MovieEntity movie in upcomingMovies)
                    {
                        if (!string.IsNullOrEmpty(movie.TwitterHandle))
                        {
                            // Call GetMovieTweets method
                            List<TwitterEntity> tweets = GetLatestTweets(movie.TwitterHandle);
                            // Add this movie to cache
                            if (tweets != null && tweets.Count > 0)
                            {
                                List<TwitterEntity> movieTweets = new List<TwitterEntity>();
                                if (CacheManager.TryGet(CacheConstants.TwitterJson + movie.UniqueName, out movieTweets))
                                {
                                    CacheManager.Remove(CacheConstants.TwitterJson + movie.UniqueName);
                                }

                                CacheManager.Add(CacheConstants.TwitterJson + movie.UniqueName, tweets);
                            }

                            if (tweets != null && tweets.Count > 5)
                            {
                                // Just keep top 5 tweets
                                tweets.RemoveRange(5, tweets.Count - 5);
                                twitterList.AddRange(tweets);
                            }
                            else if (tweets != null)
                                twitterList.AddRange(tweets); // Merge the tweet lists into one
                        }
                    }
                }

                if (twitterList.Count > 0)
                    CacheManager.Add(CacheConstants.TwitterJson + "NowPlaying", twitterList);
            }

            return twitterList;
        }

        private string GetTwitterHandle(string type, string name)
        {
            TableManager tbl = new TableManager();

            switch (type)
            {
                case "movie":
                    MovieEntity movie = tbl.GetMovieByUniqueName(name);
                    return movie.TwitterHandle;
                case "artist":
                    ArtistEntity artist = tbl.GetArtist(name);
                    return artist.TwitterHandle;
                case "critics":
                    return string.Empty;
            }

            return string.Empty;
        }

        private List<TwitterEntity> GetTweet(string type, string name)
        {
            switch (type)
            {
                case "home":
                    var upcoming = GetUpcomingMovieTweets();
                    var now = GetNowPlayingMovieTweets();
                    upcoming.AddRange(now);
                    return upcoming;
                case "critics":
                    // Get the critics twitter handle
                    // get tweets
                    break;
                case "artist":
                    // Get the artist twitter handle
                    // get tweets
                    break;
                case "movie":
                    // Get the movie twitter handle
                    // get tweets
                    break;
            }

            return null;
        }

        private DateTime ParseTwitterDateTime(string date)
        {
            const string format = "ddd MMM dd HH:mm:ss zzzz yyyy";
            return DateTime.ParseExact(date, format, CultureInfo.InvariantCulture);
        }
    }
}
