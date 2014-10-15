
namespace DataStoreLib.Storage
{
    using DataStoreLib.Models;
    using Microsoft.WindowsAzure.Storage.Table;
    using System.Collections.Generic;
    using System.Diagnostics;

    internal class ReviewerTable : Table
    {
        protected ReviewerTable(CloudTable table)
            : base(table)
        {
        }

        internal static Table CreateTable(CloudTable table)
        {
            return new ReviewerTable(table);
        }

        protected override string GetParitionKey()
        {
            return ReviewerEntity.PARTITION_KEY;
        }

        public virtual IDictionary<string, TEntity> GetAllReviewers<TEntity>() where TEntity : DataStoreLib.Models.TableStorageEntity
        {
            Debug.Assert(_table != null);

            var operationList = new Dictionary<string, TableResult>();

            TableQuery<ReviewerEntity> query = new TableQuery<ReviewerEntity>().Where(TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, "CloudMovie"));

            // execute query
            IEnumerable<ReviewerEntity> movieResults = _table.ExecuteQuery<ReviewerEntity>(query);

            var returnDict = new Dictionary<string, TEntity>();
            int iter = 0;

            foreach (var tableResult in movieResults)
            {
                TEntity entity = null;

                entity = tableResult as TEntity;

                returnDict.Add(tableResult.ReviewerId, entity);
                iter++;
            }

            return returnDict;
        }
    }
}
