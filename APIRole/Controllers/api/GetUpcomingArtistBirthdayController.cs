/// <summary>
/// This api returns list of artist who have birthday in upcoming 10 days in json format
/// each list object contains three prop, ArtistName, BirthDateStr and BirthDate
/// </summary>

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
    
    // get : api/GetUpcomingArtistBirthday
    public class GetUpcomingArtistBirthdayController : BaseController
    {
        private static Lazy<JavaScriptSerializer> jsonSerializer = new Lazy<JavaScriptSerializer>(() => new JavaScriptSerializer());

        protected override string ProcessRequest()
        {
            try
            {                
                var tableMgr = new TableManager();

                var artists = tableMgr.GetAllArtist("");
                var now = DateTime.Now.Date;
                var nextNow = now.AddDays(10);

                var selectedArtist =
                  artists
                      .Where(artist => !string.IsNullOrEmpty(artist.Born))
                      .Select(artist =>
                      {
                          string birthDateStr;
                          DateTime birthDate = Utils.GetBornDate(artist.Born, out birthDateStr);

                          if (!string.IsNullOrEmpty(birthDateStr) && (birthDate >= now && birthDate <= nextNow))
                          {
                              return new { Name = artist.ArtistName, BirthDateStr = birthDateStr, BirthDate = birthDate };
                          }

                          return null;
                      })
                      .Where(ad => ad != null).OrderBy(o => o.BirthDate);

                return jsonSerializer.Value.Serialize(selectedArtist);
            }
            catch (Exception)
            {
                return jsonSerializer.Value.Serialize(new { Status = "Error", Message = "Unable to get upcoming birthdays of Artists." });
            }
        }
    }
}
