
namespace DataStoreLib.Storage
{
    using DataStoreLib.Models;
    using Microsoft.WindowsAzure.Storage.Table;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;

    internal class LikeTable : Table
    {
        protected LikeTable(CloudTable table)
            : base(table)
        {
        }

        internal static Table CreateTable(CloudTable table)
        {
            return new LikeTable(table);
        }

        protected override string GetParitionKey()
        {
            return LikeEntity.PARTITION_KEY;
        }

        public LikeEntity GetUser(string userId)
        {
            Debug.Assert(this._table != null);
            var result = this._table.Execute(TableOperation.Retrieve<LikeEntity>(LikeEntity.PARTITION_KEY, userId));
            return result.Result as LikeEntity;
        }

        public void InsertOrUpdateUser(string userId, LikeEntity entity)
        {
            var partitionKey = GetParitionKey();

            var batchOp = new TableBatchOperation();
            batchOp.InsertOrReplace(entity);

            var tableResult = this._table.ExecuteBatch(batchOp);
        }
    }
}
