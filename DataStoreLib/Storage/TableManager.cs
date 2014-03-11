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

        public IDictionary<string, Models.ReviewEntity> GetReviewsById(List<string> ids)
        {
            var reviewTable = TableStore.Instance.GetTable(TableStore.ReviewTableName);
            return reviewTable.GetItemsById<ReviewEntity>(ids);
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
    }
}
