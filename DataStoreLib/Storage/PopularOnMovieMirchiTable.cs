
namespace DataStoreLib.Storage
{
    using DataStoreLib.Models;
    using Microsoft.WindowsAzure.Storage.Table;

    internal class PopularOnMovieMirchiTable : Table
    {
        protected PopularOnMovieMirchiTable(CloudTable table)
            : base(table)
        {
        }

        internal static Table CreateTable(CloudTable table)
        {
            return new PopularOnMovieMirchiTable(table);
        }

        protected override string GetParitionKey()
        {
            return PopularOnMovieMirchiEntity.PARTITION_KEY;
        }
    }
}
