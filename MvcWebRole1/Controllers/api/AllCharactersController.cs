using DataStoreLib.Models;
using DataStoreLib.Storage;
using DataStoreLib.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web;
using System.Web.Script.Serialization;


namespace MvcWebRole1.Controllers.api
{
    public class AllCharactersController : BaseController
    {
        protected override string ProcessRequest()
        {


            JavaScriptSerializer json = new JavaScriptSerializer();
            var qpParam = HttpUtility.ParseQueryString(this.Request.RequestUri.Query);

            if (string.IsNullOrEmpty(qpParam["searchCharacter"]))
            {
                throw new ArgumentException("Search Actor/Actress is not found");
            }

            string searchCharacter = qpParam["searchCharacter"].ToString();

            var tableMgr = new TableManager();

            var movieEntity = tableMgr.SearchCharacter(searchCharacter);

            List<Cast> castList = new List<Cast>();

            if (movieEntity != null)
            {
                foreach (var cast in movieEntity)
                {
                    List<Cast> casts = json.Deserialize(cast.Casts, typeof(Cast)) as List<Cast>;

                    if (castList != null)
                    {
                        foreach (var cas in casts)
                        {
                            castList.Add(cas);
                        }
                    }
                }
            }
            // return json.Serialize();
            return json.Serialize(castList);
        }
    }
}
