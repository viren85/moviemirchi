
namespace DataStoreLib.Models
{
    using Microsoft.WindowsAzure.Storage;
    using Microsoft.WindowsAzure.Storage.Table;
    using System;
    using System.Collections.Generic;

    public class AffilationEntity : TableStorageEntity
    {
        public const string PARTITION_KEY = "CloudMovie";

        public string AffilationId { get; set; }

        public string AffilationName { get; set; }

        public string WebsiteName { get; set; }

        public string WebsiteLink { get; set; }

        public string LogoLink { get; set; }

        public string Country { get; set; }

        public AffilationEntity()
            : base(PARTITION_KEY, string.Empty)
        {
        }

        public AffilationEntity(string rowKey)
            : base(PARTITION_KEY, rowKey)
        {
        }

        public AffilationEntity(AffilationEntity entity)
            : base(PARTITION_KEY, entity.RowKey)
        {
            AffilationId = entity.AffilationId;
            AffilationName = entity.AffilationName;
            WebsiteName = entity.WebsiteName;
            WebsiteLink = entity.WebsiteLink;
            LogoLink = entity.LogoLink;
            Country = entity.Country;
        }

        public override string GetKey()
        {
            return this.AffilationId;
        }

        public static AffilationEntity CreateAffilationEntity(string affilationName, string websiteName, string websiteLink, string logoLink, string country)
        {
            var affilationId = Guid.NewGuid().ToString();
            var entity = new AffilationEntity(affilationId);
            entity.AffilationId = affilationId;
            entity.AffilationName = affilationName;
            entity.WebsiteName = websiteName;
            entity.WebsiteLink = websiteLink;
            entity.LogoLink = logoLink;
            entity.Country = country;
            return entity;
        }

        public IEnumerable<string> GetAffilationName()
        {
            return Utils.Utils.GetListFromCommaSeparatedString(AffilationName);
        }

        public IEnumerable<string> GetWebsiteName()
        {
            return Utils.Utils.GetListFromCommaSeparatedString(WebsiteName);
        }

        public IEnumerable<string> GetWebsiteLink()
        {
            return Utils.Utils.GetListFromCommaSeparatedString(WebsiteLink);
        }

        public IEnumerable<string> GetLogoLink()
        {
            return Utils.Utils.GetListFromCommaSeparatedString(LogoLink);
        }

        public IEnumerable<string> GetCountry()
        {
            return Utils.Utils.GetListFromCommaSeparatedString(Country);
        }

        public void SetAffilationName(IEnumerable<string> list)
        {
            AffilationName = Utils.Utils.GetCommaSeparatedStringFromList(list);
        }

        public void SetWebsiteName(IEnumerable<string> list)
        {
            WebsiteName = Utils.Utils.GetCommaSeparatedStringFromList(list);
        }

        public void SetWebsiteLink(IEnumerable<string> list)
        {
            WebsiteLink = Utils.Utils.GetCommaSeparatedStringFromList(list);
        }

        public void SetLogoLink(IEnumerable<string> list)
        {
            LogoLink = Utils.Utils.GetCommaSeparatedStringFromList(list);
        }

        public void SetCountry(IEnumerable<string> list)
        {
            Country = Utils.Utils.GetCommaSeparatedStringFromList(list);
        }
    }
}
