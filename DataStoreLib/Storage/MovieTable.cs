
namespace DataStoreLib.Storage
{
    using DataStoreLib.Models;
    using Microsoft.WindowsAzure.Storage.Table;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;

    internal class MovieTable : Table
    {
        protected MovieTable(CloudTable table)
            : base(table)
        {
        }

        internal static Table CreateTable(CloudTable table)
        {
            return new MovieTable(table);
        }

        protected override string GetParitionKey()
        {
            return MovieEntity.PARTITION_KEY;
        }

        public IDictionary<string, TEntity> GetItemsByName<TEntity>(string name) where TEntity : DataStoreLib.Models.MovieEntity, new()
        {
            Debug.Assert(_table != null);

            var query = new TableQuery<TEntity>()
                .Where(TableQuery.GenerateFilterCondition("UniqueName", QueryComparisons.Equal, name));

            IEnumerable<TEntity> reviewResults = _table.ExecuteQuery(query);

            return
                reviewResults
                    .ToDictionary<TEntity, string, TEntity>(
                        tableResult => tableResult.MovieId,
                        tableResult => tableResult as TEntity);
        }
    }

}
