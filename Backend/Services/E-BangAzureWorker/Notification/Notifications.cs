﻿using E_BangAzureWorker.EventPublisher;
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
                case AzureFactory.AzureStrategyEnum.Add:
                    model.Message = FileAdded;
                    break;
                case AzureFactory.AzureStrategyEnum.Remove:
                    model.Message = FileDeleted;
                    break;
                case AzureFactory.AzureStrategyEnum.List_Add:
                    model.Message = ListFilesAdded; 
                    break;
                case AzureFactory.AzureStrategyEnum.List_Remove:
                    model.Message = ListFilesDeleted;
                    break;
                default:
                    throw new ArgumentException("Invalide Enum");
            }
            return model;
        }

        private readonly static string FileDeleted = "File was sucessfully deleted";

        private readonly static string FileAdded = "File was sucessfully added";

        private readonly static string ListFilesDeleted = "List of files was sucessfully deleted";

        private readonly static string ListFilesAdded = "List of files was sucessfully added";
    }
}
