using App.EmailRender.Shared.Abstraction;

namespace App.EmailHelper.EmailParameters.Header
{
    public class DefaultHeaderParameters : IEmailParameters
    {
        public string AppName { get; set; }
    }
}
