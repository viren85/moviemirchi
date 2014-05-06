using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace MvcWebRole2.Controllers
{
    [DataContract]
    public class SearchResult
    {
        [DataMember(Name = "created_at", Order = 2)]
        public string CreatedAt { get; set; }

        [DataMember(Name = "id", Order = 3)]
        public long Id { get; set; }

        [DataMember(Name = "id_str", Order = 4)]
        public string Id_Str { get; set; }

        [DataMember(Name = "text", Order = 5)]
        public string Text { get; set; }

        [DataMember(Name = "source", Order = 6)]
        public string Source { get; set; }

        [DataMember(Name = "user", Order = 7)]
        public User User { get; set; }

        [DataMember(Name = "in_reply_to_user_id", Order = 8)]
        public long? ToUserId { get; set; }

        [DataMember(Name = "in_reply_to_screen_name", Order = 9)]
        public string ToUserName { get; set; }

        [DataMember(Name = "metadata", Order = 10)]
        public MetaData SearchMetaData { get; set; }
    }

    [DataContract]
    public class SearchResults
    {
        public SearchResults()
        {
            this.Results = new List<SearchResult>();
        }

        [DataMember(Name = "statuses")]
        public List<SearchResult> Results { get; set; }
    }

    [DataContract]
    public class User
    {
        [DataMember(Name = "screen_name", Order = 11)]
        public string FromUser { get; set; }

        [DataMember(Name = "id", Order = 12)]
        public long? FromUserId { get; set; }

        [DataMember(Name = "profile_image_url", Order = 13)]
        public string ProfileImageUrl { get; set; }

        [DataMember(Name = "profile_image_url_https", Order = 14)]
        public string ProfileImageUrlHttps { get; set; }
    }

    [DataContract]
    public class MetaData
    {
        [DataMember(Name = "result_type", Order = 0)]
        public string ResultType { get; set; }

        [DataMember(Name = "iso_language_code", Order = 1)]
        public string IsoLanguageCode { get; set; }
    }
}
