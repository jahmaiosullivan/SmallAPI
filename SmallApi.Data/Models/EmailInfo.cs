using System;
using System.CodeDom;
using SmallApi.Data.Azure;

namespace SmallApi.Data.Models
{
    public class EmailInfo : AzureAdminTableEntity
    {
        public string Referrer { get; set; }

        private string listName;
        public string List
        {
            get
            {
                return listName;
            }
            set
            {
                listName = value;
                RowKey = listName + "_" + Email;
            }
        }

        private string email;
        public string Email
        {
            get
            {
                return email;
            }
            set
            {
                email = value;
                RowKey = List + "_" + email;
            }
        }

        public string ApiKey
        {
            get { return PartitionKey; }
            set { PartitionKey = value; }
        }
    }
}
