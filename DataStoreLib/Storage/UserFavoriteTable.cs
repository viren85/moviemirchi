
namespace DataStoreLib.Storage
{
    using DataStoreLib.Models;
    using Microsoft.WindowsAzure.Storage.Table;
    using System.Collections.Generic;
    using System.Linq;

    internal class UserFavoriteTable : Table
    {
        protected UserFavoriteTable(CloudTable table)
            : base(table)
        {
        }

        internal static Table CreateTable(CloudTable table)
        {
            return new UserFavoriteTable(table);
        }

        protected override string GetParitionKey()
        {
            return UserFavoriteEntity.PARTITION_KEY;
        }

        public UserFavoriteEntity GetUserFavoritesByUserId(string userId)
        {
            var query = new TableQuery<UserFavoriteEntity>()
                .Where(TableQuery.GenerateFilterCondition("UserId", QueryComparisons.Equal, userId));

            var reviewResults = _table.ExecuteQuery(query);

            return reviewResults.FirstOrDefault();
        }
    }
}
