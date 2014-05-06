
namespace DataStoreLib.Storage
{
    using DataStoreLib.Models;
    using Microsoft.WindowsAzure.Storage.Table;
    using System.Collections.Generic;
    using System.Diagnostics;

    internal class TwitterTable : Table
    {
        protected TwitterTable(CloudTable table)
            : base(table)
        {
        }

        internal static Table CreateTable(CloudTable table)
        {
            return new TwitterTable(table);
        }

        protected override string GetParitionKey()
        {
            return TwitterEntity.PARTITION_KEY;
        }

        public IDictionary<string, TEntity> GetItemsByTwitterId<TEntity>(string twitterId) where TEntity : DataStoreLib.Models.TableEntity
        {
            Debug.Assert(_table != null);

            var operationList = new Dictionary<string, TableResult>();

            TableQuery<TwitterEntity> query = new TableQuery<TwitterEntity>().Where(TableQuery.GenerateFilterCondition("TwitterId", QueryComparisons.Equal, twitterId));

            IEnumerable<TwitterEntity> reviewResults = _table.ExecuteQuery<TwitterEntity>(query);

            var returnDict = new Dictionary<string, TEntity>();
            int iter = 0;

            foreach (var tableResult in reviewResults)
            {
                TEntity entity = null;

                entity = tableResult as TEntity;

                returnDict.Add(tableResult.TwitterId, entity);
                iter++;
            }

            return returnDict;
        }


        public IDictionary<string, TEntity> GetReviewByMovieAndReviewId<TEntity>(string twitterId) where TEntity : DataStoreLib.Models.TableEntity
        {
            var operationList = new Dictionary<string, TableResult>();

            TableQuery<TwitterEntity> query = new TableQuery<TwitterEntity>().Where(
                                                                TableQuery.GenerateFilterCondition("TwitterId", QueryComparisons.Equal, twitterId)
                                                                );
            IEnumerable<TwitterEntity> updateReviewResult = _table.ExecuteQuery<TwitterEntity>(query);
            var returnDict = new Dictionary<string, TEntity>();
            int iter = 0;
            foreach (var tableResult in updateReviewResult)
            {

                TEntity entity = null;

                entity = tableResult as TEntity;

                returnDict.Add(tableResult.TwitterId, entity);
                iter++;
            }

            return returnDict;
        }
    }
}
