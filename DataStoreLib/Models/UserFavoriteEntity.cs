
namespace DataStoreLib.Models
{
    using Microsoft.WindowsAzure.Storage.Table;
    using System;
    using System.Collections.Generic;

    public class UserFavoriteEntity: TableStorageEntity
    {
        #region table members
        public static readonly string PARTITION_KEY = "CloudMovie";
        public string UserFavoriteId { get; set; }
        public string UserId { get; set; }
        public string Favorites { get; set; }
        public string DateCreated { get; set; }

        #endregion

        public UserFavoriteEntity()
            : base(PARTITION_KEY, string.Empty)
        {

        }

        public UserFavoriteEntity(string rowKey)
            : base(PARTITION_KEY, rowKey)
        {
            UserFavoriteId = rowKey;
        }

        public UserFavoriteEntity(UserFavoriteEntity userFavorite)
            : base(userFavorite.PartitionKey, userFavorite.RowKey)
        {
            UserFavoriteId = userFavorite.UserFavoriteId;
            UserId = userFavorite.UserId;
            Favorites = userFavorite.Favorites;
            DateCreated = userFavorite.DateCreated;            
        }

        public override string GetKey()
        {
            return this.UserFavoriteId;
        }

        public static UserFavoriteEntity CreateReviewEntity(string userId, string favorites, string dateCreated)
        {
            var userfavoriteId = Guid.NewGuid().ToString();
            var userFavoriteEntity = new UserFavoriteEntity(userfavoriteId);
                        
            userFavoriteEntity.UserId = userId;            
            userFavoriteEntity.Favorites= favorites;
            userFavoriteEntity.DateCreated = dateCreated;
            
            return userFavoriteEntity;
        }
    }
}
