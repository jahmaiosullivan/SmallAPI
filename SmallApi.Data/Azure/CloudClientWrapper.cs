using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using Microsoft.WindowsAzure.Storage.Queue;
using Microsoft.WindowsAzure.Storage.Table;

namespace SmallApi.Data.Azure
{
    public class CloudClientWrapper : ICloudClientWrapper
    {
        private readonly CloudStorageAccount storageAccount;

        public CloudClientWrapper(CloudStorageAccount storageAccount)
        {
            this.storageAccount = storageAccount;
        }

        private CloudTableClient tableClient;
        public CloudTableClient TableClient
        {
            get { return tableClient ?? (tableClient = storageAccount.CreateCloudTableClient()); }
        }

        private CloudBlobClient blobClient;
        public CloudBlobClient BlobClient
        {
            get { return blobClient ?? (blobClient = storageAccount.CreateCloudBlobClient()); }
        }

        private CloudQueueClient queueClient;
        public CloudQueueClient QueueClient
        {
            get { return queueClient ?? (queueClient = storageAccount.CreateCloudQueueClient()); }
        }
        
    }
}
