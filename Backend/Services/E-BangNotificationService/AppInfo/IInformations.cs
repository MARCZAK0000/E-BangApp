namespace E_BangNotificationService.AppInfo
{
    public interface IInformations
    {
        DateTime InitTime { get; set; }
        DateTime ClosedTime { get; set; }
        DateTime CurrentTime { get; set; }
        bool IsWorking { get; set; }
    }
}
