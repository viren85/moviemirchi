
namespace CloudMovie.APIRole.API
{
    using DataStoreLib.Models;
    using DataStoreLib.Storage;
    using DataStoreLib.Utils;
    using LuceneSearchLibrary;
    using Microsoft.WindowsAzure;
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Web;
    using System.Web.Mvc;
    using System.Web.Script.Serialization;

    public class AutoCompleteController : BaseController
    {
        protected override string ProcessRequest()
        {
            string queryParameters = this.Request.RequestUri.Query;
            JavaScriptSerializer json = new JavaScriptSerializer();

            if (queryParameters != null)
            {
                var qpParams = HttpUtility.ParseQueryString(queryParameters);

                if (!string.IsNullOrEmpty(qpParams["query"]))
                {
                    string query = qpParams["query"].ToString().ToLower();
                    var users = LuceneSearch.Search(query);

                    return json.Serialize(users);
                }
                else
                    return json.Serialize(new MovieSearchData());
            }

            return string.Empty;
        }
    }
}
