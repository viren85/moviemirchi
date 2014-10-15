
namespace DataStoreLib.Models
{
    using Microsoft.WindowsAzure.Storage.Table;
    using System;
    using System.Collections.Generic;

    public class UserEntity : TableStorageEntity
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
        public string SiteFeedbackScore { get; set; }

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
            SiteFeedbackScore = entity.SiteFeedbackScore;
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
