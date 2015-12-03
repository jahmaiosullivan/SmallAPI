using System.Collections.Generic;
using System.Drawing;
using Microsoft.WindowsAzure.Storage.Blob;

namespace SmallApi.Data.Repository
{
    public interface IBlobStorageRepository<T>
    {
        CloudBlobContainer GetContainer(string containerName);
        void UploadImage(string filePath);
        void DownloadImage(string fileName, string outputFilePath);
        void DeleteImage(string fileName);
        Image GetImage(string fileName);
        CloudBlockBlob GetImageBlob(string fileName);
        IEnumerable<IListBlobItem> GetBlobs(CloudBlobContainer container);
        void Insert(string containerName, string id, T item);
    }
}