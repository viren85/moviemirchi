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
       public IDictionary<string, MovieEntity> GetMoviesByid(List<string> ids)
        {
            var movieTable = TableStore.Instance.GetTable(TableStore.MovieTableName);
            return movieTable.GetItemsById<MovieEntity>(ids);
        }

       public IDictionary<string, MovieEntity> GetMoviesByUniqueName(string name)
       {
           var movieTable = TableStore.Instance.GetTable(TableStore.MovieTableName) as MovieTable;
           return movieTable.GetItemsByName<MovieEntity>(name);
       }

        public IDictionary<string, ReviewEntity> GetReviewsById(List<string> ids)
        {
            var reviewTable = TableStore.Instance.GetTable(TableStore.ReviewTableName);
            return reviewTable.GetItemsById<ReviewEntity>(ids);
        }

        public IDictionary<string, Models.ReviewerEntity> GetReviewersById(List<string> ids)
        {
            var reviewerTable = TableStore.Instance.GetTable(TableStore.ReviewerTableName);
            return reviewerTable.GetItemsById<ReviewerEntity>(ids);
        }
        public IDictionary<MovieEntity, bool> UpdateMoviesById(List<Models.MovieEntity> movies)
        {
            var movieTable = TableStore.Instance.GetTable(TableStore.MovieTableName);
            Debug.Assert(movieTable != null);

            var movieList = new List<DataStoreLib.Models.TableEntity>(movies).ConvertAll(x => (ITableEntity) x);
            var returnOp = movieTable.UpdateItemsById(movieList);

            var returnTranslateOp = new Dictionary<MovieEntity, bool>();
            foreach (var b in returnOp.Keys)
            {
                returnTranslateOp.Add(b as MovieEntity, returnOp[b]);
            }
            return returnTranslateOp;
        }

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

        /* Added a method for getting all movies*/
        public IDictionary<string, MovieEntity> GetAllMovies()
        {
            var movieTable = TableStore.Instance.GetTable(TableStore.MovieTableName);
            return movieTable.GetAllItems<MovieEntity>();
        }

        public IDictionary<string, Models.ReviewEntity> GetReviewsByMovieId(string movieId)
        {
            var reviewTable = TableStore.Instance.GetTable(TableStore.ReviewTableName);
            return reviewTable.GetItemsByMovieId<ReviewEntity>(movieId);
        }

        public IDictionary<string, ReviewEntity> GetReviewsByReviewer(string reviewerName)
        {
            var reviewTable = TableStore.Instance.GetTable(TableStore.ReviewTableName);
            return reviewTable.GetItemsByReivewer<ReviewEntity>(reviewerName);
        }
        /*end*/

        public IDictionary<string, UserEntity> GetUsersByName(string userName)
        {
            var loginTable = TableStore.Instance.GetTable(TableStore.UserTableName) as UserTable;
            return loginTable.GetItemsByUserName<UserEntity>(userName);
        }
        /*end*/

        public IDictionary<string, UserEntity> GetUsersById(List<string> userIds)
        {
            var userTable = TableStore.Instance.GetTable(TableStore.UserTableName);
            return userTable.GetItemsById<UserEntity>(userIds);
        }
        public IDictionary<string, UserEntity> GetAllUser()
        {
            var loginTable = TableStore.Instance.GetTable(TableStore.UserTableName);
            return loginTable.GetAllItems<UserEntity>();
        }

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

       public IDictionary<string, AffilationEntity> GetAllAffilation()
        {
            var affilationTable = TableStore.Instance.GetTable(TableStore.AffilationTableName);
            return affilationTable.GetAllAffilationItems<AffilationEntity>();
        }

       public IDictionary<string, AffilationEntity> GetAffilationsByid(List<string> ids)
       {
           var affilationTable = TableStore.Instance.GetTable(TableStore.AffilationTableName);
           return affilationTable.GetItemsById<AffilationEntity>(ids);
       }

       public IDictionary<string, ReviewerEntity> GetReviewerById(List<string> reviewerIds)
       {
           var reviewerTable = TableStore.Instance.GetTable(TableStore.ReviewerTableName);
           return reviewerTable.GetItemsById<ReviewerEntity>(reviewerIds);
       }




       public IDictionary<string, ReviewerEntity> GetAllReviewer()
       {

           var reviewTable = TableStore.Instance.GetTable(TableStore.ReviewerTableName) as ReviewerTable;
           return reviewTable.GetAllReviewers<ReviewerEntity>();

           //var reviewList = TableStore.Instance.GetTable(TableStore.ReviewTableName) as ReviewTable; ;
           //return reviewList.GetAllReviewItems<ReviewEntity>();
       }

       public IDictionary<string, ReviewEntity> UpdateReviewesByReviewerId(List<string> ids)
       {
          // var reviewTable = TableStore.Instance.GetTable(TableStore.ReviewTableName);
        var reviewerTable = TableStore.Instance.GetTable(TableStore.ReviewerTableName);
        return reviewerTable.GetItemsById<ReviewEntity>(ids);

       }



   
















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

       
    }
}
