using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DataStoreLib.Utils;
using DataStoreLib.Models;
using DataStoreLib.Storage;
using Microsoft.WindowsAzure.Storage.Table;

namespace DataStoreLib.Storage
{
    public class AffilationTable : Table
    {
         //protected CloudTable _table;
        public AffilationTable(CloudTable table) : base(table)
        {
        }

        internal static Table CreateTable(CloudTable table)
        {
            return new AffilationTable(table);
        }

        protected override string GetParitionKey()
        {
            return AffilationEntity.PARTITION_KEY;
        }

    }
}
