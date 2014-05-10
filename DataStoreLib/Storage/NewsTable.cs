
namespace DataStoreLib.Storage
{
    using DataStoreLib.Models;
    using Microsoft.WindowsAzure.Storage.Table;
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

    }
}
