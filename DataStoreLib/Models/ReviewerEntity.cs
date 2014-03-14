using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataStoreLib.Models;
using Microsoft.WindowsAzure.Storage.Table;

namespace DataStoreLib.Models
{
    public class ReviewerEntity : TableEntity
    {
        public static readonly string PARTITION_KEY = "CloudMovie";
        public string ReviewerId { get; set; }
        public string ReviewerName { get; set; }
        public string ReviewerImage { get; set; }
        public string Affilation { get; set; }
        
        public override void ReadEntity(IDictionary<string, EntityProperty> properties,
       Microsoft.WindowsAzure.Storage.OperationContext operationContext)
        {
            base.ReadEntity(properties, operationContext);
            ReviewerId = ReadString(properties, "ReviewerId");
            ReviewerName = ReadString(properties, "ReviewerName");
            ReviewerImage = ReadString(properties, "ReviewerImage");
            Affilation = ReadString(properties, "Affilation");
           

        }


        public override IDictionary<string, EntityProperty> WriteEntity(Microsoft.WindowsAzure.Storage.OperationContext operationContext)
        {
            var dict = MergeDicts(base.WriteEntity(operationContext));

            WriteString(dict, "ReviewerId", ReviewerId);
            WriteString(dict, "ReviewerName", ReviewerName);
            WriteString(dict, "ReviewerImage", ReviewerImage);
            WriteString(dict, "Affilation", Affilation);
            return dict;
        }

        public ReviewerEntity()
            : base(PARTITION_KEY, "")
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

        public List<string> GetReviewerName()
        {
            return Utils.utils.GetListFromCommaSeparatedString(ReviewerName);
        }

        public List<string> GetReviewerImage()
        {
            return Utils.utils.GetListFromCommaSeparatedString(ReviewerImage);
        }

        public List<string> GetAffilation()
        {
            return Utils.utils.GetListFromCommaSeparatedString(Affilation);
        }


        public void SetReviewerName(List<string> list)
        {
            ReviewerName = Utils.utils.GetCommaSeparatedStringFromList(list);
        }

        public void SetReviewerImage(List<string> list)
        {
            ReviewerImage = Utils.utils.GetCommaSeparatedStringFromList(list);
        }

        public void SetAffilation(List<string> list)
        {
            Affilation = Utils.utils.GetCommaSeparatedStringFromList(list);
        }

     
    }
}
