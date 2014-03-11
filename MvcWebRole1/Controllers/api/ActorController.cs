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
    public class ActorController : BaseController
    {
        protected override string ProcessRequest()
        {
            

            JavaScriptSerializer json = new JavaScriptSerializer();  
            var qpParam = HttpUtility.ParseQueryString(this.Request.RequestUri.Query);

            if (string.IsNullOrEmpty(qpParam["searchActor"]))
            {
                throw new ArgumentException("Search Actor/Actress is not found");
            }

            string searchActor = qpParam["searchActor"].ToString();

            var tableMgr = new TableManager();
            var actorList = tableMgr.SearchActor(searchActor);


            List<Cast> actors = new List<Cast>();

            if (actorList != null)
            {
                foreach (var actor in actorList)
                {
                    List<Cast> acts = json.Deserialize(actor.Casts, typeof(Cast)) as List<Cast>;  

                    if (actors != null)
                    {
                        foreach (var act in acts)
                        {
                            actors.Add(act);
                        }
                    }

                }
            }
            return json.Serialize(actors);
        }
    }
}
