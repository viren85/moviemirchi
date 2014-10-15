
namespace DataStoreLib.Models
{
    using Microsoft.WindowsAzure.Storage.Table;
    using System;
    using System.Collections.Generic;

    public class TwitterEntity : TableStorageEntity
    {
        public static readonly string PARTITION_KEY = "CloudMovie";
        
        public string TwitterId { get; set; }
        public string TwitterIdString { get; set; }
        public string TextMessage { get; set; }
        public string Source { get; set; }
        public string FromUser { get; set; }
        public string FromUserId { get; set; }
        public string ProfileImageUrl { get; set; }
        public string ProfileSecureImageUrl { get; set; }
        public string ReplyUserId { get; set; }
        public string ReplyScreenName { get; set; }
        public string ResultType { get; set; }
        public string LanguageCode { get; set; }
        public DateTimeOffset Created_At { get; set; }
        public string Status { get; set; }
        public string TweetType { get; set; }
        public string MovieName { get; set; }
        public string ArtistName { get; set; }
        public bool IsActive { get; set; }

        public TwitterEntity()
            : base(PARTITION_KEY, string.Empty)
        { }

        public TwitterEntity(string rowKey)
            : base(PARTITION_KEY, rowKey)
        { }

        public TwitterEntity(TwitterEntity entity)
            : base(PARTITION_KEY, entity.RowKey)
        {
            TwitterId = entity.TwitterId;
            TwitterIdString = entity.TwitterIdString;
            TextMessage = entity.TextMessage;

            Source = entity.Source;
            FromUser = entity.FromUser;
            FromUserId = entity.FromUserId;
            ReplyUserId = entity.ReplyUserId;
            ReplyScreenName = entity.ReplyScreenName;
            ResultType = entity.ResultType;
            LanguageCode = entity.LanguageCode;

            ProfileImageUrl = entity.ProfileImageUrl;
            ProfileSecureImageUrl = entity.ProfileSecureImageUrl;

            Created_At = entity.Created_At;
            Status = "-1";
            TweetType = entity.TweetType;
            MovieName = entity.MovieName;
            ArtistName = entity.ArtistName;
            IsActive = entity.IsActive;
        }

        public override string GetKey()
        {
            return this.RowKey;
        }

        public static TwitterEntity CreateTwitterEntity(string fromUserId, string textMessage)
        {
            var twitterId = Guid.NewGuid().ToString();
            var entity = new TwitterEntity(twitterId);
            entity.TwitterId = twitterId;
            entity.FromUserId = fromUserId;
            entity.TextMessage = textMessage;
            entity.Status = "-1";
            return entity;
        }
    }
}
