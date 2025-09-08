using App.EmailRender.Shared.Abstraction;

namespace App.EmailHelper.EmailParameters.Body
{
    public class RegistrationAccountParameters : IEmailParameters
    {
        public string Email { get; set; }

        public string Token { get; set; }

        public string Url { get; set; }
    }
}
