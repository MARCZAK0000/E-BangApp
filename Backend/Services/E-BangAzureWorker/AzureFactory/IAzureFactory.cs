namespace E_BangAzureWorker.AzureFactory
{
    public interface IAzureFactory
    {
        public IAzureBase RoundRobin(AzureStrategyEnum azureEnum);
       
    }
}
