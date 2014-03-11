using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataStoreLib.Models
{
    public class AuthenticationEntity : TableEntity
    {
        public static readonly string PARTITION_KEY = "CloudMovie";
        public string UserId { get; set; }
        public string UserName { get; set; }

        public string Password { get; set; }

        public override void ReadEntity(IDictionary<string, EntityProperty> properties,
        Microsoft.WindowsAzure.Storage.OperationContext operationContext)
        {
            base.ReadEntity(properties, operationContext);

            UserId = ReadString(properties, "UserId");
            UserName = ReadString(properties, "UserName");
            Password = ReadString(properties, "Password");

        }

        public AuthenticationEntity()
            : base(PARTITION_KEY, "")
        {

        }

        public AuthenticationEntity(string rowKey)
            : base(PARTITION_KEY, rowKey)
        {
        }

        public AuthenticationEntity(AuthenticationEntity entity)
            : base(PARTITION_KEY, entity.RowKey)
        {
            UserId = entity.UserId;
            UserName = entity.UserName;
            Password = entity.Password;

        }


        public static AuthenticationEntity CreateAuthenticationEntity(string userName, string password)
        {
            var userId = Guid.NewGuid().ToString();
            var entity = new AuthenticationEntity(userId);
            entity.UserId = userId;
            entity.UserName = userName;
            entity.Password = password;
            return entity;
        }

        public List<string> GetUserName()
        {
            return Utils.utils.GetListFromCommaSeparatedString(UserName);
        }

        public List<string> GetPassword()
        {
            return Utils.utils.GetListFromCommaSeparatedString(Password);
        }

        public void SetUserName(List<string> list)
        {
            UserName = Utils.utils.GetCommaSeparatedStringFromList(list);
        }

        public void SetPassword(List<string> list)
        {
            Password = Utils.utils.GetCommaSeparatedStringFromList(list);
        }

    }
}
