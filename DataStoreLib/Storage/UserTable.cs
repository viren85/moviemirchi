
namespace DataStoreLib.Storage
{
    using DataStoreLib.Models;
    using Microsoft.WindowsAzure.Storage.Table;
    using System.Collections.Generic;
    using System.Diagnostics;

    internal class UserTable : Table
    {
        protected UserTable(CloudTable table)
            : base(table)
        {
        }

        internal static Table CreateTable(CloudTable table)
        {
            return new UserTable(table);
        }

        protected override string GetParitionKey()
        {
            return ReviewEntity.PARTITION_KEY;
        }


        public IDictionary<string, TEntity> GetItemsByUserName<TEntity>(string userName) where TEntity : DataStoreLib.Models.TableStorageEntity
        {
            Debug.Assert(_table != null);

            var operationList = new Dictionary<string, TableResult>();

            TableQuery<UserEntity> query = new TableQuery<UserEntity>().Where(TableQuery.GenerateFilterCondition("UserName", QueryComparisons.Equal, userName));

            IEnumerable<UserEntity> loginResults = _table.ExecuteQuery<UserEntity>(query);

            var returnDict = new Dictionary<string, TEntity>();
            int iter = 0;

            foreach (var tableResult in loginResults)
            {
                TEntity entity = null;

                entity = tableResult as TEntity;

                returnDict.Add(tableResult.UserName, entity);
                iter++;
            }

            return returnDict;
        }
        /*
       
        */
        public IDictionary<string, TEntity> UserAuthentication<TEntity>(string userName, string password) where TEntity : DataStoreLib.Models.TableStorageEntity
        {

            Debug.Assert(_table != null);

            var operationList = new Dictionary<string, TableResult>();

            TableQuery<UserEntity> query = new TableQuery<UserEntity>().Where(TableQuery.CombineFilters(
                                                                                TableQuery.GenerateFilterCondition("UserName", QueryComparisons.Equal, userName),
                                                                                TableOperators.And,
                                                                                TableQuery.GenerateFilterCondition("Password", QueryComparisons.Equal, password)));


            IEnumerable<UserEntity> loginResults = _table.ExecuteQuery<UserEntity>(query);

            var returnDict = new Dictionary<string, TEntity>();
            int iter = 0;

            foreach (var tableResult in loginResults)
            {
                TEntity entity = null;

                entity = tableResult as TEntity;

                returnDict.Add(tableResult.UserName, entity);
                iter++;
            }

            return returnDict;
        }
    }
}
