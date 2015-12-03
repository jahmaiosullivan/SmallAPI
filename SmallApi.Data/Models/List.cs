using System;
using SmallApi.Data.Azure;

namespace SmallApi.Data.Models
{
    public class List : AzureAdminTableEntity
    {
        public string Name
        {
            get { return RowKey; }
            set { RowKey = value.ToLower().Trim(); }
        }

        public string ApiKey
        {
            get { return PartitionKey; }
            set { PartitionKey = value; }
        }
    }
}
