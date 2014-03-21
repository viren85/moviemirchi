using DataStoreLib.Constants;
using DataStoreLib.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Script.Serialization;

namespace MvcWebRole1.Controllers.api
{
    public class SaveUserFavoriteController : BaseController
    {
        protected override string ProcessRequest()
        {
            JavaScriptSerializer json = new JavaScriptSerializer();

            try
            {
                var tableMgr = new TableManager();

                // get query string parameters
                var qpParams = HttpUtility.ParseQueryString(this.Request.RequestUri.Query);
                if (string.IsNullOrEmpty(qpParams["u"]) || string.IsNullOrEmpty(qpParams["d"]))
                {
                    throw new ArgumentException(Constants.API_EXC_SEARCH_TEXT_NOT_EXIST);
                }

                string userId = qpParams["u"];
                string favorites = qpParams["d"];

                var user = tableMgr.GetUserById(userId);

                if (user != null)
                {
                    user.Favorite = favorites;
                    tableMgr.UpdateUserById(user);

                    // serialize songs list and then return.
                    return json.Serialize(new { Status = "Ok", Favorite= favorites });
                }
                else
                {
                    return json.Serialize(new { Status = "Error" });
                }
            }
            catch (Exception ex)
            {
                // if any error occured then return User friendly message with system error message
                return json.Serialize(new { Status = "Error", UserMessage = Constants.UM_WHILE_SEARCHING_SONGS, ActualError = ex.Message });
            }

        }
    }
}
