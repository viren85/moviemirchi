using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace MvcWebRole1.Controllers.api
{
    public class PopularController : BaseController
    {
        private static Lazy<JavaScriptSerializer> jsonSerializer = new Lazy<JavaScriptSerializer>(() => new JavaScriptSerializer());
        protected override string ProcessRequest()
        {
            string queryParameters = this.Request.RequestUri.Query;
            string popularType = "all";
            if (!string.IsNullOrWhiteSpace(queryParameters))
            {
                var qpParams = HttpUtility.ParseQueryString(queryParameters);

                if (!string.IsNullOrEmpty(qpParams["type"]))
                {
                    popularType = qpParams["type"].ToString().ToLower();
                }

                /* Prepare JSON object like:
                [
                    [
                        { "UniqueName": "taran-adarsh", "Name": "Taran Adarsh", "Role": "Reviewer", "Weight": "3" },
                        { "UniqueName": "anupama-chopra", "Name": "Anupama Chopra", "Role": "Reviewer", "Weight": "4" },
                        { "UniqueName": "rachit-gupta", "Name": "Rachit Gupta", "Role": "Reviewer", "Weight": "2" }
                    ],
                    [
                        { "UniqueName": "mickey-virus", "Name": "Mickey Virus", "Role": "Movie", "Weight": "1" },
                        { "UniqueName": "krrish-3", "Name": "Krrish 3", "Role": "Movie", "Weight": "4" }
                    ],
                    [
                        { "UniqueName": "Deepika-Padukone", "Name": "Deepika Padukone", "Role": "Artists", "Weight": "5" },
                        { "UniqueName": "ranveer-singh", "Name": "Ranveer Singh", "Role": "Artists", "Weight": "4" },
                        { "UniqueName": "aditya-roy-kapoor", "Name": "Aditya Roy Kapoor", "Role": "Artists", "Weight": "1" },
                        { "UniqueName": "sanjay-leela-bhansali", "Name": "Sanjay Leela Bhansali", "Role": "Artists", "Weight": "2" }
                    ],
                    [
                        { "UniqueName": "Romance", "Name": "Romance", "Role": "Genre", "Weight": "5" },
                        { "UniqueName": "Action", "Name": "Action", "Role": "Genre", "Weight": "3" },
                        { "UniqueName": "Drama", "Name": "Drama", "Role": "Genre", "Weight": "1" }
                    ]
                ]
                */
                try
                {
                    switch (popularType)
                    {
                        case "movie":
                            break;
                        case "artist":
                            break;
                        case "genre":
                            break;
                        case "critic":
                            break;
                        case "all":
                            break;
                    }
                }
                catch (Exception ex)
                {
                    // if any error occured then return User friendly message with system error message
                    return jsonSerializer.Value.Serialize(new { Status = "Error", UserMessage = "Error occurred while getting popular tags", ActualError = ex.Message });
                }
            }

            return string.Empty;
        }
    }
}
