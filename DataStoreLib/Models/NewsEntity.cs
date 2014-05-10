
namespace DataStoreLib.Models
{
    using Microsoft.WindowsAzure.Storage.Table;
    using System;
    using System.Collections.Generic;

    public class NewsEntity : TableEntity
    {
        public static readonly string PARTITION_KEY = "CloudMovie";
        public string NewsId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
        public string Link { get; set; }
        public string PublishDate { get; set; }
        public string Source { get; set; }
        public string FutureJson { get; set; }

        public override void ReadEntity(IDictionary<string, EntityProperty> properties, Microsoft.WindowsAzure.Storage.OperationContext operationContext)
        {
            base.ReadEntity(properties, operationContext);

            NewsId = ReadString(properties, "NewsId");
            Title = ReadString(properties, "Title");
            Description = ReadString(properties, "Description");

            Image = ReadString(properties, "Image");
            Link = ReadString(properties, "Link");
            PublishDate = ReadString(properties, "PublishDate");
            Source = ReadString(properties, "Source");
            FutureJson = ReadString(properties, "FutureJson");
        }

        public override IDictionary<string, EntityProperty> WriteEntity(Microsoft.WindowsAzure.Storage.OperationContext operationContext)
        {
            var dict = MergeDicts(base.WriteEntity(operationContext));

            WriteString(dict, "NewsId", NewsId);
            WriteString(dict, "Title", Title);
            WriteString(dict, "Description", Description);

            WriteString(dict, "Image", Image);
            WriteString(dict, "Link", Link);
            WriteString(dict, "PublishDate", PublishDate);
            WriteString(dict, "Source", Source);
            WriteString(dict, "FutureJson", FutureJson);

            return dict;
        }
        public NewsEntity()
            : base(PARTITION_KEY, string.Empty)
        { }

        public NewsEntity(string rowKey)
            : base(PARTITION_KEY, rowKey)
        { }

        public NewsEntity(NewsEntity entity)
            : base(PARTITION_KEY, entity.RowKey)
        {
            NewsId = entity.NewsId;
            Title = entity.Title;
            Description = entity.Description;

            Image = entity.Image;
            Link = entity.Link;
            PublishDate = entity.PublishDate;
            Source = entity.Source;
            FutureJson = entity.FutureJson;
        }

        public override string GetKey()
        {
            return this.NewsId;
        }

        public static NewsEntity CreateNewsEntity(string title, string description, string image, string link, string publishDate, string source, string futureJson)
        {
            var newsId = Guid.NewGuid().ToString();
            var newsEntity = new NewsEntity(newsId);
            newsEntity.Title = title;
            newsEntity.Description = description;
            newsEntity.Image = image;
            newsEntity.Link = link;
            newsEntity.PublishDate = publishDate;
            newsEntity.Source = source;
            newsEntity.FutureJson = futureJson;
            return newsEntity;
        }
    }
}
