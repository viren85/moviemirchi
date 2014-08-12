
namespace DataStoreLib.Storage
{
    using DataStoreLib.Models;
    using Microsoft.WindowsAzure.Storage.Table;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Net;

    public abstract class Table
    {
        protected CloudTable _table;

        protected Table(CloudTable table)
        {
            this._table = table;
        }

        public virtual IDictionary<string, TEntity> GetItemsById<TEntity>(IEnumerable<string> idSource, string partitionKey = "") where TEntity : DataStoreLib.Models.TableEntity
        {
            var idSourceList = idSource.ToList();
            Debug.Assert(idSourceList.Count != 0);
            Debug.Assert(this._table != null);

            if (string.IsNullOrWhiteSpace(partitionKey))
            {
                partitionKey = GetParitionKey();
            }

            var operationList =
                idSourceList
                    .ToDictionary<string, string, TableResult>(
                        (id) => id,
                        (id) => this._table.Execute(TableOperation.Retrieve<TEntity>(partitionKey, id))
                    );

            return
                operationList
                    .ToDictionary<KeyValuePair<string, TableResult>, string, TEntity>(
                        (pair) => pair.Key,
                        (pair) =>
                        {
                            if (pair.Value.HttpStatusCode != (int)HttpStatusCode.OK)
                            {
                                Trace.TraceWarning("Couldn't retrieve info for id {0}", pair.Key);
                            }

                            return pair.Value.Result as TEntity;
                        }
                    );
        }

        public virtual IDictionary<ITableEntity, bool> UpdateItemsById(IEnumerable<ITableEntity> items, string partitionKey = "")
        {
            var returnDict = new Dictionary<ITableEntity, bool>();

            if (string.IsNullOrWhiteSpace(partitionKey))
            {
                partitionKey = GetParitionKey();
            }

            var batchOp = new TableBatchOperation();
            foreach (var item in items)
            {
                //batchOp.Insert(item);
                //Replace if entity exists otherwise add new entity.
                batchOp.InsertOrReplace(item);
            }

            var tableResult = this._table.ExecuteBatch(batchOp);

            foreach (var result in tableResult)
            {
                Debug.Assert((result.Result as ITableEntity) != null);
                if (result.HttpStatusCode >= 200 || result.HttpStatusCode < 300)
                {
                    returnDict[result.Result as ITableEntity] = true;
                }
                else
                {
                    returnDict[result.Result as ITableEntity] = false;
                }
            }

            return returnDict;
        }

        public virtual IDictionary<string, TEntity> GetAllItems<TEntity>() where TEntity : DataStoreLib.Models.TableEntity, new()
        {
            Debug.Assert(this._table != null);

            var query = new TableQuery<TEntity>()
                .Where(TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, "CloudMovie"));

            IEnumerable<TEntity> tableResults = this._table.ExecuteQuery(query);

            return
                tableResults
                    .ToDictionary<TEntity, string, TEntity>(
                        (tableResult) => tableResult.GetKey(),
                        (tableResult) => tableResult as TEntity
                    );
        }

        public virtual IDictionary<string, TEntity> GetItemsByMovieId<TEntity>(string movieId) where TEntity : DataStoreLib.Models.TableEntity
        {
            Debug.Assert(this._table != null);

            var operationList = new Dictionary<string, TableResult>();

            TableQuery<ReviewEntity> query = new TableQuery<ReviewEntity>().Where(TableQuery.GenerateFilterCondition("MovieId", QueryComparisons.Equal, movieId));
            //TableQuery<TEntity> query = new TableQuery<TEntity>().Where(TableQuery.GenerateFilterCondition("MovieId", QueryComparisons.Equal, movieId));

            // execute query
            //IEnumerable<TEntity> reviewResults = this._table.ExecuteQuery<TEntity>(query);
            IEnumerable<ReviewEntity> reviewResults = this._table.ExecuteQuery<ReviewEntity>(query);

            var returnDict = new Dictionary<string, TEntity>();
            int iter = 0;

            foreach (var tableResult in reviewResults)
            {
                try
                {
                    TEntity entity = null;

                    entity = tableResult as TEntity;

                    returnDict.Add(tableResult.ReviewId, entity);
                    iter++;
                }
                catch (System.Exception)
                {
                    // continue the iterator - the exception is occurred most probably due to missing data
                }
            }

            return returnDict;
        }

