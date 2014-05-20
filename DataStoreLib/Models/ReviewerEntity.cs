
namespace DataStoreLib.Models
{
    using Microsoft.WindowsAzure.Storage;
    using Microsoft.WindowsAzure.Storage.Table;
    using System;
    using System.Collections.Generic;

    public class ReviewerEntity : TableEntity
    {
        public static readonly string PARTITION_KEY = "CloudMovie";
        public string ReviewerId { get; set; }
        public string ReviewerName { get; set; }
        public string ReviewerImage { get; set; }
        public string Affilation { get; set; }

        public override void ReadEntity(IDictionary<string, EntityProperty> properties, OperationContext operationContext)
        {
            base.ReadEntity(properties, operationContext);
            ReviewerId = ReadString(properties, "ReviewerId");
            ReviewerName = ReadString(properties, "ReviewerName");
            ReviewerImage = ReadString(properties, "ReviewerImage");
            Affilation = ReadString(properties, "Affilation");


        }

        public override IDictionary<string, EntityProperty> WriteEntity(OperationContext operationContext)
        {
            var dict = MergeDicts(base.WriteEntity(operationContext));

            WriteString(dict, "ReviewerId", ReviewerId);
            WriteString(dict, "ReviewerName", ReviewerName);
            WriteString(dict, "ReviewerImage", ReviewerImage);
            WriteString(dict, "Affilation", Affilation);
            return dict;
        }

        public ReviewerEntity()
            : base(PARTITION_KEY, string.Empty)
        {

        }

        public ReviewerEntity(string rowKey)
            : base(PARTITION_KEY, rowKey)
        {
        }

        public ReviewerEntity(ReviewerEntity entity)
            : base(PARTITION_KEY, entity.RowKey)
        {
            ReviewerId = entity.ReviewerId;
            ReviewerName = entity.ReviewerName;
            ReviewerImage = entity.ReviewerImage;
            Affilation = entity.Affilation;
        }

        public override string GetKey()
        {
            return this.ReviewerId;
        }

        public static ReviewerEntity CreateAffilationEntity(string reviewerName, string reviewerImage, string affilation)
        {
            var reviewerId = Guid.NewGuid().ToString();
            var entity = new ReviewerEntity(reviewerId);
            entity.ReviewerId = reviewerId;
            entity.ReviewerName = reviewerName;
            entity.ReviewerImage = reviewerImage;
            entity.Affilation = affilation;

            return entity;
        }

        public IEnumerable<string> GetReviewerName()
        {
            return Utils.Utils.GetListFromCommaSeparatedString(ReviewerName);
        }

        public IEnumerable<string> GetReviewerImage()
        {
            return Utils.Utils.GetListFromCommaSeparatedString(ReviewerImage);
        }

        public IEnumerable<string> GetAffilation()
        {
            return Utils.Utils.GetListFromCommaSeparatedString(Affilation);
        }


        public void SetReviewerName(IEnumerable<string> list)
        {
            ReviewerName = Utils.Utils.GetCommaSeparatedStringFromList(list);
        }

        public void SetReviewerImage(IEnumerable<string> list)
        {
            ReviewerImage = Utils.Utils.GetCommaSeparatedStringFromList(list);
        }

        public void SetAffilation(IEnumerable<string> list)
        {
            Affilation = Utils.Utils.GetCommaSeparatedStringFromList(list);
        }
    }
}
