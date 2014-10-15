
namespace DataStoreLib.Models
{
    using Microsoft.WindowsAzure.Storage.Table;
    using System;
    using System.Collections.Generic;
    using System.Web.Script.Serialization;

    public class NewsEntity : TableStorageEntity
    {
        public static readonly string PARTITION_KEY = "CloudMovie";

        public string Desc
        {
            get
            {
                return (this.Description ?? string.Empty).Substring(0, Math.Min(this.Description.Length, 190));
            }
        }

        public string Title { get; set; }
        public string Image { get; set; }
        public string Link { get; set; }
        public string Source { get; set; }

        [ScriptIgnore]
        public string NewsId { get; set; }
        [ScriptIgnore]
        public string Description { get; set; }
        [ScriptIgnore]
        public string PublishDate { get; set; }
        [ScriptIgnore]
        public string MovieName { get; set; }
        [ScriptIgnore]
        public string ArtistName { get; set; }
        [ScriptIgnore]
        public string FutureJson { get; set; }
        [ScriptIgnore]
        public bool  IsActive { get; set; }

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
            MovieName = entity.MovieName;
            ArtistName = entity.ArtistName;
            FutureJson = entity.FutureJson;
            IsActive = entity.IsActive;
        }

        public override string GetKey()
        {
            return this.NewsId;
        }

        public static NewsEntity CreateNewsEntity(string title, string description, string image, string link, string publishDate, string source, string movieName, string artistName, string futureJson, bool isActive)
        {
            var newsId = Guid.NewGuid().ToString();
            var newsEntity = new NewsEntity(newsId);
            newsEntity.Title = title;
            newsEntity.Description = description;
            newsEntity.Image = image;
            newsEntity.Link = link;
            newsEntity.PublishDate = publishDate;
            newsEntity.Source = source;
            newsEntity.ArtistName = movieName;
            newsEntity.MovieName = artistName;
            newsEntity.FutureJson = futureJson;
            newsEntity.IsActive = isActive;
            return newsEntity;
        }
    }
}
