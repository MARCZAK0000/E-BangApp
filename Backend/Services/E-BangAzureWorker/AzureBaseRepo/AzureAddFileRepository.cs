using System.Runtime.CompilerServices;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using E_BangAzureWorker.AzureFactory;
using E_BangAzureWorker.Model;

namespace E_BangAzureWorker.AzureBaseRepo
{
    public class AzureAddFileRepository : IAzureBase
    {
        private readonly BlobServiceClient _blobServiceClient;
        public async Task<bool> HandleAzureAsync(MessageModel model, CancellationToken token)
        {
            if (file == null || file.Length == 0) return false;
            var container = _blobServiceClient.GetBlobContainerClient(blobContainerName: _blobStorageTablesNames.profileImage);
            await container.DeleteBlobIfExistsAsync(blobName: userID, DeleteSnapshotsOption.None, cancellationToken: token);


            var newBlob = container.GetBlobClient(blobName: userID);

            var blobHeader = new BlobHttpHeaders()
            {
                ContentType = $"image/jpeg"
            };
            using var stream = new MemoryStream(file); //Never use IFormFile in BackgroundService
            await newBlob.UploadAsync(content: stream
            , options: new BlobUploadOptions { HttpHeaders = blobHeader }
            , cancellationToken: token);

            return true;
        }
    }
}
