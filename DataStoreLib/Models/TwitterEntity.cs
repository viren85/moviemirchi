
namespace DataStoreLib.Models
{
    using Microsoft.WindowsAzure.Storage.Table;
    using System;
    using System.Collections.Generic;

    public class TwitterEntity : TableEntity
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

        public override void ReadEntity(IDictionary<string, EntityProperty> properties, Microsoft.WindowsAzure.Storage.OperationContext operationContext)
        {
            base.ReadEntity(properties, operationContext);

            TwitterId = ReadString(properties, "TwitterId");
            TwitterIdString = ReadString(properties, "TwitterIdString");
            TextMessage = ReadString(properties, "TextMessage");
            Source = ReadString(properties, "Source");
            FromUser = ReadString(properties, "FromUser");
            FromUserId = ReadString(properties, "FromUserId");
            ReplyUserId = ReadString(properties, "ReplyUserId");
            ReplyScreenName = ReadString(properties, "ReplyScreenName");
            ResultType = ReadString(properties, "ResultType");
            LanguageCode = ReadString(properties, "LanguageCode");
            ProfileImageUrl = ReadString(properties, "ProfileImageUrl");
            ProfileSecureImageUrl = ReadString(properties, "ProfileSecureImageUrl");
            Created_At = ReadTimestamp(properties, "Created_At");
            Status = ReadString(properties, "Status");
            TweetType = ReadString(properties, "TweetType");
            MovieName = ReadString(properties, "MovieName");
            ArtistName = ReadString(properties, "ArtistName");
            IsActive = ReadBool(properties, "IsActive");
        }

        public override IDictionary<string, EntityProperty> WriteEntity(Microsoft.WindowsAzure.Storage.OperationContext operationContext)
        {
            var dict = MergeDicts(base.WriteEntity(operationContext));

            WriteString(dict, "TwitterId", TwitterId);
            WriteString(dict, "TwitterIdString", TwitterIdString);
            WriteString(dict, "TextMessage", TextMessage);

            WriteString(dict, "Source", Source);
            WriteString(dict, "FromUser", FromUser);
            WriteString(dict, "FromUserId", FromUserId);
            WriteString(dict, "ReplyUserId", ReplyUserId);
            WriteString(dict, "ReplyScreenName", ReplyScreenName);
            WriteString(dict, "ResultType", ResultType);
            WriteString(dict, "LanguageCode", LanguageCode);
            WriteString(dict, "ProfileImageUrl", ProfileImageUrl);
            WriteString(dict, "ProfileSecureImageUrl", ProfileSecureImageUrl);

            WriteTimestamp(dict, "Created_At", Created_At);
            WriteString(dict, "Status", Status);
            WriteString(dict, "TweetType", TweetType);
            WriteString(dict, "MovieName", MovieName);
            WriteString(dict, "ArtistName", ArtistName);

            WriteBool(dict, "IsActive", IsActive);

            return dict;
        }
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
