namespace E_BangEmailWorker.Repository
{
    public interface IDatabaseRepository
    {
        Task SaveEmailInfo(string address, bool isSend, CancellationToken token);

    }
}
