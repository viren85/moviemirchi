
namespace DataStoreLib.Storage
{
    using DataStoreLib.Models;
    using Microsoft.WindowsAzure.Storage.Table;
    using System.Collections.Generic;
    using System.Diagnostics;
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
            return movieTable.GetAllItems<MovieEntity>();
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
            var reviewTable = TableStore.Instance.GetTable(TableStore.ReviewTableName);
            return reviewTable.GetItemsByReviewer<ReviewEntity>(reviewerName);
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
        public IDictionary<string, TwitterEntity> GetRecentTweets(int startIndex = 0, int pageSize = 20)
        {
            var twitterTable = TableStore.Instance.GetTable(TableStore.TwitterTableName);
            var allTweets = twitterTable.GetAllItems<TwitterEntity>();
            // TODO: Uncomment the Where once we have the system end-to-end hooked up
            var activeTweets = allTweets; //.Where(t => t.Value.Status == "1"); // Pick only active tweets
            var sortedTweets = activeTweets.OrderByDescending(t => t.Value.Created_At); // Sort by Created date
            var paginatedTweets =
                (startIndex > 0 && pageSize > 0) ?
                    sortedTweets.Skip(startIndex).Take(pageSize) // Skip first x tweets, and then take next y tweets
                    : sortedTweets;
            var result = paginatedTweets.ToDictionary(t => t.Key, t => t.Value);

            return result;
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
        #endregion
    }
}
