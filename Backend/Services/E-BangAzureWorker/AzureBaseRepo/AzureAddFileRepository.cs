using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using E_BangAzureWorker.AzureFactory;
using E_BangAzureWorker.Model;

namespace E_BangAzureWorker.AzureBaseRepo
{
    public class AzureAddFileRepository : IAzureBase
    {
        private readonly BlobServiceClient _blobServiceClient;

        private readonly IContainerSettings _containerSettings;

        public AzureAddFileRepository(BlobServiceClient blobServiceClient, IContainerSettings containerSettings)
        {
            _blobServiceClient = blobServiceClient;
            _containerSettings = containerSettings;
        }

        public async Task<FileChangesResponse> HandleAzureAsync(MessageModel model, CancellationToken token)
        {
            var fileChangesResponse = new FileChangesResponse();
            fileChangesResponse.FileChangesInformations = [];
            var containerInfo = _containerSettings
                .Containers
                .FirstOrDefault(pr => pr.Id == model.ContainerID)
                ?? throw new ArgumentNullException("Invalid container ID");

            var container = _blobServiceClient.GetBlobContainerClient(blobContainerName: containerInfo.Name);
            var blobHeader = new BlobHttpHeaders();
            switch (model.ContainerID)
            {
                case 1:
                    {
                        await container.DeleteBlobIfExistsAsync(blobName: model.AccountID, DeleteSnapshotsOption.None, cancellationToken: token);
                        var newBlob = container.GetBlobClient(blobName: model.AccountID);
                        var file = model.Data.FirstOrDefault() ??
                            throw new ArgumentNullException("Empty File");
                        blobHeader.ContentType = file.DataType;
                        if (file.Data == null || file.Data == Array.Empty<byte>())
                            throw new ArgumentNullException("Invalid File");
                        using (var stream = new MemoryStream(file.Data))
                        {
                            await newBlob.UploadAsync(content: stream
                            , options: new BlobUploadOptions { HttpHeaders = blobHeader }
                            , cancellationToken: token);
                        }
                        ;
                        fileChangesResponse.IsDone = true;
                        fileChangesResponse.IsRemoved = false;
                        fileChangesResponse.FileChangesInformations.Add(
                            new FileChangesInformations
                            {
                                ContainerId = containerInfo.Id,
                                FileName = model.AccountID,
                                FileType = blobHeader.ContentType
                            });
                        return fileChangesResponse ;
                    }
                case 2:
                case 3:
                    {
                        var files = model.Data.ToList();
                        foreach (var item in files)
                        {
                            await container
                                .DeleteBlobIfExistsAsync
                                    (string.Format(model.AccountID+"_"+model.ProductID+ "_" + item.DataName), DeleteSnapshotsOption.None, cancellationToken: token);
                            var blobClient = container.GetBlobClient(blobName: string.Format(model.AccountID + "_" + model.ProductID + "_" + item.DataName));
                            blobHeader.ContentType = item.DataType;
                            if (item.Data == null || item.Data == Array.Empty<byte>())
                                throw new ArgumentNullException("Invalid File");
                            using (var stream = new MemoryStream(item.Data))
                            {
                                await blobClient.UploadAsync(content: stream
                                , options: new BlobUploadOptions { HttpHeaders = blobHeader }
                                , cancellationToken: token);
                            };
                            fileChangesResponse.FileChangesInformations.Add(
                            new FileChangesInformations
                            {
                                ContainerId = containerInfo.Id,
                                FileName = model.AccountID + "_" + model.ProductID + "_" + item.DataName,
                                FileType = blobHeader.ContentType
                            });
                        }
                        fileChangesResponse.IsDone = true;
                        fileChangesResponse.IsRemoved = false;
                        
                        return fileChangesResponse;
                    }
                default: throw new ArgumentException("Invalid Enum");
            }

        }
    }
}
