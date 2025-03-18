using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_BangAzureWorker.Model
{
    class FileChangesInformations
    {
        public int ContainerId { get; set; }
        public string FileName { get; set; }
        public string FileType { get; set; }
    }
}
