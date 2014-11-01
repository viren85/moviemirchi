
using DataStoreLib.Models;
using Microsoft.WindowsAzure.Storage.Table;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace DataStoreLib.Storage
{
    internal class ArtistTable : Table
    {
        protected ArtistTable(CloudTable table)
            : base(table)
        {
        }

        internal static Table CreateTable(CloudTable table)
        {
            return new ArtistTable(table);
        }

        protected override string GetParitionKey()
        {
            return ArtistEntity.PARTITION_KEY;
        }

        public ArtistEntity GetArtist(string artistName)
        {
            var lowerCaseArtistName = artistName.ToLower();
            var filterByUniqueName = TableQuery.CombineFilters(
                        TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, "CloudMovie"),
                        TableOperators.And,
                        TableQuery.GenerateFilterCondition("RowKey", QueryComparisons.Equal, lowerCaseArtistName));
            var query = new TableQuery<ArtistEntity>().Where(filterByUniqueName);
            return base._table.ExecuteQuery(query).FirstOrDefault();
        }
    }
}
