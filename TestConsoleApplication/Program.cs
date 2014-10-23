using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.Storage;
using DataStoreLib.Storage;
using DataStoreLib.Models;
using Microsoft.WindowsAzure.Storage.Blob;
using Microsoft.WindowsAzure.Storage.Table;
using Microsoft.WindowsAzure.Storage.Table.Queryable;
using System.Linq.Expressions;

namespace TestConsoleApplication
{
    class Program
    {
        static void Main(string[] args)
        {
            var connectionString = CloudConfigurationManager.GetSetting("StorageTableConnectionString");
            var account = CloudStorageAccount.Parse(connectionString);
            var cloudTableClient = account.CreateCloudTableClient();
            var artistTable = cloudTableClient.GetTableReference("Artist");

            //const string artistName = "Jayaprakash";
            //var artistTable = TableStore.Instance.GetTable(TableStore.ArtistTableName);
            var artistQuery = (from ent in artistTable.CreateQuery<ArtistEntity>()
                          //where string.Equals(ent.ArtistName.ToLower().Trim(), "jayaprakash")
                               //where ent.ArtistName.Contains("Amar Shetty")
                               select ent).Where(e => e.RowKey.StartsWith("03ce6e61"));

            foreach (var artist in artistQuery)
            {
                Console.WriteLine(artist.ArtistName);
            }

            //Console.WriteLine(artist.FirstOrDefault().ArtistName);
        }
    }
}
