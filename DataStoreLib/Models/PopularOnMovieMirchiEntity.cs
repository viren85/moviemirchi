
namespace DataStoreLib.Models
{
    using Microsoft.WindowsAzure.Storage.Table;
    using System;
    using System.Collections.Generic;

    public class PopularOnMovieMirchiEntity : TableStorageEntity
    {
        #region table members
        public static readonly string PARTITION_KEY = "CloudMovie";
        public string PopularOnMovieMirchiId { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public int Counter { get; set; }
        public string DateUpdated { get; set; }

        #endregion

        public PopularOnMovieMirchiEntity()
            : base(PARTITION_KEY, string.Empty)
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

        public override string GetKey()
        {
            return this.PopularOnMovieMirchiId;
        }

        public static PopularOnMovieMirchiEntity CreateReviewEntity(string name, string type, int counter, string dateUpdated)
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
