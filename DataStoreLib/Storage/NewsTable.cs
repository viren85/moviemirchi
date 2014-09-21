
namespace DataStoreLib.Storage
{
    using DataStoreLib.Models;
    using Microsoft.WindowsAzure.Storage.Table;
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;

    internal class NewsTable : Table
    {
        protected NewsTable(CloudTable table)
            : base(table)
        {
        }

        internal static Table CreateTable(CloudTable table)
        {
            return new NewsTable(table);
        }

        protected override string GetParitionKey()
        {
            return NewsEntity.PARTITION_KEY;
        }


        public bool DeleteNewsById(List<string> newsId)
        {
            try
            {
                var newEntity = GetItemsById<NewsEntity>(newsId, NewsEntity.PARTITION_KEY);

                foreach (var toBeIndexedEntity in newEntity)
                {
                    if (toBeIndexedEntity.Value != null)
                    {
                        try
                        {
                            var tableOperation = TableOperation.Delete(toBeIndexedEntity.Value);
                            _table.Execute(tableOperation);
                        }
                        catch (Exception)
                        {
                            Trace.TraceError("Couldn't delete entity id {0}", toBeIndexedEntity.Value.NewsId);
                        }
                    }
                }

                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
