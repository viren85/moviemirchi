
namespace DataStoreLib.Models
{
    using Microsoft.WindowsAzure.Storage.Table;
    using System;
    using System.Collections.Generic;

    public class UserEntity : TableEntity
    {
        public static readonly string PARTITION_KEY = "CloudMovie";
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string UserType { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Mobile { get; set; }
        public string DateOfBirth { get; set; }
        public string Gender { get; set; }
        public string City { get; set; }
        public string Profile_Pic_Http { get; set; }
        public string Profile_Pic_Https { get; set; }
        public string Country { get; set; }
        public int Status { get; set; }
        public string Favorite { get; set; }
        public DateTimeOffset Created_At { get; set; }



        public override void ReadEntity(IDictionary<string, EntityProperty> properties, Microsoft.WindowsAzure.Storage.OperationContext operationContext)
        {
            base.ReadEntity(properties, operationContext);

            UserId = ReadString(properties, "UserId");
            UserName = ReadString(properties, "UserName");
            Password = ReadString(properties, "Password");

            UserType = ReadString(properties, "UserType");
            FirstName = ReadString(properties, "FirstName");
            LastName = ReadString(properties, "LastName");
            Email = ReadString(properties, "Email");
            Mobile = ReadString(properties, "Mobile");
            DateOfBirth = ReadString(properties, "DateOfBirth");
            Gender = ReadString(properties, "Gender");
            City = ReadString(properties, "City");
            Profile_Pic_Http = ReadString(properties, "Profile_Pic_Http");
            Profile_Pic_Https = ReadString(properties, "Profile_Pic_Https");
            Country = ReadString(properties, "Country");
            Status = ReadInt(properties, "Status");
            Created_At = ReadTimestamp(properties, "Created_At");
            Favorite = ReadString(properties, "Favorite");
        }

        public override IDictionary<string, EntityProperty> WriteEntity(Microsoft.WindowsAzure.Storage.OperationContext operationContext)
        {
            var dict = MergeDicts(base.WriteEntity(operationContext));

            WriteString(dict, "UserId", UserId);
            WriteString(dict, "UserName", UserName);
            WriteString(dict, "Password", Password);

            WriteString(dict, "UserType", UserType);
            WriteString(dict, "FirstName", FirstName);
            WriteString(dict, "LastName", LastName);
            WriteString(dict, "Email", Email);
            WriteString(dict, "Mobile", Mobile);
            WriteString(dict, "DateOfBirth", DateOfBirth);
            WriteString(dict, "Gender", Gender);
            WriteString(dict, "City", City);
            WriteString(dict, "Profile_Pic_Http", Profile_Pic_Http);
            WriteString(dict, "Profile_Pic_Https", Profile_Pic_Https);
            WriteString(dict, "Country", Country);
            WriteInt(dict, "Status", Status);
            WriteTimestamp(dict, "Created_At", Created_At);
            WriteString(dict, "Favorite", Favorite);

            return dict;
        }
        public UserEntity()
            : base(PARTITION_KEY, string.Empty)
        { }

        public UserEntity(string rowKey)
            : base(PARTITION_KEY, rowKey)
        { }

        public UserEntity(UserEntity entity)
            : base(PARTITION_KEY, entity.RowKey)
        {
            UserId = entity.UserId;
            UserName = entity.UserName;
            Password = entity.Password;

            UserType = entity.UserType;
            FirstName = entity.FirstName;
            LastName = entity.LastName;
            Email = entity.Email;
            Mobile = entity.Mobile;
            DateOfBirth = entity.DateOfBirth;
            Gender = entity.Gender;
            City = entity.City;
            Profile_Pic_Http = entity.Profile_Pic_Http;
            Profile_Pic_Https = entity.Profile_Pic_Https;
            Country = entity.Country;
            Status = entity.Status;
            Created_At = entity.Created_At;
            Favorite = entity.Favorite;
        }

        public override string GetKey()
        {
            return this.UserId;
        }

        public static UserEntity CreateUserEntity(string userName, string password)
        {
            var userId = Guid.NewGuid().ToString();
            var entity = new UserEntity(userId);
            entity.UserId = userId;
            entity.UserName = userName;
            entity.Password = password;
            return entity;
        }
    }
}
