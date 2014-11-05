
namespace CloudMovie.APIRole.API
{
    using System;
    using DataStoreLib.Storage;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using System.Web.Script.Serialization;
    using System.Configuration;
    using Crawler;
    using System.Web.Http;
    using System.Xml;
    using System.Diagnostics;
    using LuceneSearchLibrary;
    using CloudMovie.APIRole.UDT;
    using DataStoreLib.Models;
    using Crawler.Reviews;
    using MovieCrawler;
    using System.Security.Cryptography;
    using System.Text;

    /// <summary>
    /// This API returns list of all the movies details from the file on type.    
    /// </summary>
    public class UserSignupController : BaseController
    {
        private static Lazy<JavaScriptSerializer> jsonSerializer = new Lazy<JavaScriptSerializer>(() => new JavaScriptSerializer());
        private static object _object = new object();

        protected override string ProcessRequest()
        {
            return string.Empty;
        }

        [HttpPost]
        public string Register(UserDetails data)
        {
            if (data == null || string.IsNullOrEmpty(data.UserId))
            {
                return "ERROR";
            }

            // Validate the details before storing it in DB
            // Encrypt the password before storing it in DB
            try
            {
                JavaScriptSerializer json = new JavaScriptSerializer();
                TableManager tblMgr = new TableManager();

                IDictionary<string, UserEntity> users = tblMgr.GetUsersByName(data.UserId);
                if (users != null && users.Count > 0)
                {
                    return "OK";
                }

                if (data != null)
                {
                    UserEntity user = new UserEntity();
                    user.UserName = data.UserId;
                    user.Password = string.IsNullOrEmpty(data.UserType) ? GetHashPassword(data.Password) : data.Country; // Country field is having the access token
                    user.City = string.IsNullOrEmpty(data.City) ? string.Empty : data.City;
                    user.Created_At = DateTime.Now;
                    user.DateOfBirth = string.IsNullOrEmpty(data.DateOfBirth) ? string.Empty : data.DateOfBirth;
                    user.Email = data.Email;
                    //user.Country = string.IsNullOrEmpty(data.Country) ? string.Empty : data.Country;
                    user.Profile_Pic_Http = string.IsNullOrEmpty(data.ProfilePic) ? string.Empty : data.ProfilePic;
                    user.Profile_Pic_Https = string.IsNullOrEmpty(data.SecureProfilePic) ? string.Empty : data.SecureProfilePic;
                    user.FirstName = data.FirstName;
                    user.Gender = string.IsNullOrEmpty(data.Gender) ? string.Empty : data.Gender;
                    user.LastName = data.LastName;
                    user.Mobile = string.IsNullOrEmpty(data.UserType) ? string.Empty : data.Mobile;
                    user.UserId = user.RowKey = Guid.NewGuid().ToString();
                    user.UserType = string.IsNullOrEmpty(data.UserType) ? "Application" : data.UserType;
                    user.Status = 1;

                    tblMgr.UpdateUserById(user);
                }
            }
            catch (Exception ex)
            {
                return "ERROR";
            }

            return "OK";
        }

        private string GetHashPassword(string password)
        {
            using (MD5 md5Hash = MD5.Create())
            {
                byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(password));

                StringBuilder sBuilder = new StringBuilder();

                for (int i = 0; i < data.Length; i++)
                {
                    sBuilder.Append(data[i].ToString("x2"));
                }

                return sBuilder.ToString();
            }
        }
    }

    public class UserDetails
    {
        public string UserId { get; set; }
        public string Password { get; set; }
        public string UserType { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Mobile { get; set; }
        public string DateOfBirth { get; set; }
        public string Gender { get; set; }
        public string City { get; set; }

        public string Country { get; set; }
        public string ProfilePic { get; set; }
        public string SecureProfilePic { get; set; }
        public string AccessToken { get; set; }
        public string Favorite { get; set; }
    }
}
