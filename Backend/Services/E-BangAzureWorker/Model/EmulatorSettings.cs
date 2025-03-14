namespace E_BangAzureWorker.Model
{
    public interface IEmulatorSettingss
    {
        public string FileName { get; set; }
        public string Verb { get; set; }

        public bool Enabled { get; set; }
    }

    public class EmulatorSettings : IEmulatorSettingss
    {
        public string FileName { get ; set ; }
        public string Verb { get ; set ; }

        public bool Enabled { get; set; }
    }
}
