using Microsoft.Azure;

namespace TestConsoleApplication
{
    class Program
    {
        static void Main(string[] args)
        {
            var connectionString = CloudConfigurationManager.GetSetting("StorageTableConnectionString");
            var account = Microsoft.WindowsAzure.Storage.CloudStorageAccount.Parse(connectionString);
            var cloudTableClient = account.CreateCloudTableClient();
            //var artistTable = cloudTableClient.GetTableReference("Artist");
            //var artistTableBackup = cloudTableClient.GetTableReference("ArtistBackup");
            //const string artistName = "Jayaprakash";
            //var artistTable = TableStore.Instance.GetTable(TableStore.ArtistTableName);
            //var artistQuery = (from ent in artistTableBackup.CreateQuery<ArtistEntity>()
            //where string.Equals(ent.ArtistName.ToLower().Trim(), "jayaprakash")
            //where ent.ArtistName.Contains("Amar Shetty")
            //select ent);
            //;//.Where(e => e.RowKey.StartsWith("03ce6e61"));

            //var query = new TableQuery<ArtistEntity>()
            //    .Where(TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, "CloudMovie"));
            //var artistBackups = artistTableBackup.ExecuteQuery(query).ToList();

            //var filterByUniqueName = TableQuery.CombineFilters(
            //            TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, "CloudMovie"),
            //            TableOperators.And,
            //            TableQuery.GenerateFilterCondition("RowKey", QueryComparisons.Equal, "Mithun-D'Souza"));
            //var query1 = new TableQuery<ArtistEntity>().Where(filterByUniqueName);
            //var artists = artistTable.ExecuteQuery(query).ToList();

            //var artistMissing = artistBackups.Where(artistEntity => (artists.FirstOrDefault(a => string.Equals(artistEntity.RowKey, a.RowKey)) == null)).ToList();
            //foreach (var a in artistMissing)
            //{
            //    artistTable.Execute(TableOperation.Insert(a));
            //}

            //var temp = new Dictionary<string, IList<ArtistEntity>>();
            //long t = 0;
            //artistBackups.ForEach(a =>
            //{
            //    if (temp.ContainsKey(a.UniqueName))
            //    {
            //        temp[a.UniqueName].Add(a);
            //        Console.WriteLine(temp[a.UniqueName].Count);
            //    }
            //    else
            //    {
            //        var l = new List<ArtistEntity>() { a };
            //        temp[a.UniqueName] = l;
            //    }
            //});

            //using (var s = new StreamWriter(@"X:\tempMovie.txt"))
            //{
            //    foreach (var ttt in temp)
            //    {
            //        if (ttt.Value.Count > 1)
            //        {
            //            s.WriteLine(ttt.Key);
            //            foreach (var un in ttt.Value)
            //            {
            //                string outputString =
            //                    string.Format(
            //                        "UniqueName: '{0}', ArtistId: '{1}', ArtistName: '{2}', Born: '{3}', MovieList: '{4}', Popularity: '{5}', Posters: '{6}', MyScore: '{7}', JsonString: '{8}', Bio: '{9}'",
            //                        un.UniqueName, un.ArtistId, un.ArtistName, un.Born, un.MovieList, un.Popularity,
            //                        un.Posters, un.MyScore, un.JsonString, un.Bio);
            //                s.WriteLine(outputString);
            //            }

            //            s.WriteLine();
            //            s.WriteLine();
            //            s.WriteLine();
            //            s.WriteLine();
            //            s.WriteLine();
            //            s.WriteLine();

            //            t++;
            //        }

            //    }
            //}

            //long count = 0;
            //foreach (var artist in artists)
            //{
                //    //Console.WriteLine(artist.ArtistName);
                //    //artist.RowKey = artist.UniqueName;
                //    //artistTableBackup.Execute(TableOperation.Insert(artist));

                //    //if (string.Equals(artist.UniqueName, artist.RowKey))
                //    //{
                //    //    //

                //    //}
                //    //else
                //    //{
                //try
                //{
                //    if (artist.RowKey.Contains("deepika"))
                //    {
                //        count++;
                //    }
                    //var newartistEntity = new ArtistEntity()
                    //{
                    //    PartitionKey = artist.PartitionKey,
                    //    RowKey = artist.UniqueName.ToLower(),
                    //    ArtistId = artist.ArtistId,
                    //    ArtistName = artist.ArtistName,
                    //    UniqueName = artist.UniqueName,
                    //    Bio = artist.Bio,
                    //    Born = artist.Born,
                    //    MovieList = artist.MovieList,
                    //    Popularity = artist.Popularity,
                    //    Posters = artist.Posters,
                    //    MyScore = artist.MyScore,
                    //    JsonString = artist.JsonString
                    //};
                    //TableBatchOperation batchOperation = new TableBatchOperation();
                    //batchOperation.Add(TableOperation.Delete(artist));
                    //batchOperation.Add(TableOperation.Insert(newartistEntity));
                    //artistTable.ExecuteBatch(batchOperation);
                    
                //}
                //catch (Exception)
                //{
                //    count++;
                    //Console.WriteLine(artist.UniqueName);
                //}
                //    //}
                //    //break;
            //}

            //Console.WriteLine(count);
            //Console.WriteLine(artist.FirstOrDefault().ArtistName);
        }
    }
}