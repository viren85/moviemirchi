
namespace DataStoreLib.Storage
{
    using DataStoreLib.BlobStorage;
    using DataStoreLib.Models;
    using DataStoreLib.Utils;
    using Microsoft.WindowsAzure.Storage.Table;
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;

    public class TableManager : IStore
    {
        #region Login
        /// <summary>
        /// Return the List of all UserName
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public IDictionary<string, UserEntity> GetUsersByName(string userName)
        {
            var loginTable = TableStore.Instance.GetTable(TableStore.UserTableName) as UserTable;
            return loginTable.GetItemsByUserName<UserEntity>(userName);
        }
        /// <summary>
        /// Return the users by its id
        /// </summary>
        /// <param name="userIds"></param>
        /// <returns></returns>
        public IDictionary<string, UserEntity> GetUsersById(IEnumerable<string> userIds)
        {
            var userTable = TableStore.Instance.GetTable(TableStore.UserTableName);
            return userTable.GetItemsById<UserEntity>(userIds);
        }
        /// <summary>
        /// Return the all user
        /// </summary>
        /// <returns></returns>
        public IDictionary<string, UserEntity> GetAllUser()
        {
            var loginTable = TableStore.Instance.GetTable(TableStore.UserTableName);
            return loginTable.GetAllItems<UserEntity>();
        }
        /// <summary>
        /// Update the user by its Id
        /// </summary>
        /// <param name="users"></param>
        /// <returns></returns>
        public IDictionary<UserEntity, bool> UpdateUsersById(IEnumerable<Models.UserEntity> users)
        {
            var userTable = TableStore.Instance.GetTable(TableStore.UserTableName);
            Debug.Assert(userTable != null);

            var updateResult = userTable.UpdateItemsById(users.Cast<ITableEntity>());
            return
                updateResult
                    .ToDictionary(
                        pair => pair.Key as UserEntity,
                        pair => pair.Value);

            ////TODO: Clean comments
            ////var userList = new List<DataStoreLib.Models.TableEntity>(users).ConvertAll(x => (ITableEntity)x);
            ////var returnOp = userTable.UpdateItemsById(userList);

            ////var returnTranslateOp = new Dictionary<UserEntity, bool>();
            ////foreach (var b in returnOp.Keys)
            ////{
            ////    returnTranslateOp.Add(b as UserEntity, returnOp[b]);
            ////}
            ////return returnTranslateOp;

        }
        #endregion

        #region Movie
        /// <summary>
        /// Return the all Movies
        /// </summary>
        /// <returns></returns>
        public IDictionary<string, MovieEntity> GetAllMovies()
        {
            var movieTable = TableStore.Instance.GetTable(TableStore.MovieTableName);

            IDictionary<string, MovieEntity> movies;
            if (!CacheManager.TryGet<IDictionary<string, MovieEntity>>(CacheConstants.AllMovieEntities, out movies))
            {
                movies = movieTable.GetAllItems<MovieEntity>();
                movies =
                    movies
                    .OrderByDescending(m => m.Value.PublishDate)
                    .ToDictionary(
                        g => g.Key,
                        g => g.Value
                    );

                CacheManager.Add<IDictionary<string, MovieEntity>>(CacheConstants.AllMovieEntities, movies);
            }

            return movies;
        }

        public IDictionary<string, MovieEntity> GetArtistMovies()
        {
            return this.GetAllMovies();
        }

        public IDictionary<string, MovieEntity> GetGenrewiseMovies()
        {
            return this.GetAllMovies();
        }

