
namespace DataStoreLib.Storage
{
    using DataStoreLib.Utils;
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

        internal IDictionary<string, Table> tableList = new ConcurrentDictionary<string, Table>();

        public static readonly string MovieTableName = "Movie";
        public static readonly string ReviewTableName = "Review";
        public static readonly string UserTableName = "User";
        public static readonly string AffilationTableName = "Affilation";
        public static readonly string ReviewerTableName = "Reviewer";
        public static readonly string ToBeIndexedTableName = "TobeIndexedTable";
        public static readonly string UserFavoriteTableName = "UserFavorites";
        public static readonly string PopularOnMovieMirchiName = "PopularOnMovieMirchi";

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
                    {PopularOnMovieMirchiName, PopularOnMovieMirchiTable.CreateTable}
                };

        public Table GetTable(string tableName)
        {
            Table table = null;
            if (tableList.ContainsKey(tableName))
            {
                table = tableList[tableName];
                Debug.Assert(table != null);
            }
            else
            {
                table = CreateTableIfNotExist(tableName);

                if (table == null)
                {
                    throw new ArgumentException(string.Format("failed to create/get table {0}", tableName));
                }

                tableList.Add(tableName, table);
                Trace.TraceInformation("Added {0} to the store", tableName);
            }

            return table;
        }

        internal Table CreateTableIfNotExist(string tableName)
        {
            Debug.Assert(!string.IsNullOrWhiteSpace(ConnectionSettingsSingleton.Instance.StorageConnectionString));
            var account = CloudStorageAccount.Parse(ConnectionSettingsSingleton.Instance.StorageConnectionString);

            var cloudTableClient = account.CreateCloudTableClient();
            var table = cloudTableClient.GetTableReference(tableName);
            table.CreateIfNotExists();

            return tableDict[tableName](table);
        }
    }
}
