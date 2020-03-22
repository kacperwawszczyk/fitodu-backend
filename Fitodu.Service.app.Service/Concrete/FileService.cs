using System;
using System.IO;
using System.Threading.Tasks;
using Fitodu.Core.Enums;
using Fitodu.Service.Abstract;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.Storage;
using Microsoft.Azure.Storage.Blob;
using Microsoft.Extensions.Configuration;

namespace Fitodu.Service.Concrete
{
    public class FileService : IFileService
    {
        private readonly string _storageConnection;

        public FileService(IConfiguration configuration)
        {
            _storageConnection = configuration["ConnectionStrings:StorageConnection"];
        }

        public async Task Save(IFormFile file, FolderName folder, Guid fileId, bool isPrivate = true)
        {
            // Retrieve reference to a blob
            var blobContainer = GetBlobContainer(folder, isPrivate);
            var blob = blobContainer.GetBlockBlobReference(fileId + file.FileName);

            // Set the blob content type
            blob.Properties.ContentType = file.ContentType;

            await blob.UploadFromStreamAsync(file.OpenReadStream());
        }

        public async Task<MemoryStream> Get(FolderName folder, Guid fileId, string fileName)
        {
            // Retrieve reference to a blob
            var blobContainer = GetBlobContainer(folder);
            var blob = blobContainer.GetBlockBlobReference(fileId + fileName);

            var fileStream = new MemoryStream();
            await blob.DownloadToStreamAsync(fileStream);

            fileStream.Position = 0;

            return fileStream;
        }

        public async Task<bool> Exists(FolderName folder, Guid fileId, string fileName)
        {
            // Retrieve reference to a blob
            var blobContainer = GetBlobContainer(folder);
            var blob = blobContainer.GetBlockBlobReference(fileId + fileName);

            return await blob.ExistsAsync();
        }

        public async Task<bool> Delete(FolderName folder, Guid fileId, string fileName)
        {
            // Retrieve reference to a blob
            var blobContainer = GetBlobContainer(folder);
            var blob = blobContainer.GetBlockBlobReference(fileId + fileName);

            return await blob.DeleteIfExistsAsync();
        }

        private CloudBlobContainer GetBlobContainer(FolderName folder, bool isPrivate = true)
        {
            // use the connection string to get the storage account
            var storageAccount = CloudStorageAccount.Parse(_storageConnection);

            // using the storage account, create the blob client
            var blobClient = storageAccount.CreateCloudBlobClient();

            // finally, using the blob client, get a reference to our container
            var container = blobClient.GetContainerReference(folder.ToString().ToLower());

            // if we had not created the container in the portal, this would automatically create it for us at run time
            container.CreateIfNotExists();

            // by default, blobs are private and would require your access key to download.
            // You can allow public access to the blobs by making the container public.   
            if (isPrivate)
            {
                container.SetPermissions(new BlobContainerPermissions
                {
                    PublicAccess = BlobContainerPublicAccessType.Off
                });
            }
            else
            {
                container.SetPermissions(new BlobContainerPermissions
                {
                    PublicAccess = BlobContainerPublicAccessType.Blob
                });
            }

            return container;
        }
    }
}
