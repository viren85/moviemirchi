
namespace DataStoreLib.Storage
{
    using DataStoreLib.Models;
    using Microsoft.WindowsAzure.Storage.Table;
    using System;
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

        public bool DeleteTwitterById(List<string> twitterIds)
        {
            try
            {
                var twitterEntities = GetItemsById<TwitterEntity>(twitterIds, TwitterEntity.PARTITION_KEY);

                foreach (var twitterEntity in twitterEntities)
                {
                    if (twitterEntity.Value != null)
                    {
                        try
                        {
                            var tableOperation = TableOperation.Delete(twitterEntity.Value);
                            _table.Execute(tableOperation);
                        }
                        catch (Exception)
                        {
                            Trace.TraceError("Couldn't delete entity id {0}", twitterEntity.Value.TwitterId);
                        }
                    }
                }

                return true;
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }
    }
}
