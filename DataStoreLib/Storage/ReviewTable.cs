
namespace DataStoreLib.Storage
{
    using DataStoreLib.Models;
    using Microsoft.WindowsAzure.Storage.Table;
    using System.Collections.Generic;
    using System.Diagnostics;

    internal class ReviewTable : Table
    {
        protected ReviewTable(CloudTable table)
            : base(table)
        {
        }

        internal static Table CreateTable(CloudTable table)
        {
            return new ReviewTable(table);
        }

        protected override string GetParitionKey()
        {
            return ReviewEntity.PARTITION_KEY;
        }

        public IDictionary<string, TEntity> GetItemsByReviewerAndMovieId<TEntity>(string reviewerId) where TEntity : DataStoreLib.Models.TableStorageEntity
        {
            Debug.Assert(_table != null);

            var operationList = new Dictionary<string, TableResult>();

            TableQuery<ReviewEntity> query = new TableQuery<ReviewEntity>().Where(TableQuery.GenerateFilterCondition("ReviewerId", QueryComparisons.Equal, reviewerId));

            IEnumerable<ReviewEntity> reviewResults = _table.ExecuteQuery<ReviewEntity>(query);

            var returnDict = new Dictionary<string, TEntity>();
            int iter = 0;

            foreach (var tableResult in reviewResults)
            {
                TEntity entity = null;

                entity = tableResult as TEntity;

                returnDict.Add(tableResult.ReviewerId, entity);
                iter++;
            }

            return returnDict;
        }


        public IDictionary<string, TEntity> GetReviewByMovieAndReviewId<TEntity>(string reviewerId, string movieId) where TEntity : DataStoreLib.Models.TableStorageEntity
        {
            var operationList = new Dictionary<string, TableResult>();

            TableQuery<ReviewEntity> query = new TableQuery<ReviewEntity>().Where(TableQuery.CombineFilters(
                                                                TableQuery.GenerateFilterCondition("ReviewerId", QueryComparisons.Equal, reviewerId),
                                                                TableOperators.And,
                                                                TableQuery.GenerateFilterCondition("MovieId", QueryComparisons.Equal, movieId)));
            IEnumerable<ReviewEntity> updateReviewResult = _table.ExecuteQuery<ReviewEntity>(query);
            var returnDict = new Dictionary<string, TEntity>();
            int iter = 0;
            foreach (var tableResult in updateReviewResult)
            {

                TEntity entity = null;

                entity = tableResult as TEntity;

                returnDict.Add(tableResult.ReviewId, entity);
                iter++;
            }

            return returnDict;
        }
    }
}
