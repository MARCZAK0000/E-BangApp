using System.Diagnostics;
using E_BangAzureWorker.Model;

namespace E_BangAzureWorker.Azurite
{
    public class EmulatorInvoke
    {
        private readonly ILogger<EmulatorInvoke> _logger;
        private readonly IEmulatorSettingss _emulatorSettingss;

        public EmulatorInvoke(ILogger<EmulatorInvoke> logger, IEmulatorSettingss emulatorSettingss)
        {
            _logger = logger;
            _emulatorSettingss = emulatorSettingss;
        }

        public void RunEmulator()
        {
            try
            {
                if (!_emulatorSettingss.Enabled)
                    return;

                var process = new Process()
                {
                    StartInfo = new()
                    {
                        FileName = @"",
                        Verb = "",
                        UseShellExecute = true,
                        CreateNoWindow = false,
                        RedirectStandardError = false
                    }
                };
                process.Start();
                _logger.LogInformation("Emulator initialized at {DateTime}", DateTime.Now);
            }
            catch (Exception)
            {
                _logger.LogError("Error: Emulator doesn't initalized at {DateTime}", DateTime.Now);
                throw;
            }

        }
    }
}
