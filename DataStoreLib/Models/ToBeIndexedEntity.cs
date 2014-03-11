using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataStoreLib.Models
{
    public class ToBeIndexedEntity : TableEntity
    {
        public static readonly string MoviePartitionKey = "Movie";
        public static readonly string ReviewPartitionkey = "Review";
        public static readonly string LoginPartitionkey = "Login";
        public string EntityId { get; set; }

        public ToBeIndexedEntity()
            : base(MoviePartitionKey, "")
        {
            
        }


        public ToBeIndexedEntity(string paritionKey, string entityId)
            : base(paritionKey, entityId)
        {
            EntityId = entityId;
        }

        ToBeIndexedEntity(ToBeIndexedEntity e)
            :base(e.PartitionKey, e.RowKey)
        {
            EntityId = e.EntityId;
        }

        public override void ReadEntity(IDictionary<string, EntityProperty> properties,
                                        Microsoft.WindowsAzure.Storage.OperationContext operationContext)
        {
            base.ReadEntity(properties, operationContext);

            EntityId = ReadString(properties, "EntityId");
        }

        public override IDictionary<string, EntityProperty> WriteEntity(Microsoft.WindowsAzure.Storage.OperationContext operationContext)
        {
            var dict = MergeDicts(base.WriteEntity(operationContext));

            WriteString(dict, "EntityId", EntityId);
            return dict;
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
    }
}
