using Azure.Storage.Blobs.Models;
using Azure.Storage.Blobs;
using E_BangAzureWorker.Model;
using E_BangAzureWorker.AzureStrategy;

namespace E_BangAzureWorker.AzureBaseRepo
{
    public class AzureRemoveFileRepository : IAzureBase
    {
        private readonly BlobServiceClient _blobServiceClient;

        private readonly IContainerSettings _containerSettings;

        public AzureRemoveFileRepository(BlobServiceClient blobServiceClient, IContainerSettings containerSettings)
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
                        fileChangesResponse.IsDone = true;
                        fileChangesResponse.IsRemoved = true;
                        fileChangesResponse.FileChangesInformations.Add(new FileChangesInformations
                        {
                            ContainerId = containerInfo.Id,
                            FileName = model.AccountID,
                            FileType = blobHeader.ContentType,
                            AccountID = model.AccountID,
                            ProductID = model.ProductID
                        });
                        return fileChangesResponse;
                    }
                case 2:
                case 3:
                    {
                        var files = model.Data.ToList();
                        foreach (var item in files)
                        {
                            await container
                                .DeleteBlobIfExistsAsync
                                    (string.Format(model.AccountID + "_" + model.ProductID + "_" + item.DataName), DeleteSnapshotsOption.None, cancellationToken: token);
                            fileChangesResponse.FileChangesInformations.Add(new FileChangesInformations
                            {
                                ContainerId = containerInfo.Id,
                                FileName = string.Format(model.AccountID + "_" + model.ProductID + "_" + item.DataName),
                                FileType = blobHeader.ContentType,
                                AccountID = model.AccountID,
                                ProductID = model.ProductID
                            });
                        }
                        fileChangesResponse.IsDone = true;
                        fileChangesResponse.IsRemoved = true;
                        
                        return fileChangesResponse;
                    }
                default: throw new ArgumentException("Invalid Enum");
            }
        }
    }
}
