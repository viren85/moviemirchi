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

                var artists = tableMgr.GetAllArtist("").ToList();

                var list = Enumerable.Empty<object>()
                    .Select(r => new { Name = "", BirthDateStr = "", BirthDate = DateTime.Now }) // prototype of anonymous type
                    .ToList();

                foreach (ArtistEntity artist in artists)
                {
                    if (!string.IsNullOrEmpty(artist.Born))
                    {
                        string birthDateStr;
                        DateTime birthDate = Utils.GetBornDate(artist.Born, out birthDateStr);

                        if (!string.IsNullOrEmpty(birthDateStr))
                        {
                            list.Add(new { Name = artist.ArtistName, BirthDateStr = birthDateStr, BirthDate = birthDate });
                        }
                    }
                }

                list = list.Where(ad => ad.BirthDate >= DateTime.Now && ad.BirthDate < DateTime.Now.AddDays(10)).ToList().OrderBy(ad => ad.BirthDate).ToList();

                return jsonSerializer.Value.Serialize(list);
            }
            catch (Exception)
            {
                return jsonSerializer.Value.Serialize(new { Status = "Error", Message = "Unable to get upcoming birthdays of Artists." });
            }
        }
    }
}
