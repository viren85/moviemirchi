﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage.Table;

namespace DataStoreLib.Models
{
    public class ReviewEntity : TableEntity
    {
        #region table emembers
        public static readonly string PARTITION_KEY = "CloudMovie";

        public string ReviewId { get; set; }
        public string MovieId { get; set; }
        public string ReviewerId { get; set; }
        public string ReviewerName { get; set; }
        public string Review { get; set; }
        public int ReviewerRating { get; set; }
        public int SystemRating { get; set; }
        public bool Hot { get; set; }
        public string OutLink { get; set; }
        public string Affiliation { get; set; }
        public string Summary { get; set; }

        public override void ReadEntity(IDictionary<string, EntityProperty> properties, Microsoft.WindowsAzure.Storage.OperationContext operationContext)
        {
            base.ReadEntity(properties, operationContext);

            ReviewId = ReadString(properties, "ReviewId");
            ReviewerId = ReadString(properties, "ReviewerId");
            ReviewerName = ReadString(properties, "ReviewerName");
            Review = ReadString(properties, "Review");
            ReviewerRating = ReadInt(properties, "ReviewerRating");
            SystemRating = ReadInt(properties, "SystemRating");
            MovieId = ReadString(properties, "MovieId");
            Hot = ReadBool(properties, "Hot");
            OutLink = ReadString(properties, "OutLink");
            Affiliation = ReadString(properties, "Affiliation");
            Summary = ReadString(properties, "Summary");
        }

        public override IDictionary<string, EntityProperty> WriteEntity(Microsoft.WindowsAzure.Storage.OperationContext operationContext)
        {
            var dict = MergeDicts(base.WriteEntity(operationContext));

            WriteString(dict, "ReviewId", ReviewId);
            WriteString(dict, "ReviewerName", ReviewerName);
            WriteString(dict, "Review", Review);
            WriteInt(dict, "ReviewerRating", ReviewerRating);
            WriteInt(dict, "SystemRating", SystemRating);
            WriteString(dict, "MovieId", MovieId);
            WriteString(dict, "ReviewerId", ReviewerId);
            WriteBool(dict, "Hot", Hot);
            WriteString(dict, "OutLink", OutLink);
            WriteString(dict, "Affiliation", Affiliation);
            WriteString(dict, "Summary", Summary);
            return dict;
        }

        #endregion

        public ReviewEntity()
            : base(PARTITION_KEY, "")
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
        }

        public static ReviewEntity CreateReviewEntity(string reviewrName, string review, string movieId, string reviewerId, bool hot, string outLink, string affiliation, string summary, int reviewerRating = 0, int systemRating = 0)
        {
            var reviewId = Guid.NewGuid().ToString();
            var reviewEntity = new ReviewEntity(reviewId);
            reviewEntity.ReviewId = reviewId;
            reviewEntity.ReviewerName = reviewrName;
            reviewEntity.Review = review;
            reviewEntity.ReviewerRating = reviewerRating;
            reviewEntity.SystemRating = systemRating;
            reviewEntity.MovieId = movieId;
            reviewEntity.ReviewerId = reviewerId;
            reviewEntity.Hot = hot;
            reviewEntity.OutLink = outLink;
            reviewEntity.Affiliation = affiliation;
            reviewEntity.Summary = summary;
            return reviewEntity;
        }
    }
}
