using DataStoreLib.Models;
using DataStoreLib.Utils;
using Microsoft.WindowsAzure.Storage;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage.Table;

namespace DataStoreLib.Storage
{
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
        public IDictionary<string, UserEntity> GetUsersById(List<string> userIds)
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
        public IDictionary<UserEntity, bool> UpdateUsersById(List<Models.UserEntity> users)
        {
            var userTable = TableStore.Instance.GetTable(TableStore.UserTableName);
            Debug.Assert(userTable != null);

            var userList = new List<DataStoreLib.Models.TableEntity>(users).ConvertAll(x => (ITableEntity)x);
            var returnOp = userTable.UpdateItemsById(userList);

            var returnTranslateOp = new Dictionary<UserEntity, bool>();
            foreach (var b in returnOp.Keys)
            {
                returnTranslateOp.Add(b as UserEntity, returnOp[b]);
            }
            return returnTranslateOp;

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
        public IDictionary<string, MovieEntity> GetMoviesByid(List<string> ids)
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
        public IDictionary<MovieEntity, bool> UpdateMoviesById(List<Models.MovieEntity> movies)
        {
            var movieTable = TableStore.Instance.GetTable(TableStore.MovieTableName);
            Debug.Assert(movieTable != null);

            var movieList = new List<DataStoreLib.Models.TableEntity>(movies).ConvertAll(x => (ITableEntity)x);
            var returnOp = movieTable.UpdateItemsById(movieList);

            var returnTranslateOp = new Dictionary<MovieEntity, bool>();
            foreach (var b in returnOp.Keys)
            {
                returnTranslateOp.Add(b as MovieEntity, returnOp[b]);
            }
            return returnTranslateOp;
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
        public IDictionary<string, ReviewEntity> GetReviewsById(List<string> ids)
        {
            var reviewTable = TableStore.Instance.GetTable(TableStore.ReviewTableName);
            return reviewTable.GetItemsById<ReviewEntity>(ids);
        }
        /// <summary>
        /// Update the reviews by id
        /// </summary>
        /// <param name="reviews"></param>
        /// <returns></returns>
        public IDictionary<ReviewEntity, bool> UpdateReviewsById(List<Models.ReviewEntity> reviews)
        {
            var reviewTable = TableStore.Instance.GetTable(TableStore.ReviewTableName);
            Debug.Assert(reviewTable != null);

            var reviewList = new List<DataStoreLib.Models.TableEntity>(reviews).ConvertAll(x => (ITableEntity)x);
            var returnOp = reviewTable.UpdateItemsById(reviewList);

            var returnTranslateOp = new Dictionary<ReviewEntity, bool>();
            foreach (var b in returnOp.Keys)
            {
                returnTranslateOp.Add(b as ReviewEntity, returnOp[b]);
            }
            return returnTranslateOp;
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
            return reviewTable.GetItemsByReivewer<ReviewEntity>(reviewerName);
        }
        /// <summary>
        /// Return the list of reviewes bu reviewerId
        /// </summary>
        /// <param name="review"></param>
        /// <returns></returns>
        public IDictionary<ReviewEntity, bool> UpdateReviewesByReviewerId(List<ReviewEntity> review)
        {
            var reviewTable = TableStore.Instance.GetTable(TableStore.ReviewTableName);
            Debug.Assert(reviewTable != null);

            var reviewerList = new List<DataStoreLib.Models.TableEntity>(review).ConvertAll(x => (ITableEntity)x);
            var returnOp = reviewTable.UpdateItemsById(reviewerList);

            var returnTranslateOp = new Dictionary<ReviewEntity, bool>();
            foreach (var b in returnOp.Keys)
            {
                returnTranslateOp.Add(b as ReviewEntity, returnOp[b]);
            }
            return returnTranslateOp;
        }

        /// <summary>
        /// Update Reviewes by reviewerId
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public IDictionary<string, ReviewEntity> UpdateReviewesByReviewerId(List<string> ids)
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
        public IDictionary<string, ReviewerEntity> GetReviewerById(List<string> reviewerIds)
        {
            var reviewerTable = TableStore.Instance.GetTable(TableStore.ReviewerTableName);
            return reviewerTable.GetItemsById<ReviewerEntity>(reviewerIds);
        }
        /// <summary>
        /// Return the List of reviewers by its id
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public IDictionary<string, Models.ReviewerEntity> GetReviewersById(List<string> ids)
        {
            var reviewerTable = TableStore.Instance.GetTable(TableStore.ReviewerTableName);
            return reviewerTable.GetItemsById<ReviewerEntity>(ids);
        }
        /// <summary>
        /// Return the list of reviewers to update 
        /// </summary>
        /// <param name="reviewers"></param>
        /// <returns></returns>
        public IDictionary<ReviewerEntity, bool> UpdateReviewers(List<ReviewerEntity> reviewers)
        {
            var reviewerTable = TableStore.Instance.GetTable(TableStore.ReviewerTableName);
            Debug.Assert(reviewerTable != null);

            var reviewerList = new List<DataStoreLib.Models.TableEntity>(reviewers).ConvertAll(x => (ITableEntity)x);
            var returnOp = reviewerTable.UpdateItemsById(reviewerList);

            var returnTranslateOp = new Dictionary<ReviewerEntity, bool>();
            foreach (var b in returnOp.Keys)
            {
                returnTranslateOp.Add(b as ReviewerEntity, returnOp[b]);
            }
            return returnTranslateOp;
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
        public IDictionary<string, AffilationEntity> GetAffilationsByid(List<string> ids)
        {
            var affilationTable = TableStore.Instance.GetTable(TableStore.AffilationTableName);
            return affilationTable.GetItemsById<AffilationEntity>(ids);
        }
        /// <summary>
        /// Update the affiliation by id
        /// </summary>
        /// <param name="affilations"></param>
        /// <returns></returns>
        public IDictionary<AffilationEntity, bool> UpdateAffilationsById(List<AffilationEntity> affilations)
        {
            var affilationTable = TableStore.Instance.GetTable(TableStore.AffilationTableName);
            Debug.Assert(affilationTable != null);

            var afillationList = new List<DataStoreLib.Models.TableEntity>(affilations).ConvertAll(x => (ITableEntity)x);
            var returnOp = affilationTable.UpdateItemsById(afillationList);

            var returnTranslateOp = new Dictionary<AffilationEntity, bool>();
            foreach (var b in returnOp.Keys)
            {
                returnTranslateOp.Add(b as AffilationEntity, returnOp[b]);
            }
            return returnTranslateOp;
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
        public IDictionary<string, UserFavoriteEntity> GetUserFavoritesById(List<string> ids)
        {
            var userFavoritesTable = TableStore.Instance.GetTable(TableStore.UserFavoriteTableName);
            return userFavoritesTable.GetItemsById<UserFavoriteEntity>(ids);
        }

        public IDictionary<UserFavoriteEntity, bool> UpdateUserFavoritesById(List<Models.UserFavoriteEntity> userFavorites)
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
        public IDictionary<string, PopularOnMovieMirchiEntity> GetPopularOnMovieMirchisById(List<string> ids)
        {
            var popularOnMoiveMirchiTable = TableStore.Instance.GetTable(TableStore.PopularOnMovieMirchiName);
            return popularOnMoiveMirchiTable.GetItemsById<PopularOnMovieMirchiEntity>(ids);
        }

        public IDictionary<PopularOnMovieMirchiEntity, bool> UpdatePopularOnMovieMirchisById(List<Models.PopularOnMovieMirchiEntity> popularOnMovieMirchis)
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
    }
}
