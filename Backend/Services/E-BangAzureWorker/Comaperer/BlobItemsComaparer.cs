using E_BangAzureWorker.Model;
using System.Diagnostics.CodeAnalysis;

namespace E_BangAzureWorker.Comaperer
{
    public class BlobItemsComaparer : IEqualityComparer<BlobItems>
    {
        public bool Equals(BlobItems? x, BlobItems? y)
        {
            return x != null
                && y != null
                && x.ContainerID == y.ContainerID
                && x.BlobItemName == y.BlobItemName
                && x.BlobItemType == y.BlobItemType
                && x.ProductId == y.ProductId
                && x.AccountId == y.ProductId;
        }

        public int GetHashCode([DisallowNull] BlobItems obj)
        {
            throw new NotImplementedException();
        }
    }
}
