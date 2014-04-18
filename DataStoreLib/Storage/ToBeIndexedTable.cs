using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataStoreLib.Models;
using Microsoft.WindowsAzure.Storage.Table;

namespace DataStoreLib.Storage
{
    public class ToBeIndexedTable : Table
    {
        public ToBeIndexedTable()
            : base(null)
        {
            
        }

        public ToBeIndexedTable(CloudTable table)
            : base(table)
        {
            
        }

        protected override string GetParitionKey()
        {
            throw new NotImplementedException();
        }

        internal static Table CreateTable(CloudTable table)
        {
            return new ToBeIndexedTable(table);
        }

        internal ISet<string> GetAllItemsFromParttion(string partitionKey)
        {
            var returnSet = new HashSet<string>();

            try
            {
                var tableQuery =
                    new TableQuery<ToBeIndexedEntity>().Where(TableQuery.GenerateFilterCondition("PartitionKey",
                                                                                                 QueryComparisons.Equal,
                                                                                                 partitionKey));
                var exec = _table.ExecuteQuery(tableQuery);

                foreach (var toBeIndexedEntity in exec)
                {
                    returnSet.Add(toBeIndexedEntity.EntityId);
                }

            }
            catch (Exception ex)
            {
                Trace.TraceError("Error in getting all entities for parition {0}, exception {1}", partitionKey, ex);
            }

            return returnSet;
        }

        public ISet<string> GetMoviesTobeIndexed()
        {
            return GetAllItemsFromParttion(ToBeIndexedEntity.MoviePartitionKey);
        }

        public ISet<string> GetReviewsToBeIndexed()
        {
            return GetAllItemsFromParttion(ToBeIndexedEntity.ReviewPartitionkey);
        }

        internal IDictionary<string, bool> RemoveItemFromTable(string paritionkey, List<string> ids)
        {
            var returnDict = new Dictionary<string, bool>();
            foreach (var id in ids)
            {
                returnDict[id] = false;
            }

            var items = GetItemsById<ToBeIndexedEntity>(ids, paritionkey);

            foreach (var toBeIndexedEntity in items)
            {
                if (toBeIndexedEntity.Value != null)
                {
                    try
                    {
                        var tableOperation = TableOperation.Delete(toBeIndexedEntity.Value);
                        _table.Execute(tableOperation);
                        returnDict[toBeIndexedEntity.Value.EntityId] = true;
                    }
                    catch (Exception)
                    {
                        Trace.TraceError("Couldn't delete entity id {0}", toBeIndexedEntity.Value.EntityId);
                    }
                }
            }

            return returnDict;
        }

        public IDictionary<string, bool> IndexedMovies(List<string> ids)
        {
            return RemoveItemFromTable(ToBeIndexedEntity.MoviePartitionKey, ids);
        }

        public IDictionary<string, bool> IndexedReviews(List<string> ids)
        {
            return RemoveItemFromTable(ToBeIndexedEntity.ReviewPartitionkey, ids);
        }


        public IDictionary<ITableEntity, bool> AddMovieToBeIndexed(List<string> ids)
        {
            List<ITableEntity> list = new List<ITableEntity>();
            foreach (var toBeIndexedEntity in ids)
            {
                list.Add(new ToBeIndexedEntity(ToBeIndexedEntity.MoviePartitionKey, toBeIndexedEntity));
            }
            return UpdateItemsById(list, ToBeIndexedEntity.MoviePartitionKey);
        }

        public IDictionary<ITableEntity, bool> AddReviewToBeIndexed(List<string> ids)
        {
            List<ITableEntity> list = new List<ITableEntity>();
            foreach (var toBeIndexedEntity in ids)
            {
                list.Add(new ToBeIndexedEntity(ToBeIndexedEntity.ReviewPartitionkey, toBeIndexedEntity));
            }
            return UpdateItemsById(list, ToBeIndexedEntity.ReviewPartitionkey);
        }
    }
}