        /// <summary>
        /// Return the movies by its id
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public IDictionary<string, MovieEntity> GetMoviesById(IEnumerable<string> ids)
        {
            var movieTable = TableStore.Instance.GetTable(TableStore.MovieTableName);
            return movieTable.GetItemsById<MovieEntity>(ids);
        }
        /// <summary>
        /// Return the list of movies by uniqueName
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public IDictionary<string, MovieEntity> GetMoviesByUniqueName(string name)
        {
            var movieTable = TableStore.Instance.GetTable(TableStore.MovieTableName) as MovieTable;
            return movieTable.GetItemsByName<MovieEntity>(name);
        }
        /// <summary>
        /// Update the movies by id
        /// </summary>
        /// <param name="movies"></param>
        /// <returns></returns>
        public IDictionary<MovieEntity, bool> UpdateMoviesById(IEnumerable<MovieEntity> movies)
        {
            var movieTable = TableStore.Instance.GetTable(TableStore.MovieTableName);
            Debug.Assert(movieTable != null);

            var updateResult = movieTable.UpdateItemsById(movies.Cast<ITableEntity>());
            return
                updateResult
                    .ToDictionary(
                        pair => pair.Key as MovieEntity,
                        pair => pair.Value);

            ////TODO: Clean comments
            ////var movieList = new List<DataStoreLib.Models.TableEntity>(movies).ConvertAll(x => (ITableEntity)x);
            ////var returnOp = movieTable.UpdateItemsById(movieList);

            ////var returnTranslateOp = new Dictionary<MovieEntity, bool>();
            ////foreach (var b in returnOp.Keys)
            ////{
            ////    returnTranslateOp.Add(b as MovieEntity, returnOp[b]);
            ////}
            ////return returnTranslateOp;
        }
        #endregion

