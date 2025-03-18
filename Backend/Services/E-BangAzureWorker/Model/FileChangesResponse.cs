namespace E_BangAzureWorker.Model
{
    public class FileChangesResponse
    {
        public bool IsDone { get; set; }
        public bool IsRemoved { get; set; }
        public List<FileChangesInformations> FileChangesInformations { get; set; }

        public override string ToString()
        {
            return $"Done: {IsDone}\r\n" +
                $"RemovedFile?:{IsRemoved}\r\n" +
                $"FileChangesCount: {FileChangesInformations.Count}";
        }
    }
}
