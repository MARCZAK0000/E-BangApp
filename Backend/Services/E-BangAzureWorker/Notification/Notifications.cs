using E_BangAppRabbitSharedClass.AzureRabbitModel;
using E_BangAzureWorker.AzureStrategy;
using E_BangAzureWorker.EventPublisher;
using E_BangAzureWorker.Model;

namespace E_BangAzureWorker.Notification
{
    public static class Notifications
    {
        public static SendModel GenerateMessage (EventMessageArgs args)
        {
            SendModel model = new SendModel();
            model.AccountID = args.AccountID;
            
            switch(args.AzureStrategyEnum)
            {
                case AzureStrategyEnum.Add:
                    model.Message = FileAdded;
                    break;
                case AzureStrategyEnum.Remove:
                    model.Message = FileDeleted;
                    break;
                default:
                    throw new ArgumentException("Invalide Enum");
            }
            return model;
        }

        private readonly static string FileDeleted = "File was sucessfully deleted";

        private readonly static string FileAdded = "File was sucessfully added";
    }
}
