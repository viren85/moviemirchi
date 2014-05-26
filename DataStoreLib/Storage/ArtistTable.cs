
namespace DataStoreLib.Storage
{
    using DataStoreLib.Models;
    using Microsoft.WindowsAzure.Storage.Table;
    using System.Collections.Generic;
    using System.Diagnostics;

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

    }
}
