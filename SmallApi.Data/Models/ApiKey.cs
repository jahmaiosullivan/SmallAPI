using System.Collections.Generic;
using SmallApi.Data.Azure;
using SmallApi.WorkerRole;

namespace SmallApi.Data.Models
{
    public class ApiKey : AzureAdminTableEntity
    {
        public ApiKey()
        {
            IsActive = true;
            PartitionKey = "ApiKeys";
        }
        public string Key
        {
            get { return RowKey; }
            set { RowKey = value.RemoveChars(new []{'/'}); }
        }

        public string Email { get; set; }

        public bool IsActive { get; set; }
        
    }
}
