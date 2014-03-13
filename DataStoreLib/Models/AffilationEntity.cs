using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataStoreLib.Models;

namespace DataStoreLib.Models
{
    public class AffilationEntity : TableEntity
    {
        public static readonly string PARTITION_KEY = "CloudMovie";
        public string AffilationId { get; set; }
        public string AffilationName { get; set; }

        public string WebsiteName { get; set; }

        public string WebsiteLink { get; set; }

        public string LogoLink { get; set; }

        public string Country { get; set; }

        public override void ReadEntity(IDictionary<string, EntityProperty> properties,
       Microsoft.WindowsAzure.Storage.OperationContext operationContext)
        {
            base.ReadEntity(properties, operationContext);

            AffilationId = ReadString(properties, "AffilationId");
            AffilationName = ReadString(properties, "AffilationName");
            WebsiteName = ReadString(properties, "WebsiteName");
            WebsiteLink = ReadString(properties, "WebsiteLink");
            LogoLink = ReadString(properties, "LogoLink");
            Country = ReadString(properties, "Country");

        }


        public override IDictionary<string, EntityProperty> WriteEntity(Microsoft.WindowsAzure.Storage.OperationContext operationContext)
        {
            var dict = MergeDicts(base.WriteEntity(operationContext));

            WriteString(dict, "AffilationId", AffilationId);
            WriteString(dict, "AffilationName", AffilationName);
            WriteString(dict, "WebsiteName", WebsiteName);
            WriteString(dict, "WebsiteLink", WebsiteLink);
            WriteString(dict, "LogoLink", LogoLink);
            WriteString(dict, "Country", Country);

            return dict;
        }

        public AffilationEntity()
            : base(PARTITION_KEY, "")
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

        public List<string> GetAffilationName()
        {
            return Utils.utils.GetListFromCommaSeparatedString(AffilationName);
        }

        public List<string> GetWebsiteName()
        {
            return Utils.utils.GetListFromCommaSeparatedString(WebsiteName);
        }

        public List<string> GetWebsiteLink()
        {
            return Utils.utils.GetListFromCommaSeparatedString(WebsiteLink);
        }

        public List<string> GetLogoLink()
        {
            return Utils.utils.GetListFromCommaSeparatedString(LogoLink);
        }

        public List<string> GetCountry()
        {
            return Utils.utils.GetListFromCommaSeparatedString(Country);
        }

        public void SetAffilationName(List<string> list)
        {
            AffilationName = Utils.utils.GetCommaSeparatedStringFromList(list);
        }

        public void SetWebsiteName(List<string> list)
        {
            WebsiteName = Utils.utils.GetCommaSeparatedStringFromList(list);
        }

        public void SetWebsiteLink(List<string> list)
        {
            WebsiteLink = Utils.utils.GetCommaSeparatedStringFromList(list);
        }

        public void SetLogoLink(List<string> list)
        {
            LogoLink = Utils.utils.GetCommaSeparatedStringFromList(list);
        }

        public void SetCountry(List<string> list)
        {
            Country = Utils.utils.GetCommaSeparatedStringFromList(list);
        }


    }
}
