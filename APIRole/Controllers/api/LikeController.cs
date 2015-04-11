using DataStoreLib.Storage;
using Newtonsoft.Json;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace CloudMovie.APIRole.Controllers.api
{
    public class LikeController : ApiController
    {
        private static char[] splitter = new char[] { ';' };

        [HttpGet]
        [Route("api/like/get/{userId}/{type=}")]
        public string Get(string userId, string type = "")
        {
            var tableMgr = new TableManager();
            var userEntity = tableMgr.GetLikingUser(userId);
            if (string.IsNullOrWhiteSpace(type))
            {
                return JsonConvert.SerializeObject(userEntity);
            }
            else if (userEntity != null)
            {
                type = type.ToLower();
                switch (type)
                {
                    case "movie":
                        return JsonConvert.SerializeObject(userEntity.MovieId.Split(splitter, System.StringSplitOptions.RemoveEmptyEntries));
                    case "artist":
                        return JsonConvert.SerializeObject(userEntity.ArtistId.Split(splitter, System.StringSplitOptions.RemoveEmptyEntries));
                    case "reviewer":
                        return JsonConvert.SerializeObject(userEntity.ReviewerId.Split(splitter, System.StringSplitOptions.RemoveEmptyEntries));
                    default:
                        break;
                }
            }

            return "Error";
        }

        [HttpGet]
        [Route("api/like/update/{userId}/{type}/{value}/{operation}")]
        public HttpResponseMessage Update(string userId, string type, string value, string operation)
        {
            var tableMgr = new TableManager();
            var userEntity = tableMgr.UpdateLikingUser(userId, type, value, operation);
            return new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK
            };
        }
    }
}
