
namespace DataStoreLib.Models
{
    using Microsoft.WindowsAzure.Storage.Table;
    using System;
    using System.Collections.Generic;

    public class ReviewEntity : TableStorageEntity
    {
        #region table emembers
        public static readonly string PARTITION_KEY = "CloudMovie";

        public string ReviewId { get; set; }
        public string MovieId { get; set; }
        public string ReviewerId { get; set; }
        public string ReviewerName { get; set; }
        public string Review { get; set; }
        public string ReviewerRating { get; set; }
        public int SystemRating { get; set; }
        public bool Hot { get; set; }
        public string OutLink { get; set; }
        public string Affiliation { get; set; }
        public string Summary { get; set; }

        public string MyScore { get; set; }
        
        // For future use
        public string JsonString { get; set; }

        public string CriticsRating { get; set; }

        #endregion

        public ReviewEntity()
            : base(PARTITION_KEY, string.Empty)
        {

        }

        public ReviewEntity(string rowKey)
            : base(PARTITION_KEY, rowKey)
        {
            ReviewId = rowKey;
        }

        public ReviewEntity(ReviewEntity review)
            : base(review.PartitionKey, review.RowKey)
        {
            ReviewId = review.ReviewId;
            ReviewerName = review.ReviewerName;
            Review = review.Review;
            ReviewerRating = review.ReviewerRating;
            SystemRating = review.SystemRating;
            MovieId = review.MovieId;
            Hot = review.Hot;
            OutLink = review.OutLink;
            Affiliation = review.Affiliation;
            Summary = review.Summary;
            MyScore = review.MyScore;
            JsonString = review.JsonString;
            Tags = review.Tags;
        }

        public override string GetKey()
        {
            return this.ReviewId;
        }

        public static ReviewEntity CreateReviewEntity(string reviewrName, string review, string movieId, string reviewerId, bool hot, string outLink, string affiliation, string summary, string myScore, string jsonString, int reviewerRating = 0, int systemRating = 0, string tags = null)
        {
            var reviewId = Guid.NewGuid().ToString();
            var reviewEntity = new ReviewEntity(reviewId);
            reviewEntity.ReviewId = reviewId;
            reviewEntity.ReviewerName = reviewrName;
            reviewEntity.Review = review;
            reviewEntity.ReviewerRating = reviewerRating.ToString();
            reviewEntity.SystemRating = systemRating;
            reviewEntity.MovieId = movieId;
            reviewEntity.ReviewerId = reviewerId;
            reviewEntity.Hot = hot;
            reviewEntity.OutLink = outLink;
            reviewEntity.Affiliation = affiliation;
            reviewEntity.Summary = summary;
            reviewEntity.MyScore = myScore;
            reviewEntity.JsonString = jsonString;
            reviewEntity.Tags = tags ?? string.Empty;
            return reviewEntity;
        }
    }
}
