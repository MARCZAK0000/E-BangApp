namespace App.RabbitSharedClass.UniversalModel
{
    public record ForceNotification
    {
        public bool ForceEmail { get; init; }
        public bool ForcePush { get; init; }
        public bool ForceSms { get; init; }
    }

    public static class ForceNotificationExtensions
    {
        public static bool IsAnyForce(this ForceNotification forceNotification)
        {
            return forceNotification.ForceEmail || forceNotification.ForcePush || forceNotification.ForceSms;
        }
    }

    public static class ForceNotificationDefaults
    {
        public static readonly ForceNotification None = new()
        {
            ForceEmail = false,
            ForcePush = false,
            ForceSms = false
        };
        public static readonly ForceNotification All = new()
        {
            ForceEmail = true,
            ForcePush = true,
            ForceSms = true
        };
        public static readonly ForceNotification EmailOnly = new()
        {
            ForceEmail = true,
            ForcePush = false,
            ForceSms = false
        };
        public static readonly ForceNotification PushOnly = new()
        {
            ForceEmail = false,
            ForcePush = true,
            ForceSms = false
        };
        public static readonly ForceNotification SmsOnly = new()
        {
            ForceEmail = false,
            ForcePush = false,
            ForceSms = true
        };
    }
}
