
namespace DataStoreLib.Models
{
    using Microsoft.WindowsAzure.Storage.Table;
    using System.Collections.Generic;

    public class ToBeIndexedEntity : TableStorageEntity
    {
        public static readonly string MoviePartitionKey = "Movie";
        public static readonly string ReviewPartitionkey = "Review";
        public static readonly string AffilationPartitionkey = "Affilation";
        public static readonly string ReviewerPartitionkey = "Reviewer";
        public static readonly string LoginPartitionkey = "Login";
        public string EntityId { get; set; }

        public ToBeIndexedEntity()
            : base(MoviePartitionKey, string.Empty)
        {

        }

        public ToBeIndexedEntity(string paritionKey, string entityId)
            : base(paritionKey, entityId)
        {
            EntityId = entityId;
        }

        ToBeIndexedEntity(ToBeIndexedEntity e)
            : base(e.PartitionKey, e.RowKey)
        {
            EntityId = e.EntityId;
        }

        public override string GetKey()
        {
            return this.EntityId;
        }

        public static ToBeIndexedEntity CreateMovieEntity(string entityId)
        {
            return new ToBeIndexedEntity(MoviePartitionKey, entityId);
        }

        public static ToBeIndexedEntity CreateReveiewEntity(string entityId)
        {
            return new ToBeIndexedEntity(ReviewPartitionkey, entityId);
        }

        public static ToBeIndexedEntity CreateLoginEntity(string entityId)
        {
            return new ToBeIndexedEntity(LoginPartitionkey, entityId);
        }

        public static ToBeIndexedEntity CreateAffilationEntity(string entityId)
        {
            return new ToBeIndexedEntity(AffilationPartitionkey, entityId);
        }

        public static ToBeIndexedEntity CreateReviewerEntity(string entityId)
        {
            return new ToBeIndexedEntity(ReviewerPartitionkey, entityId);
        }



    }
}
