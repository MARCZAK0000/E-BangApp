using App.EmailRender.Shared.Abstraction;
using Microsoft.AspNetCore.Components;

namespace App.EmailHelper.EmailParameters.Footer
{
    public class DefaultFooterParameters : IEmailParameters
    {
        public string AppName { get; set; }

        public string Year { get; set; } = DateTime.Now.Year.ToString();
    }
}
