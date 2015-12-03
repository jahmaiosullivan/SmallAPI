using Microsoft.WindowsAzure.Storage.Blob;
using Microsoft.WindowsAzure.Storage.Queue;
using Microsoft.WindowsAzure.Storage.Table;

namespace SmallApi.Data.Azure
{
    public interface ICloudClientWrapper
    {
        CloudTableClient TableClient { get; }

        CloudBlobClient BlobClient { get; }

        CloudQueueClient QueueClient { get; }
    }
}