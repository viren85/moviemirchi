
namespace CloudMovie.APIRole.API
{
    using CloudMovie.APIRole.UDT;
    using DataStoreLib.Models;
    using DataStoreLib.Storage;
    using DataStoreLib.Utils;
    using LuceneSearchLibrary;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using System.Web.Script.Serialization;

    public class UpdateArtistController : BaseController
    {
        private static Lazy<JavaScriptSerializer> jsonSerializer = new Lazy<JavaScriptSerializer>(() => new JavaScriptSerializer());

        protected override string ProcessRequest()
        {
            return string.Empty;
        }

        [AcceptVerbs("POST")]
        public ActionResult UpdateArtist(ArtistPostData data)
        {
            if (data == null || string.IsNullOrEmpty(data.ArtistName) || string.IsNullOrEmpty(data.UniqueName))
            {
                return null;
            }

            try
            {
                var tableMgr = new TableManager();
                ArtistEntity artist = data.GetArtistEntity();
                //artist.RowKey = artist.ArtistId;
                // as per current records in our database.
                artist.RowKey = artist.UniqueName.ToLower();
                tableMgr.UpdateArtistById(artist);
            }
            catch (Exception)
            {
                return null;
            }

            return null;
        }
    }
}