        #region Review
        /// <summary>
        /// Return the reviews detail on the basis of reviewerid and movieid
        /// </summary>
        /// <param name="reviewerId"></param>
        /// <param name="movieId"></param>
        /// <returns></returns>
        public IDictionary<string, ReviewEntity> GetReviewsDetailById(string reviewerId, string movieId)
        {
            var reviewTable = TableStore.Instance.GetTable(TableStore.ReviewTableName) as ReviewTable;
            return reviewTable.GetReviewByMovieAndReviewId<ReviewEntity>(reviewerId, movieId);
        }
        /// <summary>
        /// return the reviews by review id
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public IDictionary<string, ReviewEntity> GetReviewsById(IEnumerable<string> ids)
        {
            var reviewTable = TableStore.Instance.GetTable(TableStore.ReviewTableName);
            return reviewTable.GetItemsById<ReviewEntity>(ids);
        }
        /// <summary>
        /// Update the reviews by id
        /// </summary>
        /// <param name="reviews"></param>
        /// <returns></returns>
        public IDictionary<ReviewEntity, bool> UpdateReviewsById(IEnumerable<ReviewEntity> reviews)
        {
            var reviewTable = TableStore.Instance.GetTable(TableStore.ReviewTableName);
            Debug.Assert(reviewTable != null);

            var updateResult = reviewTable.UpdateItemsById(reviews.Cast<ITableEntity>());
            return
                updateResult
                    .ToDictionary(
                        pair => pair.Key as ReviewEntity,
                        pair => pair.Value);

            ////TODO: how is this different from UpdateReviewesByReviewerId?

            ////TODO: Clean comments
            ////var reviewList = new List<DataStoreLib.Models.TableEntity>(reviews).ConvertAll(x => (ITableEntity)x);
            ////var returnOp = reviewTable.UpdateItemsById(reviewList);

            ////var returnTranslateOp = new Dictionary<ReviewEntity, bool>();
            ////foreach (var b in returnOp.Keys)
            ////{
            ////    returnTranslateOp.Add(b as ReviewEntity, returnOp[b]);
            ////}
            ////return returnTranslateOp;
        }
        /// <summary>
        /// Update the review rating
        /// </summary>
        public bool UpdateReviewRating(string reviewId, string rating)
        {
            var reviewTable = TableStore.Instance.GetTable(TableStore.ReviewTableName);
            Debug.Assert(reviewTable != null);

            var updateResult = reviewTable.UpdateReviewRating(reviewId, rating);
            return updateResult;
        }
        /// <summary>
        /// Return the reviews by movieid
        /// </summary>
        /// <param name="movieId"></param>
        /// <returns></returns>
        public IDictionary<string, Models.ReviewEntity> GetReviewsByMovieId(string movieId)
        {
            var reviewTable = TableStore.Instance.GetTable(TableStore.ReviewTableName);
            return reviewTable.GetItemsByMovieId<ReviewEntity>(movieId);
        }
        /// <summary>
        /// Return the reviews by the reviewerid
        /// </summary>
        /// <param name="reviewerName"></param>
        /// <returns></returns>
        public IDictionary<string, ReviewEntity> GetReviewsByReviewer(string reviewerName)
        {
            try
            {
                IDictionary<string, ReviewEntity> review;

                if (!CacheManager.TryGet<IDictionary<string, ReviewEntity>>(CacheConstants.ReviewEntity + reviewerName.Replace(" ", "-").ToLower().Trim(), out review))
                {
                    var reviewTable = TableStore.Instance.GetTable(TableStore.ReviewTableName);
                    var reviews = reviewTable.GetItemsByReviewer<ReviewEntity>(reviewerName);
                    review = reviews;
                    CacheManager.Add<IDictionary<string, ReviewEntity>>(CacheConstants.ReviewEntity + reviewerName.Replace(" ", "-").ToLower().Trim(), review);
                }

                return review;
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// Return the list of reviewes bu reviewerId
        /// </summary>
        /// <param name="review"></param>
        /// <returns></returns>
        public IDictionary<ReviewEntity, bool> UpdateReviewesByReviewerId(IEnumerable<ReviewEntity> reviews)
        {
            var reviewTable = TableStore.Instance.GetTable(TableStore.ReviewTableName);
            Debug.Assert(reviewTable != null);

            var updateResult = reviewTable.UpdateItemsById(reviews.Cast<ITableEntity>());
            return
                updateResult
                    .ToDictionary(
                        pair => pair.Key as ReviewEntity,
                        pair => pair.Value);

            ////TODO: Clean comments
            ////var reviewerList = new List<DataStoreLib.Models.TableEntity>(review).ConvertAll(x => (ITableEntity)x);
            ////var returnOp = reviewTable.UpdateItemsById(reviewerList);

            ////var returnTranslateOp = new Dictionary<ReviewEntity, bool>();
            ////foreach (var b in returnOp.Keys)
            ////{
            ////    returnTranslateOp.Add(b as ReviewEntity, returnOp[b]);
            ////}
            ////return returnTranslateOp;
        }

        /// <summary>
        /// Update Reviewes by reviewerId
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public IDictionary<string, ReviewEntity> UpdateReviewesByReviewerId(IEnumerable<string> ids)
        {
            // var reviewTable = TableStore.Instance.GetTable(TableStore.ReviewTableName);
            var reviewerTable = TableStore.Instance.GetTable(TableStore.ReviewerTableName);
            return reviewerTable.GetItemsById<ReviewEntity>(ids);
        }
        #endregion

        #region Reviewer
        /// <summary>
        /// Return the reviewer by id
        /// </summary>
        /// <param name="reviewerIds"></param>
        /// <returns></returns>
        public IDictionary<string, ReviewerEntity> GetReviewerById(IEnumerable<string> reviewerIds)
        {
            var reviewerTable = TableStore.Instance.GetTable(TableStore.ReviewerTableName);
            return reviewerTable.GetItemsById<ReviewerEntity>(reviewerIds);
        }
        /// <summary>
        /// Return the List of reviewers by its id
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public IDictionary<string, Models.ReviewerEntity> GetReviewersById(IEnumerable<string> ids)
        {
            var reviewerTable = TableStore.Instance.GetTable(TableStore.ReviewerTableName);
            return reviewerTable.GetItemsById<ReviewerEntity>(ids);
        }
        /// <summary>
        /// Return the list of reviewers to update 
        /// </summary>
        /// <param name="reviewers"></param>
        /// <returns></returns>
        public IDictionary<ReviewerEntity, bool> UpdateReviewers(IEnumerable<ReviewerEntity> reviewers)
        {
            var reviewerTable = TableStore.Instance.GetTable(TableStore.ReviewerTableName);
            Debug.Assert(reviewerTable != null);

            var updateResult = reviewerTable.UpdateItemsById(reviewers.Cast<ITableEntity>());
            return
                updateResult
                    .ToDictionary(
                        pair => pair.Key as ReviewerEntity,
                        pair => pair.Value);

            ////TODO: Clean comments
            ////var reviewerList = new List<DataStoreLib.Models.TableEntity>(reviewers).ConvertAll(x => (ITableEntity)x);
            ////var returnOp = reviewerTable.UpdateItemsById(reviewerList);

            ////var returnTranslateOp = new Dictionary<ReviewerEntity, bool>();
            ////foreach (var b in returnOp.Keys)
            ////{
            ////    returnTranslateOp.Add(b as ReviewerEntity, returnOp[b]);
            ////}
            ////return returnTranslateOp;
        }
        /// <summary>
        /// Return all reviewer
        /// </summary>
        /// <returns></returns>
        public IDictionary<string, ReviewerEntity> GetAllReviewer()
        {

            var reviewTable = TableStore.Instance.GetTable(TableStore.ReviewerTableName) as ReviewerTable;
            return reviewTable.GetAllReviewers<ReviewerEntity>();
        }

        public IEnumerable<ReviewerEntity> GetAllReviewer(string reviewerName)
        {
            try
            {
                var reviewerTable = TableStore.Instance.GetTable(TableStore.ReviewerTableName) as ReviewerTable;
                var allReviewer = reviewerTable.GetAllItems<ReviewerEntity>();

                foreach (ReviewerEntity re in allReviewer.Values)
                {
                    var filePattern = re.ReviewerName.Replace(" ", "-").ToLower();

                    var file = new BlobStorageService().GetSinglFile(BlobStorageService.Blob_ImageContainer, filePattern);

                    if (!string.IsNullOrEmpty(file))
                        re.ReviewerImage = file.Substring(file.LastIndexOf("/") + 1);
                    else
                        re.ReviewerImage = "default-movie.jpg";
                }

                //Return only those artists who have associated posters.
                //Associated poster means those artists are popular. Hence they have associated posters
                if (string.IsNullOrEmpty(reviewerName))
                    return allReviewer.Values.OrderBy(a => a.ReviewerName);
                else
                    return allReviewer.Values.Where(a => a.ReviewerName.ToLower().Trim().Contains(reviewerName));
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
                return null;
            }
        }

        #endregion

        #region Affiliation
        /// <summary>
        /// Return the affiliations by id
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public IDictionary<string, AffilationEntity> GetAffilationsByid(IEnumerable<string> ids)
        {
            var affilationTable = TableStore.Instance.GetTable(TableStore.AffilationTableName);
            return affilationTable.GetItemsById<AffilationEntity>(ids);
        }
        /// <summary>
        /// Update the affiliation by id
        /// </summary>
        /// <param name="affilations"></param>
        /// <returns></returns>
        public IDictionary<AffilationEntity, bool> UpdateAffilationsById(IEnumerable<AffilationEntity> affilations)
        {
            var affilationTable = TableStore.Instance.GetTable(TableStore.AffilationTableName);
            Debug.Assert(affilationTable != null);

            var updateResult = affilationTable.UpdateItemsById(affilations.Cast<ITableEntity>());
            return
                updateResult
                    .ToDictionary(
                        pair => pair.Key as AffilationEntity,
                        pair => pair.Value);

            ////TODO: Clean comments
            ////var afillationList = new List<DataStoreLib.Models.TableEntity>(affilations).ConvertAll(x => (ITableEntity)x);
            ////var returnOp = affilationTable.UpdateItemsById(afillationList);

            ////var returnTranslateOp = new Dictionary<AffilationEntity, bool>();
            ////foreach (var b in returnOp.Keys)
            ////{
            ////    returnTranslateOp.Add(b as AffilationEntity, returnOp[b]);
            ////}
            ////return returnTranslateOp;
        }
        /// <summary>
        /// Return all affiliations
        /// </summary>
        /// <returns></returns>
        public IDictionary<string, AffilationEntity> GetAllAffilation()
        {
            var affilationTable = TableStore.Instance.GetTable(TableStore.AffilationTableName);
            return affilationTable.GetAllAffilationItems<AffilationEntity>();
        }
        #endregion

        #region User favorites tables functions
        public IDictionary<string, UserFavoriteEntity> GetUserFavoritesById(IEnumerable<string> ids)
        {
            var userFavoritesTable = TableStore.Instance.GetTable(TableStore.UserFavoriteTableName);
            return userFavoritesTable.GetItemsById<UserFavoriteEntity>(ids);
        }

        public UserFavoriteEntity GetUserFavoritesByUserId(string userId)
        {
            var userFavoriteTable = TableStore.Instance.GetTable(TableStore.UserFavoriteTableName) as UserFavoriteTable;
            return userFavoriteTable.GetUserFavoritesByUserId(userId);
        }

        public IDictionary<UserFavoriteEntity, bool> UpdateUserFavoritesById(IEnumerable<Models.UserFavoriteEntity> userFavorites)
        {
            var userFavoriteTable = TableStore.Instance.GetTable(TableStore.UserFavoriteTableName);
            Debug.Assert(userFavoriteTable != null);

            var movieList = new List<DataStoreLib.Models.TableEntity>(userFavorites).ConvertAll(x => (ITableEntity)x);
            var returnOp = userFavoriteTable.UpdateItemsById(movieList);

            var returnTranslateOp = new Dictionary<UserFavoriteEntity, bool>();
            foreach (var b in returnOp.Keys)
            {
                returnTranslateOp.Add(b as UserFavoriteEntity, returnOp[b]);
            }
            return returnTranslateOp;
        }
        #endregion

        #region Popular on movie mirchi table
        public IDictionary<string, PopularOnMovieMirchiEntity> GetPopularOnMovieMirchisById(IEnumerable<string> ids)
        {
            var popularOnMoiveMirchiTable = TableStore.Instance.GetTable(TableStore.PopularOnMovieMirchiName);
            return popularOnMoiveMirchiTable.GetItemsById<PopularOnMovieMirchiEntity>(ids);
        }

        public IDictionary<PopularOnMovieMirchiEntity, bool> UpdatePopularOnMovieMirchisById(IEnumerable<Models.PopularOnMovieMirchiEntity> popularOnMovieMirchis)
        {
            var popularOnMoiveMirchiTable = TableStore.Instance.GetTable(TableStore.PopularOnMovieMirchiName);
            Debug.Assert(popularOnMoiveMirchiTable != null);

            var movieList = new List<DataStoreLib.Models.TableEntity>(popularOnMovieMirchis).ConvertAll(x => (ITableEntity)x);
            var returnOp = popularOnMoiveMirchiTable.UpdateItemsById(movieList);

            var returnTranslateOp = new Dictionary<PopularOnMovieMirchiEntity, bool>();
            foreach (var b in returnOp.Keys)
            {
                returnTranslateOp.Add(b as PopularOnMovieMirchiEntity, returnOp[b]);
            }
            return returnTranslateOp;
        }
        #endregion

        #region Twitter

        /// <summary>
        /// TODO: Please add comments here
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public bool IsTweetExist(IEnumerable<string> ids)
        {
            var twitterTable = TableStore.Instance.GetTable(TableStore.TwitterTableName);
            var retList = twitterTable.GetItemsById<TwitterEntity>(ids);
            return
                retList
                    .Any(tweet => tweet.Value != null);

            //// TODO: Given that this method has no comments and poor code - it is ambigous as of now.
            //// I have no idea what are the intentions here - so revisit my code when you clean this up

            ////TODO: Clean comments
            ////foreach (var item in retList)
            ////{
            ////    return (item.Value != null);
            ////}

            ////return false;
        }

        public IDictionary<string, TwitterEntity> GetTweetById(string id)
        {
            var tweetTable = TableStore.Instance.GetTable(TableStore.TwitterTableName) as TwitterTable;
            return tweetTable.GetItemsByTwitterId<TwitterEntity>(id);
        }

        public IDictionary<TwitterEntity, bool> UpdateTweetById(IEnumerable<TwitterEntity> tweets)
        {
            var twitterTable = TableStore.Instance.GetTable(TableStore.TwitterTableName);
            Debug.Assert(twitterTable != null);

            var updateResult = twitterTable.UpdateItemsById(tweets.Cast<ITableEntity>());
            return
                updateResult
                    .ToDictionary(
                        pair => pair.Key as TwitterEntity,
                        pair => pair.Value);

            ////TODO: Clean comments
            ////var tweetList = new List<DataStoreLib.Models.TableEntity>(tweets).ConvertAll(x => (ITableEntity)x);
            ////var returnOp = twitterTable.UpdateItemsById(tweetList);

            ////var returnTranslateOp = new Dictionary<TwitterEntity, bool>();
            ////foreach (var b in returnOp.Keys)
            ////{
            ////    returnTranslateOp.Add(b as TwitterEntity, returnOp[b]);
            ////}

            ////return returnTranslateOp;
        }

        // Need method which returns the tweets based on keywords/movie name/actor name etc.
        // Need method which returns the count of total tweets - The total count will be used in case we need pagination
        public IEnumerable<TwitterEntity> GetRecentTweets(int startIndex = 0, int pageSize = 20)
        {
            IEnumerable<TwitterEntity> tweets;

            if (!CacheManager.TryGet<IEnumerable<TwitterEntity>>(CacheConstants.TwitterEntities, out tweets))
            {
                var twitterTable = TableStore.Instance.GetTable(TableStore.TwitterTableName);
                var allTweets = twitterTable.GetAllItems<TwitterEntity>();
                // TODO: Uncomment the Where once we have the system end-to-end hooked up
                var activeTweets = allTweets.Values.OrderByDescending(t => t.Created_At); // Sort by Created date

                tweets = (startIndex >= 0 && pageSize > 0) ? activeTweets.Skip(startIndex).Take(pageSize) // Skip first x tweets, and then take next y tweets
                                                           : activeTweets;


                CacheManager.Add<IEnumerable<TwitterEntity>>(CacheConstants.TwitterEntities, tweets);
            }

            return tweets;
        }

        public IEnumerable<TwitterEntity> GetRecentTweets(string tweetType, string name, int startIndex = 0, int pageSize = 20)
        {
            IEnumerable<TwitterEntity> tweets;

            // Home page loads the tweets in cache (20). When we access artists/movies page, tweeets are already populated
            // Hence it was never accessing the following code block.
            if (!CacheManager.TryGet<IEnumerable<TwitterEntity>>(CacheConstants.TwitterEntities + "_" + name.ToLower(), out tweets))
            {
                var twitterTable = TableStore.Instance.GetTable(TableStore.TwitterTableName);
                var allTweets = twitterTable.GetAllItems<TwitterEntity>();
                IEnumerable<TwitterEntity> sortedTweets;
                // TODO: Uncomment the Where once we have the system end-to-end hooked up
                var activeTweets = allTweets; //.Where(t => t.Value.Status == "1"); // Pick only active tweets
                if (tweetType == "movie")
                {
                    sortedTweets = allTweets.Values.Where(t => t.TweetType == tweetType && t.MovieName == name).OrderByDescending(t => t.Created_At);
                }
                else
                {
                    sortedTweets = activeTweets.Values.Where(t => t.TweetType == tweetType && t.ArtistName == name).OrderByDescending(t => t.Created_At);
                }

                tweets =
                    (startIndex > 0 && pageSize > 0) ?
                        sortedTweets.Skip(startIndex).Take(pageSize) // Skip first x tweets, and then take next y tweets
                        : sortedTweets;


                CacheManager.Add<IEnumerable<TwitterEntity>>(CacheConstants.TwitterEntities + "_" + name.ToLower(), tweets);
            }

            return tweets;
        }

        public bool DeleteTwitterItemById(List<string> twitterIds)
        {
            try
            {
                var twitterTable = TableStore.Instance.GetTable(TableStore.TwitterTableName) as TwitterTable;
                twitterTable.DeleteTwitterById(twitterIds);

                return true;
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
                throw;
            }
        }
        #endregion

        #region News
        public IDictionary<string, NewsEntity> GetRecentNews(int startIndex = 0, int pageSize = 20)
        {
            var newsTable = TableStore.Instance.GetTable(TableStore.NewsTableName);
            var allNews = newsTable.GetAllItems<NewsEntity>();
            // TODO: Uncomment the Where once we have the system end-to-end hooked up
            var activeNews = allNews; //.Where(t => t.Value.Status == "1"); // Pick only active news
            var sortedNews = activeNews.OrderByDescending(t => t.Value.PublishDate); // Sort by PublishDate
            var paginatedNews =
                (startIndex > 0 && pageSize > 0) ?
                    sortedNews.Skip(startIndex).Take(pageSize) // Skip first x news, and then take next y news
                    : sortedNews;
            var result = paginatedNews.ToDictionary(t => t.Key, t => t.Value);

            return result;
        }

        public IDictionary<string, NewsEntity> GetNewsItems()
        {
            var newsTable = TableStore.Instance.GetTable(TableStore.NewsTableName) as NewsTable;
            //return newsTable.GetItemsByTwitterId<NewsEntity>();

            ////TODO: Fix this
            return null;
        }

        public IDictionary<NewsEntity, bool> UpdateNewsItemById(IEnumerable<NewsEntity> news)
        {
            var newsTable = TableStore.Instance.GetTable(TableStore.NewsTableName);
            ////Debug.Assert(newsTable != null);

            var updateResult = newsTable.UpdateItemsById(news.Cast<ITableEntity>());
            return
                updateResult
                    .ToDictionary(
                        pair => pair.Key as NewsEntity,
                        pair => pair.Value);

            ////TODO: Clean comments
            ////var newsList = new List<DataStoreLib.Models.TableEntity>(news).ConvertAll(x => (ITableEntity)x);
            ////var returnOp = newsTable.UpdateItemsById(newsList);
            ////var returnTranslateOp = new Dictionary<NewsEntity, bool>();
            ////foreach (var b in returnOp.Keys)
            ////{
            ////    returnTranslateOp.Add(b as NewsEntity, returnOp[b]);
            ////}

            ////return returnTranslateOp;
        }

        public bool DeleteNewsItemById(List<string> newsIds)
        {
            try
            {
                var newsTable = TableStore.Instance.GetTable(TableStore.NewsTableName) as NewsTable;
                newsTable.DeleteNewsById(newsIds);

                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public IDictionary<string, NewsEntity> GetNewsById(IEnumerable<string> ids)
        {
            var newsTable = TableStore.Instance.GetTable(TableStore.NewsTableName);
            return newsTable.GetItemsById<NewsEntity>(ids);
        }
        #endregion

        #region Artists
        public IDictionary<ArtistEntity, bool> UpdateArtistItemById(IEnumerable<ArtistEntity> artist)
        {
            var artistTable = TableStore.Instance.GetTable(TableStore.ArtistTableName);

            var updateResult = artistTable.UpdateItemsById(artist.Cast<ITableEntity>());
            return
                updateResult
                    .ToDictionary(
                        pair => pair.Key as ArtistEntity,
                        pair => pair.Value);
        }

        public ArtistEntity GetArtist(string artistName)
        {
            try
            {
                ArtistEntity artist;

                if (!CacheManager.TryGet<ArtistEntity>(CacheConstants.ArtistEntity + artistName.ToLower().Trim(), out artist))
                {
                    var artistTable = TableStore.Instance.GetTable(TableStore.ArtistTableName) as ArtistTable;
                    var allArtists = artistTable.GetAllItems<ArtistEntity>();
                    artist = allArtists.Values.SingleOrDefault(a => a.ArtistName.ToLower().Trim() == artistName.ToLower().Trim());
                    CacheManager.Add<ArtistEntity>(CacheConstants.ArtistEntity + artistName.ToLower().Trim(), artist);
                }

                return artist;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public IEnumerable<ArtistEntity> GetAllArtist(string artistName)
        {
            try
            {
                var artistTable = TableStore.Instance.GetTable(TableStore.ArtistTableName) as ArtistTable;
                var allArtists = artistTable.GetAllItems<ArtistEntity>();

                //Return only those artists who have associated posters.
                //Associated poster means those artists are popular. Hence they have associated posters
                if (string.IsNullOrEmpty(artistName))
                    return allArtists.Values.Where(a => a.Posters.Length > 2);
                else
                    //return allArtists.Values.Where(a => a.ArtistName.ToLower().Trim() == artistName && a.Posters.Length > 2);
                    return allArtists.Values.Where(a => a.ArtistName.ToLower().Trim().Contains(artistName.ToLower()) && a.Posters.Length > 2);
            }
            catch (Exception)
            {
                return null;
            }
        }

        #endregion
    }
}