        public virtual IDictionary<string, TEntity> GetItemsByReviewer<TEntity>(string reviewerName) where TEntity : DataStoreLib.Models.TableEntity
        {
            Debug.Assert(this._table != null);
            var operationList = new Dictionary<string, TableResult>();

            TableQuery<ReviewEntity> query = new TableQuery<ReviewEntity>().Where(TableQuery.GenerateFilterCondition("ReviewerName", QueryComparisons.Equal, reviewerName));

            IEnumerable<ReviewEntity> reviewResults = this._table.ExecuteQuery<ReviewEntity>(query);

            var returnDict = new Dictionary<string, TEntity>();
            int iter = 0;

            foreach (var tableResult in reviewResults)
            {
                TEntity entity = null;

                entity = tableResult as TEntity;

                returnDict.Add(tableResult.ReviewId, entity);
                iter++;
            }

            return returnDict;
        }

        public virtual IDictionary<string, TEntity> GetAllAffilationItems<TEntity>() where TEntity : DataStoreLib.Models.TableEntity
        {
            Debug.Assert(this._table != null);

            var operationList = new Dictionary<string, TableResult>();

            TableQuery<AffilationEntity> query = new TableQuery<AffilationEntity>().Where(TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, "CloudMovie"));

            // execute query
            IEnumerable<AffilationEntity> results = this._table.ExecuteQuery<AffilationEntity>(query);

            var returnDict = new Dictionary<string, TEntity>();
            int iter = 0;

            foreach (var tableResult in results)
            {
                TEntity entity = null;

                entity = tableResult as TEntity;

                returnDict.Add(tableResult.AffilationId, entity);
                iter++;
            }

            return returnDict;
        }

        public virtual IDictionary<string, TEntity> GetAllReviewItems<TEntity>() where TEntity : DataStoreLib.Models.TableEntity
        {
            Debug.Assert(this._table != null);

            var operationList = new Dictionary<string, TableResult>();

            TableQuery<ReviewEntity> query = new TableQuery<ReviewEntity>().Where(TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, "CloudMovie"));

            // execute query
            IEnumerable<ReviewEntity> results = this._table.ExecuteQuery<ReviewEntity>(query);

            var returnDict = new Dictionary<string, TEntity>();
            int iter = 0;

            foreach (var tableResult in results)
            {
                TEntity entity = null;

                entity = tableResult as TEntity;

                returnDict.Add(tableResult.ReviewId, entity);
                iter++;
            }

            return returnDict;
        }

        protected abstract string GetParitionKey();

        internal virtual ReviewEntity GetReviewById(string reviewId)
        {
            Debug.Assert(this._table != null);

            TableQuery<ReviewEntity> query = new TableQuery<ReviewEntity>().Where(TableQuery.GenerateFilterCondition("ReviewId", QueryComparisons.Equal, reviewId));

            IEnumerable<ReviewEntity> reviewResults = this._table.ExecuteQuery<ReviewEntity>(query);
            return reviewResults.FirstOrDefault();
        }


        internal virtual bool UpdateReviewRating(string reviewId, string rating)
        {
            //var reviews = this.GetItemsById<ReviewEntity>(new string[] { reviewId });
            //var reviewPair = reviews.FirstOrDefault();
            ////Debug.Assert(reviewPair != default(KeyValuePair<string, ReviewEntity>);
            //var review = reviewPair.Value;
            var review = this.GetReviewById(reviewId);
            Debug.Assert(review != null);

            review.MyScore = rating;

            var batchOp = new TableBatchOperation();
            batchOp.InsertOrReplace(review);

            var tableResult = this._table.ExecuteBatch(batchOp);
            var result = tableResult.FirstOrDefault();

            Debug.Assert(result != null);
            Debug.Assert((result.Result as ITableEntity) != null);
            return (result.HttpStatusCode >= 200 || result.HttpStatusCode < 300);
        }
    }

}
