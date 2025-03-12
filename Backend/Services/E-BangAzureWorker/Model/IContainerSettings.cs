using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_BangAzureWorker.Model
{
    public interface IContainerSettings
    {
        string RootPath { get; set; }
        List<BlobContainer> Containers { get; set; }
    }
}
