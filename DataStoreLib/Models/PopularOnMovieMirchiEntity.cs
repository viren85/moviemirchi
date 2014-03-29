using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataStoreLib.Models
{
    public class PopularOnMovieMirchiEntity : TableEntity
    {
        #region table members
        public static readonly string PARTITION_KEY = "CloudMovie";
        public string PopularOnMovieMirchiId { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public int Counter { get; set; }
        public string DateUpdated { get; set; }

        public override void ReadEntity(IDictionary<string, EntityProperty> properties, Microsoft.WindowsAzure.Storage.OperationContext operationContext)
        {
            base.ReadEntity(properties, operationContext);

            PopularOnMovieMirchiId = ReadString(properties, "PopularOnMovieMirchiId");
            Name = ReadString(properties, "Name");
            Type = ReadString(properties, "Type");
            Counter = ReadInt(properties, "Counter");
            DateUpdated = ReadString(properties, "DateUpdated");
        }

        public override IDictionary<string, EntityProperty> WriteEntity(Microsoft.WindowsAzure.Storage.OperationContext operationContext)
        {
            var dict = MergeDicts(base.WriteEntity(operationContext));

            WriteString(dict, "PopularOnMovieMirchiId", PopularOnMovieMirchiId);
            WriteString(dict, "Name", Name);
            WriteString(dict, "Type", Type);
            WriteInt(dict, "Counter", Counter);
            WriteString(dict, "DateUpdated", DateUpdated);

            return dict;
        }

        #endregion

        public PopularOnMovieMirchiEntity()
            : base(PARTITION_KEY, "")
        {

        }

        public PopularOnMovieMirchiEntity(string rowKey)
            : base(PARTITION_KEY, rowKey)
        {
            PopularOnMovieMirchiId = rowKey;
        }

        public PopularOnMovieMirchiEntity(PopularOnMovieMirchiEntity popularOnMovieMirchi)
            : base(popularOnMovieMirchi.PartitionKey, popularOnMovieMirchi.RowKey)
        {
            PopularOnMovieMirchiId = popularOnMovieMirchi.PopularOnMovieMirchiId;
            Name = popularOnMovieMirchi.Name;
            Type = popularOnMovieMirchi.Type;
            Counter = popularOnMovieMirchi.Counter;
            DateUpdated = popularOnMovieMirchi.DateUpdated;
        }

        public static PopularOnMovieMirchiEntity CreateReviewEntity(string name, string type, int counter,string dateUpdated)
        {
            var popularOnMovieMirchiId = Guid.NewGuid().ToString();
            var popularOnMovieMirchiEntity = new PopularOnMovieMirchiEntity(popularOnMovieMirchiId);

            popularOnMovieMirchiEntity.Name = name;
            popularOnMovieMirchiEntity.Type = type;
            popularOnMovieMirchiEntity.Counter = counter;
            popularOnMovieMirchiEntity.DateUpdated = dateUpdated;

            return popularOnMovieMirchiEntity;
        }
    }
}
