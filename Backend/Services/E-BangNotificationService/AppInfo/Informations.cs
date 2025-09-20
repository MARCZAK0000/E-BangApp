namespace AppInfo
{
    public class Informations : IInformations
    {
        public DateTime InitTime { get ; set ; }
        public DateTime CurrentTime { get ; set ; }
        public bool IsWorking { get ; set ; }
        public DateTime ClosedTime { get; set; }
    }
}
