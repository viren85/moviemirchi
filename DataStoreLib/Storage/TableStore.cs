
namespace DataStoreLib.Storage
{
    using DataStoreLib.Utils;
    using Microsoft.WindowsAzure;
    using Microsoft.WindowsAzure.Storage;
    using Microsoft.WindowsAzure.Storage.Table;
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Diagnostics;

    public class TableStore
    {
        #region singleton implementation

        protected static TableStore _instance;
        protected static object lockObj = new object();

        protected TableStore()
        {
        }

        public static TableStore Instance
        {
            get
            {
                lock (lockObj)
                {
                    if (_instance == null)
                    {
                        _instance = new TableStore();
                    }
                    return _instance;
                }
            }
        }

        #endregion

        internal ConcurrentDictionary<string, Table> tableList = new ConcurrentDictionary<string, Table>();

        public static readonly string MovieTableName = "Movie";
        public static readonly string ReviewTableName = "Review";
        public static readonly string UserTableName = "User";
        public static readonly string AffilationTableName = "Affilation";
        public static readonly string ReviewerTableName = "Reviewer";
        public static readonly string ToBeIndexedTableName = "TobeIndexedTable";
        public static readonly string UserFavoriteTableName = "UserFavorites";
        public static readonly string PopularOnMovieMirchiName = "PopularOnMovieMirchi";
        public static readonly string TwitterTableName = "Twitter";
        public static readonly string NewsTableName = "News";
        public static readonly string ArtistTableName = "Artist";

        internal IDictionary<string, Func<CloudTable, Table>> tableDict =
            new Dictionary<string, Func<CloudTable, Table>>()
                {
                    {MovieTableName, MovieTable.CreateTable},
                    {UserTableName, UserTable.CreateTable},
                    {AffilationTableName, AffilationTable.CreateTable},
                    {ReviewerTableName, ReviewerTable.CreateTable},                    
                    {ReviewTableName, ReviewTable.CreateTable},
                    {ToBeIndexedTableName, ToBeIndexedTable.CreateTable},
                    {UserFavoriteTableName, UserFavoriteTable.CreateTable},
                    {PopularOnMovieMirchiName, PopularOnMovieMirchiTable.CreateTable},
                    {TwitterTableName,TwitterTable.CreateTable},
                    {NewsTableName,NewsTable.CreateTable},
                    {ArtistTableName,ArtistTable.CreateTable}
                };

        public Table GetTable(string tableName)
        {
            Table table = null;
            if (!tableList.TryGetValue(tableName, out table))
            {
                table = CreateTableIfNotExist(tableName);

                bool add = tableList.TryAdd(tableName, table);
                Trace.TraceInformation("{1} {0} to the store", tableName, add? "Added" : "Failed to add");
            }

            Debug.Assert(table != null);
            return table;
        }

        internal Table CreateTableIfNotExist(string tableName)
        {
            if (string.IsNullOrEmpty(ConnectionSettingsSingleton.Instance.StorageConnectionString))
            {
                ConnectionSettingsSingleton.Instance.StorageConnectionString = CloudConfigurationManager.GetSetting("StorageTableConnectionString");
            }

            Debug.Assert(!string.IsNullOrWhiteSpace(ConnectionSettingsSingleton.Instance.StorageConnectionString));
            var account = Microsoft.WindowsAzure.Storage.CloudStorageAccount.Parse(ConnectionSettingsSingleton.Instance.StorageConnectionString);

            var cloudTableClient = account.CreateCloudTableClient();
            var table = cloudTableClient.GetTableReference(tableName);
            table.CreateIfNotExists();

            if (table == null)
            {
                throw new ArgumentException(string.Format("failed to create/get table {0}", tableName));
            }

            return tableDict[tableName](table);
        }
    }
}
