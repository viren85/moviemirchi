using DataStoreLib.Models;
using DataStoreLib.Storage;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crawler
{
    public class ReviewCrawler
    {
        public static string SetReviewer(string reviewerName, string affiliation)
        {
            string reviewerKey = string.Empty;
            try
            {
                #region Add Reviewer In DB
                TableManager tblMgr = new TableManager();
                ReviewerEntity reviewer = new ReviewerEntity();

                bool isReviewerAlreadyPresent = false;
                
                var reviewers = tblMgr.GetAllReviewer();

                for (int rid = 0; rid < reviewers.Keys.Count; rid++)
                {
                    string key = reviewers.ElementAt(rid).Key;
                    if (reviewers[key].ReviewerName == reviewerName)
                    {
                        isReviewerAlreadyPresent = true;
                        reviewerKey = key;
                        break;
                    }
                }

                if (!isReviewerAlreadyPresent)
                {
                    reviewer.ReviewerId = Guid.NewGuid().ToString();
                    reviewer.ReviewerName = reviewerName;
                    reviewer.Affilation = affiliation;
                    reviewer.ReviewerImage = string.Empty;
                    tblMgr.UpdateReviewerById(reviewer);

                    reviewerKey = reviewer.ReviewerId;
                }
                #endregion
            }
            catch (Exception ex)
            {
                
            }

            return reviewerKey;
        }
    }
}
