
namespace MvcWebRole1.Controllers
{
    using DataStoreLib.Models;
    using DataStoreLib.Storage;
    using System;
    using System.Configuration;
    using System.Security.Cryptography;
    using System.Text;
    using System.Web.Mvc;
    using System.Web.Script.Serialization;

    public class LoginController : Controller
    {
        private const string SUCCESS = "OK";
        private const string INVALID = "INVALID";
        private const string MISSING = "MISSING";
        private const string ERROR = "ERROR";

        [HttpPost]
        public string UserLogin(LoginProperties data)
        {
            if (data == null)
            {
                return MISSING;
            }

            try
            {
                JavaScriptSerializer json = new JavaScriptSerializer();
                UserEntity auth = new UserEntity();

                auth.UserName = data.UserName;
                auth.Password = GetHashPassword(data.Password);

                if (!string.IsNullOrEmpty(auth.UserName))
                {
                    TableManager tblMgr = new TableManager();
                    UserEntity user = tblMgr.GetUserByName(auth.UserName);

                    if (user != null && user.UserName == auth.UserName && user.Password == auth.Password)
                    {
                        return Login(user.UserId, user.UserType, user.FirstName, user.LastName, user.Email, user.Mobile, user.DateOfBirth, user.Gender, user.City, user.Favorite);
                    }
                    else
                    {
                        return INVALID;
                    }
                }
                else
                {
                    return MISSING;
                }
            }
            catch (Exception)
            {
                return ERROR;
            }
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
        public string Login(string userid, string usertype, string firstName, string lastName, string email, string mobile, string dob, string gender, string city, string favorite)
        {
            if (Session["username"] != null)
            {
                return "OK";
            }

            try
            {
                // Set session
                Session["userid"] = userid;
                Session["type"] = usertype;
                Session["firstname"] = firstName;
                Session["lastname"] = lastName;
                Session["email"] = email;
                Session["mobile"] = mobile;
                Session["dob"] = dob;
                Session["gender"] = gender;
                Session["city"] = city;
                Session["username"] = firstName + " " + lastName;
                Session["favorite"] = favorite;

                return "OK";
            }
            catch (Exception)
            {
                return "ERROR";
            }
        }

        public void Logout()
        {
            if (Session["username"] != null)
            {
                Session["username"] = null;
                Session.Abandon();
            }
        }
    }

    public class LoginProperties
    {
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
