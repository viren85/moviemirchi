
namespace DataStoreLib.Models
{
    using Microsoft.WindowsAzure.Storage.Table;
    using System;
    using System.Collections.Generic;

    public class UserFavoriteEntity: TableEntity
    {
        #region table members
        public static readonly string PARTITION_KEY = "CloudMovie";
        public string UserFavoriteId { get; set; }
        public string UserId { get; set; }
        public string Favorites { get; set; }
        public string DateCreated { get; set; }

        public override void ReadEntity(IDictionary<string, EntityProperty> properties, Microsoft.WindowsAzure.Storage.OperationContext operationContext)
        {
            base.ReadEntity(properties, operationContext);

            UserFavoriteId = ReadString(properties, "UserFavoriteId");
            UserId = ReadString(properties, "UserId");
            Favorites = ReadString(properties, "Favorites");
            DateCreated = ReadString(properties, "DateCreated");            
        }

        public override IDictionary<string, EntityProperty> WriteEntity(Microsoft.WindowsAzure.Storage.OperationContext operationContext)
        {
            var dict = MergeDicts(base.WriteEntity(operationContext));

            WriteString(dict, "UserFavoriteId", UserFavoriteId);
            WriteString(dict, "UserId", UserId);
            WriteString(dict, "Favorites", Favorites);
            WriteString(dict, "DateCreated", DateCreated);
            
            return dict;
        }

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
