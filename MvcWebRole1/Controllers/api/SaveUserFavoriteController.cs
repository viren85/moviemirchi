
namespace MvcWebRole1.Controllers.api
{
    using DataStoreLib.Constants;
    using DataStoreLib.Models;
    using DataStoreLib.Storage;
    using System;
    using System.Collections.Generic;
    using System.Web;
    using System.Web.Script.Serialization;

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
                if (string.IsNullOrEmpty(qpParams["d"]))
                {
                    //throw new ArgumentException(Constants.API_EXC_SEARCH_TEXT_NOT_EXIST);
                    return json.Serialize(new { Status = "Error", Message = Constants.API_EXC_SEARCH_TEXT_NOT_EXIST });
                }

                string userId = qpParams["u"];
                string cFavoriteId = qpParams["c"];
                string favorites = qpParams["d"];

                List<PopularOnMovieMirchiEntity> popularOnMovieMirchi = json.Deserialize(favorites, typeof(List<PopularOnMovieMirchiEntity>)) as List<PopularOnMovieMirchiEntity>;

                if (popularOnMovieMirchi != null)
                {
                    foreach (PopularOnMovieMirchiEntity objPopular in popularOnMovieMirchi)
                    {
                        PopularOnMovieMirchiEntity oldObjPopular = tableMgr.GetPopularOnMovieMirchiById(objPopular.Name + "=" + objPopular.Type);

                        if (oldObjPopular == null)
                        {
                            objPopular.PopularOnMovieMirchiId = Guid.NewGuid().ToString();
                            objPopular.RowKey = objPopular.Name + "=" + objPopular.Type;
                            objPopular.Counter = 1;
                            objPopular.DateUpdated = DateTime.Now.ToString();

                            tableMgr.UpdatePopularOnMovieMirchiId(objPopular);
                        }
                        else
                        {
                            oldObjPopular.Counter = oldObjPopular.Counter + 1;
                            objPopular.DateUpdated = DateTime.Now.ToString();

                            tableMgr.UpdatePopularOnMovieMirchiId(oldObjPopular);
                        }
                    }
                }

                if (string.IsNullOrEmpty(userId) && string.IsNullOrEmpty(cFavoriteId))
                {
                    UserFavoriteEntity userFavorite = new UserFavoriteEntity();
                    userFavorite.RowKey = userFavorite.UserFavoriteId = Guid.NewGuid().ToString();
                    userFavorite.UserId = "";
                    userFavorite.Favorites = favorites;
                    userFavorite.DateCreated = DateTime.Now.ToString();

                    tableMgr.UpdateUserFavoriteById(userFavorite);

                    return json.Serialize(new { Status = "Ok", Message = "Set Cookie", FavoriteId = userFavorite.UserFavoriteId });
                }
                else if (!string.IsNullOrEmpty(userId) && userId.ToLower() != "undefined" && !string.IsNullOrEmpty(cFavoriteId))
                {
                    UserFavoriteEntity userFavorite = tableMgr.GetUserFavoritesByUserId(userId);

                    if (userFavorite == null)
                    {
                        userFavorite = tableMgr.GetUserFavoriteById(cFavoriteId);

                        if (userFavorite != null)
                        {
                            userFavorite.UserId = userId;
                            userFavorite.Favorites = favorites;
                            userFavorite.DateCreated = DateTime.Now.ToString();

                            tableMgr.UpdateUserFavoriteById(userFavorite);
                        }
                    }
                    else
                    {
                        userFavorite.Favorites = favorites;
                        userFavorite.DateCreated = DateTime.Now.ToString();

                        tableMgr.UpdateUserFavoriteById(userFavorite);
                    }

                    return json.Serialize(new { Status = "Ok", Message = "Updated" });
                }
                else if (!string.IsNullOrEmpty(userId) && string.IsNullOrEmpty(cFavoriteId))
                {
                    UserFavoriteEntity userFavorite = tableMgr.GetUserFavoritesByUserId(userId);

                    if (userFavorite != null)
                    {
                        userFavorite.Favorites = favorites;
                        userFavorite.DateCreated = DateTime.Now.ToString();

                        tableMgr.UpdateUserFavoriteById(userFavorite);

                        return json.Serialize(new { Status = "Ok", Message = "Updated" });
                    }                    
                }

                return json.Serialize(new { Status = "Error", Message = "No Updated" });
            }
            catch (Exception ex)
            {
                // if any error occured then return User friendly message with system error message
                
                return json.Serialize(
                  new
                  {
                      Status = "Error",
                      UserMessage = "Unable to save your favorites. Please try again later.",
                      ActualError = ex.Message
                  });
            }

        }
    }
}
