using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text;
using Microsoft.WindowsAzure.Storage.Blob;
using Newtonsoft.Json;
using SmallApi.Data.Azure;

namespace SmallApi.Data.Repository
{
    public class BlobStorageRepository<T> : IBlobStorageRepository<T>  where T : new()
    {
        private readonly ICloudClientWrapper cloudClientWrapper;

        public BlobStorageRepository(ICloudClientWrapper cloudClientWrapper)
        {
            this.cloudClientWrapper = cloudClientWrapper;
        }
        

        public CloudBlobContainer GetContainer(string containerName)
        {
            containerName = containerName.ToLower();
            // Retrieve a reference to a container. 
            var container = cloudClientWrapper.BlobClient.GetContainerReference(containerName);

            // Create the container if it doesn't already exist.
            container.CreateIfNotExists();

            container.SetPermissions(new BlobContainerPermissions { PublicAccess = BlobContainerPublicAccessType.Blob });

            return container;
        }

        public void UploadImage(string filePath)
        {
            var filename = Path.GetFileName(filePath);
            var blockBlob = GetImageBlob(filename);
            blockBlob.Properties.ContentType = "image\\jpeg";

            using (var fileStream = File.OpenRead(filePath))
            {
                blockBlob.UploadFromStream(fileStream);
            }
        }

        public void DownloadImage(string fileName, string outputFilePath)
        {
            var blockBlob = GetImageBlob(fileName);
            blockBlob.Properties.ContentType = "image\\jpeg";

            // Save blob contents to a file.
            using (var fileStream = File.OpenWrite(outputFilePath))
            {
                blockBlob.DownloadToStream(fileStream);
            }
        }

        public void DeleteImage(string fileName)
        {
            var image = GetImageBlob(fileName);
            // Delete the blob.
            image.Delete();
        }

        public Image GetImage(string fileName)
        {
            var blockBlob = GetImageBlob(fileName);
            blockBlob.Properties.ContentType = "image\\jpeg";

            Image image;
            using (var memoryStream = new MemoryStream())
            {
                blockBlob.DownloadToStream(memoryStream);
                image = Image.FromStream(memoryStream);
            }

            return image;
        }

        public CloudBlockBlob GetImageBlob(string fileName)
        {
            // Retrieve reference to a previously created container.
            var container = GetContainer("Images");

            // Retrieve reference to a blob named "myblob.txt".
            return container.GetBlockBlobReference(fileName);
        }

        public IEnumerable<IListBlobItem> GetBlobs(CloudBlobContainer container)
        {
            return container.ListBlobs(null, true);
        }
        
        
        public void Insert(string containerName, string id, T item)
        {
            var container = GetContainer(containerName);
            
            var blockBlob = container.GetBlockBlobReference(id);
            
            var jsonSerializer = JsonSerializer.Create();
            using (var stream = new MemoryStream())
            {
                using (var streamWriter = new StreamWriter(stream, Encoding.UTF8))
                using (var jsonWriter = new JsonTextWriter(streamWriter))
                {
                    jsonSerializer.Serialize(jsonWriter, item);
                    blockBlob.UploadFromStream(stream);
                }
            }
        }
    }
}