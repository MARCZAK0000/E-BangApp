using App.EmailRender.Shared.Abstraction;

namespace App.EmailHelper.EmailParameters.Body
{
    public class TwoWayTokenParameters : IEmailParameters
    {
        public string Email { get; set; }
        public string Token { get; set; }
    }
}
