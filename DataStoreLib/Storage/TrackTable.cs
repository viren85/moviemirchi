
namespace DataStoreLib.Storage
{
    using DataStoreLib.Models;
    using Microsoft.WindowsAzure.Storage.Table;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;

    internal class TrackTable : Table
    {
        protected TrackTable(CloudTable table)
            : base(table)
        {
        }

        internal static Table CreateTable(CloudTable table)
        {
            return new TrackTable(table);
        }

        protected override string GetParitionKey()
        {
            return TrackEntity.PARTITION_KEY;
        }

        public TrackEntity GetUser(string userId)
        {
            Debug.Assert(this._table != null);
            var result = this._table.Execute(TableOperation.Retrieve<TrackEntity>(TrackEntity.PARTITION_KEY, userId));
            return result.Result as TrackEntity;
        }

        public void InsertOrUpdateUser(string userId, TrackEntity entity)
        {
            var partitionKey = GetParitionKey();

            var batchOp = new TableBatchOperation();
            batchOp.InsertOrReplace(entity);

            var tableResult = this._table.ExecuteBatch(batchOp);
        }
    }
}
