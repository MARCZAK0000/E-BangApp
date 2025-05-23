using System.Diagnostics.CodeAnalysis;
using E_BangAzureWorker.Model;

namespace E_BangAzureWorker.Comaperer
{
    public class BlobContainerComparer : IEqualityComparer<BlobContainer>
    {
        public bool Equals(BlobContainer? x, BlobContainer? y)
        {
            return x is not null
                && y is not null
                && x.Name == y.Name
                && x.RootFilePath == y.RootFilePath
                && x.Enabled == y.Enabled;
        }

        public int GetHashCode([DisallowNull] BlobContainer obj)
        {
            return HashCode.Combine(
                obj.Name,
                obj.RootFilePath,
                obj.Enabled
            );
        }
    }
}
